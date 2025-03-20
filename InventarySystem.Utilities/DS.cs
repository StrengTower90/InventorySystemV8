using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Utilities
{
    public static class DS
    {
        public const string Success = "Success";
        public const string Error = "Error";

        public const string ImagePath = @"\images\products\";
        public const string ssShoppingCart = "Shopping Cart Session";

        public const string Role_Admin = "Admin";
        public const string Role_Client = "Client";
        public const string Role_Inventory = "Inventory";

        // Order Status
        public const string PendingStatus = "Pending";
        public const string ApprovedStatus = "Approved";
        public const string InProcessStatus = "Processing";
        public const string SentStatus = "Sent";
        public const string CanceledStatus = "Canceled";
        public const string ReturnedStatus = "Returned";
        // Order payment status 
        public const string PaymentPendingStatus = "Pending";
        public const string PaymentApprovedStatus = "Approved";
        public const string PaymentStatusDelayed = "Delayed";
        public const string PaymentStatusRejected = "Rejected";

    }
}
