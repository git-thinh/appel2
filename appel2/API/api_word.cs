using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace appel
{
    public class api_word : api_base, IAPI
    {
        static ConcurrentDictionary<string, string> dicWordLink = null;
        static ConcurrentDictionary<string, List<string>> dicPron = null;
        static List<string> listWord = null;
        static readonly object _lock = new object();
        const int page_size_default = 100;

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


        #region [ API ]

        public bool Open { set; get; } = false;
        
        public void Init()
        {
            dicWordLink = new ConcurrentDictionary<string, string>();
            listWord = new List<string>();

            var ws = Directory.GetFiles("words", "*.txt")
                .Select(x => Path.GetFileName(x).ToLower())
                .Select(x => x.Substring(0, x.Length - 4))
                .Distinct().ToArray();

            listWord.AddRange(ws);
            listWord.Sort();

            for (int i = 0; i < ws.Length; i++)
                dicWordLink.TryAdd(ws[i], string.Empty);

            dicPron = new ConcurrentDictionary<string, List<string>>();
        }

        public msg Execute(msg m)
        {
            if (m == null || Open == false) return m;
            switch (m.KEY)
            {
                case _API.WORD_KEY_INITED:
                    break;
                case _API.WORD_KEY_ANALYTIC:
                    #region 
                    break;
                    #endregion
            }
            return m;
        }

        public void Close()
        {
        }

        #endregion

        #region [ METHOD ]

        public static oWordCollectionResult f_get_Items(int page_number, int page_size = page_size_default)
        {
            int min = (page_number - 1) * page_size;
            lock (_lock)
            {
                if (listWord.Count > 0)
                {
                    if (listWord.Count > page_size_default)
                        return new oWordCollectionResult(listWord.Count, listWord.Count, page_number, page_size, listWord.Skip(min).Take(page_size).ToArray());
                    else
                        return new oWordCollectionResult(listWord.Count, listWord.Count, page_number, page_size, listWord.ToArray());
                }
            }
            return new oWordCollectionResult(0, 0, page_number, page_size, new string[] { });
        }

        public static oWordCollectionResult f_find_Items(string key, int page_number, int page_size = page_size_default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return f_get_Items(page_number, page_size);

            int min = (page_number - 1) * page_size;
            string[] rs = new string[] { };

            lock (_lock)
            {
                if (listWord.Count > 0)
                    rs = listWord.Where(x => x.Contains(key)).ToArray();
            }

            if (rs.Length > page_size_default)
                return new oWordCollectionResult(listWord.Count, rs.Length, page_number, page_size, rs.Skip(min).Take(page_size).ToArray());
            else
                return new oWordCollectionResult(listWord.Count, rs.Length, page_number, page_size, rs);
        }

        #endregion

    }

    public class oPronExam
    {
        public string Pronunciation { set; get; }
        public List<string> Words { set; get; }
    }

    public class oWordResult
    {
        public oWordLink[] WordLink { get; set; }
        public string MeanVi { get; set; }
        public string Pronunciation { get; set; }
        public string Type { get; set; }
        public string[] UrlAudio { get; set; }

        public oWordResult()
        {
            WordLink = new oWordLink[] { };
            MeanVi = string.Empty;
            Pronunciation = string.Empty;
            Type = string.Empty;
            UrlAudio = new string[] { };
        }
    }

    public class oWordLink
    {
        public string Word { get; set; }
        public string MeanVi { get; set; }
        public string Pronunciation { get; set; }
        public string Type { get; set; }

        public oWordLink()
        {
            Word = string.Empty;
            MeanVi = string.Empty;
            Pronunciation = string.Empty;
            Type = string.Empty;
        }
    }

    public class oWordCollectionResult
    {
        public string[] Words { get; }
        public int Counter { get; }
        public int Total { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int PageTotal { get; }

        public oWordCollectionResult(int total, int counter, int page_number, int page_size, string[] words)
        {
            Total = total;
            Counter = counter;
            PageNumber = page_number;
            PageSize = page_size;

            PageTotal = 0;
            if (counter > 0 && page_size > 0)
            {
                PageTotal = counter / page_size;
                if (counter % page_size == 0) PageTotal++;
                if (PageTotal == 0) PageTotal = 1;
            }

            Words = words;
        }
    }
}
