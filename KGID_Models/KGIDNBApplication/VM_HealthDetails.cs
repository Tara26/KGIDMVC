using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KGID_Models.KGIDNBApplication
{
    public class VM_HealthDetails
    {
        public long EmpCode { get; set; }
        public bool HavingIllnessinChest { get; set; }
        public string HavingIllnessinChestDesc { get; set; }
        public bool HavingIllnessinTeeth { get; set; }
        public string HavingIllnessinTeethDesc { get; set; }
        public bool Disability { get; set; }
        public string DisabilityDesc { get; set; }
        public bool HaveThyroid { get; set; }
        public string HaveThyroidDesc { get; set; }
        public bool EnlargementSpleenLiver { get; set; }
        public string EnlargementSpleenLiverDesc { get; set; }
        public bool GastroIntestinateTrack { get; set; }
        public string GastroIntestinateTrackDesc { get; set; }
        public bool SufferFromHernia { get; set; }
        public string SufferFromHerniaDesc { get; set; }

        public HttpPostedFileBase HavingIllnessinChestDoc { get; set; }
        public HttpPostedFileBase HavingIllnessinTeethDoc { get; set; }
        public HttpPostedFileBase DisabilityDoc { get; set; }
        public HttpPostedFileBase HaveThyroidDoc { get; set; }
        public HttpPostedFileBase EnlargementSpleenLiverDoc { get; set; }
        public HttpPostedFileBase GastroIntestinateTrackDoc { get; set; }
        public HttpPostedFileBase SufferFromHerniaDoc { get; set; }
        public string HavingIllnessinChestDocFileName { get; set; }
        public string HavingIllnessinTeethDocFileName { get; set; }
        public string DisabilityDocFileName { get; set; }
        public string HaveThyroidDocFileName { get; set; }
        public string EnlargementSpleenLiverDocFileName { get; set; }
        public string GastroIntestinateTrackDocFileName { get; set; }
        public string SufferFromHerniaDocFileName { get; set; }
    }
}
