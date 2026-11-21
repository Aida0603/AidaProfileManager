using AidaProfileManager.Models;
using AidaProfileManager.Services;
using AidaProfileManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AidaProfileManager.Controllers;

public sealed class ProfilesController(IProfileService profiles, ILogger<ProfilesController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? search, string? status, CancellationToken cancellationToken) =>
        View(await profiles.GetProfilesAsync(search, status, cancellationToken));

    [HttpGet]
    public IActionResult Create() => View(new ProfileCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfileCreateViewModel model, CancellationToken cancellationToken)
    {
        await AddDuplicateErrorsAsync(model.Username, model.Email, cancellationToken);
        if (!ModelState.IsValid) return View(model);

        try
        {
            await profiles.CreateAsync(model, cancellationToken);
            TempData["Toast"] = "Жаңа профиль сәтті тіркелді.";
            TempData["ToastKind"] = "success";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception, "A profile could not be created because a unique value was already in use.");
            AddUniqueConstraintErrors();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var profile = await profiles.GetByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : View(ToDetails(profile));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var profile = await profiles.GetByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : View(ToEdit(profile));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProfileEditViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        var profile = await profiles.GetByIdAsync(id, cancellationToken);
        if (profile is null) return NotFound();

        await AddDuplicateErrorsAsync(model.Username, model.Email, cancellationToken, id);
        if (!ModelState.IsValid) return View(model);

        try
        {
            await profiles.UpdateAsync(profile, model, cancellationToken);
            TempData["Toast"] = "Профиль жаңартылды.";
            TempData["ToastKind"] = "success";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (DbUpdateException exception)
        {
            logger.LogWarning(exception, "A profile could not be updated because a unique value was already in use.");
            AddUniqueConstraintErrors();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var profile = await profiles.GetByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : View(ToDetails(profile));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var profile = await profiles.GetByIdAsync(id, cancellationToken);
        if (profile is null) return NotFound();
        await profiles.DeleteAsync(profile, cancellationToken);
        TempData["Toast"] = "Профиль жойылды.";
        TempData["ToastKind"] = "success";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult ValidationGuide() => View(new ProfileCreateViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ValidationGuide(ProfileCreateViewModel model)
    {
        ViewBag.ValidationPassed = ModelState.IsValid;
        return View(model);
    }

    private async Task AddDuplicateErrorsAsync(string username, string email, CancellationToken cancellationToken, int? excludingId = null)
    {
        if (await profiles.UsernameExistsAsync(username, excludingId, cancellationToken))
            ModelState.AddModelError(nameof(ProfileCreateViewModel.Username), "Бұл пайдаланушы аты бұрын тіркелген.");
        if (await profiles.EmailExistsAsync(email, excludingId, cancellationToken))
            ModelState.AddModelError(nameof(ProfileCreateViewModel.Email), "Бұл электрондық пошта бұрын тіркелген.");
    }

    private void AddUniqueConstraintErrors()
    {
        ModelState.AddModelError(nameof(ProfileCreateViewModel.Username), "Пайдаланушы аты немесе электрондық пошта басқа профильде қолданылған.");
        ModelState.AddModelError(nameof(ProfileCreateViewModel.Email), "Пайдаланушы аты немесе электрондық пошта басқа профильде қолданылған.");
    }

    private static ProfileDetailsViewModel ToDetails(Profile profile) => new()
    {
        Id = profile.Id,
        FullName = profile.FullName,
        Username = profile.Username,
        Email = profile.Email,
        Age = profile.Age,
        PhoneNumber = profile.PhoneNumber,
        Gender = profile.Gender,
        Bio = profile.Bio,
        IsActive = profile.IsActive,
        CreatedAtUtc = profile.CreatedAtUtc,
        UpdatedAtUtc = profile.UpdatedAtUtc
    };

    private static ProfileEditViewModel ToEdit(Profile profile) => new()
    {
        Id = profile.Id,
        FullName = profile.FullName,
        Username = profile.Username,
        Email = profile.Email,
        Age = profile.Age,
        PhoneNumber = profile.PhoneNumber,
        Gender = profile.Gender,
        Bio = profile.Bio,
        IsActive = profile.IsActive
    };
}
