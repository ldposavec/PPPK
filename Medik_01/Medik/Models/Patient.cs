using Medik.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medik.Models
{
    public class Patient
    {
        private DateTime dateOfBirth;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB must be 11 digits.")]
        [Display(Name = "OIB")]
        public string Oib { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth 
        { 
            get => dateOfBirth; 
            set => dateOfBirth = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
        }

        [Required]
        public GenderEnum Gender { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "Number of patient must be 5 digits.")]
        public string? NumberOfPatient { get; set; }

        public ICollection<MedDocumentation> MedDocumentations { get; set; } = new List<MedDocumentation>();
        public ICollection<Examination> Examinations { get; set; } = new List<Examination>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
