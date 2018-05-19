using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public enum CHANNEL_TYPE {
        YOUTUBE_CHANNEL,
        FACEBOOK_PAGE,
    }

    public class oMediaChannel
    {
        public string Id { set; get; }
        public string Title { set; get; }
        public CHANNEL_TYPE Type { set; get; }

        public List<long> ListMedia = new List<long>();

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Title);
        }
    }
}
