using KGID_Models.KGID_Report;

namespace DLL.KGIDReportsDLL
{
    public interface IKGIDReportsDLL
    {
        VM_KGIDApplicationReports GetKGIDApplicationsReport(long loggedInUserId, VM_KGIDApplicationReportDetails applicationReportDetails, int empType);
    }
}