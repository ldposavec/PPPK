using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class MedDocumentation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Start of illness")]
        public DateTime StartIllness { get; set; }

        [Display(Name = "End of illness")]
        public DateTime? EndIllness { get; set; }

        public Patient Patient { get; set; }
    }
}
