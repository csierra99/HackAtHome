
using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Theme = "@android:style/Theme.Holo", Label = "Hack@Home", Icon = "@drawable/icon")]
    public class EvidenceDetailsActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EvidenceDetails);

            string Token = Intent.GetStringExtra("Token");
            string FullName = Intent.GetStringExtra("FullName");
            int EvidenceID = Intent.GetIntExtra("EvidenceID", 0);
            string Title = Intent.GetStringExtra("Title");
            string Status = Intent.GetStringExtra("Status");

            var Client = new ServiceClient();
            var EvidenceDetail = await Client.GetEvidenceByIDAsync(Token, EvidenceID);

            var TextViewFullName = FindViewById<TextView>(Resource.Id.textViewFullName);
            var TextViewTitle = FindViewById<TextView>(Resource.Id.textViewTitle);
            var TextViewStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            var WebViewDescriptionEvidenceDetail = FindViewById<WebView>(Resource.Id.webViewDescriptionEvidenceDetail);            
            var ImageViewEvidence = FindViewById<ImageView>(Resource.Id.imageViewEvidence);

            TextViewFullName.Text = FullName;
            TextViewTitle.Text = Title;
            TextViewStatus.Text = Status;
            WebViewDescriptionEvidenceDetail.LoadDataWithBaseURL(null, EvidenceDetail.Description, "text/html", "utf-8", null);
            Koush.UrlImageViewHelper.SetUrlDrawable(ImageViewEvidence, EvidenceDetail.Url);
        }
    }
}