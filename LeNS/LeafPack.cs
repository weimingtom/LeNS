using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

// LeafPack�t�@�C���N���X
namespace LeNS
{
    class LeafPack
    {
        // �^�C�v
        public enum LeafPackType
        {
            LPTYPE_SIZUWIN,
            LPTYPE_KIZUWIN,
            LPTYPE_TOHEART,
            LPTYPE_SAORIN,
            LPTYPE_UNKNOWN
        }

        public enum LeNSFileType
        {
            ScenarioFile,   // �V�i���I�t�@�C��
            BGGraphic,      // �w�i�A�r�W���A����(640x400�Ƀ��T�C�Y)
            CharaGraphic,   // �����G(320or640 x 400�Ƀ��T�C�Y)
            EtcGraphic,     // ���̑��O���t�B�b�N(���T�C�Y�Ȃ�)
            SpecialFile,    // ����(�v��p����)
            SoundFile       // ���ʉ�
        }

        // �t�@�C�����
        public struct LeafFileInfo
        {
            public String Name;                 /* �t�@�C����           */
            public Int32 Pos;                   /* �擪�|�C���^         */
            public Int32 Length;                /* �t�@�C����           */
            public LeNSFileType Type;           /* �t�@�C���^�C�v       */
            public Int32 PlayTime;              /* �Đ�����(P16�̂�)    */
        }

        private FileStream fs;                  /* �t�@�C���X�g���[��   */
        private BinaryReader br;

        private const int LP_KEY_LEN = 11;      /* �W�J�p�L�[��         */
        private Int16[] key;                    /* �W�J�p�L�[           */

        private LeafPackType _type;             /* �p�b�N�̎��         */
        private long _size;                     /* �T�C�Y               */
        private Int16 _fileNum;                 /* �p�b�N���̃t�@�C���� */

        private Int16 _scenarioNum;
        private Int16 _otherNum;

        /* �t�@�C���e�[�u��     */
        private Dictionary<String, LeafFileInfo> _files;

        // �v���p�e�B
        public LeafPackType Type { get { return _type; } }
        public long Size { get { return _size; } }
        public long FileNum { get { return (long)_fileNum; } }
        public Dictionary<String, LeafFileInfo> Files { get { return _files; } }
        public long ScenarioNum { get { return (long)_scenarioNum; } }
        public long OtherNum { get { return (long)_otherNum; } }

        // �I�[�v��
        public void Open(String Path, FileMode Mode)
        {
            key = new Int16[LP_KEY_LEN];
            _type = LeafPackType.LPTYPE_UNKNOWN;
            _size = -1;
            _fileNum = -1;
            _scenarioNum = 0;
            _otherNum = 0;
            _files = new Dictionary<String, LeafFileInfo>();


            // �t�@�C���X�g���[���I�[�v��
            fs = new FileStream(Path, Mode);

            // �o�C�i�����[�_�I�[�v��
            br = new BinaryReader(fs);

            if (Mode == FileMode.Open)
            {
                // �t�@�C���T�C�Y
                _size = fs.Length;

                // �}�W�b�N�R�[�h�̃`�F�b�N
                String strBuf = new String(br.ReadChars(8));
                if (!strBuf.Equals("LEAFPACK"))
                {
                    throw new InvalidDataException("LeafPack�ł͂���܂���B");
                }

                // �t�@�C�����̎擾
                _fileNum = br.ReadInt16();

                // �^�C�v�`�F�b�N
                switch (_fileNum)
                {
                    case 0x0248:
                        // LVNS3DAT.PAK
                        _type = LeafPackType.LPTYPE_TOHEART;
                        break;
                    case 0x03e1:
                        // LVNS3SCN.PAK
                        _type = LeafPackType.LPTYPE_TOHEART;
                        break;
                    case 0x01fb:
                        // MAX2DATA.PAK
                        _type = LeafPackType.LPTYPE_KIZUWIN;
                        break;
                    case 0x0193:
                        // MAX_DATA.PAK
                        _type = LeafPackType.LPTYPE_SIZUWIN;
                        break;
                    case 0x0072:
                        _type = LeafPackType.LPTYPE_SAORIN;
                        break;
                    default:
                        _type = LeafPackType.LPTYPE_UNKNOWN;
                        break;
                }

                // KEY�̎擾
                guessKey();

                // �t�@�C���e�[�u���̎擾
                extractTable();
            }
            else
            {
                throw new InvalidOperationException("�����ȃ��[�h�ŃI�[�v������܂����B");
            }
        }

