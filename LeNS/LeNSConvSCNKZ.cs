using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// sncn
// Eno^dc^S
// EøÊ¹
// EGtFNgnÀ

namespace LeNS
{
    class LeNSConvSCNKZ : LeNSConvSCN
    {
        // [hÌPCMîñÛp
        protected byte loadedPCM = 0xff;    // [hÌobl
        protected int waitPCM = 0;          // [hoblÌÄ¶Ô

        protected Boolean onTextBr = true;  // ¼OÉüs³êÄé©

        public LeNSConvSCNKZ(LeNSConvOption Option)
            : base(Option)
        {
            // tHge[uÌì¬
            // ¦ê§ä¶ÉñÎBcrÍSpXy[XA!?ÍIÉÏ·³êÜ·B
            //fontTable = "@¡`abcdefghijklmnopqrstuvwxyOPQRSTUVWX ¢¤¦¨©«­¯±³µ·¹»½¿ÂÄÆÈÉÊËÌÍÐÓÖÙÜÝÞßàâäæçèéêëíðñª¬®°²´¶¸º¼¾ÀÃÅÇÎÑÔ×ÚÏÒÕØÛ¡£¥§áãåÁ¡ACEGIJLNPRTVXZ\^`cegijklmnqtwz}~KMOQSUWY[]_adfhorux{psvy|@BDFHbwxuvijHI!?|[`BADCEdcX¹ª«cr£¤¥§«¬³µ¶¹ÀÃÄÅÈÊËÌÍÐÓÔÕÖ×ÙÚÝßáâãäæçêìíðóôõöøùú@ABCEFHJQRZ\^_cefhijkpqstvwxz~¡£¤©­®¯°´¶·¸¹º»¼½¿ÁÂÄÅÆÈÉÊËÌÍÎÒÓÔ×ØÛÜÝßàäæèìîïðñòóõö÷ùúüBCEFGJKOPQSWX_deijklmnoprsuvwyz{|£¦§¨©ª­®¯±³´µ·¸½¾¿ÃÄÅÇÈÉÌÏÑÔÕÖ×ÙÛÜßáâæçèéëìíîïðñóôõö÷ùú@ACFGKLMNPQSUVYZ[]^`cgilpqrstuvwxyz{|}~£¤¥¦©«­¯°²³´µ¶·¹º»½¾¿ÁÂÃÆÇÈÉÊÍÎÏÖØÙÛÝßàâãåæêëìïðóôö÷ü@CFIJNQUWXYZ^`befghimnopquvxy} ¢¤¦§¨©«¬­®¯±³´µ¶¸¹º»¾ÀÂÃÄÅÇÈÉËÌÒÖÙÛÜÝßãäåêëìîðóõö÷øúûüACDHIKLNQRSTUXZ\abcdegijklmqrsuvwx|~ ¡¢¦§¨ª¬°­²¶·¸¹»½¾ÀÃÄÅÇÈÊËÎÏÐÓÕ×ØÙÚÛÝÞßàçìíðôõö÷û@ABCDEGKOQRSUY^_acdefghijlmnopqstuvwx{|}~¡¥¦¨©«­®¯´µ¶·¸¹º¼¾¿ÀÅÉÊËÌÍÎÏÐÒÓÔÕ×ØÜÞßáâãåæçèêëìíïðñóôö÷ùûüACEGHIKLOPRTWXZ[\]_`abcdefhknopqtu¤¥§«¬­°³µ¶¸»ÀÁÂÄÅÆÇÈÉËÍÎÏÐÕØÚÛàáãäæçèéêìíîðñòóô÷ü@ABDEFGHJKLMNOQRSTUVZ[\^_cefghijklnoqusw{}¡¢£¥¦§¨©ª«¬¯°³´µ¶·¸º»¼½¾¿ÂÃÄÅÆÈÊÌÎÏÐÑÓÔÕØÚÛÜÝßàáâãæçéêëìíîðòóôõöùüADFIMNOPRSU^_acfghijnorstwz{|}~ ¢£¤¥¦§©ª«¬®¯°±²³µ¶¸¹º¼½¾¿ÁÅÊÌÎÏÑÒÓÔÖÙÜÝÞàãäåæèìîðòóõ÷ø@BDEJNPQSTWZ[acdefgijklmnprsuvxz{| £¤¥©ª¬­®°²´µ·¸¹¼¾¿ÁÂÃÇÉÊÍ×ÜÞßàáâåæèêëíïñö÷ùú@BDEGHIKMOPSTVWXYZ\]_`cdfghiknoqrswxyz{|¡¢¤¥¦§©ª¬­®¯°±²µ·¹¾¿ÁÅÆÇËÍ×ØÛÝßàäéîïñõö÷úûüACEFGLMNOPRSVYZ[\]`cdghjnpqrstwxyz{~ §ª­¯°²µº»¼½ÂÅÆÉÊÍÓÔÕÚÛÞßàâäåæçéíïðñòõö÷ûü@CFGIKPSWXY\]`abcinpqrstvwxz|}~ ¡¢£¥¦§¨ª®±´µ¶·¹º½¿ÀÂÄÇÈÊÌÏÐÒÓÔÖ×ÙÛßàâåæçéêïðñóöøúû@ACDEFJKLMORSTVWYZ[\]^_`bcehjklouv{|}¡¢£§¨¬­¯°±²³¶º¼½¾ÀÁÂÅÊÍÏÑÒÔÕØÙÚßâãäåçéìîïðñòóôöùûü@BDEFHLRSTUVXY[\]^_acdeghjlmnptvxyz{}~ ¢£¤¥¦§ª¬­¯±²³´¶·¸»¼½¿ÁÂÃÇÊËÌÍÎÑÖ×ÜÝÞßáâãçéêìíîðñòóôö÷AFHIJLQRVYZ^_abcefghrêïòbjklq}ø±ÉCX¹BÓøàÎázáááá¿áÉáÙãYã©ãµäSä\ä©æbæùéx²MÕåíRÊÓTò¡ÕJo±MéDÀÁäXkÄØKoeçl¢«´ÑR¬»¾ÃËpwµÃ×éUê©CMoqëR½jpðgBT¿á°ã÷äNæÒçOçSç[èõÆäÐHØÞ¾áûU¬HTùIèÕu^qÐ´êeÁãi]áôBKX³NOàøáÆãJärça¢ÜV±ÙãùNlàuå¿çLçmT¥¦àôú|ø·î¨âµàyãÄ«ABÝIàßàææÃçTQßgâ"
            //            .ToCharArray();
            includeFile = "kizuato.txt";
            fontTable = "@¡`abcdefghijklmnopqrstuvwxyOPQRSTUVWX ¢¤¦¨©«­¯±³µ·¹»½¿ÂÄÆÈÉÊËÌÍÐÓÖÙÜÝÞßàâäæçèéêëíðñª¬®°²´¶¸º¼¾ÀÃÅÇÎÑÔ×ÚÏÒÕØÛ¡£¥§áãåÁ¡ACEGIJLNPRTVXZ\^`cegijklmnqtwz}~KMOQSUWY[]_adfhorux{psvy|@BDFHbwxuvijHI@|[`BADCEdcX¹ª«@£¤¥§«¬³µ¶¹ÀÃÄÅÈÊËÌÍÐÓÔÕÖ×ÙÚÝßáâãäæçêìíðóôõöøùú@ABCEFHJQRZ\^_cefhijkpqstvwxz~¡£¤©­®¯°´¶·¸¹º»¼½¿ÁÂÄÅÆÈÉÊËÌÍÎÒÓÔ×ØÛÜÝßàäæèìîïðñòóõö÷ùúüBCEFGJKOPQSWX_deijklmnoprsuvwyz{|£¦§¨©ª­®¯±³´µ·¸½¾¿ÃÄÅÇÈÉÌÏÑÔÕÖ×ÙÛÜßáâæçèéëìíîïðñóôõö÷ùú@ACFGKLMNPQSUVYZ[]^`cgilpqrstuvwxyz{|}~£¤¥¦©«­¯°²³´µ¶·¹º»½¾¿ÁÂÃÆÇÈÉÊÍÎÏÖØÙÛÝßàâãåæêëìïðóôö÷ü@CFIJNQUWXYZ^`befghimnopquvxy} ¢¤¦§¨©«¬­®¯±³´µ¶¸¹º»¾ÀÂÃÄÅÇÈÉËÌÒÖÙÛÜÝßãäåêëìîðóõö÷øúûüACDHIKLNQRSTUXZ\abcdegijklmqrsuvwx|~ ¡¢¦§¨ª¬°­²¶·¸¹»½¾ÀÃÄÅÇÈÊËÎÏÐÓÕ×ØÙÚÛÝÞßàçìíðôõö÷û@ABCDEGKOQRSUY^_acdefghijlmnopqstuvwx{|}~¡¥¦¨©«­®¯´µ¶·¸¹º¼¾¿ÀÅÉÊËÌÍÎÏÐÒÓÔÕ×ØÜÞßáâãåæçèêëìíïðñóôö÷ùûüACEGHIKLOPRTWXZ[\]_`abcdefhknopqtu¤¥§«¬­°³µ¶¸»ÀÁÂÄÅÆÇÈÉËÍÎÏÐÕØÚÛàáãäæçèéêìíîðñòóô÷ü@ABDEFGHJKLMNOQRSTUVZ[\^_cefghijklnoqusw{}¡¢£¥¦§¨©ª«¬¯°³´µ¶·¸º»¼½¾¿ÂÃÄÅÆÈÊÌÎÏÐÑÓÔÕØÚÛÜÝßàáâãæçéêëìíîðòóôõöùüADFIMNOPRSU^_acfghijnorstwz{|}~ ¢£¤¥¦§©ª«¬®¯°±²³µ¶¸¹º¼½¾¿ÁÅÊÌÎÏÑÒÓÔÖÙÜÝÞàãäåæèìîðòóõ÷ø@BDEJNPQSTWZ[acdefgijklmnprsuvxz{| £¤¥©ª¬­®°²´µ·¸¹¼¾¿ÁÂÃÇÉÊÍ×ÜÞßàáâåæèêëíïñö÷ùú@BDEGHIKMOPSTVWXYZ\]_`cdfghiknoqrswxyz{|¡¢¤¥¦§©ª¬­®¯°±²µ·¹¾¿ÁÅÆÇËÍ×ØÛÝßàäéîïñõö÷úûüACEFGLMNOPRSVYZ[\]`cdghjnpqrstwxyz{~ §ª­¯°²µº»¼½ÂÅÆÉÊÍÓÔÕÚÛÞßàâäåæçéíïðñòõö÷ûü@CFGIKPSWXY\]`abcinpqrstvwxz|}~ ¡¢£¥¦§¨ª®±´µ¶·¹º½¿ÀÂÄÇÈÊÌÏÐÒÓÔÖ×ÙÛßàâåæçéêïðñóöøúû@ACDEFJKLMORSTVWYZ[\]^_`bcehjklouv{|}¡¢£§¨¬­¯°±²³¶º¼½¾ÀÁÂÅÊÍÏÑÒÔÕØÙÚßâãäåçéìîïðñòóôöùûü@BDEFHLRSTUVXY[\]^_acdeghjlmnptvxyz{}~ ¢£¤¥¦§ª¬­¯±²³´¶·¸»¼½¿ÁÂÃÇÊËÌÍÎÑÖ×ÜÝÞßáâãçéêìíîðñòóôö÷AFHIJLQRVYZ^_abcefghrêïòbjklq}ø±ÉCX¹BÓøàÎázáááá¿áÉáÙãYã©ãµäSä\ä©æbæùéx²MÕåíRÊÓTò¡ÕJo±MéDÀÁäXkÄØKoeçl¢«´ÑR¬»¾ÃËpwµÃ×éUê©CMoqëR½jpðgBT¿á°ã÷äNæÒçOçSç[èõÆäÐHØÞ¾áûU¬HTùIèÕu^qÐ´êeÁãi]áôBKX³NOàøáÆãJärça¢ÜV±ÙãùNlàuå¿çLçmT¥¦àôú|ø·î¨âµàyãÄ«ABÝIàßàææÃçTQßgâ"
                        .ToCharArray();

            // tOe[uÌì¬
            // [JtO
            flagTable.Add(0x00, 0);     // 00:Æïbµ½
            flagTable.Add(0x01, 1);     // 01:©ªªÆlÅÍÈ¢
            flagTable.Add(0x02, 2);     // 02:çß³ñð^¤
            flagTable.Add(0x03, 3);     // 03:²Æ»êÉÞ©Á½
            flagTable.Add(0x04, 4);     // 04:²ViIx@Édb
            flagTable.Add(0x05, 5);     // 05:OrAðÁ½
            flagTable.Add(0x06, 6);     // 06:´ðÁ½
            flagTable.Add(0x07, 7);     // 07:¹¿áñð±íªç¹È¢
            flagTable.Add(0x08, 8);     // 08:¨Üàèðí½µ½
            flagTable.Add(0x09, 9);     // 09:¢gp
            flagTable.Add(0x0a, 10);    // 0a:GfBOãtO§äp
            flagTable.Add(0x0b, 11);    // 0b:GfBOãtO§äp
            flagTable.Add(0x0c, 12);    // 0c:GfBOãtO§äp
            flagTable.Add(0x0d, 13);    // 0d:GfBOãtO§äp
            flagTable.Add(0x0e, 14);    // 0e:GfBOãtO§äp
            flagTable.Add(0x0f, 15);    // 0f:GfBOãtO§äp
            flagTable.Add(0x10, 16);    // 10:GfBOãtO§äp
            flagTable.Add(0x11, 17);    // 11:GfBOãtO§äp
            flagTable.Add(0x12, 18);    // 12:GfBOãtO§äp
            flagTable.Add(0x13, 19);    // 13:GfBOãtO§äp

            // O[otO
            flagTable.Add(0x14, 200);   // 14: çßBADGhð©½
            flagTable.Add(0x15, 201);   // 15: çßGfBOð©½
            flagTable.Add(0x16, 202);   // 16: ² Happy ð©½
            flagTable.Add(0x17, 203);   // 17:  BAD ð©½
            flagTable.Add(0x18, 204);   // 18:  HAPPY ð©½ 
            flagTable.Add(0x19, 205);   // 19: öìGhð©½
            flagTable.Add(0x1a, 206);   // 1a: ¹Ghð©½
            flagTable.Add(0x73, 207);   // 73: SGfBOð©½©Ç¤©
            flagTable.Add(0x74, 208);   // 74: HHH
        }

