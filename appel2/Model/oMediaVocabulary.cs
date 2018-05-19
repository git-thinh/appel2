using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    [ProtoContract]
    public class oMediaVocabulary
    {
        [ProtoMember(1)]
        public string Word { get; set; }

        [ProtoMember(2)]
        public string Pronounce { get; set; }

        [ProtoMember(3)]
        public string ShortMeaningVi { get; set; }
    }

    [ProtoContract]
    public class oMediaVocabularyMeaning
    {
        [ProtoMember(1)]
        public string Word { get; set; }
        
        [ProtoMember(2)]
        public string MeaningEn { get; set; }

        [ProtoMember(3)]
        public string MeaningVi { get; set; }
    }
}
