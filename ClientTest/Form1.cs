using NET.Dubbox.Consumer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        RoundRobbinsManager _manager = RoundRobbinsManager.GetInstance();

        private void button1_Click(object sender, EventArgs e)
        {
            string host = _manager["com.alibaba.dubbo.demo.user.facade.AnotherUserRestService"].GetHost();
            Console.WriteLine(host);

            HttpClient client = new HttpClient();


            string url = "http://" + host + "/services/users/1.json"; //http://172.16.0.105:8888/services/users/1.json

            string result = client.GetStringAsync(url).Result;
            Console.WriteLine(result);



        }
    }
}