        // ViIfR[_
        protected override LeNSConvResult decodeEvent(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData)
        {
            int scn_no = int.Parse(srcInfo.Name.Substring(0, 3));

            // TCYÆ©ubNîñÆ©üÁÄéÌÅ
            // ê¥12oCgß©çX^[g
            int p = 11;

            StringBuilder str = new StringBuilder();
            str.AppendFormat("; ---- {0} START -----------------------------------------------------------\r\n", srcInfo.Name);
            str.AppendFormat("*scn{0}\r\n\r\n", scn_no.ToString("000"));

            while (p < scnData.Length)
            {
                // xLbVª¶Ý·éêÍxðoÍ
                if (labelCache.ContainsKey(getLabelString(scn_no, p)))
                    str.AppendLine(getLabelString(scn_no, p) + "\r\n");

                switch (scnData[p])
                {
                    case 0x20:  // I¹
                        str.AppendLine("reset");
                        p++;
                        break;
                    case 0x23:
                        //str.AppendFormat("; 0x23:¢è`½ßH {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x24:  // Wv
                        // WvubNÌwèÍíÉ1ÈÌÅAt@CæªÅnj
                        str.AppendFormat("goto *scn{0}\r\n\r\n", scnData[p + 1].ToString("000"));
                        p += 3;
                        break;
                    case 0x25:  // Ið
                        parseText(txtData, scnData[p + 1], str);

                        str.AppendLine("br");
                        str.AppendLine("select");
                        for (int i = 0; i < scnData[p + 2]; i++)
                        {
                            // eLXgf[^ÄÑoµ(¶ÈOÌR}hð³)
                            str.Append("\"");
                            parseText(txtData, scnData[p + 3 + (i * 2)], str, true);
                            str.Append("\", ");

                            // WvæÌAhXðßÄxð¶¬
                            String label = getLabelString(scn_no, p + 3 + (scnData[p + 2] * 2) + (scnData[p + 4 + (i * 2)]));
                            str.Append(label);

                            // xLbVðÇÁ
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }

                            if (i + 1 < scnData[p + 2]) { str.Append(","); }
                            str.AppendLine("");
                        }
                        str.AppendLine("");

                        p += 3 + (scnData[p + 2] * 2);
                        break;
                    case 0x27:
                        //str.AppendLine("0x27:OÌIðÉßéóÔÛ¶");
                        p++;
                        break;
                    case 0x41:  // ªò(³)
                        {
                            // WvæÌAhXðßÄxð¶¬
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}=={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // xLbVðÇÁ
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x42:  // ªò(Û)
                        {
                            // WvæÌAhXðßÄxð¶¬
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}!={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // xLbVðÇÁ
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x4b:  // tOZbg
                        str.AppendFormat("mov %{0}, {1}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2]);
                        p += 3;
                        break;
                    case 0x51:  // eLXgoÍ
                        parseText(txtData, scnData[p + 1], str);
                        p += 2;
                        break;
                    case 0x52:
                        //str.AppendLine("0x52:IðÉt®·é½ß");
                        p++;
                        break;
                    case 0x94:
                        //str.AppendLine("0x94:GfBOÉt®·é½ß");
                        p++;
                        break;
                    case 0x95:
                        //str.AppendFormat("0x95:GfBOaflwè {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x96:
                        //str.AppendFormat("0x96:BGfBOÌwè {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    default:
                        parseCommand(scnData, ref p, str);
                        break;
                }
            }
            // XNvgI[ÉB
            str.AppendFormat("\r\n; ---- {0} END -------------------------------------------------------------\r\n\r\n", srcInfo.Name);

            writeFile(srcInfo, str);
            return LeNSConvResult.ok;
        }

        protected void parseText(byte[] txtData, int no, StringBuilder str)
        {
            parseText(txtData, no, str, false);
        }

        // eLXgoÍ
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

        // ÄpR}hÀs
        protected Boolean parseCommand(byte[] data, ref int p, StringBuilder str, Boolean onText, Boolean onSelect)
        {
            // eLXg\¦É½ß¶ª½Æ«A"/"ð}üµÄüs·é
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
                    // ªÍÀµ½çÁ³È¢Æ_
                    // ÈºÌ½ßÍ½àµÈ¢(½ß¶ð¶¬µÈ¢R}h)
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
                case 0x30:  // ³µwi[h
                    str.Append(getBGCommand(data[p + 1], 0));
                    p += 2;
                    break;
                case 0x31:
                    str.AppendFormat("; 0x31:GtFNgÖAH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x34:
                    str.AppendFormat("; 0x34:æÊÁH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x35:
                    str.AppendFormat("; 0x35:GtFNgÖAH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x36:  // ³µVisual[h
                    str.Append(getPictureCommand("V", data[p + 1], 0));
                    p += 2;
                    break;
                case 0x37:  // ³µLN^[h
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    p += 3;
                    break;
                case 0x38:
                    //str.AppendFormat("; 0x38:¼Ú\¦ {0}, {1}\n", data[p + 1], data[p + 2]);
                    // GtFNgÌIN/OUTH
                    str.AppendFormat("print 2\r\n");
                    p += 3;
                    break;
                case 0x39:
                    str.AppendFormat("; 0x39:\¦ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3a:
                    str.AppendFormat("; 0x3a:pbgêÏXwè(¦½f) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3b:
                    str.AppendFormat("; 0x3b:\¦pbgÏXwè(¦½fÍµÈ¢) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3c:
                    str.AppendFormat("; 0x3c:F½]wè {0}\n", data[p + 1]);
                    p += 2;
                    break;
                // ¢½ßYSTART
                case 0x60:
                    //str.AppendLine("0x60:ä pbg½fH");
                    p++;
                    break;
                case 0x61:
                    //str.AppendLine("0x61:ä pbgð0ÉH");
                    p++;
                    break;
                case 0x62:
                    //str.AppendFormat("0x62:ä GtFNgwèH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x63:
                    //str.AppendFormat("0x63:ä GtFNgwèH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x64:
                    //str.AppendLine("0x64:ä GtFNgÖAH");
                    p++;
                    break;
                case 0x65:
                    //str.AppendLine("0x65:ä GtFNgÖAH");
                    p++;
                    break;
                case 0x66:
                    //str.AppendLine("0x66:ä GtFNgÖAH");
                    p++;
                    break;
                case 0x67:
                    //str.AppendFormat("0x67:ä GtFNgwèH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x68:
                    //str.AppendFormat("0x68:ä GtFNgwèH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x69:
                    //str.AppendLine("; 0x69:tF[hCH");
                    p++;
                    break;
                case 0x6a:
                    //str.AppendLine("; 0x6a:tF[hAEgH");
                    p++;
                    break;
                // ¢½ßYEND
                case 0x6b:
                    //str.AppendFormat("0x6b:ä eLXgGtFNgÖAH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x6c:
                    //str.AppendFormat("0x6c:ä eLXgGtFNgÖAH {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x80:  // aflJn
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.Play));

                    p += 2;
                    break;
                case 0x81:
                    // str.AppendLine("; 0x81:afltF[h");
                    // str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    // Éaflª½Æ«tF[h·éõH
                    // XNvgkêÈ¢ÌÅAÀ·éÈç©OÅXNvggÞµ©cB
                    // ÊÈ­Äà®¢ÄéÁÛ¢B

                    p++;
                    break;
                case 0x82:  // aflâ~
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.Stop));

                    p++;
                    break;
                case 0x84:
                    // str.AppendFormat("; 0x84:V[ÌaflJn {0}\n", data[p + 1]);
                    // xlvnsÅÍRgAEgBÈ­Äà®­H
                    p += 2;
                    break;
                case 0x85:
                    // str.AppendLine("; 0x85:afltF[hÒ¿");
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    p++;
                    break;
                case 0x87:
                    // str.AppendFormat("; 0x87:aflJn(tF[h) {0}\n", data[p + 1]);
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.FadeStart));
                    p += 2;
                    break;
                case 0xa0:  // oblÇÝÝ
                    // ¿ÈÝÉdwaveloadÍÄ¶ñÌÖWÅg¦È¢cB
                    loadedPCM = data[p + 1];
                    p += 2;
                    break;
                case 0xa1:  // Ä¶Ìoblðâ~
                    str.Append("dwavestop 1\r\n");
                    p++;
                    break;
                case 0xa2:
                    // par1ªÄ¶ñApar2ÍÄ¶¹Ê
                    // str.AppendFormat("; 0xa2:oblÄ¶ {0}, {1}\n", data[p + 1], data[p + 2]);

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

                        // resettimerµÄâ~Ò¿ÅÄ¶Ô - ^C}[·êÎ
                        // â~Ò¿ÉÈéH }CiXÍêâèÈ³»¤cB
                        str.Append("resettimer\r\n");
                    }
                    p += 3;
                    break;
                case 0xa3:  // oblâ~Ò¿
                    str.Append("gettimer %199\r\n");
                    str.AppendFormat("delay {0} - %199\r\n", waitPCM);
                    p++;
                    break;
                case 0xa6:
                    // str.AppendLine("; 0xa6:oblÖAH");
                    p++;
                    break;
                case 0xaf:  // bZ[WI¹
                    p++;
                    return true;
                case 0xb0:  // üs
                    onTextBr = true;
                    if (!onSelect) str.AppendLine("");
                    p++;
                    break;
                case 0xb2:  // üÍÒ¿
                    str.Append("@");
                    p++;
                    break;
                case 0xb3:  // y[WXVÒ¿
                    onTextBr = true;
                    str.AppendLine("\\\r\n");
                    p++;
                    break;
                case 0xb6:
                    if (!onSelect) {/*str.AppendFormat("0xb6:¶`æ¬xwè {0}(*10ms)\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xb7:
                    //str.AppendFormat("0xb7:ÔÒ¿ {0}(*10ms)\n", data[p + 1]);
                    p += 2;
                    break;
                case 0xb9:
                    if (!onSelect) {/*str.AppendFormat("0xb9:¶`æItZbgwè {0}\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xbb:  // tbV
                    str.Append("lsp 0, \":c;graphics\\WHITE.PNG\", 0, 40\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    str.Append("wait 200\r\n");
                    str.Append("csp 0\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    p++;
                    break;
                case 0xbc:  // æÊU®
                    str.Append("chvol 1, 75\r\n");
                    str.AppendFormat("dwave 1, \"{0}KZ_VD03.WAV\"\r\n", LeNSMain.SoundPath);
                    str.Append("quake 4, 550\r\n");
                    p++;
                    break;
                case 0xbd:  // Êíwi[h
                case 0xbe:  // wi[hH
                    //str.AppendFormat("; 0xbd:Êíwi[h {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    //str.AppendFormat("; 0xbe:wi[hH {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // ½ªpar2ªhmApar3ªntsÌGtFNgwèB
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 1], 2));
                    p += 4;
                    break;
                case 0xbf:
                    // str.AppendFormat("; 0xbf:rWAV[[h {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // ½ªpar2ªhmApar3ªntsÌGtFNgwèB
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("V", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc0:
                    //str.AppendFormat("0xc0:gV[[h {0}, {1}, {2}n", data[p + 1], data[p + 2], data[p + 3]);
                    // ½ªpar2ªhmApar3ªntsÌGtFNgwèB
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("H", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc1:  // LN^ÏX
                case 0xc2:  // LN^\¦
                    // GtFNgÍtF[hÅè
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc3:  // SLÁãAL\¦
                    // GtFNgÍtF[hÅè
                    str.Append("cl a, 0\r\n");
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc4:
                    // str.AppendFormat("; 0xc4:wit«LN^\¦ {0}, {1}, {2}, {3}, {4}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5]);
                    // ½ªpar4ªhmApar5ªntsÌGtFNgwèB
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 6;
                    break;
                case 0xc6:
                    //str.AppendFormat("; 0xc6:RL¯\¦ {0}, {1}, {2}, {3}, {4}, {5}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5], data[p + 6]);
                    // GtFNgÍtF[hÅè
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.Append(getLDCommand(data[p + 4], data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 6], data[p + 5], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 7;
                    break;
                case 0xc8:
                    //str.AppendFormat("; 0xc8:½ÔñøÊ {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xc9:
                    //str.AppendLine("; 0xc9:½ÔñøÊ");
                    p++;
                    break;
                case 0xca:
                    //str.AppendLine("; 0xca:SÌÜ");
                    p++;
                    break;
                case 0xcb:
                    //str.AppendLine("; 0xcb:½ÔñøÊ");
                    p++;
                    break;
                case 0xcc:
                    // str.AppendFormat("; 0xcc:X^XN[Jn {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xcd:
                    //str.AppendLine("; 0xcd:µÔ«");
                    p++;
                    break;
                case 0xff:  // XNvgI[
                    p++;
                    break;
                default:
                    throw new InvalidDataException("³øÈ½ßÅ·B[" + data[p].ToString() + "]");
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

            // épbgÌtOðZbg
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
                // »óªéÌÆ«Íép§¿Gð\¦
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
