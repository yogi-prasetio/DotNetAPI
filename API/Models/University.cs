using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TBL_University")]
    public class University
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Education> Education { get; set; }
    }
}
