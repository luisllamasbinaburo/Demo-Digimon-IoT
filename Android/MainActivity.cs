using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using System;
using System.Linq;
using System.Text;

namespace NFCAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]
    public class MainActivity : AppCompatActivity
    {
        private NfcAdapter _nfcAdapter;
        private MqttService _mqttService;
        private RestService _restService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            
            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            // la parte de MQTT no la usamos, pero os la dejo por si algun dia la necesitáis
            //_mqttService = new MqttService();
            //_mqttService.Initialize("192.168.1.xxx");

            _restService = new RestService();
            _restService.Initialize("http://192.168.1.xxx:3000");
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_nfcAdapter == null)
            {
                var alert = new Android.App.AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.Show();
            }
            else
            {
                var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
                var ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
                var techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);
                var filters = new[] { ndefDetected, tagDetected, techDetected };

                var intent = new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop);

                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

                // Gives your current foreground activity priority in receiving NFC events over all other activities.
                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Action != NfcAdapter.ActionTagDiscovered) return;

            var myTag = (Tag)intent.GetParcelableExtra(NfcAdapter.ExtraTag);
            if (myTag == null) return;

            var tagIdBytes = myTag.GetId();
            long cardId = Utils.TagId_To_CardId(tagIdBytes);

            var alertMessage = new Android.App.AlertDialog.Builder(this).Create();
            alertMessage.SetMessage("CardId:" + cardId);
            alertMessage.Show();

            //_mqttService.SendMqtt(cardId.ToString());
            _restService.PostItem(cardId.ToString());
        }
    

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            return id == Resource.Id.action_settings || base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}