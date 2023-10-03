using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TBL_Profiling")]
    public class Profiling
    {
        [Key]
        [ForeignKey("Account")]
        public string NIK { get; set; }

        [ForeignKey("Education")]
        public int Education_Id { get; set; }
        public Education? Education { get; set; }
        public Account? Account { get; set; }
    }
}
