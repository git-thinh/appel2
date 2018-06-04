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
        #region [ CONST ]

        const string lip3 = "The front of the tongue (including the tip) and the tooth ridge: The tooth ridge is the bony bump directly behind the top front teeth (behind the tooth ridge is the hard palate). Accuracy of tongue position in relation to the tooth ridge is necessary.";
        static readonly Dictionary<string, string> proTutorial = new Dictionary<string, string>() {
            { "p", "The lips: The sound is created by pressing the lips together" },
            { "b", "The lips: The sound is created by pressing the lips together" },
            { "m", "The lips: The sound is created by pressing the lips together" },
            { "f", "The lips: The sound requires interaction between the bottom lip and the top teeth." },
            { "v", "The lips: The sound requires interaction between the bottom lip and the top teeth." },
            { "ð", "The 'th sound' /ð/, The tip of the tongue and the front teeth: The unvoiced th and voiced th sounds are created by controlling how the close the tip of the tongue is to the front teeth." },
            { "θ", "The 'th sound' /θ/, The tip of the tongue and the front teeth: The unvoiced th and voiced th sounds are created by controlling how the close the tip of the tongue is to the front teeth." },
            { "t", lip3 },
            { "d", lip3 },
            { "j", lip3 },
            { "s", lip3 },
            { "z", lip3 },
            { "l", lip3 },
            { "n", lip3 },
            { "ʧ", "The 'ch sound' /ʧ/, " + lip3 + " \r\n The 'ch sound' /ʧ/ is unvoiced (the vocal cords do not vibrate during its production), and is the counterpart to the voiced 'j sound' /ʤ/. \r\n To create the /ʧ/, air is briefly prevented from leaving the vocal tract when the tip of the tongue presses against the back tooth ridge while the sides of the tongue press against the upper side teeth. The sound is aspirated when the air is released with friction (similar to the friction of an sh sound). \r\n Many English language learning pronunciation students often find it helpful to think of the /ʧ/ as stopping the air similar to a 't sound' (but with the tongue a bit further back on the tooth ridge), and then releasing it with the friction of an 'sh sound' /ʃ/."},
            { "ʃ", "The 'sh sound' /ʃ/, " + lip3 + " \r\n The 'sh sound' /ʃ/ is unvoiced (the vocal cords do not vibrate during its production), and is the counterpart to the voiced 'zh sound' /ʒ/. \r\n To create the /ʃ/, air is forced between a wide groove in the center of the front of the tongue and the back of the tooth ridge. The sides of the blade of the tongue may touch the side teeth. The lips are kept slightly tense, and may protrude somewhat during the production of the sound. \r\n This sound is a continuous consonant, meaning that it should be capable of being produced for a few seconds with even and smooth pronunciation for the entire duration."},
            { "ʤ", "The 'j sound' /ʤ/, " + lip3 },
            // The words have lastest characters are 'ge': beige, garage, massage 
            { "ʒ", "The 'zh sound' /ʒ/, " + lip3 + " \r\n The 'zh sound' /ʒ/ is voiced (the vocal cords vibrate during its production), and is the counterpart to the unvoiced 'sh sound' /ʃ/. \r\n To create the /ʒ/, air is forced between a wide groove in the center of the front of the tongue and the back of the tooth ridge. The sides of the blade of the tongue may touch the side teeth. The lips are kept slightly tense, and may protrude somewhat during the production of the sound. \r\n The /ʒ/ is a continuous consonant, meaning that it should be capable of being held for a few seconds with even and smooth pronunciation for the entire duration."},
         };

        static readonly Dictionary<string, string> proExamples = new Dictionary<string, string>() {
            { "p", "" },
            { "b", "" },
            { "m", "" },
            { "f", "" },
            { "v", "" },
            { "ð", "" },
            { "θ", "" },
            { "t", "" },
            { "d", "" },
            { "j", "" },
            { "s", "" },
            { "z", "" },
            { "l", "" },
            { "n", "" },
            { "ʧ", "" },
            { "ʃ", "" },
            { "ʤ", "" },
            // The words have lastest characters are 'ge': beige, garage, massage 
            { "ʒ", "" },
         };


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

        #endregion

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
            if (File.Exists(file))
            {
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
