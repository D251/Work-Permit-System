using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ManageVisitors.Models;

namespace ManageVisitors.Adapter
{
    public class RPItemListForVisitorAdapter : BaseAdapter<ListProcessRequestByVisitorUserModel>
    {
        Activity context;
        List<ListProcessRequestByVisitorUserModel> list;
        int SrNo;

        public RPItemListForVisitorAdapter(Activity _context, List<ListProcessRequestByVisitorUserModel> _list)
                : base()
        {
            this.context = _context;
            this.list = _list;

        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ListProcessRequestByVisitorUserModel this[int index]
        {
            get { SrNo = 1; return list[index]; }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.CheckVisitorRequestForVisitorListItem, parent, false);

            ListProcessRequestByVisitorUserModel item = this[position];
            if (item != null)
            {
                view.FindViewById<TextView>(Resource.Id.lblRPVisitorRequestID).Text = item.RequestProcessSrNo == null ? "" : item.RequestProcessSrNo.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVisitorEmployeeName).Text = item.EmployeeName == null ? "" : item.EmployeeName.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVisitorStartTime).Text = item.VisitStartTime == null ? "" : item.VisitStartTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVisitorEndTime).Text = item.VisitEndTime == null ? "" : item.VisitEndTime.ToString();
                view.FindViewById<TextView>(Resource.Id.lblRPVisitorReason).Text = item.VisitorVisitResons == null ? "" : item.VisitorVisitResons.ToString();

                SrNo++;
            }
            return view;

        }
    }
}