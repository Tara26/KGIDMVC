using BLL.DistrictMasterBLL;
using BLL.RemarkMasterBLL;
using DLL.DBConnection;
using DLL.NewEmployeeDLL;
using BLL.NBLoanBLL;
using KGID_Models.KGID_VerifyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KGID_Models.NBApplication;
using DLL;
using KGID_Models.Ticketing_Tool;

namespace KGID.Models
{
    public class KGID_Master
    {
        private readonly DbConnectionKGID _db = new DbConnectionKGID();
        public static IEnumerable<tbl_district_master> GetDistList()
        {
            DistrictMasterBLL _Dist = new DistrictMasterBLL();
            return _Dist.DistrictMasterbll();
        }
        public VM_BasicDetails GetEmployeeWorkDetailsByEmployeeId(long empId)
        {
            return (from emp in _db.tbl_employee_basic_details
                    join _WD in _db.tbl_employee_work_details on emp.employee_id equals _WD.ewd_emp_id
                    join _DDOM in _db.tbl_ddo_master on _WD.ewd_ddo_id equals _DDOM.dm_ddo_id
                    
                    where emp.employee_id == empId //&& emp.active_status == true
                    select new VM_BasicDetails()
                    {
                        dm_ddo_code=_DDOM.dm_ddo_code,
                        dm_dept_code=_DDOM.dm_dept_code,
                        ewd_place_of_posting=_WD.ewd_place_of_posting
                    }).FirstOrDefault();
        }
    }
    public class VM_Remarks
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        public static IEnumerable<SelectListItem> GetRemarkList(int ModuleType)
        {
            //var types = new List<SelectListItem>();
            //types.Add(new SelectListItem() { Text = "NO CORRECTION FOUND", Value = "NO CORRECTION FOUND" });
            //types.Add(new SelectListItem() { Text = "REQUIRES FILE CHECK/CLARIFICATION", Value = "REQUIRES FILE CHECK/CLARIFICATION" });
            //types.Add(new SelectListItem() { Text = "FILE NOT FOUND", Value = "FILE NOT FOUND" });
            //types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER KGID", Value = "DATA CORRECTED AS PER KGID" });
            //types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER HRMS", Value = "DATA CORRECTED AS PER HRMS" });
            //types.Add(new SelectListItem() { Text = "DETAILS MISSING IN THE FILE", Value = "DETAILS MISSING IN THE FILE" });
            //types.Add(new SelectListItem() { Text = "FILE DISCHARGED", Value = "FILE DISCHARGED" });

            //return types;
            RemarkMasterBLL _Remarks = new RemarkMasterBLL();
            return _Remarks.GetRemarkList(ModuleType);
        }
    }
    public class VM_ReceiptTypes
    {
        public static IEnumerable<SelectListItem> GetReceiptTypeList(int ModuleType)
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetReceiptTypeList(ModuleType);
        }
    }
    public class VM_PurposeTypes
    {
        public static IEnumerable<SelectListItem> GetPurposeTypeList(int ReceiptTypeID)
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetPurposeTypeList(ReceiptTypeID);
        }
    }
    public class VM_SubPurposeTypes
    {
        public static IEnumerable<SelectListItem> GetSubPurposeTypeList(int PurposeID)
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetSubPurposeTypeList(PurposeID);
        }
    }
    public class VM_HOATypes
    {
        public static IEnumerable<SelectListItem> GetHOATypeList()
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetHOATypeList();
        }
    }


    public class VM_MI_Remarks
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        public static IEnumerable<SelectListItem> GetRemarkList()
        {
            var types = new List<SelectListItem>();
            types.Add(new SelectListItem() { Text = "NO CORRECTION FOUND", Value = "NO CORRECTION FOUND" });
            types.Add(new SelectListItem() { Text = "ISSUE IN APPLICATION FORM", Value = "ISSUE IN APPLICATION FORM" });
            //types.Add(new SelectListItem() { Text = "FILE NOT FOUND", Value = "FILE NOT FOUND" });
            //types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER KGID", Value = "DATA CORRECTED AS PER KGID" });
            //types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER HRMS", Value = "DATA CORRECTED AS PER HRMS" });
            //types.Add(new SelectListItem() { Text = "DETAILS MISSING IN THE FILE", Value = "DETAILS MISSING IN THE FILE" });
            //types.Add(new SelectListItem() { Text = "FILE DISCHARGED", Value = "FILE DISCHARGED" });

            return types;
            //RemarkMasterBLL _Remarks = new RemarkMasterBLL();
            //return _Remarks.GetRemarkList(ModuleType);
        }
        public static IEnumerable<SelectListItem> ODClaimGetRemarkList()
        {
            var types = new List<SelectListItem>();
            types.Add(new SelectListItem() { Text = "NO CORRECTION FOUND", Value = "1" });
            types.Add(new SelectListItem() { Text = "REQUIRES FILE CHECK/CLARIFICATION", Value = "2" });
            types.Add(new SelectListItem() { Text = "FILE NOT FOUND", Value = "3" });
            types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER KGID", Value = "4" });
            types.Add(new SelectListItem() { Text = "DATA CORRECTED AS PER HRMS", Value = "5" });
            types.Add(new SelectListItem() { Text = "DETAILS MISSING IN THE FILE", Value = "6" });
            types.Add(new SelectListItem() { Text = "FILE DISCHARGED", Value = "7" });

            return types;
            //RemarkMasterBLL _Remarks = new RemarkMasterBLL();
            //return _Remarks.GetRemarkList(ModuleType);
        }
    }
    //Family
    public class VM_FamilyRelationList
    {
        public static IEnumerable<SelectListItem> GetFamilyRelationList(long EmpID)
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetFamilyRelationList(EmpID);
        }
    }
    //Loan
    public class VM_LoanSelectList
    {
        public static IEnumerable<SelectListItem> GetLoanFamilyRelationList()
        {
            NBLoanBLL _LoanBLL = new NBLoanBLL();
            return _LoanBLL.GetLoanFamilyRelationList();
        }
        public static IEnumerable<SelectListItem> GetLoanPurposeList()
        {
            NBLoanBLL _LoanBLL = new NBLoanBLL();
            return _LoanBLL.GetLoanPurposeList();
        }
    }

    public class VM_DRTypes
    {
        public static IEnumerable<SelectListItem> GetDRTypeList()
        {
            NBApplicationDll _ApplicationDLL = new NBApplicationDll();
            return _ApplicationDLL.GetDRTypeList();
        }
    }
    public class TT_Mast
    {
        public static IEnumerable<tbl_module_type_master> GetModuleList()
        {
            TicketingToolDLL _ApplicationDLL = new TicketingToolDLL();
            return _ApplicationDLL.GetModuleListDLL();
        }
        public static IEnumerable<tbl_problem_type_master> GetProblemTypeList()
        {
            TicketingToolDLL _ApplicationDLL = new TicketingToolDLL();
            return _ApplicationDLL.GetProblemTypeListDLL();
        }
    }

    public class VM_HelpDeskUsersList
    {
        private static readonly DbConnectionKGID _db = new DbConnectionKGID();
        public static IEnumerable<SelectListItem> GetHelpDeskUsersList()
        {
            var HelpDeskUserList = new List<SelectListItem>();

            HelpDeskUserList = (from t in _db.tbl_agency_login
                                where t.al_user_category_id == "14"
                     select (new SelectListItem { Text = t.al_agency_user_id, Value = t.al_agency_login_id.ToString() })).ToList();

            return HelpDeskUserList;
        }
    }
    public class VM_Ticket_Status
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        public static IEnumerable<SelectListItem> GetTicketStatusList()
        {
            var TicketList = new List<SelectListItem>();
            TicketList.Add(new SelectListItem() { Text = "Pending", Value = "Pending" });
            TicketList.Add(new SelectListItem() { Text = "InProgress", Value = "InProgress" });
            TicketList.Add(new SelectListItem() { Text = "Resolved", Value = "Resolved" });

            return TicketList;
            //RemarkMasterBLL _Remarks = new RemarkMasterBLL();
            //return _Remarks.GetRemarkList(ModuleType);
        }
    }

}