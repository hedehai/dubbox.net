using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZookeeperExplorer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (ConfigurationManager.AppSettings["zookeeperHost"] == null)
            {
                MessageBox.Show("找不到zookeeperHost配置项");
                Application.Exit();
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
