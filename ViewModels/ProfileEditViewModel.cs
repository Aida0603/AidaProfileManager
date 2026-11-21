using System.ComponentModel.DataAnnotations;
using AidaProfileManager.Validation;

namespace AidaProfileManager.ViewModels;

public sealed class ProfileEditViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Толық аты-жөнін енгізіңіз.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Аты-жөні 2–120 таңбадан тұруы керек.")]
    [Display(Name = "Толық аты-жөні")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пайдаланушы атын енгізіңіз.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Пайдаланушы аты 3–30 таңбадан тұруы керек.")]
    [RegularExpression("^[A-Za-z0-9._-]+$", ErrorMessage = "Тек латын әріптері, сандар, нүкте, сызықша және астын сызу таңбасын қолданыңыз.")]
    [Display(Name = "Пайдаланушы аты")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Электрондық поштаны енгізіңіз.")]
    [StringLength(254, ErrorMessage = "Электрондық пошта 254 таңбадан аспауы керек.")]
    [EmailAddress(ErrorMessage = "Электрондық пошта пішімі дұрыс емес.")]
    [Display(Name = "Электрондық пошта")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Жасын енгізіңіз.")]
    [Range(16, 100, ErrorMessage = "Жас 16 мен 100 аралығында болуы керек.")]
    [Display(Name = "Жасы")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Телефон нөмірін енгізіңіз.")]
    [RegularExpression("^(?=(?:.*\\d){10,15}$)\\+?[0-9()\\-\\s]{10,24}$", ErrorMessage = "Телефон нөмірінің пішімі дұрыс емес.")]
    [Display(Name = "Телефон нөмірі")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 8, ErrorMessage = "Құпиясөз 8–100 таңбадан тұруы керек.")]
    [StrongPassword]
    [DataType(DataType.Password)]
    [Display(Name = "Жаңа құпиясөз")]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Құпиясөздер сәйкес келмейді.")]
    [DataType(DataType.Password)]
    [Display(Name = "Жаңа құпиясөзді растау")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Жынысын таңдаңыз.")]
    [RegularExpression("^(Әйел|Ер|Көрсеткім келмейді)$", ErrorMessage = "Тізімнен бір мән таңдаңыз.")]
    [Display(Name = "Жынысы")]
    public string Gender { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Өзі туралы ақпарат 500 таңбадан аспауы керек.")]
    [Display(Name = "Өзі туралы")]
    public string? Bio { get; set; }

    [Display(Name = "Белсенді профиль")]
    public bool IsActive { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
        {
            yield return new ValidationResult("Жаңа құпиясөзді растаңыз.", [nameof(ConfirmPassword)]);
        }

        if (Age.HasValue && Age.Value < 18 && (string.IsNullOrWhiteSpace(Bio) || Bio.Trim().Length < 20))
        {
            yield return new ValidationResult(
                "18 жасқа толмаған пайдаланушы өзі туралы кемінде 20 таңба жазуы керек.",
                [nameof(Bio)]);
        }
    }
}