        // �N���[�Y
        public void Close()
        {
            if (fs != null)
            {
                fs.Close();
            }
            if (br != null)
            {
                br.Close();
            }
        }

        // KEY�̎擾
        private void guessKey()
        {
            byte[] buf;

            // �t�@�C���e�[�u���擪�ɃV�[�N
            fs.Seek(-(24 * _fileNum), SeekOrigin.End);

            // 64�o�C�g���ǂݎ��
            buf = br.ReadBytes(64);

            // �L�[����
            // AND 0xff��3�o�C�g�ȍ~�؂�̂Ă̈Ӗ�
            key[0] = buf[11];
            key[1] = (Int16)((buf[12] - 0x0a) & 0xff);
            key[2] = buf[13];
            key[3] = buf[14];
            key[4] = buf[15];
            key[5] = (Int16)((buf[38] - buf[22] + key[0]) & 0xff);
            key[6] = (Int16)((buf[39] - buf[23] + key[1]) & 0xff);
            key[7] = (Int16)((buf[62] - buf[46] + key[2]) & 0xff);
            key[8] = (Int16)((buf[63] - buf[47] + key[3]) & 0xff);
            key[9] = (Int16)((buf[20] - buf[36] + key[3]) & 0xff);
            key[10] = (Int16)((buf[21] - buf[37] + key[4]) & 0xff);
        }

