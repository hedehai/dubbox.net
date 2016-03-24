using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRegister
{
 
    [AttributeUsage(AttributeTargets.Class)]
    public class DubboxServiceAttribute : Attribute
    {

        public string ServiceName { get; set; }

      


    }

}