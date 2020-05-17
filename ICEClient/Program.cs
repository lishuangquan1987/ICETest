using Ice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICEClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var communicator = Ice.Util.initialize())
            {
                //此处的testIndentity:与服务端的Identity一致，否则连接不到服务器
                var proxy = communicator.stringToProxy("testIndentity:default -h localhost -p 10005");
                var instance = TestInterface1.TestPrxHelper.checkedCast(proxy);

                //创建adapter监听端口
                string address = "default -h localhost -p 10006";
                var adapter = communicator.createObjectAdapterWithEndpoints("client", address);
                var callback = new ClientImplement();
                adapter.add(callback,new Identity() { name= "client" });
                //启动监听
                adapter.activate();
                //告知服务端客户端已经监听的地址，服务端可以连接到客户端
                instance.Register(address);

                Console.WriteLine("按住Ctrl+C退出.");
                while (true)
                {
                    Console.WriteLine("请输入发给客户端的消息，心跳输入0，按Enter发送");
                    string msg = Console.ReadLine();
                    if (msg == "0")
                    {
                        instance.CheckAlive();
                    }
                    else
                    {
                        instance.SendMessageToServer(msg);
                    }
                }
            }
        }
    }
    class ClientImplement : TestInterface2.TestCallBackDisp_
    {
        public override void CheckAlive(Current current = null)
        {
            Console.WriteLine("check alive");
        }

        public override void SendMessageToClient(string msg, Current current = null)
        {
            Console.WriteLine("收到消息：" + msg);
        }
    }
}
