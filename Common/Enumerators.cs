using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public enum WorkFlowStatuses : int
    {
        INITIAL = 1,
        SENDBACKTOEMPLOYEE = 2,
        FORWARDTODDO = 3,
        SENDBACKTODDO = 4,
        FORWARDTOCASEWORKER = 5,
        SENDBACKTOCASEWORKER = 6,
        FORWARDTOSUPERINTENDENT = 7,
        SENDBACKTOSUPERINTENDENT = 8,
        FORWARDTODIO = 9,
        SENDBACKTODIO = 10
    }

    public enum Verifiers
    {
        DDO,
        CW,
        SUPERINTENDENT,
        DIO,
        DEPUTYDIRECTOR,
        DIRECTOR,
        AVGCW,
        ASSITANTDIRECTOR,
        SURVEYOR

    }

    public enum UserCategories : int
    {
        EMPLOYEE = 1,
        DDO = 2,
        CASEWORKER = 3,
        SUPERINTENDENT = 4,
        DIO = 5,
        DEPUTYDIRECTOR = 6,
        DIRECTOR = 7,
        ADMIN = 8,
        SUPERADMIN = 9,
        AVGCASEWORKER = 10,
        AGENCY=11,
        SURVEYOR = 13,
        HELPDESK = 14,
        ASSITANTDIRECTOR=15
    }

    public enum Relations : int
    {
        FATHER = 1,
        MOTHER = 2,
        SISTER = 3,
        BROTHER = 4,
        SPOUSE = 5,
        DAUGHTER = 6,
        SON = 7
    }

    public enum ClaimTypes : int
    {
        MATURITY = 1,
        PREMATURITY = 2,
        DEATH = 3
    }

    public enum ClaimSubTypes : int
    {
        VOLUNTARYRETIREMENT = 1,
        TERMINATIONFROMSERVICE = 2,
        ILLNESS = 3,
        UNNATURALDEATH = 4,
        MISSINGABSCONDING = 5
    }

    public enum ClaimDocuments: int
    {
        CLAIMAPPLICATION = 1,
        DEATHCERTIFICATE = 2,
        POLICYBONDS = 3, 
        MEDICALATTENDANCEREPORT = 4,
        SURVIVALCERTIFICATE = 5,
        LEAVESAVAILEDONMEDICALGROUNDS = 6,
        DETAILSOFMEDICALREIMBURSEMENT = 7,
        DEATHSUMMARY = 8,
        DISCHARGESUMMARY = 9,
        POLICEREPORT = 10,
        FIR = 11,
        POSTMORTEMREPORT = 12,
        COURTORDERDECLARINGDEAD = 13,
        SUCCESSIONCERTIFICATE = 14,
        CONSENTLETTER = 15,
        COURTJUDGMENT = 16,
        GUARDIANSHIPCERTIFICATE = 17,
        MARRIAGECERTIFICATE = 18,
        OTHERDOCUMENT = 19,
        RESIGNATIONLETTER = 20,
        RESIGNATIONACCEPTANCELETTER = 21,
        RELIEVINGLETTER = 22,
        TERMINATIONLETTER = 23
    }
}
