module TestInterface1
{
	interface Test
	{
		void SendMessageToServer(string msg);
		void CheckAlive();
		//�ͻ��˵��÷�������ע��
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