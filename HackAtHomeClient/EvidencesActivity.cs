using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.Entities;
using HackAtHome.SAL;
using System.Collections.Generic;

namespace HackAtHomeClient
{
    [Activity(Theme = "@android:style/Theme.Holo", Label = "Hack@Home", Icon = "@drawable/icon")]
    public class EvidencesActivity : Activity
    {
        private class ObjectFragment : Fragment
        {
            public List<Evidence> Evidences { get; set; }
            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                RetainInstance = true;
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Evidences);

            string Token = Intent.GetStringExtra("Token");
            string FullName = Intent.GetStringExtra("FullName");

            var TextViewFullName = FindViewById<TextView>(Resource.Id.textViewFullName);
            TextViewFullName.Text = FullName;

            ObjectFragment Data = (ObjectFragment)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                Data = new ObjectFragment();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();

                var Client = new ServiceClient();
                Data.Evidences = await Client.GetEvidencesAsync(Token);
            }

            var ListViewEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);
            ListViewEvidences.Adapter = new EvidencesAdapter(this, Data.Evidences, Resource.Layout.EvidenceItem, Resource.Id.textViewTitle, Resource.Id.textViewStatus);

            ListViewEvidences.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                var Evidence = Data.Evidences[e.Position];

                var Intent = new Android.Content.Intent(this, typeof(EvidenceDetailsActivity));
                Intent.PutExtra("Token", Token);
                Intent.PutExtra("FullName", FullName);
                Intent.PutExtra("EvidenceID", Evidence.EvidenceID);
                Intent.PutExtra("Title", Evidence.Title);
                Intent.PutExtra("Status", Evidence.Status);
                StartActivity(Intent);
            };
        }
    }
}