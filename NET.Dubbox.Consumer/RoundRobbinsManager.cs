using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace NET.Dubbox.Consumer
{
    /// <summary>
    /// 
    /// </summary>
    public class RoundRobbinsManager
    {
        private static Dictionary<String, RoundRobbin> _dictionary = new Dictionary<string, RoundRobbin>();
        private static RoundRobbinsManager _roundRobbinsManager = new RoundRobbinsManager();
        private static System.Timers.Timer _tmr = new System.Timers.Timer();
        private static ZooKeeper _zk;


        /// <summary>
        /// 
        /// </summary>
         static RoundRobbinsManager()
        {
            if (ConfigurationManager.AppSettings["zookeeperHost"] == null)
            {
                throw new ApplicationException("未找到配置节zookeeperHost");
            }
            else
            {
                _zk = new ZooKeeper(ConfigurationManager.AppSettings["zookeeperHost"], new TimeSpan(0, 0, 60), new Watcher());
                _tmr.Interval = 2000; // ms
                _tmr.Elapsed += _tmr_Elapsed;
                _tmr.Start();
                _tmr_Elapsed(null, null); //初次运行
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private RoundRobbinsManager()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateServiceList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RoundRobbinsManager GetInstance()
        {
            return _roundRobbinsManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public RoundRobbin this[string serviceName]
        {
            get
            {
                return _dictionary[serviceName];
            }
            set
            {
                _dictionary[serviceName] = value;
            }
        }



        /// <summary>
        ///更新服务列表，整个替换
        /// </summary>

        private static void updateServiceList()
        {
            Dictionary<String, RoundRobbin> newDictionary = new Dictionary<string, RoundRobbin>();
            try
            {
                List<string> rootChildren = (List<string>)_zk.GetChildren("/dubbo", false);
                foreach (var rootChild in rootChildren)
                {
                    List<string> children = (List<string>)_zk.GetChildren("/dubbo/" + rootChild + "/providers", false);

                    List<string> serviceHosts = new List<string>();
                    foreach (var child in children)
                    {
                        string childNode = System.Web.HttpUtility.UrlDecode(child);
                        int index1 = childNode.IndexOf("rest://");  // 查找rest://的位置
                        if (childNode.Contains("rest://") == true)
                        {
                            int index = childNode.IndexOf("/", 7); // “rest://”有7个字符
                            string host = childNode.Substring(7, index - 7);
                            serviceHosts.Add(host);
                        }
                    }
                    newDictionary.Add(rootChild, new RoundRobbin(serviceHosts));
                }
            }
            catch (Exception exp)
            {
                // TODO:日志
            }
            _dictionary = newDictionary;
        }





    }
}
