using Android.App;
using Android.Widget;
using Android.OS;

namespace TryRC_Android
{
	[Activity (Label = "TryRC_Android", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var sendSmsButton = FindViewById<Button> (Resource.Id.sendSmsButton);
			sendSmsButton.Click += delegate {
				sendSmsButton.Text = "clicked";
			};
		}
	}
}
