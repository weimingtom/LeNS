using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// �s�n�c�n
// �E�n�o�^�d�c�^���S
// �E���ʉ�
// �E�G�t�F�N�g�n����

namespace LeNS
{
    class LeNSConvSCNKZ : LeNSConvSCN
    {
        // ���[�h����PCM���ێ��p
        protected byte loadedPCM = 0xff;    // ���[�h���̂o�b�l
        protected int waitPCM = 0;          // ���[�h���o�b�l�̍Đ�����

        protected Boolean onTextBr = true;  // ���O�ɉ��s����Ă邩

        public LeNSConvSCNKZ(LeNSConvOption Option)
            : base(Option)
        {
            // �t�H���g�e�[�u���̍쐬
            // ���ꕔ���䕶���ɔ�Ή��Bcr�͑S�p�X�y�[�X�A!?�́I�ɕϊ�����܂��B
            //fontTable = "�@���`�a�b�c�d�e�f�g�h�i�j�k�l�m�n�o�p�q�r�s�t�u�v�w�x�y�O�P�Q�R�S�T�U�V�W�X�����������������������������������ĂƂȂɂʂ˂̂͂Ђӂւق܂݂ނ߂�������������񂪂����������������������Âłǂ΂тԂׂڂς҂Ղ؂ۂ�����������������A�C�E�G�I�J�L�N�P�R�T�V�X�Z�\�^�`�c�e�g�i�j�k�l�m�n�q�t�w�z�}�~�����������������������������K�M�O�Q�S�U�W�Y�[�]�_�a�d�f�h�o�r�u�x�{�p�s�v�y�|�@�B�D�F�H�������b�����w�x�u�v�i�j�H�I!?�|�[�`�B�A�D�C�E�d�c���X����������cr�����������������������ÈĈňȈʈˈ͈̈ЈӈԈՈֈ׈وڈ݈߈�����������������������@�A�B�C�E�F�H�J�Q�R�Z�\�^�_�c�e�f�h�i�j�k�p�q�s�t�v�w�x�z�~�����������������������������������������������������������������������������ĉŉƉȉɉʉˉ͉̉Ή҉Ӊԉ׉؉ۉ܉݉߉�������������������������B�C�E�F�G�J�K�O�P�Q�S�W�X�_�d�e�i�j�k�l�m�n�o�p�r�s�u�v�w�y�z�{�|�������������������������������������������������ÊĊŊǊȊɊ̊ϊъԊՊ֊׊يۊ܊ߊ��������������������������@�A�C�F�G�K�L�M�N�P�Q�S�U�V�Y�Z�[�]�^�`�c�g�i�l�p�q�r�s�t�u�v�w�x�y�z�{�|�}�~�������������������������������������������������������������������������������������������ËƋǋȋɋʋ͋΋ϋ֋؋ًۋ݋ߋ��������������������@�C�F�I�J�N�Q�U�W�X�Y�Z�^�`�b�e�f�g�h�i�m�n�o�p�q�u�v�x�y�}���������������������������������������������������������������������������������������������ÌČŌǌȌɌˌ̌Ҍ֌ٌی܌݌ߌ������������������������A�C�D�H�I�K�L�N�Q�R�S�T�U�X�Z�\�a�b�c�d�e�g�i�j�k�l�m�q�r�s�u�v�w�x�|�~���������������������������������������������������������������������������ÍčōǍȍʍˍ΍ύЍӍՍ׍؍ٍڍۍݍލߍ����������������@�A�B�C�D�E�G�K�O�Q�R�S�U�Y�^�_�a�c�d�e�f�g�h�i�j�l�m�n�o�p�q�s�t�u�v�w�x�{�|�}�~�������������������������������������������������������������������������������������������ŎɎʎˎ͎̎ΎώЎҎӎԎՎ׎؎܎ގߎ���������������������������A�C�E�G�H�I�K�L�O�P�R�T�W�X�Z�[�\�]�_�`�a�b�c�d�e�f�h�k�n�o�p�q�t�u�������������������������������������������������������������������������ďŏƏǏȏɏˏ͏ΏϏЏՏ؏ڏۏ�����������������������@�A�B�D�E�F�G�H�J�K�L�M�N�O�Q�R�S�T�U�V�Z�[�\�^�_�c�e�f�g�h�i�j�k�l�n�o�q�u�s�w�{�}���������������������������������������������������������������������������������ÐĐŐƐȐʐ̐ΐϐАѐӐԐՐؐڐېܐݐߐ��������������������������A�D�F�I�M�N�O�P�R�S�U�^�_�a�c�f�g�h�i�j�n�o�r�s�t�w�z�{�|�}�~���������������������������������������������������������������������������������������������őʑ̑Αϑёґӑԑّܑ֑ݑޑ������������������@�B�D�E�J�N�P�Q�S�T�W�Z�[�a�c�d�e�f�g�i�j�k�l�m�n�p�r�s�u�v�x�z�{�|�����������������������������������������������������������������������������Òǒɒʒ͒גܒޒߒ��������������������@�B�D�E�G�H�I�K�M�O�P�S�T�V�W�X�Y�Z�\�]�_�`�c�d�f�g�h�i�k�n�o�q�r�s�w�x�y�z�{�|�������������������������������������������������������������������������œƓǓ˓͓דؓۓݓߓ�������������������A�C�E�F�G�L�M�N�O�P�R�S�V�Y�Z�[�\�]�`�c�d�g�h�j�n�p�q�r�s�t�w�x�y�z�{�~�����������������������������������������������������ŔƔɔʔ͔ӔԔՔڔ۔ޔߔ�����������������������@�C�F�G�I�K�P�S�W�X�Y�\�]�`�a�b�c�i�n�p�q�r�s�t�v�w�x�z�|�}�~�������������������������������������������������������������������������������ĕǕȕʕ̕ϕЕҕӕԕ֕וٕەߕ��������������������@�A�C�D�E�F�J�K�L�M�O�R�S�T�V�W�Y�Z�[�\�]�^�_�`�b�c�e�h�j�k�l�o�u�v�{�|�}�����������������������������������������������������������������������Ŗʖ͖ϖіҖԖՖٖؖږߖ�����������������������@�B�D�E�F�H�L�R�S�T�U�V�X�Y�[�\�]�^�_�a�c�d�e�g�h�j�l�m�n�p�t�v�x�y�z�{�}�~���������������������������������������������������������������������������×Ǘʗ˗̗͗Ηї֗חܗݗޗߗ�������������������A�F�H�I�J�L�Q�R�V�Y�Z�^�_�a�b�c�e�f�g�h�r����b�j�k�l�q�}���������ɝC�X�����B�ӟ����z���������Y���S�\��b���x���M�Չ��R�ʊӋT���򌡌ՍJ�o�����M��D��������X�k�đؒK�o�e��l���������ѕR���������Õ˖p�w���Ö��U�ꗒ���C�M�o�q��R���j�p��g�B�T������N���O�S�[���Ǝ�ОH�؈މ�����U�����H�T���I�Ր��u�^�q�Г���e�����㖗�i�]������B�K�X�����N�O�����J�r�a���܌��V���ٓ���N�l�����u��L瞑����m�T����������|����������⟵�y�ĉ����A�B�ݗI�������T�Q�ߛg��"
            //            .ToCharArray();
            includeFile = "kizuato.txt";
            fontTable = "�@���`�a�b�c�d�e�f�g�h�i�j�k�l�m�n�o�p�q�r�s�t�u�v�w�x�y�O�P�Q�R�S�T�U�V�W�X�����������������������������������ĂƂȂɂʂ˂̂͂Ђӂւق܂݂ނ߂�������������񂪂����������������������Âłǂ΂тԂׂڂς҂Ղ؂ۂ�����������������A�C�E�G�I�J�L�N�P�R�T�V�X�Z�\�^�`�c�e�g�i�j�k�l�m�n�q�t�w�z�}�~�����������������������������K�M�O�Q�S�U�W�Y�[�]�_�a�d�f�h�o�r�u�x�{�p�s�v�y�|�@�B�D�F�H�������b�����w�x�u�v�i�j�H�I�@�|�[�`�B�A�D�C�E�d�c���X�����������@�����������������������ÈĈňȈʈˈ͈̈ЈӈԈՈֈ׈وڈ݈߈�����������������������@�A�B�C�E�F�H�J�Q�R�Z�\�^�_�c�e�f�h�i�j�k�p�q�s�t�v�w�x�z�~�����������������������������������������������������������������������������ĉŉƉȉɉʉˉ͉̉Ή҉Ӊԉ׉؉ۉ܉݉߉�������������������������B�C�E�F�G�J�K�O�P�Q�S�W�X�_�d�e�i�j�k�l�m�n�o�p�r�s�u�v�w�y�z�{�|�������������������������������������������������ÊĊŊǊȊɊ̊ϊъԊՊ֊׊يۊ܊ߊ��������������������������@�A�C�F�G�K�L�M�N�P�Q�S�U�V�Y�Z�[�]�^�`�c�g�i�l�p�q�r�s�t�u�v�w�x�y�z�{�|�}�~�������������������������������������������������������������������������������������������ËƋǋȋɋʋ͋΋ϋ֋؋ًۋ݋ߋ��������������������@�C�F�I�J�N�Q�U�W�X�Y�Z�^�`�b�e�f�g�h�i�m�n�o�p�q�u�v�x�y�}���������������������������������������������������������������������������������������������ÌČŌǌȌɌˌ̌Ҍ֌ٌی܌݌ߌ������������������������A�C�D�H�I�K�L�N�Q�R�S�T�U�X�Z�\�a�b�c�d�e�g�i�j�k�l�m�q�r�s�u�v�w�x�|�~���������������������������������������������������������������������������ÍčōǍȍʍˍ΍ύЍӍՍ׍؍ٍڍۍݍލߍ����������������@�A�B�C�D�E�G�K�O�Q�R�S�U�Y�^�_�a�c�d�e�f�g�h�i�j�l�m�n�o�p�q�s�t�u�v�w�x�{�|�}�~�������������������������������������������������������������������������������������������ŎɎʎˎ͎̎ΎώЎҎӎԎՎ׎؎܎ގߎ���������������������������A�C�E�G�H�I�K�L�O�P�R�T�W�X�Z�[�\�]�_�`�a�b�c�d�e�f�h�k�n�o�p�q�t�u�������������������������������������������������������������������������ďŏƏǏȏɏˏ͏ΏϏЏՏ؏ڏۏ�����������������������@�A�B�D�E�F�G�H�J�K�L�M�N�O�Q�R�S�T�U�V�Z�[�\�^�_�c�e�f�g�h�i�j�k�l�n�o�q�u�s�w�{�}���������������������������������������������������������������������������������ÐĐŐƐȐʐ̐ΐϐАѐӐԐՐؐڐېܐݐߐ��������������������������A�D�F�I�M�N�O�P�R�S�U�^�_�a�c�f�g�h�i�j�n�o�r�s�t�w�z�{�|�}�~���������������������������������������������������������������������������������������������őʑ̑Αϑёґӑԑّܑ֑ݑޑ������������������@�B�D�E�J�N�P�Q�S�T�W�Z�[�a�c�d�e�f�g�i�j�k�l�m�n�p�r�s�u�v�x�z�{�|�����������������������������������������������������������������������������Òǒɒʒ͒גܒޒߒ��������������������@�B�D�E�G�H�I�K�M�O�P�S�T�V�W�X�Y�Z�\�]�_�`�c�d�f�g�h�i�k�n�o�q�r�s�w�x�y�z�{�|�������������������������������������������������������������������������œƓǓ˓͓דؓۓݓߓ�������������������A�C�E�F�G�L�M�N�O�P�R�S�V�Y�Z�[�\�]�`�c�d�g�h�j�n�p�q�r�s�t�w�x�y�z�{�~�����������������������������������������������������ŔƔɔʔ͔ӔԔՔڔ۔ޔߔ�����������������������@�C�F�G�I�K�P�S�W�X�Y�\�]�`�a�b�c�i�n�p�q�r�s�t�v�w�x�z�|�}�~�������������������������������������������������������������������������������ĕǕȕʕ̕ϕЕҕӕԕ֕וٕەߕ��������������������@�A�C�D�E�F�J�K�L�M�O�R�S�T�V�W�Y�Z�[�\�]�^�_�`�b�c�e�h�j�k�l�o�u�v�{�|�}�����������������������������������������������������������������������Ŗʖ͖ϖіҖԖՖٖؖږߖ�����������������������@�B�D�E�F�H�L�R�S�T�U�V�X�Y�[�\�]�^�_�a�c�d�e�g�h�j�l�m�n�p�t�v�x�y�z�{�}�~���������������������������������������������������������������������������×Ǘʗ˗̗͗Ηї֗חܗݗޗߗ�������������������A�F�H�I�J�L�Q�R�V�Y�Z�^�_�a�b�c�e�f�g�h�r����b�j�k�l�q�}���������ɝC�X�����B�ӟ����z���������Y���S�\��b���x���M�Չ��R�ʊӋT���򌡌ՍJ�o�����M��D��������X�k�đؒK�o�e��l���������ѕR���������Õ˖p�w���Ö��U�ꗒ���C�M�o�q��R���j�p��g�B�T������N���O�S�[���Ǝ�ОH�؈މ�����U�����H�T���I�Ր��u�^�q�Г���e�����㖗�i�]������B�K�X�����N�O�����J�r�a���܌��V���ٓ���N�l�����u��L瞑����m�T����������|����������⟵�y�ĉ����A�B�ݗI�������T�Q�ߛg��"
                        .ToCharArray();

            // �t���O�e�[�u���̍쐬
            // ���[�J���t���O
            flagTable.Add(0x00, 0);     // 00:���Ɖ�b����
            flagTable.Add(0x01, 1);     // 01:�������Ɛl�ł͂Ȃ�
            flagTable.Add(0x02, 2);     // 02:��߂�����^��
            flagTable.Add(0x03, 3);     // 03:���Ǝ�������ɂނ�����
            flagTable.Add(0x04, 4);     // 04:���V�i���I�x�@�ɓd�b
            flagTable.Add(0x05, 5);     // 05:�O���r�A�𔃂���
            flagTable.Add(0x06, 6);     // 06:���𔃂���
            flagTable.Add(0x07, 7);     // 07:�������������킪�点�Ȃ�
            flagTable.Add(0x08, 8);     // 08:���܂�����킽����
            flagTable.Add(0x09, 9);     // 09:���g�p
            flagTable.Add(0x0a, 10);    // 0a:�G���f�B���O��t���O����p
            flagTable.Add(0x0b, 11);    // 0b:�G���f�B���O��t���O����p
            flagTable.Add(0x0c, 12);    // 0c:�G���f�B���O��t���O����p
            flagTable.Add(0x0d, 13);    // 0d:�G���f�B���O��t���O����p
            flagTable.Add(0x0e, 14);    // 0e:�G���f�B���O��t���O����p
            flagTable.Add(0x0f, 15);    // 0f:�G���f�B���O��t���O����p
            flagTable.Add(0x10, 16);    // 10:�G���f�B���O��t���O����p
            flagTable.Add(0x11, 17);    // 11:�G���f�B���O��t���O����p
            flagTable.Add(0x12, 18);    // 12:�G���f�B���O��t���O����p
            flagTable.Add(0x13, 19);    // 13:�G���f�B���O��t���O����p

            // �O���[�o���t���O
            flagTable.Add(0x14, 200);   // 14: ���BAD�G���h������
            flagTable.Add(0x15, 201);   // 15: ��߃G���f�B���O������
            flagTable.Add(0x16, 202);   // 16: �� Happy ������
            flagTable.Add(0x17, 203);   // 17: �� BAD ������
            flagTable.Add(0x18, 204);   // 18: �� HAPPY ������ 
            flagTable.Add(0x19, 205);   // 19: ����G���h������
            flagTable.Add(0x1a, 206);   // 1a: �����G���h������
            flagTable.Add(0x73, 207);   // 73: �S�G���f�B���O���������ǂ���
            flagTable.Add(0x74, 208);   // 74: �H�H�H
        }

