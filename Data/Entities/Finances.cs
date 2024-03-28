using DangDucThuanFinalYear.Constants;
using System.ComponentModel.DataAnnotations;

namespace DangDucThuanFinalYear.Data.Entities
{
    public class Finances
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameFinance { get; set; }
        public string Reason { get;set; }
        public decimal Money { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreationTime { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public virtual ApplicationUser AddByUser { get; set; }
        public bool IsDeleted { get;  set; }
        public Finances Clone() => (MemberwiseClone() as Finances)!;

    }
}
