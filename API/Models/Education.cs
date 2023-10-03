using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("TBL_Education")]
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public Degree Degree { get; set; }
        public float GPA { get; set; }
        [ForeignKey("University")]
        public int University_Id { get; set; }

        public University? University { get; set; }
        public ICollection<Profiling>? Profiling { get; set; }
    }
    public enum Degree
    {
        D3,
        D4,
        S1,
        S2,
        S3
    }
}
