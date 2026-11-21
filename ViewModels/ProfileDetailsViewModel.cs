namespace AidaProfileManager.ViewModels;

public sealed class ProfileDetailsViewModel
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Age { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public string? Bio { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
}
