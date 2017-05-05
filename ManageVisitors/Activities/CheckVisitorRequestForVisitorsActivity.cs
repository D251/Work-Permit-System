using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using ManageVisitors.Models;
using Newtonsoft.Json;
using ManageVisitors.Adapter;
using System.Threading;

namespace ManageVisitors.Activities
{
    [Activity(Label = "CheckVisitorRequestForVisitorsActivity")]
    public class CheckVisitorRequestForVisitorsActivity : AppCompatActivity
    {
        ListView lvVisitorRequestList;
        TextView tvlblVisitorRequestList, lblTokenNo, lblDepartment, lblEmployeeName;
        ListView mListView;
        ListProcessRequestByVisitorUserModel RPSRNO = new ListProcessRequestByVisitorUserModel();
        List<ListProcessRequestByVisitorUserModel> ResultListProcessRequestByVisitorUserModel;
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CheckVisitorRequestForVisitorlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            
            mListView = FindViewById<ListView>(Resource.Id.RPVisitorlistView);

            mListView.ItemClick += MListView_ItemClick;
            mListView.ItemLongClick += MListView_ItemLongClick;
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            RPSRNO = (ResultListProcessRequestByVisitorUserModel.ElementAt(e.Position));

            StatusModel.RequestProcessSrNo = RPSRNO.RequestProcessSrNo.ToString();
            Intent intent = new Intent(this, typeof(RequestProcessStatusFlowActivity));
            this.StartActivity(intent);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RPSRNO = (ResultListProcessRequestByVisitorUserModel.ElementAt(e.Position));

            StatusModel.RequestProcessSrNo = RPSRNO.RequestProcessSrNo.ToString();
            Intent intent = new Intent(this, typeof(CheckVisitorRequestDetailsForVisitorActivity));
            this.StartActivity(intent);
        }

        //private void TvlblVisitorRequestList_Click(object sender, EventArgs e)
        //{
        //    Intent intent = new Intent(this, typeof(CheckVisitorRequestDetailsActivity));
        //    this.StartActivity(intent);
        //}


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }

        public async void GetAllEmployeeDepartmentInfoByTokenNo()
        {
            try
            {
                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Checking User Info...</font>"), true);

                string Url = StatusModel.Url + "GetVisitorUserInformationByVisitorUserID";
                WebHelpper _objHelper = new WebHelpper();
                VisitorUserRegistrationModel _objVisitorUserRegistrationModel = new VisitorUserRegistrationModel();

                _objVisitorUserRegistrationModel.VisitorUserID = StatusModel.LoginUserName;

                var PostString = JsonConvert.SerializeObject(_objVisitorUserRegistrationModel);
                var request = await _objHelper.MakePostRequest(Url, PostString, true);

                VisitorUserRegistrationModel ResultVisitorUserRegistrationModel = JsonConvert.DeserializeObject<VisitorUserRegistrationModel>(request);
                


                string Url1 = StatusModel.Url + "GetProcessRequestByVisitorUserSrNo";
                WebHelpper _objHelper1 = new WebHelpper();
               
                var PostString1 = JsonConvert.SerializeObject(ResultVisitorUserRegistrationModel);
                var request1 = await _objHelper1.MakePostRequest(Url1, PostString1, true);

                ResultListProcessRequestByVisitorUserModel = JsonConvert.DeserializeObject<List<ListProcessRequestByVisitorUserModel>>(request1);


                mListView.Adapter = new RPItemListForVisitorAdapter(this, ResultListProcessRequestByVisitorUserModel);
                
                progressDialog.Hide();

            }
            catch (Exception e)
            {
                progressDialog.Hide();

                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.VisitorsRequestList);
            GetAllEmployeeDepartmentInfoByTokenNo();
            base.OnResume();
        }
    }
}