using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace WebApiRegister
{
    public class Watcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDataChanged)
            {
                Console.WriteLine(@event.Path);
            }


        }
    }
}
