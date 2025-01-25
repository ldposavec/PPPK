using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string Medication { get; set; }

        [Required]
        public string Dose { get; set; }

        [Required]
        public string Frequency { get; set; }

        [Required]
        public string Duration { get; set; }

        public Patient Patient { get; set; }
    }
}
