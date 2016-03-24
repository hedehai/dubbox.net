using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRegister
{
    public class DubboxService
    {
        public string ServiceName { get; set; }

        public List<string> MethodNames { get; set; }


        public DubboxService()
        {
            this.MethodNames = new List<string>();
        }

    }
}