module TestInterface1
{
	interface Test
	{
		void SendMessageToServer(string msg);
		void CheckAlive();
		//客户端调用服务端完成注册
		void Register(string address);
	};
};
module TestInterface2
{
	interface TestCallBack
	{
		void SendMessageToClient(string msg);
		void CheckAlive();
	};
};