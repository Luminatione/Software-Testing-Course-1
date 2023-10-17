using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
	[TestClass]
	public class LoginControllerTest
	{
		private LoginController GetLoginController()
		{

		}

		[TestMethod]
		public void Login_ForEmptyUser_ThrowsArgumentException()
		{
			
			LoginController loginController = new LoginControllerTest();
		}
	}
}
