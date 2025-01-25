using Medik.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class Examination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Date of examination")]
        public DateTime DateOfExam { get; set; }

        [Required]
        [Display(Name = "Type of examination")]
        public ExamEnum ExamType { get; set; }
        public string? PicturePath { get; set; }

        public Patient Patient { get; set; }
    }
}