        // �t�@�C���e�[�u���̎擾
        private void extractTable()
        {
            int i, j, k = 0;
            char[] filename;
            byte[] b;
            LeafFileInfo info;

            // �t�@�C���e�[�u���擪�ɃV�[�N
            fs.Seek(-(24 * _fileNum), SeekOrigin.End);

            // �t�@�C���e�[�u���擾
            for (i = 0; i < _fileNum; i++)
            {
                info = new LeafFileInfo();

                filename = new char[12];
                // �t�@�C����
                for (j = 0; j < 12; j++)
                {
                    filename[j] = (char)((br.ReadByte() - key[k]) & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Name = regularizeName(filename);

                // �t�@�C���ʒu
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Pos = (int)(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);

                // �t�@�C����
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }
                info.Length = (int)(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);

                // ���t�@�C���ʒu(���g�p�H)
                b = br.ReadBytes(4);
                for (j = 0; j < 4; j++)
                {
                    b[j] = (byte)(b[j] - key[k] & 0xff);
                    k = (++k) % LP_KEY_LEN;
                }

                // �t�@�C���^�C�v(��ɉ摜�ϊ��Ŏg�p)
                info.Type = getFileType(_type, info.Name);

                // �o�b�l�Đ�����(msec)
                if (info.Type == LeNSFileType.SoundFile)
                {
                    info.PlayTime = info.Length * 1000 / 44100;
                }
                else
                {
                    info.PlayTime = -1;
                }

                _files.Add(info.Name, info);
            }
        }

        // �t�@�C�������K��
        private String regularizeName(char[] name)
        {
            int i;
            char[] buf = new char[12];

            // �X�y�[�X�܂Ŏ擾
            for (i = 0; i < 8 && name[i] != 0x20; i++)
            {
                buf[i] = name[i];
            }

            // �g���q
            buf[i++] = '.';
            buf[i++] = name[8];
            buf[i++] = name[9];
            buf[i++] = name[10];

            // ���K�������t�@�C������Ԃ�(NULL�����͐؂�̂�)
            return (new String(buf)).TrimEnd('\0').ToUpper();
        }

        // �t�@�C���𓾂�
        public byte[] Get(String FileName)
        {
            byte[] ret;
            LeafFileInfo info;

            if (_files.ContainsKey(FileName))
            {
                info = _files[FileName];
                // �Y������t�@�C����byte�z��ŕԂ�
                fs.Seek(info.Pos, SeekOrigin.Begin);
                ret = br.ReadBytes(info.Length);

                // �ǂݏo���f�[�^�̕�����
                int i;
                for (i = 0; i < info.Length; i++)
                {
                    int buf = (int)ret[i];
                    buf = (buf - key[i % LP_KEY_LEN]) & 0xff;
                    ret[i] = (byte)buf;
                }

                // ���ʂ�Ԃ�
                return ret;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        // �t�@�C�����݊m�F
        public Boolean Exists(String FileName)
        {
            return _files.ContainsKey(FileName);
        }

        // �t�@�C���^�C�v�̔��ʏ���
        private LeNSFileType getFileType(LeafPackType packType, String fileName)
        {
            LeNSFileType file_type = LeNSFileType.EtcGraphic;
            String ext = Path.GetExtension(fileName);
            String typeString;

            switch (packType)
            {
                case LeafPackType.LPTYPE_SIZUWIN:
                    if (fileName.Equals("LEAF.LFG"))
                    {
                        // ���[�t���S(���ȑO)
                        file_type = LeNSFileType.EtcGraphic;
                    }
                    else if (fileName.Equals("KNJ_ALL.KNJ"))
                    {
                        // �����t�@�C��
                        file_type = LeNSFileType.SpecialFile;
                    }
                    else if (fileName.Substring(0, 3).Equals("SCN"))
                    {
                        // �V�i���I�t�@�C��
                        file_type = LeNSFileType.ScenarioFile;
                    }
                    else if (ext.Equals(".P16"))
                    {
                        // ���ʉ��t�@�C��
                        file_type = LeNSFileType.SoundFile;
                    }
                    else if (fileName.Substring(0, 3).Equals("HVS"))
                    {
                        // �g�V�[��
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("MAX_C"))
                    {
                        // �����G
                        file_type = LeNSFileType.CharaGraphic;
                    }
                    else if (fileName.Substring(0, 4).Equals("NEXT"))
                    {
                        // ���f���p�H
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("MAX_S"))
                    {
                        // �w�i
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 2).Equals("OP"))
                    {
                        // �I�[�v�j���O
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else if (fileName.Substring(0, 5).Equals("TITLE"))
                    {
                        // �^�C�g��
                        file_type = LeNSFileType.EtcGraphic;
                    }
                    else if (fileName.Substring(0, 3).Equals("VIS"))
                    {
                        // �r�W���A���V�[��
                        file_type = LeNSFileType.BGGraphic;
                    }
                    else
                    {
                        throw new InvalidDataException("����`�̃t�@�C�������o���܂����B");
                    }
                    break;
                case LeafPackType.LPTYPE_KIZUWIN:
                    if (fileName.Equals("LEAF.LFG"))
                    {
                        // ���[�t���S(���ȍ~)
                        file_type = LeNSFileType.SpecialFile;
                    }
                    else if (fileName.Equals("KNJ_ALL.KNJ"))
                    {
                        // �����t�@�C��
                        file_type= LeNSFileType.SpecialFile;
                    }
                    else if (ext.Equals(".SCN"))
                    {
                        // �V�i���I�t�@�C��
                        file_type = LeNSFileType.ScenarioFile;
                    }
                    else if (ext.Equals(".P16"))
                    {
                        // ���ʉ��t�@�C��
                        file_type = LeNSFileType.SoundFile;
                    }
                    else
                    {
                        typeString = fileName.Substring(0, 4);
                        switch (typeString)
                        {
                            case "BLDW":    // �����Ԃ�
                                file_type = LeNSFileType.BGGraphic;
                                break;
                            case "CLAW":    // ��
                                file_type = LeNSFileType.BGGraphic;
                                break;
                            case "TITL":    // �^�C�g��
                                file_type = LeNSFileType.EtcGraphic;
                                break;
                            default:
                                typeString = fileName.Substring(0, 1);
                                switch (typeString)
                                {
                                    case "C":   // �����G
                                        file_type = LeNSFileType.CharaGraphic;
                                        break;
                                    case "H":   // �g�V�[��
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    case "O":   // �I�[�v�j���O
                                        file_type = LeNSFileType.EtcGraphic;
                                        break;
                                    case "S":   // �w�i
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    case "V":   // �r�W���A���V�[��
                                        file_type = LeNSFileType.BGGraphic;
                                        break;
                                    default:
                                        throw new InvalidDataException("����`�̃t�@�C�������o���܂����B");
                                }
                                break;
                        }
                    }

                    break;
                case LeafPackType.LPTYPE_TOHEART:
                    break;
                default:    // �������A���̑��͔��ʖ��Ή�
                    break;
            }

            if (file_type == LeNSFileType.ScenarioFile)
            {
                _scenarioNum++;
            }
            else
            {
                _otherNum++;
            }
            return file_type;
        }
    }
}
