using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    [ProtoContract]
    public class oMediaSearchLocalResult
    {
        [ProtoMember(1)]
        public int TotalItem { set; get; } = 0;

        [ProtoMember(2)]
        public int CountResult { set; get; } = 0;

        [ProtoMember(3)]
        public int PageNumber { set; get; } = 1;

        [ProtoMember(4)]
        public int PageSize { set; get; } = 10;

        [ProtoMember(5)]
        public List<long> MediaIds = new List<long>();

        public oMediaSearchLocalResult clone() { 
           return Serializer.DeepClone<oMediaSearchLocalResult>(this);
        }
    }
}
