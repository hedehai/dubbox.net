using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRegister
{


    [AttributeUsage(AttributeTargets.Method)]
    public class DubboxMethodAttribute : Attribute
    {
        public string MethodName { get; set; }

    }
}