
<h5>使用方法：</h5>
(1)启动zookeeper。<br />
(2)启动dubbox中的dubbo-demo-provider。为看到负载均衡的效果，请启动2个以上的实例。dubbo-demo-provider需要配置文件dubbo.properties中添加一个配置项：dubbo.protocol.host=172.16.0.105，它的作用是指定dubbo服务的主机名称，若不指定的话，dubbox会使用第一个网卡的地址，这很有可能会出错。<br />
(3)修改配置文件App.config中的zookeeperHost为你的zookeeper的地址和端口。<br />
(4)F5启动。<br />


