using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.NBApplication
{
    public class VM_proposalList
    {
        public VM_proposalList()
        {
            listDetails = new List<VM_PolicyGeneration>();
        }
        public List<VM_PolicyGeneration> listDetails { get; set; }
    }
    public class VM_PolicyGeneration
    {
        public long anp_id { get; set; }
        public long anp_emp_id { get; set; }
        public long anp_kgid_no { get; set; }
        public string anp_dob { get; set; }
        public string anp_appointment_letter { get; set; }
        public string anp_joining_letter { get; set; }
        public bool anp_status { get; set; }
        public DateTime anp_creation_datetime { get; set; }
        public DateTime anp_updation_datetime { get; set; }
        public int anp_category_id { get; set; }
        public int anp_updated_by { get; set; }
        public int anp_created_by { get; set; }
        public HttpPostedFileBase JoiningLetterDoc { get; set; }
        public HttpPostedFileBase AppointmentLetterDoc { get; set; }
        public HttpPostedFileBase DOBDoc { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string DateofBirth { get; set; }
        public string DepartmentName { get; set; }
    }
}