        // �V�i���I�f�R�[�_
        protected override LeNSConvResult decodeEvent(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData)
        {
            int scn_no = int.Parse(srcInfo.Name.Substring(0, 3));

            // �T�C�Y�Ƃ��u���b�N���Ƃ������Ă�̂�
            // �ꗥ12�o�C�g�߂���X�^�[�g
            int p = 11;

            StringBuilder str = new StringBuilder();
            str.AppendFormat("; ---- {0} START -----------------------------------------------------------\r\n", srcInfo.Name);
            str.AppendFormat("*scn{0}\r\n\r\n", scn_no.ToString("000"));

            while (p < scnData.Length)
            {
                // ���x���L���b�V�������݂���ꍇ�̓��x�����o��
                if (labelCache.ContainsKey(getLabelString(scn_no, p)))
                    str.AppendLine(getLabelString(scn_no, p) + "\r\n");

                switch (scnData[p])
                {
                    case 0x20:  // �I��
                        str.AppendLine("reset");
                        p++;
                        break;
                    case 0x23:
                        //str.AppendFormat("; 0x23:����`���߁H {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x24:  // �W�����v
                        // �W�����v�u���b�N�̎w��͏��1�Ȃ̂ŁA�t�@�C���擪�łn�j
                        str.AppendFormat("goto *scn{0}\r\n\r\n", scnData[p + 1].ToString("000"));
                        p += 3;
                        break;
                    case 0x25:  // �I����
                        parseText(txtData, scnData[p + 1], str);

                        str.AppendLine("br");
                        str.AppendLine("select");
                        for (int i = 0; i < scnData[p + 2]; i++)
                        {
                            // �e�L�X�g�f�[�^�Ăяo��(�����ȊO�̃R�}���h�𖳎�)
                            str.Append("\"");
                            parseText(txtData, scnData[p + 3 + (i * 2)], str, true);
                            str.Append("\", ");

                            // �W�����v��̃A�h���X�����߂ă��x���𐶐�
                            String label = getLabelString(scn_no, p + 3 + (scnData[p + 2] * 2) + (scnData[p + 4 + (i * 2)]));
                            str.Append(label);

                            // ���x���L���b�V����ǉ�
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }

                            if (i + 1 < scnData[p + 2]) { str.Append(","); }
                            str.AppendLine("");
                        }
                        str.AppendLine("");

                        p += 3 + (scnData[p + 2] * 2);
                        break;
                    case 0x27:
                        //str.AppendLine("0x27:�O�̑I�����ɖ߂��ԕۑ�");
                        p++;
                        break;
                    case 0x41:  // ����(��)
                        {
                            // �W�����v��̃A�h���X�����߂ă��x���𐶐�
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}=={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // ���x���L���b�V����ǉ�
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x42:  // ����(��)
                        {
                            // �W�����v��̃A�h���X�����߂ă��x���𐶐�
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}!={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // ���x���L���b�V����ǉ�
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x4b:  // �t���O�Z�b�g
                        str.AppendFormat("mov %{0}, {1}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2]);
                        p += 3;
                        break;
                    case 0x51:  // �e�L�X�g�o��
                        parseText(txtData, scnData[p + 1], str);
                        p += 2;
                        break;
                    case 0x52:
                        //str.AppendLine("0x52:�I�����ɕt�����閽��");
                        p++;
                        break;
                    case 0x94:
                        //str.AppendLine("0x94:�G���f�B���O�ɕt�����閽��");
                        p++;
                        break;
                    case 0x95:
                        //str.AppendFormat("0x95:�G���f�B���O�a�f�l�w�� {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x96:
                        //str.AppendFormat("0x96:���B�G���f�B���O�̎w�� {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    default:
                        parseCommand(scnData, ref p, str);
                        break;
                }
            }
            // �X�N���v�g�I�[�ɓ��B
            str.AppendFormat("\r\n; ---- {0} END -------------------------------------------------------------\r\n\r\n", srcInfo.Name);

            writeFile(srcInfo, str);
            return LeNSConvResult.ok;
        }

        protected void parseText(byte[] txtData, int no, StringBuilder str)
        {
            parseText(txtData, no, str, false);
        }

        // �e�L�X�g�o��
        protected void parseText(byte[] txtData, int no, StringBuilder str ,Boolean onSelect)
        {
            int p = getTextAddress(txtData, no);
            while (p < txtData.Length)
            {
                if (txtData[p] >= 0 && txtData[p] <= 0x20)
                {
                    int code = (txtData[p] << 8) | txtData[p + 1];
                    if (code < fontTable.Length && code >= 0)
                    {
                        if (!onSelect || code != 0)
                        {
                            str.Append(fontTable[code]);
                            onTextBr = false;
                        }
                    }
                    p += 2;
                }
                else
                {
                    if (parseCommand(txtData, ref p, str, true, onSelect))
                    {
                        return;
                    }
                }
            }

            return;
        }

        private int getTextAddress(byte[] txtData, int no)
        {
            return getShort(txtData, (no + 1) * 2);
        }

        // �ėp�R�}���h���s
        protected Boolean parseCommand(byte[] data, ref int p, StringBuilder str, Boolean onText, Boolean onSelect)
        {
            // �e�L�X�g�\�����ɖ��ߕ��������Ƃ��A"/"��}�����ĉ��s����
            if (!onSelect && onText && !onTextBr)
            {
                switch (data[p])
                {
                    case 0x38:
                    case 0x60:
                    case 0x61:
                    case 0x62:
                    case 0x63:
                    case 0x64:
                    case 0x65:
                    case 0x66:
                    case 0x67:
                    case 0x68:
                    case 0x69:
                    case 0x6a:
                    case 0x6b:
                    case 0x6c:
                    case 0x81:
                    case 0x84:
                    case 0xb6:
                    case 0xb7:
                    case 0xb9:
                    case 0xc8:
                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                    case 0xcc:
                    case 0xcd:
                    // ���͎�������������Ȃ��ƃ_��
                    // �ȉ��̖��߂͉������Ȃ�(���ߕ��𐶐����Ȃ��R�}���h)
                    case 0xaf: case 0xb0: case 0xb2: case 0xb3: case 0xff: case 0xa6:
                        break;
                    default:
                        str.Append("/\r\n");
                        onTextBr = true;
                        break;
                }
            }

            switch (data[p])
            {
                case 0x30:  // ���������w�i���[�h
                    str.Append(getBGCommand(data[p + 1], 0));
                    p += 2;
                    break;
                case 0x31:
                    str.AppendFormat("; 0x31:�G�t�F�N�g�֘A�H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x34:
                    str.AppendFormat("; 0x34:��ʏ����H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x35:
                    str.AppendFormat("; 0x35:�G�t�F�N�g�֘A�H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x36:  // ��������Visual���[�h
                    str.Append(getPictureCommand("V", data[p + 1], 0));
                    p += 2;
                    break;
                case 0x37:  // ���������L�����N�^���[�h
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    p += 3;
                    break;
                case 0x38:
                    //str.AppendFormat("; 0x38:���ڕ\������ {0}, {1}\n", data[p + 1], data[p + 2]);
                    // �G�t�F�N�g��IN/OUT�H
                    str.AppendFormat("print 2\r\n");
                    p += 3;
                    break;
                case 0x39:
                    str.AppendFormat("; 0x39:�\������ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3a:
                    str.AppendFormat("; 0x3a:�p���b�g�ꎞ�ύX�w��(�������f) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3b:
                    str.AppendFormat("; 0x3b:�\�����p���b�g�ύX�w��(�������f�͂��Ȃ�) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3c:
                    str.AppendFormat("; 0x3c:�F���]�w�� {0}\n", data[p + 1]);
                    p += 2;
                    break;
                // ���������߃YSTART
                case 0x60:
                    //str.AppendLine("0x60:�� �p���b�g���f�H");
                    p++;
                    break;
                case 0x61:
                    //str.AppendLine("0x61:�� �p���b�g��0�ɁH");
                    p++;
                    break;
                case 0x62:
                    //str.AppendFormat("0x62:�� �G�t�F�N�g�w��H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x63:
                    //str.AppendFormat("0x63:�� �G�t�F�N�g�w��H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x64:
                    //str.AppendLine("0x64:�� �G�t�F�N�g�֘A�H");
                    p++;
                    break;
                case 0x65:
                    //str.AppendLine("0x65:�� �G�t�F�N�g�֘A�H");
                    p++;
                    break;
                case 0x66:
                    //str.AppendLine("0x66:�� �G�t�F�N�g�֘A�H");
                    p++;
                    break;
                case 0x67:
                    //str.AppendFormat("0x67:�� �G�t�F�N�g�w��H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x68:
                    //str.AppendFormat("0x68:�� �G�t�F�N�g�w��H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x69:
                    //str.AppendLine("; 0x69:�t�F�[�h�C���H");
                    p++;
                    break;
                case 0x6a:
                    //str.AppendLine("; 0x6a:�t�F�[�h�A�E�g�H");
                    p++;
                    break;
                // ���������߃YEND
                case 0x6b:
                    //str.AppendFormat("0x6b:�� �e�L�X�g�G�t�F�N�g�֘A�H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x6c:
                    //str.AppendFormat("0x6c:�� �e�L�X�g�G�t�F�N�g�֘A�H {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x80:  // �a�f�l�J�n
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.Play));

                    p += 2;
                    break;
                case 0x81:
                    // str.AppendLine("; 0x81:�a�f�l�t�F�[�h");
                    // str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    // ���ɂa�f�l�������Ƃ��t�F�[�h���鏀���H
                    // �X�N���v�g�k��Ȃ��̂ŁA��������Ȃ玩�O�ŃX�N���v�g�g�ނ����c�B
                    // ���ʂȂ��Ă������Ă���ۂ��B

                    p++;
                    break;
                case 0x82:  // �a�f�l��~
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.Stop));

                    p++;
                    break;
                case 0x84:
                    // str.AppendFormat("; 0x84:���V�[���̂a�f�l�J�n {0}\n", data[p + 1]);
                    // xlvns�ł̓R�����g�A�E�g�B�Ȃ��Ă������H
                    p += 2;
                    break;
                case 0x85:
                    // str.AppendLine("; 0x85:�a�f�l�t�F�[�h�҂�");
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    p++;
                    break;
                case 0x87:
                    // str.AppendFormat("; 0x87:�a�f�l�J�n(�t�F�[�h) {0}\n", data[p + 1]);
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.FadeStart));
                    p += 2;
                    break;
                case 0xa0:  // �o�b�l�ǂݍ���
                    // ���Ȃ݂�dwaveload�͍Đ��񐔂̊֌W�Ŏg���Ȃ��c�B
                    loadedPCM = data[p + 1];
                    p += 2;
                    break;
                case 0xa1:  // �Đ����̂o�b�l���~
                    str.Append("dwavestop 1\r\n");
                    p++;
                    break;
                case 0xa2:
                    // par1���Đ��񐔁Apar2�͍Đ�����
                    // str.AppendFormat("; 0xa2:�o�b�l�Đ� {0}, {1}\n", data[p + 1], data[p + 2]);

                    {
                        String se_file;
                        if (data[p + 2] <= 1)
                        {
                            se_file = String.Format("{0}KZ_VD{1:00}.WAV", LeNSMain.SoundPath, loadedPCM);
                        }
                        else
                        {
                            se_file = String.Format("{0}KZ_VD{1:00}_{2}.WAV", LeNSMain.SoundPath, data[p + 2], loadedPCM);
                        }

                        if (data[p + 1] == 0)
                        {
                            str.Append("chvol 1, 75\r\n");
                        }
                        else
                        {
                            str.AppendFormat("chvol 1, {0}\r\n", (int)(75 + (25 * data[p + 1] / 100)));
                        }

                        if (data[p + 2] == 0)
                        {
                            str.AppendFormat("dwaveloop 1, \"{0}\"\r\n", se_file);
                            waitPCM = SoundCache[String.Format("KZ_VD{0:00}.P16", loadedPCM)];
                        }
                        else
                        {
                            str.AppendFormat("dwave 1, \"{0}\"\r\n", se_file);
                            waitPCM = SoundCache[String.Format("KZ_VD{0:00}.P16", loadedPCM)] * data[p + 2];
                        }

                        // resettimer���Ē�~�҂��ōĐ����� - �^�C�}�[�����
                        // ��~�҂��ɂȂ�H �}�C�i�X���͈ꉞ���Ȃ������c�B
                        str.Append("resettimer\r\n");
                    }
                    p += 3;
                    break;
                case 0xa3:  // �o�b�l��~�҂�
                    str.Append("gettimer %199\r\n");
                    str.AppendFormat("delay {0} - %199\r\n", waitPCM);
                    p++;
                    break;
                case 0xa6:
                    // str.AppendLine("; 0xa6:�o�b�l�֘A�H");
                    p++;
                    break;
                case 0xaf:  // ���b�Z�[�W�I��
                    p++;
                    return true;
                case 0xb0:  // ���s
                    onTextBr = true;
                    if (!onSelect) str.AppendLine("");
                    p++;
                    break;
                case 0xb2:  // ���͑҂�
                    str.Append("@");
                    p++;
                    break;
                case 0xb3:  // �y�[�W�X�V�҂�
                    onTextBr = true;
                    str.AppendLine("\\\r\n");
                    p++;
                    break;
                case 0xb6:
                    if (!onSelect) {/*str.AppendFormat("0xb6:�����`�摬�x�w�� {0}(*10ms)\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xb7:
                    //str.AppendFormat("0xb7:���ԑ҂� {0}(*10ms)\n", data[p + 1]);
                    p += 2;
                    break;
                case 0xb9:
                    if (!onSelect) {/*str.AppendFormat("0xb9:�����`��I�t�Z�b�g�w�� {0}\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xbb:  // �t���b�V��
                    str.Append("lsp 0, \":c;graphics\\WHITE.PNG\", 0, 40\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    str.Append("wait 200\r\n");
                    str.Append("csp 0\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    p++;
                    break;
                case 0xbc:  // ��ʐU��
                    str.Append("chvol 1, 75\r\n");
                    str.AppendFormat("dwave 1, \"{0}KZ_VD03.WAV\"\r\n", LeNSMain.SoundPath);
                    str.Append("quake 4, 550\r\n");
                    p++;
                    break;
                case 0xbd:  // �ʏ�w�i���[�h
                case 0xbe:  // �w�i���[�h�H
                    //str.AppendFormat("; 0xbd:�ʏ�w�i���[�h {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    //str.AppendFormat("; 0xbe:�w�i���[�h�H {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // ����par2���h�m�Apar3���n�t�s�̃G�t�F�N�g�w��B
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 1], 2));
                    p += 4;
                    break;
                case 0xbf:
                    // str.AppendFormat("; 0xbf:�r�W���A���V�[�����[�h {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // ����par2���h�m�Apar3���n�t�s�̃G�t�F�N�g�w��B
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("V", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc0:
                    //str.AppendFormat("0xc0:�g�V�[�����[�h {0}, {1}, {2}n", data[p + 1], data[p + 2], data[p + 3]);
                    // ����par2���h�m�Apar3���n�t�s�̃G�t�F�N�g�w��B
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("H", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc1:  // �L�����N�^�ύX
                case 0xc2:  // �L�����N�^�\��
                    // �G�t�F�N�g�̓t�F�[�h�Œ�
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc3:  // �S�L����������A�L�����\��
                    // �G�t�F�N�g�̓t�F�[�h�Œ�
                    str.Append("cl a, 0\r\n");
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc4:
                    // str.AppendFormat("; 0xc4:�w�i�t���L�����N�^�\�� {0}, {1}, {2}, {3}, {4}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5]);
                    // ����par4���h�m�Apar5���n�t�s�̃G�t�F�N�g�w��B
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 6;
                    break;
                case 0xc6:
                    //str.AppendFormat("; 0xc6:�R�L���������\�� {0}, {1}, {2}, {3}, {4}, {5}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5], data[p + 6]);
                    // �G�t�F�N�g�̓t�F�[�h�Œ�
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.Append(getLDCommand(data[p + 4], data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 6], data[p + 5], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 7;
                    break;
                case 0xc8:
                    //str.AppendFormat("; 0xc8:���Ԃ���� {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xc9:
                    //str.AppendLine("; 0xc9:���Ԃ����");
                    p++;
                    break;
                case 0xca:
                    //str.AppendLine("; 0xca:�S�̒�");
                    p++;
                    break;
                case 0xcb:
                    //str.AppendLine("; 0xcb:���Ԃ����");
                    p++;
                    break;
                case 0xcc:
                    // str.AppendFormat("; 0xcc:���X�^�X�N���[���J�n {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xcd:
                    //str.AppendLine("; 0xcd:�����Ԃ�");
                    p++;
                    break;
                case 0xff:  // �X�N���v�g�I�[
                    p++;
                    break;
                default:
                    throw new InvalidDataException("�����Ȗ��߂ł��B[" + data[p].ToString() + "]");
            }

            return false;
        }

        protected Boolean parseCommand(byte[] data, ref int p, StringBuilder str, Boolean onText)
        {
            return parseCommand(data, ref p, str, onText, false);
        }

        protected Boolean parseCommand(byte[] data, ref int p, StringBuilder str)
        {
            return parseCommand(data, ref p, str, false, false);
        }

        protected String getPositionString(byte pos)
        {
            switch (pos)
            {
                case 0:
                    return "l";
                case 2:
                    return "c";
                case 1:
                default:
                    return "r";
            }
        }

        protected String getBGCommand(byte no, byte effect)
        {
            StringBuilder str = new StringBuilder();
            String[] bg_prefix = {"", "B", "C", "D", "", "E"};

            // ��p���b�g�̃t���O���Z�b�g
            if ((int)(no / 50) != 2)
            {
                str.Append("mov %100, 0\r\n");
            }
            else
            {
                str.Append("mov %100, 1\r\n");
            }

            if (no % 50 != 0x00)
            {
                str.AppendFormat("bg \"{0}S{1:00}{2}.PNG\", {3}\r\n", LeNSMain.GraphicPath, no % 50, bg_prefix[(int)(no / 50)], effect);
            }
            else
            {
                str.AppendFormat("bg black, {0}\r\n", effect);
            }

            return str.ToString();
        }

        protected String getLDCommand(byte no, byte pos, byte effect)
        {
            StringBuilder str = new StringBuilder();

            if (no != 0xff)
            {
                // ���󂪖�̂Ƃ��͖�p�����G��\��
                str.AppendFormat("mov $101, \":a;{0}C{1:X2}\"\r\n", LeNSMain.GraphicPath, no);
                str.Append("if %100 == 1 add $101, \"N\"\r\n");
                str.Append("add $101, \".PNG\"\r\n");
                str.AppendFormat("ld {0}, $101, {1}\r\n", getPositionString(pos), effect);
            }
            else
            {
                str.AppendFormat("cl {0}, 2\r\n", getPositionString(pos));
            }

            return str.ToString();
        }

        protected String getPictureCommand(String prefix, byte no, byte effect)
        {
            return String.Format("bg \"{0}{1}{2:00}.PNG\", {3}\r\n", LeNSMain.GraphicPath, prefix, no, effect);
        }
    }
}
