using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class AddReceiptForm : BusinessObjectModel
    {
        public string ReceivedFrom { get; set; }
        public string DistrictStation { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string ReceivedBy { get; set; }

        public string Bank { get; set; }
        public string CheckNumber { get; set; }
        public DateTime CheckDate { get; set; }

        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
