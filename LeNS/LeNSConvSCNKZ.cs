using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// ＴＯＤＯ
// ・ＯＰ／ＥＤ／ロゴ
// ・効果音
// ・エフェクト系実装

namespace LeNS
{
    class LeNSConvSCNKZ : LeNSConvSCN
    {
        // ロード中のPCM情報保持用
        protected byte loadedPCM = 0xff;    // ロード中のＰＣＭ
        protected int waitPCM = 0;          // ロード中ＰＣＭの再生時間

        protected Boolean onTextBr = true;  // 直前に改行されてるか

        public LeNSConvSCNKZ(LeNSConvOption Option)
            : base(Option)
        {
            // フォントテーブルの作成
            // ※一部制御文字に非対応。crは全角スペース、!?は！に変換されます。
            //fontTable = "　■ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ０１２３４５６７８９あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉゃゅょっ■アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォャュョッヶヴ『』「」（）？！!?－ー～。、．，・‥…○々了＞↑↓＜cr哀愛挨逢悪握圧扱宛飴安暗案闇以位依偉囲威意慰易椅為異移胃衣違遺医井域育一溢逸芋印咽員因引飲淫院陰隠韻右宇羽雨渦嘘瓜噂運雲営影映栄永泳洩英衛鋭液益駅悦越円園延援沿演炎煙縁艶遠鉛塩汚甥央奥往応押横殴王黄億屋憶臆俺恩温穏音下化仮何価加可夏嫁家科暇果架歌河火稼箇花荷華課嘩貨過霞我画芽餓介会解回塊壊快怪悔懐戒改械海界皆絵開階外咳害慨蓋街垣嚇各格核殻獲確穫覚角較郭隔革学楽額顎掛割活滑叶鞄噛乾寒刊勘勧巻姦完官干患感慣換敢歓汗漢甘監看管簡緩肝観貫間閑関陥館丸含玩眼岩頑顔願企危喜器基奇嬉寄希幾忌揮机既期機帰気祈季規記貴起輝飢鬼偽儀戯技擬犠疑義議吉喫詰却客脚虐逆丘久仇休及吸宮弓急救朽求泣球究窮級給旧牛去居巨拒拠挙虚許距魚享京供競共凶協叫境強怯恐挟教橋況狂狭胸脅興郷鏡響驚仰凝業局曲極玉僅勤均禁筋緊菌襟近金銀九句区苦躯駆具愚空偶遇隅屈掘靴熊栗繰君群袈係傾刑兄型形恵憩掲携敬景稽系経継繋蛍計警軽迎劇撃激隙傑欠決潔穴結血月件健兼剣喧堅嫌建懸拳検権犬研絹県肩見賢軒遣鍵険験元原厳幻減源玄現言限個古呼固孤己庫戸故股誇雇鼓五互午後御悟語誤護乞交候光公功効厚口向喉垢好工巧幸広康慌抗拘控攻更校構溝甲皇硬稿紅絞綱耕考肯航荒行講貢購郊鋼降香高剛号合豪轟克刻告国酷黒獄腰惚骨込頃今困恨懇昏根混魂痕佐左差査沙砂鎖裟座催再最塞妻彩才歳済災砕祭細菜裁載際在材罪財咲作削昨策索錯桜冊察拶撮擦札殺雑錆三参山惨散産賛酸斬残仕仔伺使刺司史四士始姉姿子市師志思指支施旨枝止死氏私糸紙紫肢脂至視詞試誌資飼歯事似侍児字寺持時次治磁示耳自辞鹿式識雫七叱執失嫉室湿疾質実芝舎写射捨赦斜煮社者謝車遮邪借灼酌釈若寂弱主取守手殊狩珠種趣酒首受呪授樹需収周就修拾秀秋終習臭衆襲蹴週集醜住充十従柔汁渋獣縦重銃叔宿縮熟出術述春瞬準潤純巡順処初所暑庶緒署書諸助女序徐除傷償勝商唱奨将小少床承招掌昇晶沼消渉焼焦照症省硝祥章笑粧紹衝証詳象鐘障上丈乗冗剰城場嬢常情条杖浄状畳譲飾拭植殖織職色触食辱尻伸信侵唇寝審心慎振新浸深申真神芯親診身辛進針震人刃塵尋訊尽陣須図吹垂推水睡粋衰遂酔随崇数据裾澄寸世瀬是凄制勢姓征性成星晴正清牲生盛精声製西誠誓請青静斉税脆席戚昔石積籍績責赤跡切接摂折設節説雪絶舌先千宣専尖川戦扇栓泉浅洗染潜旋線羨船詮選閃鮮前善然全繕楚狙疎祖素組蘇訴阻創双喪壮奏層想捜掃挿掻操早争痩相窓総草葬蒼装走送遭騒像増憎臓蔵造促側則即息束測足速属賊族続卒袖揃存尊損村他多太汰唾打駄体対耐帯待怠態替胎袋貸退隊代台大第題卓宅択沢濯託濁諾叩達奪脱谷誰単嘆担探淡短端誕団壇弾断暖段男談値知地恥池痴置致遅築畜竹秩茶着中仲宙忠抽昼柱注虫駐著貯丁兆喋帳張彫徴朝潮町眺聴腸調超跳長頂鳥直沈珍鎮陳津追痛通掴潰爪釣鶴亭低停貞呈定底庭弟抵提程締訂諦邸釘泥摘敵滴的適溺徹撤鉄典天展店添纏貼転点伝田電吐堵塗妬徒渡登賭途都努度土奴怒倒凍刀唐投東桃盗湯灯当等答筒統到藤討豆踏逃透陶頭闘働動同堂導憧瞳胴道得徳特毒独読突届沌豚呑鈍那内謎馴軟難二匂賑肉日乳入尿任忍認濡猫熱年念捻燃粘之悩濃納能脳覗把播波派破馬廃拝排敗杯背肺輩配倍梅買売這伯剥博拍泊白薄迫漠爆縛箱肌八発髪伐抜鳩伴判半反板版犯繁般範晩番盤卑否彼悲扉披比泌疲皮秘被費避非飛備尾微眉美鼻匹彦膝肘必姫百標氷漂表評描病秒苗品貧頻敏瓶不付夫婦富布怖扶敷普浮父腐膚負赴侮撫武舞部封風伏副復服福腹複覆淵払沸仏物分憤奮紛雰文聞併兵平柄並閉米壁癖別蔑変片編辺返便勉弁保捕歩補募墓慕暮母包呆報宝崩抱放方法泡砲縫胞芳褒訪豊邦飽乏亡傍坊妨忘忙房暴望某棒冒肪膨貌防頬北僕撲勃没本翻凡磨魔麻埋妹昧枚毎幕膜枕末万慢満漫味未魅密蜜脈妙民眠務夢無霧娘名命明迷銘鳴滅面模妄毛猛網耗木黙目戻問悶紋門也夜野矢厄役約薬訳躍柳愉油癒諭唯優勇友幽有由祐裕誘遊郵雄夕予余与誉預幼妖容揚揺曜様洋溶用葉要踊遥陽養抑欲浴翌翼裸来頼雷絡落乱卵利履理裏里離陸律率立略流溜留粒隆竜龍慮旅虜僚両凌料涼猟療良量陵領力緑林輪隣涙累類令例冷励礼鈴隷霊麗齢歴列劣烈裂恋憐連炉路露労廊浪漏老郎六録論和話歪脇惑枠鷲腕呟呻咄喘嗚嗅嗟嘔嘲囁奢恍愕慄戮拗揉攣曖渾滲猥痙痺癇癲眩睨瞼綺罠羞膣臀茫訝踵騙梓窺苛牙駕崖缶鑑亀汲喰牽虎巷腔忽些皿朱愁盾逝醒蝉噌遡惰滞狸弛兎鍋罵柏箸鉢閥飯紐楓噴塀弊陛瞥朴殆矛姪儲餠爺嵐掠呂弄椀碗俯儚埒徊徘晰枷檻鬱涸皺胱膀贅躇躊躰頷哉惹巾曰畏萎伽蚊拐涯渇堪稀撒錠蝕靱錐爽耽智漬洞縄覇莫斑斐沫擁肋刹吼咆咤哮唸峙撼朦朧璧眸絆舐軋阿緯訣昂棲壷薙捧湧詫揶揄炒蠢蹲迸槽狽瀕狼丼屹煌廻囚掟葛串弦腫妾寵凪涎炸翔怨喚窟坑吊悠珀琥貪躓讐逮嬌≫"
            //            .ToCharArray();
            includeFile = "kizuato.txt";
            fontTable = "　■ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ０１２３４５６７８９あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽぁぃぅぇぉゃゅょっ■アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポァィゥェォャュョッヶヴ『』「」（）？！　－ー～。、．，・‥…○々了＞↑↓＜　哀愛挨逢悪握圧扱宛飴安暗案闇以位依偉囲威意慰易椅為異移胃衣違遺医井域育一溢逸芋印咽員因引飲淫院陰隠韻右宇羽雨渦嘘瓜噂運雲営影映栄永泳洩英衛鋭液益駅悦越円園延援沿演炎煙縁艶遠鉛塩汚甥央奥往応押横殴王黄億屋憶臆俺恩温穏音下化仮何価加可夏嫁家科暇果架歌河火稼箇花荷華課嘩貨過霞我画芽餓介会解回塊壊快怪悔懐戒改械海界皆絵開階外咳害慨蓋街垣嚇各格核殻獲確穫覚角較郭隔革学楽額顎掛割活滑叶鞄噛乾寒刊勘勧巻姦完官干患感慣換敢歓汗漢甘監看管簡緩肝観貫間閑関陥館丸含玩眼岩頑顔願企危喜器基奇嬉寄希幾忌揮机既期機帰気祈季規記貴起輝飢鬼偽儀戯技擬犠疑義議吉喫詰却客脚虐逆丘久仇休及吸宮弓急救朽求泣球究窮級給旧牛去居巨拒拠挙虚許距魚享京供競共凶協叫境強怯恐挟教橋況狂狭胸脅興郷鏡響驚仰凝業局曲極玉僅勤均禁筋緊菌襟近金銀九句区苦躯駆具愚空偶遇隅屈掘靴熊栗繰君群袈係傾刑兄型形恵憩掲携敬景稽系経継繋蛍計警軽迎劇撃激隙傑欠決潔穴結血月件健兼剣喧堅嫌建懸拳検権犬研絹県肩見賢軒遣鍵険験元原厳幻減源玄現言限個古呼固孤己庫戸故股誇雇鼓五互午後御悟語誤護乞交候光公功効厚口向喉垢好工巧幸広康慌抗拘控攻更校構溝甲皇硬稿紅絞綱耕考肯航荒行講貢購郊鋼降香高剛号合豪轟克刻告国酷黒獄腰惚骨込頃今困恨懇昏根混魂痕佐左差査沙砂鎖裟座催再最塞妻彩才歳済災砕祭細菜裁載際在材罪財咲作削昨策索錯桜冊察拶撮擦札殺雑錆三参山惨散産賛酸斬残仕仔伺使刺司史四士始姉姿子市師志思指支施旨枝止死氏私糸紙紫肢脂至視詞試誌資飼歯事似侍児字寺持時次治磁示耳自辞鹿式識雫七叱執失嫉室湿疾質実芝舎写射捨赦斜煮社者謝車遮邪借灼酌釈若寂弱主取守手殊狩珠種趣酒首受呪授樹需収周就修拾秀秋終習臭衆襲蹴週集醜住充十従柔汁渋獣縦重銃叔宿縮熟出術述春瞬準潤純巡順処初所暑庶緒署書諸助女序徐除傷償勝商唱奨将小少床承招掌昇晶沼消渉焼焦照症省硝祥章笑粧紹衝証詳象鐘障上丈乗冗剰城場嬢常情条杖浄状畳譲飾拭植殖織職色触食辱尻伸信侵唇寝審心慎振新浸深申真神芯親診身辛進針震人刃塵尋訊尽陣須図吹垂推水睡粋衰遂酔随崇数据裾澄寸世瀬是凄制勢姓征性成星晴正清牲生盛精声製西誠誓請青静斉税脆席戚昔石積籍績責赤跡切接摂折設節説雪絶舌先千宣専尖川戦扇栓泉浅洗染潜旋線羨船詮選閃鮮前善然全繕楚狙疎祖素組蘇訴阻創双喪壮奏層想捜掃挿掻操早争痩相窓総草葬蒼装走送遭騒像増憎臓蔵造促側則即息束測足速属賊族続卒袖揃存尊損村他多太汰唾打駄体対耐帯待怠態替胎袋貸退隊代台大第題卓宅択沢濯託濁諾叩達奪脱谷誰単嘆担探淡短端誕団壇弾断暖段男談値知地恥池痴置致遅築畜竹秩茶着中仲宙忠抽昼柱注虫駐著貯丁兆喋帳張彫徴朝潮町眺聴腸調超跳長頂鳥直沈珍鎮陳津追痛通掴潰爪釣鶴亭低停貞呈定底庭弟抵提程締訂諦邸釘泥摘敵滴的適溺徹撤鉄典天展店添纏貼転点伝田電吐堵塗妬徒渡登賭途都努度土奴怒倒凍刀唐投東桃盗湯灯当等答筒統到藤討豆踏逃透陶頭闘働動同堂導憧瞳胴道得徳特毒独読突届沌豚呑鈍那内謎馴軟難二匂賑肉日乳入尿任忍認濡猫熱年念捻燃粘之悩濃納能脳覗把播波派破馬廃拝排敗杯背肺輩配倍梅買売這伯剥博拍泊白薄迫漠爆縛箱肌八発髪伐抜鳩伴判半反板版犯繁般範晩番盤卑否彼悲扉披比泌疲皮秘被費避非飛備尾微眉美鼻匹彦膝肘必姫百標氷漂表評描病秒苗品貧頻敏瓶不付夫婦富布怖扶敷普浮父腐膚負赴侮撫武舞部封風伏副復服福腹複覆淵払沸仏物分憤奮紛雰文聞併兵平柄並閉米壁癖別蔑変片編辺返便勉弁保捕歩補募墓慕暮母包呆報宝崩抱放方法泡砲縫胞芳褒訪豊邦飽乏亡傍坊妨忘忙房暴望某棒冒肪膨貌防頬北僕撲勃没本翻凡磨魔麻埋妹昧枚毎幕膜枕末万慢満漫味未魅密蜜脈妙民眠務夢無霧娘名命明迷銘鳴滅面模妄毛猛網耗木黙目戻問悶紋門也夜野矢厄役約薬訳躍柳愉油癒諭唯優勇友幽有由祐裕誘遊郵雄夕予余与誉預幼妖容揚揺曜様洋溶用葉要踊遥陽養抑欲浴翌翼裸来頼雷絡落乱卵利履理裏里離陸律率立略流溜留粒隆竜龍慮旅虜僚両凌料涼猟療良量陵領力緑林輪隣涙累類令例冷励礼鈴隷霊麗齢歴列劣烈裂恋憐連炉路露労廊浪漏老郎六録論和話歪脇惑枠鷲腕呟呻咄喘嗚嗅嗟嘔嘲囁奢恍愕慄戮拗揉攣曖渾滲猥痙痺癇癲眩睨瞼綺罠羞膣臀茫訝踵騙梓窺苛牙駕崖缶鑑亀汲喰牽虎巷腔忽些皿朱愁盾逝醒蝉噌遡惰滞狸弛兎鍋罵柏箸鉢閥飯紐楓噴塀弊陛瞥朴殆矛姪儲餠爺嵐掠呂弄椀碗俯儚埒徊徘晰枷檻鬱涸皺胱膀贅躇躊躰頷哉惹巾曰畏萎伽蚊拐涯渇堪稀撒錠蝕靱錐爽耽智漬洞縄覇莫斑斐沫擁肋刹吼咆咤哮唸峙撼朦朧璧眸絆舐軋阿緯訣昂棲壷薙捧湧詫揶揄炒蠢蹲迸槽狽瀕狼丼屹煌廻囚掟葛串弦腫妾寵凪涎炸翔怨喚窟坑吊悠珀琥貪躓讐逮嬌≫"
                        .ToCharArray();

            // フラグテーブルの作成
            // ローカルフラグ
            flagTable.Add(0x00, 0);     // 00:楓と会話した
            flagTable.Add(0x01, 1);     // 01:自分が犯人ではない
            flagTable.Add(0x02, 2);     // 02:千鶴さんを疑う
            flagTable.Add(0x03, 3);     // 03:梓と事件現場にむかった
            flagTable.Add(0x04, 4);     // 04:梓シナリオ警察に電話
            flagTable.Add(0x05, 5);     // 05:グラビアを買った
            flagTable.Add(0x06, 6);     // 06:雫を買った
            flagTable.Add(0x07, 7);     // 07:初音ちゃんをこわがらせない
            flagTable.Add(0x08, 8);     // 08:おまもりをわたした
            flagTable.Add(0x09, 9);     // 09:未使用
            flagTable.Add(0x0a, 10);    // 0a:エンディング後フラグ制御用
            flagTable.Add(0x0b, 11);    // 0b:エンディング後フラグ制御用
            flagTable.Add(0x0c, 12);    // 0c:エンディング後フラグ制御用
            flagTable.Add(0x0d, 13);    // 0d:エンディング後フラグ制御用
            flagTable.Add(0x0e, 14);    // 0e:エンディング後フラグ制御用
            flagTable.Add(0x0f, 15);    // 0f:エンディング後フラグ制御用
            flagTable.Add(0x10, 16);    // 10:エンディング後フラグ制御用
            flagTable.Add(0x11, 17);    // 11:エンディング後フラグ制御用
            flagTable.Add(0x12, 18);    // 12:エンディング後フラグ制御用
            flagTable.Add(0x13, 19);    // 13:エンディング後フラグ制御用

            // グローバルフラグ
            flagTable.Add(0x14, 200);   // 14: 千鶴BADエンドを見た
            flagTable.Add(0x15, 201);   // 15: 千鶴エンディングを見た
            flagTable.Add(0x16, 202);   // 16: 梓 Happy を見た
            flagTable.Add(0x17, 203);   // 17: 楓 BAD を見た
            flagTable.Add(0x18, 204);   // 18: 楓 HAPPY を見た 
            flagTable.Add(0x19, 205);   // 19: 柳川エンドを見た
            flagTable.Add(0x1a, 206);   // 1a: 初音エンドを見た
            flagTable.Add(0x73, 207);   // 73: 全エンディングを見たかどうか
            flagTable.Add(0x74, 208);   // 74: ？？？
        }

