using Android.App;
using Android.Widget;
using Android.OS;

namespace TryRC_Android
{
	[Activity (Label = "TryRC_Android", MainLauncher = true, 
		Icon = "@mipmap/icon", Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{
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

				//Show a toast
				RunOnUiThread(() => Toast.MakeText(this, "sms sent", ToastLength.Short).Show());
			};
		}
	}
}
