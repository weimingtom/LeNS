using System;
using System.Collections.Generic;
using System.Text;

namespace LeNS
{
    class LeNSConvSCNSZ : LeNSConvSCN
    {
        public LeNSConvSCNSZ(LeNSConvOption Option)
            : base(Option)
        {
            fontTable = "@¡‚ ‚¢‚¤‚¦‚¨‚©‚«‚­‚¯‚±‚³‚µ‚·‚¹‚»‚½‚¿‚Â‚Ä‚Æ‚È‚É‚Ê‚Ë‚Ì‚Í‚Ð‚Ó‚Ö‚Ù‚Ü‚Ý‚Þ‚ß‚à‚â‚ä‚æ‚ç‚è‚é‚ê‚ë‚í‚ð‚ñ‚á‚ã‚å‚Á‚ª‚¬‚®‚°‚²‚´‚¶‚¸‚º‚¼‚¾‚À‚Ã‚Å‚Ç‚Î‚Ñ‚Ô‚×‚Ú‚Ï‚Ò‚Õ‚Ø‚Û‚O‚P‚Q‚R‚S‚T‚U‚V‚W‚XBAEcwxuvijHI!?|[„ƒcrƒAƒCƒEƒGƒIƒJƒLƒNƒPƒRƒTƒVƒXƒZƒ\ƒ^ƒ`ƒcƒeƒgƒiƒjƒkƒlƒmƒnƒqƒtƒwƒzƒ}ƒ~ƒ€ƒƒ‚ƒ„ƒ†ƒˆƒ‰ƒŠƒ‹ƒŒƒƒƒ’ƒ“ƒƒƒ…ƒ‡ƒbƒKƒMƒOƒQƒSƒUƒWƒYƒ[ƒ]ƒ_ƒaƒdƒfƒhƒoƒrƒuƒxƒ{ƒpƒsƒvƒyƒ|‚`‚a‚b‚c‚d‚e‚f‚g‚h‚i‚j‚k‚l‚m‚n‚o‚p‚q‚r‚s‚t‚u‚v‚w‚x‚yˆ£ˆ¤ˆ§ˆ«ˆ¬ˆµˆÄˆ¹ˆÀˆÃˆÅˆÏˆÈˆÊˆÌˆÍˆÓˆÔˆÖˆ×ˆÙˆÚˆÛˆßˆáˆãˆäˆæˆçˆêˆìˆõˆóˆôˆöˆøˆùˆú‰@‰B‰E‰F‰J‰Q‰R‰\‰^‰_‰p‰e‰f‰i‰t‰x‰z‰~‰€‰‰‰Š‰Œ‰‰“‰˜‰™‰ž‰Ÿ‰¡‰£‰©‰®‰œ‰¯‰°‰·‰´‰¹‰º‰»‰¼‰½‰¿‰Á‰Â‰Ä‰Æ‰È‰Ê‰Î‰Ô‰Û‰Ü‰ß‰à‰ä‰æ‰î‰ï‰ð‰ñ‰ó‰õ‰ö‰÷‰ù‰úŠBŠCŠDŠEŠFŠGŠJŠKŠOŠPŠQŠTŠWŠXŠiŠjŠkŠmŠoŠpŠrŠwŠyŠzŠ|Š„ŠˆŠ“Š£Š¦Š¨ŠªŠ­Š®Š´Š¾ŠÂŠÄŠÇŠÈŠÉŠÌŠÏŠÑŠÔŠÖŠÙŠÛŠÜŠáŠæŠçŠèŠëŠíŠïŠðŠñŠôŠõŠ÷ŠùŠú‹@‹A‹C‹K‹L‹M‹N‹P‹S‹Y‹^‹c‹i‹l‹p‹t‹v‹x‹z‹}‹~‹‹ƒ‹…‹‹‹‰‹Ž‹‹“‹•‹–‹—‹Ÿ‹¤‹¦‹©‹«‹­‹°‹³‹µ‹¶‹¹‹º‹»‹¾‹¿‹Á‹É‹Í‹Î‹Ö‹Ø‹Ù‹ß‹à‹ã‹å‹ê‹ì‹ó‹ô‹÷‹üŒCŒGŒJŒNŒQŒWŒXŒYŒZŒ^Œ`ŒhŒiŒoŒqŒrŒvŒxŒyŒbŒ}Œ‚ŒƒŒ„Œ‡ŒˆŒ‹ŒŒŒŽŒŒ’Œ•Œ–Œ™ŒšŒœŒŒŸŒ Œ¢Œ§Œ¨Œ©Œ®Œ¯Œ±Œ³Œ´ŒµŒ¶Œ¸Œ¹ŒºŒ»Œ¾ŒÀŒÂŒÃŒÄŒÅŒËŒÎŒÛŒÌŒÜŒÝŒßŒãŒêŒëŒìŒðŒõŒöŒúŒûŒüACDEHILNQXZ\bilsz~‚a†‡‹‘•–˜œž ¡¢ª¬­¶·¸¹»ÀÄÅÊÏÐÕ×ÙÛÝßìðôö÷Ž@ŽCŽGŽOŽQŽSŽUŽRŽcŽdŽeŽfŽgŽhŽmŽnŽpŽqŽtŽvŽwŽxŽ~Ž€ŽŽ„Ž…Ž†Ž‹ŽŠŽ–Ž—ŽšŽœŽŽžŽŸŽ¡Ž¦Ž¨Ž©Ž«Ž­Ž®Ž¯Ž´Ž·Ž¸ŽºŽ¿ŽÀŽÅŽÉŽÊŽËŽÌŽÎŽÐŽÒŽÓŽÔŽÕŽ×ŽØŽÜŽßŽâŽãŽåŽæŽçŽèŽíŽðŽñŽóŽôŽöŽüHIKRTWXZ\_adifloptuƒ„€ˆ‰Š‘•—™œŸ¥«¬­°¶¸¼ÁÂÄÅÆÇËÌÎÚÛàáãäæçéêìíîðó@ADEFGHLMNOQSUV[\^_ceghijklqu‚„…‹Œ”ž¢£¥¦§¨«¬­®¯°³´¶·¸¹º¼ÂÃÄÆÈÊÏÓÔÕØÚÜßàâãæçíõöü‘@‘F‘I‘N‘O‘R‘S‘^‘f‘g‘i‘n‘z‘{‘‘ˆ‘Š‘‹‘‘‘’‘–‘—‘›ŒJ‘œ‘‘ž‘Ÿ‘£‘¤‘¥‘§‘©‘ª‘«‘¬‘®‘°‘±‘³‘µ‘¶‘¸‘¼‘½‘¾‘¿‘Ê‘Ì‘Î‘Ò‘Ô‘Õ‘Ö‘Ý‘ã‘å‘æ‘è‘ä‘î‘ð‘÷’@’B’N’P’S’T’U’W’Z’[’c’e’f’g’i’j’k’l’m’n’p’u’x’€’ƒ’…’†’‡’ˆ’‹’£’¨’¬’®’²’´’·’¸Ÿ’¼’¾’¿’Ç’É’Ê’Ü’á’ã’è’ê’ë’í’ñ’ö’÷“I“S“T“V“W“]“_“`“c“d“f“k“n“o“r“sŒË“x“z“{“y“|“‚“‡“’“–“š“›“¥“¦“ª“Œ“®“¯“±“²“µ“¶“¹“º“¾“Á“Å“Ç“Ë“Í“×“Û“Ü“Ý“Þ“à“ä“é“ï“ì“ñ“÷“ú“ü”C”F”G”L”M”N”O”R”Y”Z”[”\”]”`”g”h”j”n”w”x”z”{”ƒ”‡””’”–”—”™”š”›” ”§”­”¯”²”»”¼”½”Â”Æ”É”Ê”Í”Ó”Ô”Û”Þ”ß”à”ä”ç”é”ð”ñ”ò”õ”ö”÷”û”ü•@•G•K•S•X•Y•\•`•a•i•p•q•s•t•v•~••‚•ƒ•…•‰•|••‘•”•—•š•›•ž• •¡•¢•¥•¦•¨•ª•±•²•´•µ•¶•·•¿•À•Â•Ç•È•Ê•Ï•Ó•Ô•×•Ù•Û•ß•à•â•ä•é•ê•ï•ð•ñ•ö•ø•ú•û–@–K–O–Y–]–c–e–h–j–k–l–v–{–ƒ–„–…–‡–ˆ–––œ––ž–¡–¢–£–§–¬–­–°–±–²–³–º–¼–½–¾–Â–Å–Ê–Í–Ï–Ñ–Ò–Ô–Ø–Ù–Ú–ß–â–å–ä–é–ï–ð–ñ–ò–î–ù—B—D—F—H—L—R—S—U—V—[—\—^—c—e—g—h—j—l—n—p—t—v—z—}—~——…—ˆ—Š—‹——Ž——•——ž— —£—¦—§—¬—­—±—¶—¼—¿—Ä—Ç—Ê—Ì—Í—Õ—×—Ú—Ü—Þ—ß—á—â—ç—é—ì—í—ñ—ô—û˜A˜F˜H˜J˜L˜_˜a˜b˜c˜f˜r™êšjšqœÉXŸøáÙãYäDæbæùéxXƒ–‚Ÿ‚¡‚£‚¥‚§ƒ@ƒBƒDƒFƒH—¹ª«ƒ”ˆ³ˆËˆÐˆÕ‰A‰C‰H‰S‰Z‰h‰j‰k‰s‰v‰w‰ƒ‰„‰ˆ‰‰–‰›‰‰³‰¸‰É‰Ì‰Ó‰Ø‰è‰ò‰üŠeŠgŠnŠuŠ{Š…ŠŠŠšŠ§Š¯Š±Š³ŠµŠ·ŠÃŠÕŠ×ŠßŠìŠîŠóŠö‹F‹U‹V‹[‹q‹r‹s‹u‹y‹{‹|‹€‹‹‘‹’‹›‹£‹¯‹²‹·‹½‹Â‹Ã‹Æ‹Ç‹È‹Ý‹ë‹ï‹ð‹ö‹ûŒ@ŒIŒPŒUŒaŒmŒnŒuŒ€Œ‰ŒŠŒ‘Œ“Œ˜Œ¦ŒÇŒÈŒÒŒÚŒäŒåŒøKRUdru€ŒŽ“›¥¦§°¾ÃÇËÌÎÓàçŽDŽEŽlŽsŽuŽ{Ž‡ŽˆŽŒŽŽŽŽ‘Ž•Ž¹Ž¼ŽÍŽÏŽáŽêŽìŽû@ELOP[]bchkn|‡ž¦µÀÐÕØèñôüJKTZanos}ˆŠ¡©»¿ÉÎÝáèë‘A‘D‘M‘P‘U‘h‘j‘o‘s‘w‘}‘~‘€‘˜‘¢‘²‘Á‘Å‘Ï‘Ñ‘Ó‘Þ‘ê‘ò‘õ’D’E’H’J’d’s’z’{’‰’’Ž’“’š’›’’©’­’µ’¹’Â’Ã’Í’â’ï’ú“@“B“E“H“M“O“X“Y“Z“\“h“i“w“€“ƒ“Š“““”“™“ž“§“©“¬“°“·“Æ“Ú“õ“û”@”A”J”P”c”p”r”„”Œ””œ”±”Ì”å”æ”í•I•r•w•x•z•†•‹•œ••£•§•«•¬•½•Ì•Ð•Ö•å•ò•ó–A–C–S–V–[–\–`–d–i–o–u–€–‚–†–‹–Œ–¨–¶–À–Æ–á–ã–ì–ó–ô–û–ü—E—T—Z—]—d—m—y—‡—˜—·—¸—½—Ë—Î—Ö—ê—ë—ò—ó—ö—÷˜I˜R˜U˜V˜e˜g˜h™ï™ùšbš}š‘šøœ’œ±†¹žBŸ•àÎázáƒá’áŸá¿áÉã©ãµäSä\ä©Žäì–çDÆ‹´—´Šé‹Ð•Fˆ ˆÉˆÝ‰u‰‡‰¤‰Ë‰Í‰Ò‰×ŠsŠŠ˜‹G‹I‹Q‹Z‹g‹†‹Ê‹Ï‹Þ‹âŒFŒfŒjŒpŒ|Œ£ŒÉŒÖŒîT^mv|‰¨ÁõŽKŽYŽZŽ^ŽaŽ™Ž¶ŽÖŽÛG‹”¤»ö÷R˜ÌÑðòø‘E‘a‘q‘t‘ƒ‘‰‘•‘ ‘·‘º‘Ã‘Ü‘ñ’r’™’ª’×’å“D“G“J“K“N“P“g“¡“­“ö”D”E”t”y”º”Ú”ï•W•b•Ò–J–_–}––Ÿ—r—š—¯—Ã˜Q™òšlCŸÓà`›‰]‰¶ŠdŠ©‹w‹¥ŒgŒ«Œ÷²½ÚŽ¥V˜ÍÜ‡½ÅÍåéù‘_‘“‘¹‘à’O’v’~ŽÞ“q““¤“Â”s”Ž••••º–D—P—¤—î˜B˜Y˜^škœß‹`cŽÚv’é•Ÿ–Eâ"
                        .ToCharArray();
        }

        // ƒVƒiƒŠƒIƒfƒR[ƒ_
        protected override LeNSConvResult decodeEvent(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
