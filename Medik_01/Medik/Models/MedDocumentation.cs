using Medik.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class MedDocumentation
    {
        private DateTime startIllness;
        private DateTime? endIllness;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public long PatientId { get; set; }

        [Required]
        [Display(Name = "Start of illness")]
        public DateTime StartIllness 
        { 
            get => startIllness; 
            set => startIllness = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [EndAfterStartIllness]
        [Display(Name = "End of illness")]
        public DateTime? EndIllness 
        { 
            get => endIllness; 
            set => endIllness = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : (DateTime?)null; 
        }

        public Patient? Patient { get; set; }
    }
}
