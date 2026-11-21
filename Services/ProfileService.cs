using AidaProfileManager.Data;
using AidaProfileManager.Models;
using AidaProfileManager.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AidaProfileManager.Services;

public sealed class ProfileService(
    ApplicationDbContext db,
    IPasswordHasher<Profile> passwordHasher) : IProfileService
{
    public async Task<DashboardViewModel> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        var total = await db.Profiles.CountAsync(cancellationToken);
        var active = await db.Profiles.CountAsync(x => x.IsActive, cancellationToken);
        var recent = await db.Profiles.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(5)
            .Select(x => new ProfileListItemViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Username = x.Username,
                Email = x.Email,
                Age = x.Age,
                Gender = x.Gender,
                IsActive = x.IsActive,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new DashboardViewModel
        {
            TotalProfiles = total,
            ActiveProfiles = active,
            InactiveProfiles = total - active,
            RecentProfiles = recent
        };
    }

    public async Task<ProfileIndexViewModel> GetProfilesAsync(string? search, string? status, CancellationToken cancellationToken = default)
    {
        var query = db.Profiles.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(x => x.FullName.Contains(term) || x.Username.Contains(term) || x.Email.Contains(term));
        }

        if (status == "active") query = query.Where(x => x.IsActive);
        if (status == "inactive") query = query.Where(x => !x.IsActive);

        var profiles = await query.OrderBy(x => x.FullName)
            .Select(x => new ProfileListItemViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Username = x.Username,
                Email = x.Email,
                Age = x.Age,
                Gender = x.Gender,
                IsActive = x.IsActive,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new ProfileIndexViewModel
        {
            Profiles = profiles,
            Search = search,
            Status = status,
            TotalCount = await db.Profiles.CountAsync(cancellationToken)
        };
    }

    public Task<Profile?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        db.Profiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> UsernameExistsAsync(string username, int? excludingId = null, CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(username);
        return db.Profiles.AnyAsync(x => x.NormalizedUsername == normalized && (!excludingId.HasValue || x.Id != excludingId.Value), cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, int? excludingId = null, CancellationToken cancellationToken = default)
    {
        var normalized = Normalize(email);
        return db.Profiles.AnyAsync(x => x.NormalizedEmail == normalized && (!excludingId.HasValue || x.Id != excludingId.Value), cancellationToken);
    }

    public async Task CreateAsync(ProfileCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var profile = new Profile
        {
            FullName = model.FullName.Trim(),
            Username = model.Username.Trim(),
            NormalizedUsername = Normalize(model.Username),
            Email = model.Email.Trim(),
            NormalizedEmail = Normalize(model.Email),
            Age = model.Age,
            PhoneNumber = model.PhoneNumber.Trim(),
            Gender = model.Gender,
            Bio = CleanBio(model.Bio),
            IsActive = model.IsActive,
            CreatedAtUtc = DateTime.UtcNow
        };
        profile.PasswordHash = passwordHasher.HashPassword(profile, model.Password);
        db.Profiles.Add(profile);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Profile profile, ProfileEditViewModel model, CancellationToken cancellationToken = default)
    {
        profile.FullName = model.FullName.Trim();
        profile.Username = model.Username.Trim();
        profile.NormalizedUsername = Normalize(model.Username);
        profile.Email = model.Email.Trim();
        profile.NormalizedEmail = Normalize(model.Email);
        profile.Age = model.Age;
        profile.PhoneNumber = model.PhoneNumber.Trim();
        profile.Gender = model.Gender;
        profile.Bio = CleanBio(model.Bio);
        profile.IsActive = model.IsActive;
        profile.UpdatedAtUtc = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            profile.PasswordHash = passwordHasher.HashPassword(profile, model.Password);
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        db.Profiles.Remove(profile);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static string Normalize(string value) => value.Trim().ToUpperInvariant();
    private static string? CleanBio(string? bio) => string.IsNullOrWhiteSpace(bio) ? null : bio.Trim();
}
