using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LeNS
{
    class LeNSConvP16 : LeNSConvFile
    {
        public LeNSConvP16(LeNSConvOption Option) : base(Option) { }

        // �ϊ�����
        public override LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData)
        {
            return Conv(SrcInfo, SrcData, 1);
        }

        // �ϊ�����
        public LeNSConvResult Conv(LeafPack.LeafFileInfo SrcInfo, byte[] SrcData, int Number)
        {
            // �����������11MHzStereo�̐��f�[�^�ɂȂ�̂Ńw�b�_�t���ďo�́B
            // �ꕔ�f�[�^����o�C�g�ɂȂ��Ă���̂ŁA0��������ŃT�C�Y�����킹��B
            // ���킹�Ȃ��ƕ�����Đ��Ńo�O��B
            FileStream fs;
            BinaryWriter bw;
            int length = SrcData.Length;
            if (length % 2 == 1)
            {  
                // ��T�C�Y�̏ꍇ�̓T�C�Y��+1�o�C�g
                length++;
            }
            length *= Number;

            fs = new FileStream(getSavePath(SrcInfo.Name, Number), FileMode.Create);

            try
            {
                bw = new BinaryWriter(fs);
                try
                {
                    //�@���育����Ə����o��
                    bw.Write('R');                          // RIFF�w�b�_
                    bw.Write('I');
                    bw.Write('F');
                    bw.Write('F');
                    bw.Write((Int32)(length + 36));         // �f�[�^�T�C�Y + 36byte
                    bw.Write('W');                          // WAVE�w�b�_
                    bw.Write('A');
                    bw.Write('V');
                    bw.Write('E');
                    bw.Write('f');                          // fmt�`�����N
                    bw.Write('m');
                    bw.Write('t');
                    bw.Write(' ');
                    bw.Write((Int32)(16));                  // fmt�`�����N�T�C�Y(16)
                    bw.Write((Int16)(1));                   // �t�H�[�}�b�g(���j�APCM=1)
                    bw.Write((Int16)(2));                   // �`���l����(2)
                    bw.Write((Int32)(11025));               // �T���v�����O���[�g(11025)
                    bw.Write((Int32)(44100));               // 44100Byte/sec)
                    bw.Write((Int16)(4));                   // Byte/sample�~�`�����l����
                    bw.Write((Int16)(16));                  // 16bit
                    bw.Write('d');                          // data�`�����N
                    bw.Write('a');
                    bw.Write('t');
                    bw.Write('a');
                    bw.Write((Int32)(length));              // �f�[�^�T�C�Y

                    // �Đ��񐔕��o��
                    for (int i = 1; i <= Number; i++ )
                    {
                        bw.Write(SrcData);                  // �g�`�f�[�^
                        if (SrcData.Length % 2 == 1)
                        {
                            bw.Write((byte)0);
                        }
                    }
                }
                finally
                {
                    bw.Close();
                }
            }
            finally
            {
                fs.Close();
            }

            return LeNSConvResult.ok;
        }

        // �Z�[�u�p�X�擾
        protected override String getSavePath()
        {
            String path = base.getSavePath() + "\\se";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        protected String getSavePath(String fileName, int Number)
        {
            return getSavePath() + "\\" + GetSaveName(fileName, Number);
        }

        // �ϊ���t�@�C�����擾
        public override string GetSaveName(string FileName)
        {
            return GetSaveName(FileName, 1);
        }

        // �ϊ���t�@�C�����擾
        public string GetSaveName(string FileName, int Number)
        {
            if (Number == 1)
            {
                return Path.GetFileNameWithoutExtension(FileName) + ".WAV";
            }
            else
            {
                return Path.GetFileNameWithoutExtension(FileName) + "_" + Number.ToString("0") + ".WAV";
            }
        }
    }
}
