using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{ 
    public class api_nodeStore : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<long, oNode> dicItems = new Dictionary<long, oNode>();

        public static void Add(oNode node)
        {
            lock (_lock)
                dicItems.Add(node.id, node);
        }
        public static void Adds(oNode[] nodes)
        {
            lock (_lock)
            {
                for (int i = 0; i < nodes.Length; i++)
                    if (!dicItems.ContainsKey(nodes[i].id))
                        dicItems.Add(nodes[i].id, nodes[i]);
            }
        }

        public static oNode Get(long id)
        {
            lock (_lock)
                if (dicItems.ContainsKey(id))
                    return dicItems[id];
            return null;
        }

        public msg Execute(msg msg)
        {
            return msg;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

    }

}
