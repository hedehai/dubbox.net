using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooKeeperNet;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //创建一个Zookeeper实例，第一个参数为目标服务器地址和端口，第二个参数为Session超时时间，第三个为节点变化时的回调方法 
            using (ZooKeeper zk = new ZooKeeper("172.16.0.85:2181", new TimeSpan(0, 0, 0, 50000), new Watcher()))
            {
                var stat = zk.Exists("/root", true);

                if (stat == null)
                {
                    ////创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
                    zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);



                }

                //在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
                zk.Create("/root/childone", "childone".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                //取得/root节点下的子节点名称,返回List<String> 

                IEnumerable<string> children = zk.GetChildren("/root", true);

                //取得/root/childone节点下的数据,返回byte[] 
                //    zk.GetData("/root/childone", true, null);


                //修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
                zk.SetData("/root/childone", "childonemodify22".GetBytes(), -1);


                //删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
                //  zk.Delete("/root/childone", -1);


            }






        }

        private void button2_Click(object sender, EventArgs e)
        {
            ZooKeeper zk = new ZooKeeper("172.16.0.85:2181", new TimeSpan(0, 0, 0, 5), new Watcher());

            zk.Exists("/root/childone", new Watcher());

            //    zk.Create("/root/childone/child_ha", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);



            //  zk.SetData



            //zk.Dispose(); //退出


        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (ZooKeeper zk = new ZooKeeper("172.16.0.85:2181", new TimeSpan(0, 1, 0), new Watcher()))
            {
                var stat = zk.Exists("/dubbo/userService", new Watcher());
                if (stat == null)   // 创建永久性结点
                {
                    zk.Create("/dubbo/userService", "user service".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/userService/consumers", "consumers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/userService/routers", "routers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/userService/providers", "providers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/userService/configurators", "configurators".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }

            }
        }





    }
}
