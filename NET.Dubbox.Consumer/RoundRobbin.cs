using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Dubbox.Consumer
{

    public class RoundRobbin
    {

        List<String> _hosts;
        int _index = 0;
        int _count = 0;

        public RoundRobbin(List<string> hosts)
        {
            if (hosts == null)
            {
                throw new ArgumentException("参数为空");
            }
            else
            {
                this._hosts = hosts;
                this._count = hosts.Count;
            }
        }


        /// <summary>
        /// 获取host的数量
        /// </summary>
        /// <returns></returns>
        public int getHostCount()
        {
            return _count;
        }




        /// <summary>
        /// 获取host
        /// </summary>
        /// <returns></returns>
        public string GetHost()
        {
            if (_count == 0)
            {
                return null;
            }
            else
            {
                _index = (_index + 1) % _count;
                return _hosts[_index];
            }


        }


    }
}
