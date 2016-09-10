using System;

using Xamarin.Forms;

namespace p3XamarinApp
{
	public class App : Application
	{
        public static IAuthenticate Authenticator { get; private set; }
		public App ()
		{
			// The root page of your application
			MainPage = new TodoList();
		}
        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

