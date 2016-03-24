using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.core;
using ZooKeeperNet;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {


        ZooKeeper _zk;

        List<string> _urls = new List<string>() { "url1", "url2", "url3" };
        int _i = 0;
        RoundRobbin _roundRobin;


        RoundRobbinsManager _manager = RoundRobbinsManager.GetInstance();



        public Form3()
        {
            InitializeComponent();

            _roundRobin = new RoundRobbin(_urls);

            _manager["UserService"] = new RoundRobbin(_urls);

            _zk = new ZooKeeper("172.16.0.85:2181", new TimeSpan(0, 0, 5000), new Watcher());

        }



        private void button1_Click(object sender, EventArgs e)
        {
            _i = (_i + 1) % 3;
            Console.WriteLine(_i + " " + _urls[_i]);
        }






        private void button2_Click(object sender, EventArgs e)
        {

            Console.WriteLine(_roundRobin.GetHost());
        }


        private void button3_Click(object sender, EventArgs e)
        {

            Console.WriteLine(_manager["UserService"].GetHost());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_manager["UserService"].GetHost());
        }


        /// <summary>
        /// 根据服务名获取rest服务的列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            List<string> serviceHosts = new List<string>();

            String nodeName = "/dubbo/com.alibaba.dubbo.demo.user.facade.UserRestService/providers";

            try
            {
                List<string> children = (List<string>)_zk.GetChildren(nodeName, new Watcher2());
                _zk.Exists(nodeName, new Watcher2());


                var str = _zk.ChildWatches;


                if (children != null)
                {
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
                }
            }
            catch (Exception exp)
            {

            }

        }
 











    }
}
