using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KGID.Models
{
    public class KIIPaymentResponse
    {
        public string BankTransactionNo { get; set; }
        public string ChallanAmount { get; set; }
        public string ChallanRefNo { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string BankName { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionTimeStamp { get; set; }
        public string CheckSum { get; set; }
    }
}