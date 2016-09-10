using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace p3XamarinApp.Droid
{
	[Activity (Label = "p3XamarinApp.Droid", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity,IAuthenticate
	{
        private MobileServiceUser user;
        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this,MobileServiceAuthenticationProvider.MicrosoftAccount);
                if (user != null)
                {
                    message = $"You are Signed-In as {user.UserId}";
                    success = true;
                }
            }
            catch(Exception e)
            {
                message = e.Message;
            }
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sigin-In Result");
            builder.Create().Show();

            return success;
        }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            App.Init(this);            
			LoadApplication (new App ());
		}
	}
}

