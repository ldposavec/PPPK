using Microsoft.EntityFrameworkCore;

namespace Medik.Models
{
    public class MedikContext : DbContext
    {
        public MedikContext(DbContextOptions<MedikContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedDocumentation> MedDocumentations { get; set; }
        public DbSet<Examination> Examinations { get; set; }
    }
}
