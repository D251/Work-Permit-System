using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ManageVisitors.Models;
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Firebase.Iid;
using Android.Preferences;

namespace ManageVisitors.Activities
{
    [Activity(Label = "VisitorNewRegistrationActivity")]
    public class VisitorNewRegistrationActivity : AppCompatActivity
    {
        AutoCompleteTextView txt_VisitorContractorName;
        VisitorUserRegistrationModel _ObjVisitorUserRegistration = new VisitorUserRegistrationModel();
        List<ContractorMasterModel> ResultAllContractorMaster = new List<ContractorMasterModel>();
        Button btnRegisterVisitor, btnRegisterVisitorCancel;
        EditText txt_VisitorUserID, txt_VisitorName, txt_VisitorAddress, txt_VisitorContactNo, txt_VisitorEmailID, txt_VisitorNatureOfWork, txt_VisitorContractorContactNo, txt_VisitorPassword;
        ProgressDialog progressDialog;
        String DTI="";
        public static String SENT_TOKEN_TO_SERVER = "sentTokenToServer";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VisitorNewRegistrationlayout);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);


            btnRegisterVisitor = FindViewById<Button>(Resource.Id.btnRegisterDepartment);
            btnRegisterVisitorCancel = FindViewById<Button>(Resource.Id.btnCancelRegisterDepartment);

            txt_VisitorUserID = FindViewById<EditText>(Resource.Id.txt_VisitorUserID);
            txt_VisitorName = FindViewById<EditText>(Resource.Id.txt_VisitorName);
            txt_VisitorAddress = FindViewById<EditText>(Resource.Id.txt_VisitorAddress);
            txt_VisitorContactNo = FindViewById<EditText>(Resource.Id.txt_VisitorContactNumber);
            txt_VisitorEmailID = FindViewById<EditText>(Resource.Id.txt_VisitorEmailID);
            txt_VisitorNatureOfWork = FindViewById<EditText>(Resource.Id.txt_VisitorNatureOfWork);
            txt_VisitorContractorName = FindViewById<AutoCompleteTextView>(Resource.Id.txt_VisitorContractor);
            txt_VisitorContractorContactNo = FindViewById<EditText>(Resource.Id.txt_VisitorContractorContactNo);
            txt_VisitorPassword = FindViewById<EditText>(Resource.Id.txt_VisitorPassword);


            btnRegisterVisitor.Click += BtnRegisterVisitor_Click;
            btnRegisterVisitorCancel.Click += BtnRegisterVisitorCancel_Click;
            txt_VisitorContractorName.ItemClick += Txt_VisitorContractorName_ItemClick;
            TokenNo();
        }

        ContractorMasterModel ContractorMaster = new ContractorMasterModel();
        private void Txt_VisitorContractorName_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ContractorMaster = ResultAllContractorMaster.ElementAt(e.Position);

            if (ContractorMaster != null)
            {
                txt_VisitorContractorContactNo.Text = ContractorMaster.ContractorContactNo.ToString().Trim();
            }
        }

        private void BtnRegisterVisitorCancel_Click(object sender, EventArgs e)
        {

        }

        private void BtnRegisterVisitor_Click(object sender, EventArgs e)
        {
            VisitorRegistration();
        }


        public async void GetVisitorUserMaxSrNo()
        {
            try
            {
                WebHelpper _objHelper = new WebHelpper();

                string Url = StatusModel.Url + "GetVisitorUserMaxSrNo";

                progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Checking...</font>"), true);

                var request = await _objHelper.MakeGetRequest(Url);

                var _objVisitorUserRegistrationModel = JsonConvert.DeserializeObject<VisitorUserRegistrationModel>(request);

                txt_VisitorUserID.Text = "M&M" + _objVisitorUserRegistrationModel.VisitorSrNo;
                progressDialog.Hide();
            }
            catch (Exception e)
            {
                progressDialog.Hide();
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        public String[] Contractor;
        public async void GetAllContractor()
        {
            try
            {
                string Url = StatusModel.Url + "GetContractorMaster";
                WebHelpper _objHelper = new WebHelpper();
                ContractorMasterModel _objContractorMaster = new ContractorMasterModel();


                var request = await _objHelper.MakeGetRequest(Url);

                ResultAllContractorMaster = JsonConvert.DeserializeObject<List<ContractorMasterModel>>(request);

                Contractor = new string[ResultAllContractorMaster.Count + 1];

                int i = 1;
                Contractor[0] = "--Select Contractor--";
                foreach (var item in ResultAllContractorMaster)
                {
                    Contractor[i] = item.ContractorName;
                    i++;
                }

                ArrayAdapter _adapterContractor = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Contractor);
                txt_VisitorContractorName.Adapter = _adapterContractor;
            }
            catch (Exception e)
            {
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        private async void VisitorRegistration()
        {
            TokenNo();
            try
            {
                Drawable icon_error = Resources.GetDrawable(Resource.Drawable.alert);
                icon_error.SetBounds(0, 0, 40, 30);

                if (txt_VisitorName.Text != "")
                {
                    if (txt_VisitorAddress.Text != "")
                    {
                        if (txt_VisitorContactNo.Text != "")
                        {
                            if (txt_VisitorEmailID.Text != "")
                            {
                                if (txt_VisitorNatureOfWork.Text != "")
                                {
                                    if (txt_VisitorContractorName.Text != "")
                                    {
                                        if (txt_VisitorContractorContactNo.Text != "")
                                        {
                                            if (txt_VisitorUserID.Text != "")
                                            {
                                                if (txt_VisitorPassword.Text != "")
                                                {
                                                    foreach (var item in ResultAllContractorMaster)
                                                    {
                                                        if (item.ContractorName.ToString().Trim() == (txt_VisitorContractorName.Text.Trim()))
                                                        {
                                                            _ObjVisitorUserRegistration.VisitorContractorSrNo = item.ContractorSrNo;
                                                            _ObjVisitorUserRegistration.VisitorContractorCoNo = item.ContractorContactNo;
                                                        }
                                                    }



                                                    _ObjVisitorUserRegistration.DeviceTokenId = DTI.ToString();
                                                    _ObjVisitorUserRegistration.VisitorName = txt_VisitorName.Text;
                                                    _ObjVisitorUserRegistration.VisitorAddress = txt_VisitorAddress.Text;
                                                    _ObjVisitorUserRegistration.VisitorContactNo = txt_VisitorContactNo.Text;
                                                    _ObjVisitorUserRegistration.VisitorEmailID = txt_VisitorEmailID.Text;
                                                    _ObjVisitorUserRegistration.VisitorNatureOfWork = txt_VisitorNatureOfWork.Text;
                                                    _ObjVisitorUserRegistration.VisitorUserID = txt_VisitorUserID.Text;
                                                    _ObjVisitorUserRegistration.VisitorPassword = txt_VisitorPassword.Text;
                                                    _ObjVisitorUserRegistration.VisitorRegistrationDate =Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

                                                    // ADD Insert Code Here

                                                    WebHelpper _objHelper = new WebHelpper();

                                                    string Url = StatusModel.Url + "AddVisitorUserRegistration";

                                                    var progressDialog = ProgressDialog.Show(this, Android.Text.Html.FromHtml("<font color='#EC407A'> Please wait...</font>"), Android.Text.Html.FromHtml("<font color='#EC407A'> Data Inserting...</font>"), true);

                                                    var PostString = JsonConvert.SerializeObject(_ObjVisitorUserRegistration);
                                                    var requestTemp = await _objHelper.MakePostRequest(Url, PostString, true);
                                                    ResultModel ResultgetRequest = JsonConvert.DeserializeObject<ResultModel>(requestTemp);

                                                    if (ResultgetRequest.success == 1)
                                                    {
                                                        progressDialog.Hide();
                                                        Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                                        
                                                    }
                                                    
                                                    else
                                                    {
                                                        progressDialog.Hide();
                                                        Toast.MakeText(this, ResultgetRequest.msg, ToastLength.Short).Show();
                                                        return;
                                                    }
                                              
                                                clear();
                                            }
                                            else
                                            {
                                                txt_VisitorPassword.RequestFocus();
                                                txt_VisitorPassword.SetError("Please Enter Password First", icon_error);
                                            }
                                        }
                                        else
                                        {
                                            txt_VisitorUserID.RequestFocus();
                                            txt_VisitorUserID.SetError("Please Enter Visitor User ID First", icon_error);
                                        }
                                    }
                                    else
                                    {
                                        txt_VisitorContractorContactNo.RequestFocus();
                                        txt_VisitorContractorContactNo.SetError("Please Enter Contractor Contact Number First", icon_error);
                                    }
                                }
                                else
                                {
                                    txt_VisitorContractorName.RequestFocus();
                                    txt_VisitorContractorName.SetError("Please Enter Contractor Name First", icon_error);
                                }
                            }
                            else
                            {
                                txt_VisitorNatureOfWork.RequestFocus();
                                txt_VisitorNatureOfWork.SetError("Please Enter Nature Of Work First", icon_error);
                            }
                        }
                        else
                        {
                            txt_VisitorEmailID.RequestFocus();
                            txt_VisitorEmailID.SetError("Please Enter Email ID First", icon_error);
                        }
                    }
                    else
                    {
                        txt_VisitorContactNo.RequestFocus();
                        txt_VisitorContactNo.SetError("Please Enter Contact Number First", icon_error);
                    }
                }
                else
                {
                    txt_VisitorAddress.RequestFocus();
                    txt_VisitorAddress.SetError("Please Enter Address First", icon_error);
                }
            }
                else
                {
                txt_VisitorName.RequestFocus();
                txt_VisitorName.SetError("Please Enter Full Name First", icon_error);
            }


        }
            catch (Exception e)
            {
                progressDialog.Hide();
                string ErrorMsg = e.ToString();
                Toast.MakeText(this, ErrorMsg, ToastLength.Long).Show();
            }
        }

        const string TAG = "MyFirebaseIIDService";
        public void TokenNo()
        {
            DTI = FirebaseInstanceId.Instance.Token;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + DTI);

            // TODO: Implement this method to send any registration to your app's servers.
            sendRegistrationToServer(DTI);
        }


        private void sendRegistrationToServer(String token)
        {
            ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            sharedPreferences.Edit().PutBoolean(SENT_TOKEN_TO_SERVER, true).Apply();
        }

        public void clear()
        {
            txt_VisitorUserID.Text = "";
            txt_VisitorName.Text = "";
            txt_VisitorAddress.Text = "";
            txt_VisitorContactNo.Text = "";
            txt_VisitorEmailID.Text = "";
            txt_VisitorNatureOfWork.Text = "";
            txt_VisitorContractorName.Text = "";
            txt_VisitorContractorContactNo.Text = "";
            txt_VisitorPassword.Text = "";
            GetVisitorUserMaxSrNo();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
                Finish();

            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            SupportActionBar.SetTitle(Resource.String.VisitorRegistration);
           
            GetVisitorUserMaxSrNo();

            GetAllContractor();
            TokenNo();
            base.OnResume();
        }

    }
}