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

        static readonly Dictionary<string,string> proExam = new Dictionary<string, string>() {
            { "eɪ", "age,aid,able,agent,angel,alien,cake,rain,paid,faith,same,safe,wave,afraid,attain,famous,remain,dangerous,potato,eight,weigh,sleigh,day,play,say,stay,way,away,display,okay,today,holiday,yesterday" },
            { "æ", "act,ask,at,admit,after,answer,apple,adventure,avenue,application,chance,class,plan,staff,attack,classic,exam,standard,faculty,international" },
            { "i", "east,eat,easy,equal,even,dream,keep,seem,team,believe,machine,police,succeed,material,serious,free,tea,agree,body,degree,movie,agency,employee,ability,reality" },
            { "ɛ", "end,any,enter,entire,extend,except,educate,emphasize,energy,excellent,bed,friend,guess,said,yes,again,many,reflect,unless,memory" },
            { "ɑɪ", "eye,i,ice,ideal,island,item,isolate,child,kind,smile,type,define,device,private,science,analyze,exercise,die,fly,shy,sky,why,deny,nearby,supply,classify,identify" },
            { "ɪ", "ill,it,impose,infant,instant,into,issue,incident,initial,impossible,gift,limb,since,admit,building,commit,listen,window,history,minimum" },
            { "oʊ", "old,owe,own,ocean,only,open,over,occasionally,both,goes,most,road,going,moment,promote,suppose,total,episode,go,snow,throw,ago,below,follow,hero,shadow,potato,studio" },
            { "ɑ", "" },
            { "", "" },





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

        static readonly Dictionary<string,string> proRegex = new Dictionary<string, string>() {
            {"ɛ", "said,says,friend,guest,again" },
            {"ɪ", "give,busy,building" },
        };

        static readonly string[] proRule = {
            @"eɪ: a_e; (vowel-consonant-e), When the letter 'a' is followed by a single consonant and then the letter 'e,' the letter 'a' is usually pronounced as /eɪ/. Example: same, cake, safe",
            @"eɪ: -ai-; The 'ai' spelling is a fairly common spelling for the mid-word 'long a' sound. Example: rain, paid, faith ",
            @"eɪ: ay; Example: play, say, way",
            @"eɪ: eigh; Example: eight, weigh, sleigh ",
            @"æ:  -a-; (CVC) Example: ask, bath, class ",
            @"i:  ee; The 'ee' spelling can occur in the middle or end of a word. Example: keep, free, seem",
            @"i:  ea; The 'ea' spelling can be used to spell the 'long e' /i/ or 'short e' /ɛ/ (as in the word 'dead' /dɛd/). Example: each, dream, tea",
            @"i:  ie_e; Example: piece , grieve , achieve",
            @"i:  ie; The 'ie' spelling can also be pronounced as the 'long i' /ɑɪ/, as in the word 'pie' /pɑɪ/, or even occasionally as two adjacent vowel sounds, as in the words 'quiet' /kwɑɪət/ and 'science' /sɑɪəns/. Example: brief, priest, field,",
            @"i:  -y; Example: pony, happy, body",
            @"i:  -e; The '-e' spelling is pronounced as the /i/ only in single-syllable words where the 'e' is the final and only vowel in the spelling of the word. Example: me, we, she, he",
            @"ɛ:  -e-; (CVC) Example: bed, send, yes",
            @"ɛ:  ea*; NOTE: The 'ea' spelling is more commonly pronounced 'long e' /i/ (as in 'team'). Example: head, bread, heaven ",
            @"ɑɪ: i_e; When the letter 'i' is followed by a single consonant and then the letter 'e,' the letter 'i' is usually pronounced as /ɑɪ/. Example: ice,hide,smile",
            @"ɑɪ: igh; Example: ,light,bright,sigh",
            @"ɑɪ: ie; Along with the /ɑɪ/ pronunciation, the 'ie' spelling has words that are pronounced as 'long e' /i/ (such as the word 'brief' /brif/. It is also occasionally pronounced as two adjacent vowel sounds(such as the words 'quiet' /kwɑɪ ət/ and 'science' /sɑɪ əns/). Consult a dictionary to be certain of the pronunciation of unfamiliar words spelled 'ie.' Example: tie,pie,die",
            @"ɑɪ: -y; The letter 'y' at the end of a word can also be pronounced as a 'long e' /i/ sound, as in the word 'happy' /hæpi/ Example: shy,sky,dry",
            @"ɑɪ: -y-; The letter 'y' in the middle of a word has two common pronunciations, the /ɑɪ/ and the 'short i' /ɪ/ as in the word 'symbol' /sɪmbəl/. Example: cycle,dynamic,hyper",
            @"ɪ:  -i- (CVC); The 'short i' pronunciation can be spelled 'consonant-i-consonant' or With the letter 'y'. Example: it,lip,spin",
            @"ɪ:  -y- Example: myth, symbol, system",
            @"ɪ:  In English pronunciation, /ɪ/ does not occur at the end of words! Example: ",
            @"oʊ: o_e(vowel-consonant-e); When the letter 'o' is followed by a single consonant and then the letter 'e,' the letter '' is usually pronounced as /oʊ/. Example: joke,rope,vote",
            @"oʊ: oa; Example: boat,soap,toast",
            @"oʊ: ow; The 'ow' spelling can also be used for the 'ow sound' /aʊ/ pronunciation, such as in the word 'cow' /kaʊ/. Example: snow,own,known",
            @"oʊ: -o-; The single letter 'o' can be an extremely difficult spelling to manage when learning English pronunciation because, in addition to the /oʊ/, it is also used to spell the 'short o' as in the word 'top' /tɑp/ and the 'aw sound' (as in the word 'dog' /dɔg/). Example: most,go,both",

            @"ɑ:    Example: ",
            @":    Example: ",
            @":    Example: ",
            @":    Example: ",
        }; 
 


        static readonly Dictionary<string, string> proTutorial = new Dictionary<string, string>() {
            // Vowel Sounds
            { "eɪ", @"The 'long a' /eɪ/ is a 2-sound vowel. It is the middle sound in the word 'cake' /keɪk/.
To pronounce the sound, begin with the tongue pushed somewhat forward but in a neutral position in the mouth. Then, as the jaw closes slightly, move the body of the tongue upward until it is near the tooth ridge--similar to the position of a 'y sound' /y/. The front sides of the tongue touch the inside of the top teeth at the end of the sound." },
            { "æ", @"The front of the tongue is pushed further forward and is held lower in the mouth when forming the 'short a' /æ/ sound than with any other vowel sound. The tip of the tongue will touch the inside of the bottom front teeth. The body of the tongue is rounded slightly upward. The jaw is lowered and the lips are held apart, allowing the entire oral cavity to remain open." },
            { "i", @"The tongue is forward, with the body of the tongue near the tooth ridge. (The tongue is higher in the mouth for this sound than for any other vowel in English.) Because the tongue is so high, the jaw is relatively closed during the 'long e' /i/ sound. The sides of the tongue touch the top, side teeth during the sound. NOTE: This sound is very similar to a 'y sound' /y/." },
            { "ɛ", @"The 'short e' /ɛ/ sound is a relatively relaxed vowel sound. The middle of the tongue rounds slightly upward and the sides of the tongue may lightly touch the top and bottom side teeth. The lips and jaw are loose and relaxed." },
            { "ɑɪ", @"The 'long i' /ɑɪ/ is a 2-sound vowel that ends in a brief 'y sound' /y/. Part 1: At the beginning of the sound, the tongue is low and touches the bottom, side teeth. Part 2: As the jaw closes slightly, the body of the tongue moves upward until it is near the tooth ridge, similar to the position of a 'y sound' /y/. The front sides of the tongue touch the inside of the top, side teeth." },
            { "ɪ", @"The lips are relaxed and the central/front area of the tongue is in the central/high area of the mouth for this sound. The overall neutrality and relaxed tongue and lip position is why it is one of the pronunciations used in an unstressed vowel position." },
            { "oʊ", @"The 'long o' /oʊ/ is a two-sound vowel that ends in a brief 'w sound' /w/. Part 1: The body of the tongue is pushed back and in a low-to-mid mouth position and the bottom teeth can be felt along the sides of the tongue. Part 2: The sound moves into a 'w sound' /w/ by raising the jaw slightly while closing the lips into a small circle. The body of the tongue moves upward until the tongue is near the back of the hard palate." },
            { "ɑ", @"" },
            { "", @"" },
            { "", @"" },

            // Consonant Sounds
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
            
            // R-controlled vowels
            { "ɛr", @"The 'air sound' /ɛr/ is an r-controlled vowel. Technically this sound is two distinct sounds (vowel sound+'r sound' /r/). It is presented here under the name 'air sound' to distinguish the fact that the vowel portion of the sound is different from the 'long a' /eɪ/ (although the sound's spellings are very similar to those for the 'long a' /eɪ/).
The 'air sound' /ɛr/ begins with the tongue rounded slightly upward in the middle of the mouth. The sides of the tongue may lightly touch the bottom teeth during the formation of the beginning of this sound.
To transition to the /r/ portion of the sound, the body of the tongue moves upward and forward. The mid-section of the tongue raises so the sides of the tongue touch the mid-side teeth. The air travels over the body of the tongue to create the /r/ portion of the sound. (NOTE: The secondary method of producing the /r/ may be used to produce second portion of the 'air sound' /ɛr/ instead of this technique.)"},
            { "ɔr", @"The 'or sound' /ɔr/ is an r-controlled vowel. Technically this sound is two distinct sounds (vowel sound+r sound). It is presented here under the name 'or sound' /ɔr/ to distinguish the fact that the vowel portion of the sound is different from the various pronunciations commonly used for the 'o' spelling.
The 'or sound' /ɔr/ begins with lips rounded. The tongue is pushed back and held middle-low in the mouth, similar to the beginning of a 'long o' /oʊ/ sound. Although the International Phonetic Alphabet transcription shows that the or sound begins with the symbol used for the 'aw sound' /ɔ/, note that the tongue is not as far back as it is during that sound.
To transition to the /r/ portion of the sound, the lips relax and the body of the tongue moves upward and forward. The mid-section of the tongue raises so the sides of the tongue touch the middle teeth. The air travels over the body of the tongue to create the /r/ portion of the sound. (NOTE: The secondary method of producing the /r/ may be used to produce second portion of the /ɔr/ instead of this technique.)" },
            { "ɑr", @"The 'ar sound' /ɑr/ is an r-controlled vowel. Technically this sound is two distinct sounds (vowel sound+'r sound' /r/). It is presented with the name 'ar sound' /ɑr/ to distinguish the fact that the vowel portion of the sound is different from the various pronunciations commonly used for the letter 'a' spelling.
The 'ar sound' /ɑr/ begins with the tongue in the position of a 'short o' /ɑ/ sound. The tip and center of the tongue are set low, inside the bottom teeth. The top of the tongue is nearly even with the top of the bottom teeth.
To transition to the 'r' portion of the sound, the body of the tongue moves upward. The mid-section of the tongue rises so the sides of the tongue touch the middle teeth. The air travels over the body of the tongue to create the /r/ portion of the sound. (NOTE: The secondary method of producing the /r/ may be used to produce second portion of the 'ar sound' /ɑr/ instead of this technique.)" },
            { "ɚ", @"'Schwa+r' /ɚ/ is an r-controlled vowel. This strange sound is created the same way as the 'r sound' /r/, and therefore has the same two options available for pronouncing it.
The first option is to raise the back of the tongue so that the sides of the tongue touch the back teeth. The center of the back of the tongue is lower and the air travels through this groove to create the sound.
Alternatively, the tip of the tongue can be raised and curled back behind the tooth ridge while the back of the tongue stays low. The air still travels over the back of the tongue, but moves more along the sides and tip.
The tip of the tongue never touches the tooth ridge during the American English 'schwa+r' /ɚ/ or 'r sound' /r/." },
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
