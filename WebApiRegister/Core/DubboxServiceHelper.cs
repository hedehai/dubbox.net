using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using ZooKeeperNet;
using System.Configuration;

namespace WebApiRegister
{
    public class DubboxServiceHelper
    {


        public static List<DubboxService> GetDubboxServices()
        {
            List<DubboxService> list = new List<DubboxService>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.BaseType == typeof(ApiController))  // 取得所有的控制器
                {
                    DubboxServiceAttribute classAttribute = type.GetCustomAttribute<DubboxServiceAttribute>();
                    if (classAttribute != null)
                    {
                        DubboxService dubboxService = new DubboxService();
                        dubboxService.ServiceName = classAttribute.ServiceName;

                        MethodInfo[] methods = type.GetMethods();
                        foreach (var item in methods)
                        {
                            DubboxMethodAttribute methodAttribute = item.GetCustomAttribute<DubboxMethodAttribute>();
                            if (methodAttribute != null)
                            {
                                dubboxService.MethodNames.Add(methodAttribute.MethodName);
                            }
                        }
                        list.Add(dubboxService);
                    }
                }
            }
            return list;
        }


        //节点名称弄成类似
        // rest://192.168.99.1:8888/services/com.alibaba.dubbo.demo.user.facade.UserRestService
        //?accepts=500
        //&anyhost=true
        //&application=demo-provider
        //&dubbo=2.0.0
        //&interface=com.alibaba.dubbo.demo.user.facade.UserRestService
        //&methods=getUser,registerUser
        //&organization=dubbox
        //&owner=programmer
        //&pid=27020
        //&server=tomcat
        //&side=provider
        //&threads=500
        //&timestamp=1458472133029
        //&validation=true

        public static void RegisterDubboxService()
        {
            ZooKeeper zk = new ZooKeeper("172.16.0.85:2181", new TimeSpan(0, 1, 0), new Watcher());

            List<DubboxService> dubboxServices = GetDubboxServices();

            foreach (var dubboxService in dubboxServices)
            {
                string serviceName = dubboxService.ServiceName;
                var stat = zk.Exists("/dubbo/userService", new Watcher());
                if (stat == null)   // 创建永久性结点
                {
                    zk.Create("/dubbo/" + serviceName, "user service".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/" + serviceName + "/consumers", "consumers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/" + serviceName + "/routers", "routers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/" + serviceName + "/providers", "providers".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                    zk.Create("/dubbo/" + serviceName + "/configurators", "configurators".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
                else //添加服务节点，临时
                {
                    String methods = "";
                    dubboxService.MethodNames.ForEach((method) =>
                    {
                        if (methods == "")
                        {
                            methods = method;
                        }
                        else
                        {
                            methods = methods + "," + method;
                        }
                    });
                    string nodeName = HttpUtility.UrlEncode(GetServiceHost() + "/services/" + serviceName + "?&application=demo-provider&methods=" + methods);

                    zk.Create("/dubbo/userService/providers/" + nodeName, "node".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);



                }
            }






        }


        public static String GetServiceHost()
        {
            if (ConfigurationManager.AppSettings["serviceHost"] == null)
            {
                throw new ApplicationException("没有配置serviceHost");
            }
            else
            {
                return ConfigurationManager.AppSettings["serviceHost"];
            }
        }





    }
}