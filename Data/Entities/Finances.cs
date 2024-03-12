using DangDucThuanFinalYear.Constants;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Finances
    {
        [Key]
        public string Code { get; set; }
        public TypeFinance TypeFinance { get; set; }
        public string Reason { get;set; }
        public decimal Money { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreationTime { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public virtual ApplicationUser AddByUser { get; set; }

    }
}
