namespace AidaProfileManager.ViewModels;

public sealed class ProfileIndexViewModel
{
    public IReadOnlyList<ProfileListItemViewModel> Profiles { get; init; } = [];
    public string? Search { get; init; }
    public string? Status { get; init; }
    public int TotalCount { get; init; }
}
