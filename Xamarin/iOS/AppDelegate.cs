using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;

namespace p3XamarinApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate,IAuthenticate
	{
        private MobileServiceUser user;
               
        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(
                    UIApplication.SharedApplication.KeyWindow.RootViewController, 
                    MobileServiceAuthenticationProvider.MicrosoftAccount);
                if (user != null)
                {
                    message = $"You are Signed-In as {user.UserId}";
                    success = true;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            // IMPORTANT: uncomment this code to enable sync on Xamarin.iOS
            // For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
            SQLitePCL.CurrentPlatform.Init();
            App.Init(this);

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

