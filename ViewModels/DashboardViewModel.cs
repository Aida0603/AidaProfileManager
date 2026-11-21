namespace AidaProfileManager.ViewModels;

public sealed class DashboardViewModel
{
    public int TotalProfiles { get; init; }
    public int ActiveProfiles { get; init; }
    public int InactiveProfiles { get; init; }
    public IReadOnlyList<ProfileListItemViewModel> RecentProfiles { get; init; } = [];
}
