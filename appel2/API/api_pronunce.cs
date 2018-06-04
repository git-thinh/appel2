using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace appel
{
    public class api_pronunce : api_base, IAPI
    {
        static readonly string[] pronVowels =
        {
            "iː", // fleece, sea, machine
            "eɪ", // face, day, break
            "aɪ", // price, high, try
            "ɔɪ", // choice, boy

            "uː", // goose, two, blue, group
            "əʊ", // goat, show, no
            "aʊ", // mouth, now

            "ər", // butter, collar, flavor, firm, and burst
            "ɪə", // near, here, weary
            "eə", // square.fair, various
            "ɑː", //  start, father
            "ɔː", //  thought, law, north, war
            "ʊə", //  poor, jury, cure
            "ɜː", //  nurse, stir, learn, refer

            "ə", //   about, common, standard
            "i", //   happy, radiate.glorious
            "u", //   thank you, influence, situation
  
            "n̩", //  suddenly, cotton
            "l̩", //  middle, metal
  
            "ˈ", //   (stress mark)

            "ɪ", //	kit, bid, hymn, minute
            "e", //	dress, bed, head, many
            "æ", //	trap, bad
            "ɒ", //	lot, odd, wash
            "ʌ", //	strut, mud, love, blood
            "ʊ", //	foot, good, put
        };

        static readonly string[] pronConsonants =
        {
            "tʃ", //  church, match, nature
            "dʒ", //  judge, age, soldier

            "p", //   pen, copy, happen
            "b", //   back, baby, job
            "t", //   tea, tight, button
            "d", //   day, ladder, odd
            "k", //   key, clock, school
            "g", //   get, giggle, ghost

            "f", //   fat, coffee, rough, photo
            "v", //   view, heavy, move
            "θ", //   thing, author, path
            "ð", //   this, other, smooth
            "s", //   soon, cease, sister
            "z", //   zero, music, roses, buzz
            "ʃ", //   ship, sure, national
            "ʒ", //   pleasure, vision
            "h", //   hot, whole, ahead

            "m", //   more, hammer, sum
            "n", //   nice, know, funny, sun
            "ŋ", //   ring, anger, thanks, sung

            "l", //   light, valley, feel
            "r", //   right, wrong, sorry, arrange

            "j", //   yet, use, beauty, few
            "w", //   wet, one, when, queen

            "ʔ", //   (glottal stop) department, football
        };

        static readonly Dictionary<string, string> proReadStyle = new Dictionary<string, string>() {
            {"i:","Đọc là i nhưng dài, nặng và nhấn mạnh.   Feet /fi:t/; 		See /si:/" },
            {"i","Đọc là i ngắn và dứt khoát.  Alien /eiliən/; 		Happy /’hæpi/" },
            {"ʊ","Đọc là u ngắn và dứt khoát.  Foot /fʊt/; 		Put /pʊt/" },
            {"u:","Đọc là u dài, nặng và nhấn mạnh. Food /fu:d/; 		Too /tu:/" },
            {"iə","Đọc là iơ hoặc là ia. Here /hiə(r)/; 		Near /niə(r)/" },
            {"ei","Đọc là êi hoặc ây.   Page /peidʒ/; 		Say /sei/" },
            {"e","Đọc là e.    Bed /bed/; 		Ten /ten/" },
            {"ə","Đọc là ơ ngắn và dứt khoát.  Ago /ə´gəʊ/; 		Never /´nevə(r)/" },
            {"ɜ:","Đọc là ơ dài, nặng, nhấn mạnh. Bird /bɜ:d/; 		Nurse /nɜ:s/" },
            {"ɔ:","Đọc là o dài, nặng và nhấn mạnh. Saw /sɔ:/; 		Short /ʃɔ:t/" },
            {"ʊə","Đọc là uơ hoặc ua.   Pure /pjʊə(r)/; 		Tour /tʊə(r)/" },
            {"ɔi","Đọc là oi trong Tiếng Việt. Boy /bɔi/; 		Join /dʒɔin/" },
            {"əʊ","Đọc là âu.   Home /həʊm/; 		Low /ləʊ/" },
            {"æ","Đọc là ea nối liền nhau và nhanh. Bad /bæd/; 		Hat /hæt/" },
            {"ʌ","Đọc là â.    Cup /cʌp/; 		Drum /drʌm/" },
            {"a:","Đọc là a nhưng dài, nặng, nhấn mạnh. Arm /ɑ:m/; 		Fast /fɑ:st/" },
            {"ɒ","Đọc là o ngắn và dứt khoát.  Got /ɡɒt/; 		Shot /ʃɒt/" },
            {"eə","Đọc là eơ liền nhau, nhanh, ơ hơi câm.   Care /keə(r)/; 		Hair /heə(r)/" },
            {"ai","Đọc là ai.   Five /faiv/; 		Sky /skai/" },
            {"aʊ","Đọc là ao.   Flower /´flaʊə(r)/; 		Now /naʊ/" },
            {"p","Đọc là p ngắn và dứt khoát.  Pen /pen/; 		Soup /su:p/" },
            {"b","Đọc là b ngắn và dứt khoát.  Bad /bæd/; 		Web /web/" },
            {"t","Đọc là t ngắn và dứt khoát.  Dot /dɒt/; 		Tea /ti:/" },
            {"d","Đọc là đ ngắn và dứt khoát.  Did /did/; 		Stand /stænd/" },
            {"tʃ","Đọc là ch.   Chin /tʃin/; 		Match /mætʃ/" },
            {"dʒ","Đọc là jơ(uốn lưỡi) ngắn và dứt khoát.  June /dʒu:n/ ; 		Page /peidʒ/" },
            {"k","Đọc là c.    Cat /kæt/; 		Desk /desk/" },
            {"g","Đọc là g.    Bag /bæg/; 		Got /ɡɒt/" },
            {"f","Đọc là f.    Fall /fɔ:l/; 		Safe /seif/" },
            {"v","Đọc là v.    Voice /vɔis/; 		Wave /weiv/" },
            {"θ","Đọc là th.   Thing /θɪn/; ð Đọc là đ    Bathe /beið/; 		Then /ðen/" },
            {"s","Đọc là s nhanh, nhẹ, phát âm gió. Rice /rais/; 		So /səʊ/" },
            {"z","Đọc là z nhanh, nhẹ. Rose /rəʊz/; 		Zip /zip/" },
            {"ʃ","Đọc là s nhẹ(uốn lưỡi), hơi gió.    She /ʃi:/; 		Wash /wɒʃ/" },
            {"ʒ","Đọc là giơ nhẹ, phát âm ngắn. Measure /´meʒə/; 		Vision /´viʒn/" },
            {"m","Đọc là m.    Man /mæn/; 		Some /sʌm/" },
            {"n","Đọc là n.    No /nəʊ/; 		Mutton /´mʌtn/" },
            {"ŋ","Đọc là ng nhẹ và dứt khoát.  Singer /´siŋə/; 		Tongue /tʌŋ/" },
            {"h","Đọc là h.    How /haʊ/; 		Who /hu:/" },
            {"l","Đọc là l(lờ).   Leg /leg/; 		Metal /´metl/" },
            {"r","Đọc là r.    Red /red/; 		Train /trein/" },
            {"w","Đọc là qu.   Wet /wet/; 		Why /wai/" },
            {"j","Đọc như chữ z(nhấn mạnh).   Menu /´menju:/;             Hoặc kết hợp với chữ u → ju → đọc iu    Yes /jes/" },
         };

        #region [ METHOD ]

        public static string[] f_get_Vowels() { return pronVowels; }
        public static string[] f_get_Consonants() { return pronConsonants; }

        public static string f_get_PronunceOnline(string word_en)
        {

            using (WebClient w = new WebClient())
            {
                //w.Encoding = Encoding.UTF8; 
                string htm = w.DownloadString("https://en.wiktionary.org/wiki/" + word_en);
                //htm = HttpUtility.HtmlDecode(htm);

                if (htm.IndexOf(@"<span class=""IPA"">") != -1)
                    return htm.Split(new string[] { @"<span class=""IPA"">" }, StringSplitOptions.None)[1].Split('/')[1];
            }

            return string.Empty;
        }

        public static string f_get_PronunceMP3(string pronunce)
        {
            string fi_name = pronunce;
            if (pronunce.EndsWith(":"))
                fi_name = pronunce.Replace(':', 'L');

            string file = "pronunce/" + fi_name + ".mp3";
            if (File.Exists(file)) {
                return file;
            }
            else
            {
                try
                {
                    using (WebClient w = new WebClient())
                    {
                        //w.Encoding = Encoding.UTF8; 
                        string htm = w.DownloadString("https://en.wiktionary.org/wiki/" + pronunce);
                        //htm = HttpUtility.HtmlDecode(htm);
                        if (htm.IndexOf(@"<source src=") != -1)
                        {
                            string[] a = htm.Split(new string[] { @"<source src=" }, StringSplitOptions.None);
                            if (a.Length > 3)
                                a = a.Where(x => x.Contains("-uk-")).ToArray();

                            //htm = htm.Split(new string[] { @"<source src=" }, StringSplitOptions.None)[1].Substring(1).Split('"')[0];
                            string url = a[2].Substring(1).Split('"')[0];
                            if (url.StartsWith("http") == false) url = "https:" + url;
                            using (var client = new WebClient())
                                client.DownloadFile(url, file);
                            return file;
                        }
                    }
                }
                catch { }
            }

            return string.Empty;
        }

        #endregion

        #region [ API ]

        public bool Open { get; set; } = false;

        public void Close() { }

        public msg Execute(msg m)
        {
            return m;
        }

        public void Init()
        {
        }

        #endregion
    }
}
