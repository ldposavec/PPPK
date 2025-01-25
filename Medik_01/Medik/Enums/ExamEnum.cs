using System.ComponentModel.DataAnnotations;

namespace Medik.Enums
{
    public enum ExamEnum
    {
        [Display(Name = "General Physical Exam")]
        GP,
        [Display(Name = "Blood Test")]
        KRV,
        [Display(Name = "X-Ray")]
        X_RAY,
        [Display(Name = "CT Scan")]
        CT,
        [Display(Name = "MRI Scan")]
        MR,
        [Display(Name = "Ultrasound")]
        ULTRA,
        [Display(Name = "Electrocardiogram")]
        EKG,
        [Display(Name = "Echocardiogram")]
        ECHO,
        [Display(Name = "Eye Exam")]
        EYE,
        [Display(Name = "Dermatology Exam")]
        DERM,
        [Display(Name = "Dental Exam")]
        DENTA,
        [Display(Name = "Mammography")]
        MAMMO,
        [Display(Name = "Neurology Exam")]
        NEURO

    }
}
