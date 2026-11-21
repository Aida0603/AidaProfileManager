using AidaProfileManager.Models;
using AidaProfileManager.ViewModels;

namespace AidaProfileManager.Services;

public interface IProfileService
{
    Task<DashboardViewModel> GetDashboardAsync(CancellationToken cancellationToken = default);
    Task<ProfileIndexViewModel> GetProfilesAsync(string? search, string? status, CancellationToken cancellationToken = default);
    Task<Profile?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, int? excludingId = null, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, int? excludingId = null, CancellationToken cancellationToken = default);
    Task CreateAsync(ProfileCreateViewModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(Profile profile, ProfileEditViewModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(Profile profile, CancellationToken cancellationToken = default);
}
