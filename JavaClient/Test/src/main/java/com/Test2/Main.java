package com.Test2;

        import com.Test2.TestInterface1.TestPrx;
        import com.zeroc.Ice.*;
        import com.zeroc.Ice.Object;

        import java.util.Scanner;

public class Main {
    public  static  void  main(String[] args)
    {
        Communicator communicator = Util.initialize();
        ObjectPrx objectPrx = communicator.stringToProxy("testIndentity:default -h localhost -p 10005");
        TestPrx testPrx = TestPrx.checkedCast(objectPrx);

        String address="default -h localhost -p 10007";
        ObjectAdapter adapter = communicator.createObjectAdapterWithEndpoints("client", address);
        ClientImplement clientImplement=new ClientImplement();
        Identity identity=new Identity();
        identity.name="client";
        adapter.add((Object) clientImplement,identity);
        adapter.activate();

        testPrx.Register(address);
        Scanner scanner=new Scanner(System.in);
        while (true)
        {
            System.out.println("请输入要发送的字符串，心跳请发送0");
            String msg=scanner.nextLine();
            if (msg.equals("0"))
            {
                testPrx.CheckAlive();
            }
            else
            {
                testPrx.SendMessageToServer(msg);
            }
        }
    }
}
