using KGID_Models.KGID_Report;

namespace BLL.KGIDReportsBLL
{
    public interface IKGIDReportsBLL
    {
        VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType);
    }
}