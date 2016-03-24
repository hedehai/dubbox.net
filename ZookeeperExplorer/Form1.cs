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
using Org.Apache.Zookeeper.Data;
using System.Configuration;

namespace ZookeeperExplorer
{
    public partial class Form1 : Form
    {

        ZooKeeper _zk;


        public Form1()
        {
            InitializeComponent();

            string zookeeperHost = ConfigurationManager.AppSettings["zookeeperHost"];

            _zk = new ZooKeeper(zookeeperHost, new TimeSpan(0, 0, 60), new Watcher());

            Refresh();

        }



        /// <summary>
        /// 刷新节点
        /// </summary>
        private void Refresh()
        {
            treeView1.Nodes.Clear(); // 清空

            try
            {
                TreeNodeCollection nodes = treeView1.Nodes;
                Recurs("/", nodes);

                treeView1.ExpandAll(); // 全部展开
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }



        /// <summary>
        /// 刷新节点按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }


        /// <summary>
        /// 递归函数
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodes"></param>
        void Recurs(string nodeName, TreeNodeCollection nodes)
        {
            List<string> children = (List<string>)_zk.GetChildren(nodeName, false);

            foreach (string child in children)
            {
                string nodeFullName; // 需要进行组合
                if (nodeName == "/")
                {
                    nodeFullName = nodeName + child;
                }
                else
                {
                    nodeFullName = nodeName + "/" + child;
                }

                TreeNode node = new TreeNode(child); // 以child的名称为树节点名称
                node.Tag = nodeFullName;
                nodes.Add(node);

                Recurs(nodeFullName, node.Nodes);
            }
        }



        /// <summary>
        /// 点击节点获取节点的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            textBox1.Clear();

            String zookeeperNode = e.Node.Tag.ToString();
            Stat stat = _zk.Exists(zookeeperNode, false);

            textBox1.AppendText("ctime:" + stat.Ctime + "\n");
            textBox1.AppendText("mtime:" + stat.Mtime + "\n");
            textBox1.AppendText("numChildren:" + stat.NumChildren + "\n");
            textBox1.AppendText("dataVersion:" + stat.Version + "\n");

            byte[] bytes = _zk.GetData(zookeeperNode, false, null);
            if (bytes != null)
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string data = encoding.GetString(bytes);
                textBox1.AppendText("\n");
                textBox1.AppendText("data:" + data + "\n");
            }
        }



        /// <summary>
        /// 关闭zk client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_zk != null)
            {
                _zk.Dispose();
            }

        }

    }
}
