package com.Test2;

import com.Test2.TestInterface2.TestCallBack;
import com.Test2.TestInterface2._TestCallBackPrxI;
import com.zeroc.Ice.Current;

public  class ClientImplement implements TestCallBack {

    @Override
    public void SendMessageToClient(String msg, Current current) {
        System.out.println("收到消息:"+msg);
    }

    @Override
    public void CheckAlive(Current current) {
        System.out.println("check alive");
    }
}
