using AidaProfileManager.Services;
using AidaProfileManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AidaProfileManager.Controllers;

public sealed class HomeController(IProfileService profiles) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await profiles.GetDashboardAsync(cancellationToken));

    public IActionResult Privacy() => RedirectToAction("ValidationGuide", "Profiles");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