        // シナリオデコーダ
        protected override LeNSConvResult decodeEvent(LeafPack.LeafFileInfo srcInfo, byte[] scnData, byte[] txtData)
        {
            int scn_no = int.Parse(srcInfo.Name.Substring(0, 3));

            // サイズとかブロック情報とか入ってるので
            // 一律12バイトめからスタート
            int p = 11;

            StringBuilder str = new StringBuilder();
            str.AppendFormat("; ---- {0} START -----------------------------------------------------------\r\n", srcInfo.Name);
            str.AppendFormat("*scn{0}\r\n\r\n", scn_no.ToString("000"));

            while (p < scnData.Length)
            {
                // ラベルキャッシュが存在する場合はラベルを出力
                if (labelCache.ContainsKey(getLabelString(scn_no, p)))
                    str.AppendLine(getLabelString(scn_no, p) + "\r\n");

                switch (scnData[p])
                {
                    case 0x20:  // 終了
                        str.AppendLine("reset");
                        p++;
                        break;
                    case 0x23:
                        //str.AppendFormat("; 0x23:未定義命令？ {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x24:  // ジャンプ
                        // ジャンプブロックの指定は常に1なので、ファイル先頭でＯＫ
                        str.AppendFormat("goto *scn{0}\r\n\r\n", scnData[p + 1].ToString("000"));
                        p += 3;
                        break;
                    case 0x25:  // 選択肢
                        parseText(txtData, scnData[p + 1], str);

                        str.AppendLine("br");
                        str.AppendLine("select");
                        for (int i = 0; i < scnData[p + 2]; i++)
                        {
                            // テキストデータ呼び出し(文字以外のコマンドを無視)
                            str.Append("\"");
                            parseText(txtData, scnData[p + 3 + (i * 2)], str, true);
                            str.Append("\", ");

                            // ジャンプ先のアドレスを求めてラベルを生成
                            String label = getLabelString(scn_no, p + 3 + (scnData[p + 2] * 2) + (scnData[p + 4 + (i * 2)]));
                            str.Append(label);

                            // ラベルキャッシュを追加
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }

                            if (i + 1 < scnData[p + 2]) { str.Append(","); }
                            str.AppendLine("");
                        }
                        str.AppendLine("");

                        p += 3 + (scnData[p + 2] * 2);
                        break;
                    case 0x27:
                        //str.AppendLine("0x27:前の選択肢に戻る状態保存");
                        p++;
                        break;
                    case 0x41:  // 分岐(正)
                        {
                            // ジャンプ先のアドレスを求めてラベルを生成
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}=={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // ラベルキャッシュを追加
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x42:  // 分岐(否)
                        {
                            // ジャンプ先のアドレスを求めてラベルを生成
                            String label = getLabelString(scn_no, p + 4 + scnData[p + 3]);
                            str.AppendFormat("if %{0}!={1} goto {2}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2], label);

                            // ラベルキャッシュを追加
                            if (!labelCache.ContainsKey(label)) { labelCache.Add(label, label); }
                        }
                        p += 4;
                        break;
                    case 0x4b:  // フラグセット
                        str.AppendFormat("mov %{0}, {1}\r\n", getFragNo(scnData[p + 1]), scnData[p + 2]);
                        p += 3;
                        break;
                    case 0x51:  // テキスト出力
                        parseText(txtData, scnData[p + 1], str);
                        p += 2;
                        break;
                    case 0x52:
                        //str.AppendLine("0x52:選択肢に付属する命令");
                        p++;
                        break;
                    case 0x94:
                        //str.AppendLine("0x94:エンディングに付属する命令");
                        p++;
                        break;
                    case 0x95:
                        //str.AppendFormat("0x95:エンディングＢＧＭ指定 {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    case 0x96:
                        //str.AppendFormat("0x96:到達エンディングの指定 {0}\n", scnData[p + 1]);
                        p += 2;
                        break;
                    default:
                        parseCommand(scnData, ref p, str);
                        break;
                }
            }
            // スクリプト終端に到達
            str.AppendFormat("\r\n; ---- {0} END -------------------------------------------------------------\r\n\r\n", srcInfo.Name);

            writeFile(srcInfo, str);
            return LeNSConvResult.ok;
        }

        protected void parseText(byte[] txtData, int no, StringBuilder str)
        {
            parseText(txtData, no, str, false);
        }

        // テキスト出力
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

        // 汎用コマンド実行
        protected Boolean parseCommand(byte[] data, ref int p, StringBuilder str, Boolean onText, Boolean onSelect)
        {
            // テキスト表示中に命令文が来たとき、"/"を挿入して改行する
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
                    // ↑は実装したら消さないとダメ
                    // 以下の命令は何もしない(命令文を生成しないコマンド)
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
                case 0x30:  // 処理無し背景ロード
                    str.Append(getBGCommand(data[p + 1], 0));
                    p += 2;
                    break;
                case 0x31:
                    str.AppendFormat("; 0x31:エフェクト関連？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x34:
                    str.AppendFormat("; 0x34:画面消去？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x35:
                    str.AppendFormat("; 0x35:エフェクト関連？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x36:  // 処理無しVisualロード
                    str.Append(getPictureCommand("V", data[p + 1], 0));
                    p += 2;
                    break;
                case 0x37:  // 処理無しキャラクタロード
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    p += 3;
                    break;
                case 0x38:
                    //str.AppendFormat("; 0x38:直接表示処理 {0}, {1}\n", data[p + 1], data[p + 2]);
                    // エフェクトのIN/OUT？
                    str.AppendFormat("print 2\r\n");
                    p += 3;
                    break;
                case 0x39:
                    str.AppendFormat("; 0x39:表示処理 {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3a:
                    str.AppendFormat("; 0x3a:パレット一時変更指定(即時反映) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3b:
                    str.AppendFormat("; 0x3b:表示時パレット変更指定(即時反映はしない) {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x3c:
                    str.AppendFormat("; 0x3c:色反転指定 {0}\n", data[p + 1]);
                    p += 2;
                    break;
                // 未処理命令ズSTART
                case 0x60:
                    //str.AppendLine("0x60:謎 パレット反映？");
                    p++;
                    break;
                case 0x61:
                    //str.AppendLine("0x61:謎 パレットを0に？");
                    p++;
                    break;
                case 0x62:
                    //str.AppendFormat("0x62:謎 エフェクト指定？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x63:
                    //str.AppendFormat("0x63:謎 エフェクト指定？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x64:
                    //str.AppendLine("0x64:謎 エフェクト関連？");
                    p++;
                    break;
                case 0x65:
                    //str.AppendLine("0x65:謎 エフェクト関連？");
                    p++;
                    break;
                case 0x66:
                    //str.AppendLine("0x66:謎 エフェクト関連？");
                    p++;
                    break;
                case 0x67:
                    //str.AppendFormat("0x67:謎 エフェクト指定？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x68:
                    //str.AppendFormat("0x68:謎 エフェクト指定？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x69:
                    //str.AppendLine("; 0x69:フェードイン？");
                    p++;
                    break;
                case 0x6a:
                    //str.AppendLine("; 0x6a:フェードアウト？");
                    p++;
                    break;
                // 未処理命令ズEND
                case 0x6b:
                    //str.AppendFormat("0x6b:謎 テキストエフェクト関連？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x6c:
                    //str.AppendFormat("0x6c:謎 テキストエフェクト関連？ {0}\n", data[p + 1]);
                    p += 2;
                    break;
                case 0x80:  // ＢＧＭ開始
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.Play));

                    p += 2;
                    break;
                case 0x81:
                    // str.AppendLine("; 0x81:ＢＧＭフェード");
                    // str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    // 次にＢＧＭが来たときフェードする準備？
                    // スクリプト遡れないので、実装するなら自前でスクリプト組むしか…。
                    // 当面なくても動いてるっぽい。

                    p++;
                    break;
                case 0x82:  // ＢＧＭ停止
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.Stop));

                    p++;
                    break;
                case 0x84:
                    // str.AppendFormat("; 0x84:次シーンのＢＧＭ開始 {0}\n", data[p + 1]);
                    // xlvnsではコメントアウト。なくても動く？
                    p += 2;
                    break;
                case 0x85:
                    // str.AppendLine("; 0x85:ＢＧＭフェード待ち");
                    str.AppendFormat(getBgmString(0, LeNSBgmOperation.FadeOut));
                    p++;
                    break;
                case 0x87:
                    // str.AppendFormat("; 0x87:ＢＧＭ開始(フェード) {0}\n", data[p + 1]);
                    str.AppendFormat(getBgmString(data[p + 1] + 2, LeNSBgmOperation.FadeStart));
                    p += 2;
                    break;
                case 0xa0:  // ＰＣＭ読み込み
                    // ちなみにdwaveloadは再生回数の関係で使えない…。
                    loadedPCM = data[p + 1];
                    p += 2;
                    break;
                case 0xa1:  // 再生中のＰＣＭを停止
                    str.Append("dwavestop 1\r\n");
                    p++;
                    break;
                case 0xa2:
                    // par1が再生回数、par2は再生音量
                    // str.AppendFormat("; 0xa2:ＰＣＭ再生 {0}, {1}\n", data[p + 1], data[p + 2]);

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

                        // resettimerして停止待ちで再生時間 - タイマーすれば
                        // 停止待ちになる？ マイナス時は一応問題なさそう…。
                        str.Append("resettimer\r\n");
                    }
                    p += 3;
                    break;
                case 0xa3:  // ＰＣＭ停止待ち
                    str.Append("gettimer %199\r\n");
                    str.AppendFormat("delay {0} - %199\r\n", waitPCM);
                    p++;
                    break;
                case 0xa6:
                    // str.AppendLine("; 0xa6:ＰＣＭ関連？");
                    p++;
                    break;
                case 0xaf:  // メッセージ終了
                    p++;
                    return true;
                case 0xb0:  // 改行
                    onTextBr = true;
                    if (!onSelect) str.AppendLine("");
                    p++;
                    break;
                case 0xb2:  // 入力待ち
                    str.Append("@");
                    p++;
                    break;
                case 0xb3:  // ページ更新待ち
                    onTextBr = true;
                    str.AppendLine("\\\r\n");
                    p++;
                    break;
                case 0xb6:
                    if (!onSelect) {/*str.AppendFormat("0xb6:文字描画速度指定 {0}(*10ms)\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xb7:
                    //str.AppendFormat("0xb7:時間待ち {0}(*10ms)\n", data[p + 1]);
                    p += 2;
                    break;
                case 0xb9:
                    if (!onSelect) {/*str.AppendFormat("0xb9:文字描画オフセット指定 {0}\n", data[p + 1]);*/}
                    p += 2;
                    break;
                case 0xbb:  // フラッシュ
                    str.Append("lsp 0, \":c;graphics\\WHITE.PNG\", 0, 40\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    str.Append("wait 200\r\n");
                    str.Append("csp 0\r\n");
                    str.AppendFormat("print {0}\r\n", 2);
                    p++;
                    break;
                case 0xbc:  // 画面振動
                    str.Append("chvol 1, 75\r\n");
                    str.AppendFormat("dwave 1, \"{0}KZ_VD03.WAV\"\r\n", LeNSMain.SoundPath);
                    str.Append("quake 4, 550\r\n");
                    p++;
                    break;
                case 0xbd:  // 通常背景ロード
                case 0xbe:  // 背景ロード？
                    //str.AppendFormat("; 0xbd:通常背景ロード {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    //str.AppendFormat("; 0xbe:背景ロード？ {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // 多分par2がＩＮ、par3がＯＵＴのエフェクト指定。
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 1], 2));
                    p += 4;
                    break;
                case 0xbf:
                    // str.AppendFormat("; 0xbf:ビジュアルシーンロード {0}, {1}, {2}\n", data[p + 1], data[p + 2], data[p + 3]);
                    // 多分par2がＩＮ、par3がＯＵＴのエフェクト指定。
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("V", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc0:
                    //str.AppendFormat("0xc0:Ｈシーンロード {0}, {1}, {2}n", data[p + 1], data[p + 2], data[p + 3]);
                    // 多分par2がＩＮ、par3がＯＵＴのエフェクト指定。
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getPictureCommand("H", data[p + 1], 2));
                    p += 4;
                    break;
                case 0xc1:  // キャラクタ変更
                case 0xc2:  // キャラクタ表示
                    // エフェクトはフェード固定
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc3:  // 全キャラ消去後、キャラ表示
                    // エフェクトはフェード固定
                    str.Append("cl a, 0\r\n");
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 2));
                    p += 3;
                    break;
                case 0xc4:
                    // str.AppendFormat("; 0xc4:背景付きキャラクタ表示 {0}, {1}, {2}, {3}, {4}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5]);
                    // 多分par4がＩＮ、par5がＯＵＴのエフェクト指定。
                    str.Append("cl a, 0\r\n");
                    str.AppendFormat("bg black, {0}\r\n", 2);
                    str.Append(getBGCommand(data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 6;
                    break;
                case 0xc6:
                    //str.AppendFormat("; 0xc6:３キャラ同時表示 {0}, {1}, {2}, {3}, {4}, {5}\n", data[p + 1], data[p + 2], data[p + 3], data[p + 4], data[p + 5], data[p + 6]);
                    // エフェクトはフェード固定
                    str.Append(getLDCommand(data[p + 2], data[p + 1], 0));
                    str.Append(getLDCommand(data[p + 4], data[p + 3], 0));
                    str.Append(getLDCommand(data[p + 6], data[p + 5], 0));
                    str.AppendFormat("print {0}\r\n", 2);
                    p += 7;
                    break;
                case 0xc8:
                    //str.AppendFormat("; 0xc8:たぶん効果 {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xc9:
                    //str.AppendLine("; 0xc9:たぶん効果");
                    p++;
                    break;
                case 0xca:
                    //str.AppendLine("; 0xca:鬼の爪");
                    p++;
                    break;
                case 0xcb:
                    //str.AppendLine("; 0xcb:たぶん効果");
                    p++;
                    break;
                case 0xcc:
                    // str.AppendFormat("; 0xcc:ラスタスクロール開始 {0}, {1}\n", data[p + 1], data[p + 2]);
                    p += 3;
                    break;
                case 0xcd:
                    //str.AppendLine("; 0xcd:血しぶき");
                    p++;
                    break;
                case 0xff:  // スクリプト終端
                    p++;
                    break;
                default:
                    throw new InvalidDataException("無効な命令です。[" + data[p].ToString() + "]");
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

            // 夜パレットのフラグをセット
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
                // 現状が夜のときは夜用立ち絵を表示
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
