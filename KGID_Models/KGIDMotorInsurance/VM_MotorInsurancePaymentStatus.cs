using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGID_Models.KGIDMotorInsurance
{
    public class VM_MotorInsurancePaymentStatus
    {
        public List<VM_MotorInsurancePaymentStatus> listChallanDetails { get; set; }
        public List<VM_MotorInsurancePaymentStatus> listChallanRejected { get; set; }
        public List<VM_MotorInsurancePaymentStatus> listChallanApprove { get; set; }
        public List<VM_MotorInsurancePaymentStatus> listChallanPendingAtDDO { get; set; }
        public VM_MotorInsurancePaymentStatus()
        {
            listChallanDetails = new List<VM_MotorInsurancePaymentStatus>();
            listChallanRejected = new List<VM_MotorInsurancePaymentStatus>();
            listChallanApprove = new List<VM_MotorInsurancePaymentStatus>();
            listChallanPendingAtDDO = new List<VM_MotorInsurancePaymentStatus>();
        }

        public string cd_challan_ref_no { get; set; }
        public decimal cd_amount { get; set; }
        public int NO_OF_APPL { get; set; }
        public string Uploded { get; set; }
        public string cd_ack_status_flag { get; set; }
        public string cs_status { get; set; }

        public string miso_sanction_order_numner { get; set; }
        public string miso_sanction_order_date { get; set; }
        public string cd_file_name_xml { get; set; }
    }
}
