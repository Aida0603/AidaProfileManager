using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AidaProfileManager.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class StrongPasswordAttribute : ValidationAttribute, IClientModelValidator
{
    public StrongPasswordAttribute() : base("Құпиясөзде кемінде 8 таңба, бас және кіші әріп, сан және арнайы белгі болуы керек.") { }

    public override bool IsValid(object? value)
    {
        if (value is null || string.IsNullOrEmpty(value.ToString())) return true;

        var password = value.ToString()!;
        return password.Length >= 8
            && password.Any(char.IsUpper)
            && password.Any(char.IsLower)
            && password.Any(char.IsDigit)
            && password.Any(character => !char.IsLetterOrDigit(character));
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-strongpassword", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
    }

    private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key)) return false;
        attributes.Add(key, value);
        return true;
    }
}
