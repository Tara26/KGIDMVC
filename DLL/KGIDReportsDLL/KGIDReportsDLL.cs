using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DLL.DBConnection;
using KGID_Models.KGID_Report;

namespace DLL.KGIDReportsDLL
{
    public class KGIDReportsDLL : IKGIDReportsDLL
    {
        private readonly Common_Connection _Conn = new Common_Connection();
        private readonly AllCommon _commnobj = new AllCommon();

        public VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType)
        {
            VM_KGIDApplicationReports applicationReports = null;

            DataSet dsClaims = new DataSet();
            //if (applicationReportDetails.FromDate != null)
            //{
            //    string FDate = _commnobj.DateConversion(Convert.ToDateTime(applicationReportDetails.FromDate).ToShortDateString());
            //    applicationReportDetails.FromDate = Convert.ToDateTime(FDate);
            //}
            //if (applicationReportDetails.ToDate != null)
            //{
            //    string TDate = _commnobj.DateConversion(Convert.ToDateTime(applicationReportDetails.ToDate).ToShortDateString());
            //    applicationReportDetails.ToDate = Convert.ToDateTime(TDate);
            //}

            SqlParameter[] sqlparam =
            {
                new SqlParameter("@loggedInUserId", loggedInUserId),
                new SqlParameter("@empType", empType),
                new SqlParameter("@fromDate", applicationReportDetails.FromDate),
                new SqlParameter("@toDate", applicationReportDetails.ToDate)
            };

            dsClaims = _Conn.ExeccuteDataset(sqlparam, "sp_report_kgidApplications");
            if (dsClaims.Tables[0].Rows.Count > 0)
            {
                applicationReports = new VM_KGIDApplicationReports();
                foreach (DataRow row in dsClaims.Tables[0].Rows)
                {
                    VM_KGIDApplicationReport applicationReport = new VM_KGIDApplicationReport();
                    applicationReport.Name = row["Name"].ToString();
                    applicationReport.ApplicationRefNumber = row["ApplicationRefNumber"].ToString();
                    applicationReport.ApplicationDate = Convert.ToDateTime(row["ApplicationDate"].ToString()); 
                    applicationReport.District = row["District"].ToString();
                    applicationReport.Department = row["Department"].ToString();
                    applicationReport.Priority = row["Priority"].ToString();
                    applicationReport.Status = row["Status"].ToString();

                    applicationReports.ApplicationReports.Add(applicationReport);
                }
            }

            return applicationReports;
        }
    }
}
