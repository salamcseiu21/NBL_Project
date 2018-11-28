using NBL.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBL.Areas.SuperAdmin.Models.ViewModels
{
    public class ViewDeliveredOrder:DeliveryModel
    {
        public DateTime OrderDate { get; set; }
        public DateTime ApproveByAccountsDate  { get; set; }
        public DateTime ApproveByNsmDate { get; set; }
        public int AccountsUserId { get; set; }
        public int NsmUserId { get; set; }
        public int DeliveredOrReceiveUserId { get; set; }
    }
}