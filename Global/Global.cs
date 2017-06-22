using System;

using lw.Threading;

namespace lw.Global
{
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
		}

		protected void Application_Start(Object sender, EventArgs e)
		{
			Routing.InitRouting();

			ThreadPool threadingPool = new ThreadPool();
			threadingPool.Init();
		}

		protected void Session_Start(Object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			//lw.Error.Handler.HandleError();
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
	}
}