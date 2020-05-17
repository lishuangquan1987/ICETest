using Ice;
using IceInternal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestInterface1;

namespace ICEServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var communicator = Ice.Util.initialize())
            {
                //储存连接的客户端
                List<TestInterface2.TestCallBackPrx> clients = new List<TestInterface2.TestCallBackPrx>();
                //创建一个适配器，其中"test"名称随便写
                var adapter = communicator.createObjectAdapterWithEndpoints(
                    "test", "default -h localhost -p 10005");
                TestImplement testImplement = new TestImplement();
                //当客户端调用Register方法是会触发RegisterEvent事件，此时服务端应该在该处理事件中建立调用客户单的代理管道，
                //以保证服务端可以主动调用客户端的方法
                testImplement.RegisterEvent += address =>
                {
                    //注意：地址前面加上client:是因为要与客户端的Indentity名称一致
                    var proxy = communicator.stringToProxy("client:" + address);
                    Console.WriteLine("address："+address+"连接上了服务器。");
                    TestInterface2.TestCallBackPrx client = TestInterface2.TestCallBackPrxHelper.checkedCast(proxy);
                    clients.Add(client);
                      
                };
                //这里的服务端指定的Indetntity名称"testIndentity"，客户端连接服务端时，必须在地址前面加上此标识才能连接
                adapter.add(testImplement, communicator.stringToIdentity("testIndentity"));
                //启动服务，等待客户端连接
                adapter.activate();

                Console.WriteLine("按住Ctrl+C结束...");
               
                Console.CancelKeyPress += (sender, e) => Environment.Exit(Environment.ExitCode);
                while (true)
                {
                    Console.WriteLine("请输入信息：发送心跳请输入0，按回车发送");
                    string msg = Console.ReadLine();
                    if (msg == "0")
                    {
                        clients.ForEach(x => x.CheckAlive());
                    }
                    else
                    {
                        clients.ForEach(x => x.SendMessageToClient(msg));
                    }
                }
            }
        }

    }
    public class TestImplement : TestDisp_
    {
        public event Action<string> RegisterEvent;
        int count = 0;
        public override void CheckAlive(Current current = null)
        {
            count++;
            Console.WriteLine("check alive:"+count);
        }

        public override void SendMessageToServer(string msg, Current current = null)
        {
            Console.WriteLine("收到消息："+msg);
        }
        public override void Register(string address, Current current = null)
        {
            if (RegisterEvent != null)
            {
                RegisterEvent(address);
            }
        }
    }

}
