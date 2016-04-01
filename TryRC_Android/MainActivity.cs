using Android.App;
using Android.Widget;
using Android.OS;

namespace TryRC_Android
{
	[Activity (Label = "TryRC_Android", MainLauncher = true, 
		Icon = "@mipmap/icon", Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{
		private RingCentral.Platform platform;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// outlets
			var appKeyEditText = FindViewById<EditText> (Resource.Id.appKeyEditText);
			var appSecretEditText = FindViewById<EditText> (Resource.Id.appSecretEditText);
			var serverSpinner = FindViewById<Spinner> (Resource.Id.serverSpinner);
			var usernameEditText = FindViewById<EditText> (Resource.Id.usernameEditText);
			var passwordEditText = FindViewById<EditText> (Resource.Id.passwordEditText);
			var sendToEditText = FindViewById<EditText> (Resource.Id.sendToEditText);
			var messageEditText = FindViewById<EditText> (Resource.Id.messageEditText);
			var sendSmsButton = FindViewById<Button> (Resource.Id.sendSmsButton);

			// populate server spinner
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.servers_array, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			serverSpinner.Adapter = adapter;

			// load data 
			var preferencess = Application.Context.GetSharedPreferences("TryRC_Android", 
				Android.Content.FileCreationMode.Private);
			appKeyEditText.Text = preferencess.GetString("appKey", null) ?? "";
			appSecretEditText.Text = preferencess.GetString ("appSecret", null) ?? "";
			serverSpinner.SetSelection (preferencess.GetInt ("server", 0));
			usernameEditText.Text = preferencess.GetString("username", null) ?? "";
			passwordEditText.Text = preferencess.GetString ("password", null) ?? "";
			sendToEditText.Text = preferencess.GetString ("sendTo", null) ?? "";
			messageEditText.Text = preferencess.GetString ("message", null) ?? "";

			// when button is clicked
			sendSmsButton.Click += delegate {
				// save user input
				var editor = preferencess.Edit();
				editor.PutString("appKey", appKeyEditText.Text);
				editor.PutString("appSecret", appSecretEditText.Text);
				editor.PutInt("server", serverSpinner.SelectedItemPosition);
				editor.PutString("username", usernameEditText.Text);
				editor.PutString("password", passwordEditText.Text);
				editor.PutString("sendTo", sendToEditText.Text);
				editor.PutString("message", messageEditText.Text);
				editor.Commit();

				// send sms
				if (platform == null) {
					platform = new RingCentral.SDK (appKeyEditText.Text, appSecretEditText.Text,
						serverSpinner.SelectedItemPosition == 0 ? "https://platform.devtest.ringcentral.com" : "https://platform.ringcentral.com"
					).GetPlatform ();
				}
				if (!platform.LoggedIn ()) {
					var tokens = usernameEditText.Text.Split ('-');
					var username = tokens [0];
					var extension = tokens.Length > 1 ? tokens [1] : null;
					platform.Authorize (username, extension, passwordEditText.Text, true);
				}
				var request = new RingCentral.Http.Request ("/account/~/extension/~/sms",
					string.Format ("{{ \"text\": \"{0}\", \"from\": {{ \"phoneNumber\": \"{1}\" }}, \"to\": [{{ \"phoneNumber\": \"{2}\" }}]}}",
						messageEditText.Text, usernameEditText.Text, sendToEditText.Text));
				var response = platform.Post (request);

				//Show a toast
				RunOnUiThread(() => Toast.MakeText(this, "Sms sent, status code is: " + response.GetStatus (), ToastLength.Long).Show());
			};
		}
	}
}
