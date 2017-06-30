using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;
using System;

namespace HackAtHomeClient
{
    [Activity(Theme = "@android:style/Theme.Holo", Label = "Hack@Home", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var ButtonValidate = FindViewById<Button>(Resource.Id.buttonValidate);
            ButtonValidate.Click += async (object sender, System.EventArgs e) =>
            {
                var EditTextEmail = FindViewById<TextView>(Resource.Id.editTextEmail);
                var EditTextPassword = FindViewById<TextView>(Resource.Id.editTextPassword);

                var TiCapacitacionClient = new ServiceClient();
                var Result = await TiCapacitacionClient.AutenticateAsync(EditTextEmail.Text, EditTextPassword.Text);
                if (Result.Status == Status.Success)
                {
                    var MicrosoftEvidence = new LabItem
                    {
                        Email = EditTextEmail.Text,
                        Lab = "Hack@Home",
                        DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
                    };
                    var MicrosoftClient = new MicrosoftServiceClient();
                    await MicrosoftClient.SendEvidence(MicrosoftEvidence);

                    var Intent = new Android.Content.Intent(this, typeof(EvidencesActivity));
                    Intent.PutExtra("Token", Result.Token);
                    Intent.PutExtra("FullName", Result.FullName);
                    StartActivity(Intent);
                }
                else
                {
                    Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
                    AlertDialog Alert = Builder.Create();
                    Alert.SetTitle("Resultado de la verificación");
                    Alert.SetIcon(Resource.Drawable.Icon);
                    Alert.SetMessage($"{Enum.GetName(typeof(Status), Result.Status)}");
                    Alert.SetButton("Ok", (s, ev) => { });
                    Alert.Show();
                }
            };
        }
    }
}

