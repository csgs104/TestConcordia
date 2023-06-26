namespace ConcordiaMVC.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;

public class HomeController : Controller
{
    private readonly BackgroundSynchronizer _backgroundSync;
    private readonly ILogger<HomeController> _logger;

    public HomeController(BackgroundSynchronizer backgroundSync, ILogger<HomeController> logger)
     : base()
    {
        _backgroundSync = backgroundSync;
        _logger = logger;
    }

    public virtual JsonResult Synchronizing()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.Synchronizing)} was called.");
        return Json(new { synchronizing = _backgroundSync.Synchronized });
    }

    public IActionResult About()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.About)} was called.");
        return View();
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.Index)} was called.");
        return View();
    }

    public IActionResult Privacy()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.Privacy)} was called.");
        return View();
    }

    public IActionResult GoToTasks()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.GoToTasks)} was called.");
        _logger.LogInformation($"Redirect to {nameof(TasksController)}.{nameof(TasksController.Index)}");
        return RedirectToAction("Index", "Tasks");
    }

    public IActionResult GoToUsers()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.GoToUsers)} was called.");
        _logger.LogInformation($"Redirect to {nameof(UsersController)}.{nameof(UsersController.Index)}");
        return RedirectToAction("Index", "Users");
    }

    public IActionResult Synchronization()
    {
        _logger.LogInformation($"{nameof(HomeController)}.{nameof(HomeController.Synchronization)} was called.");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        return View("Error", model);
    }
}