using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TBL_Account")]
    public class Account
    {
        [Key]
        [ForeignKey("Employee")]
        public string NIK { get; set; }
        public string Password { get; set; }
        public string? OTP { get; set; }
        public DateTime? Expired { get; set; }

        public Employee? Employee { get; set; }
        
        public Profiling? Profiling { get; set; }    
    }
}
