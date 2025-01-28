using Medik.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class Examination
    {
        private DateTime dateOfExam;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long PatientId { get; set; }

        [Required]
        [Display(Name = "Date of examination")]
        public DateTime DateOfExam 
        { 
            get => dateOfExam; 
            set => dateOfExam = DateTime.SpecifyKind(value, DateTimeKind.Utc); }

        [Required]
        [Display(Name = "Type of examination")]
        public ExamEnum ExamType { get; set; }
        public string? PicturePath { get; set; }

        public Patient? Patient { get; set; }
    }
}
