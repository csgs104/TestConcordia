namespace ConcordiaMVC.Controllers;

using Microsoft.AspNetCore.Mvc;
using ConcordiaDBLibrary;
using ConcordiaDBLibrary.Models.Classes;
// using ConcordiaDBLibrary.Models.Extensions.Abstract;
using ConcordiaDBLibrary.Models.Extensions.Classes;
using ConcordiaDBLibrary.Gateways.Abstract;
using Models;

public class UsersController : Controller
{
    private readonly ITrelloEntityGateway<Experiment> _experimentsGway;
    private readonly ITrelloEntityGateway<Scientist> _scientistsGway;
    private readonly IEntityGateway<Participant> _participantsGway;

    private readonly ILogger<UsersController> _logger;

    public UsersController(ITrelloEntityGateway<Experiment> experimentsGway,
                           ITrelloEntityGateway<Scientist> scientistsGway,
                           IEntityGateway<Participant> participantsGway,
                           ILogger<UsersController> logger)
     : base()
    {
        _experimentsGway = experimentsGway;
        _scientistsGway = scientistsGway;
        _participantsGway = participantsGway;
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{nameof(UsersController)}.{nameof(UsersController.Index)} was called.");
        var scientists = _scientistsGway.GetAll().ToList();
        if (scientists is null)
	    {
            return View(new UsersList());
        }
        return View(new UsersList(scientists));
    }

    public IActionResult DetailSingle(int id)
    {
        _logger.LogInformation($"{nameof(UsersController)}.{nameof(UsersController.DetailSingle)} was called.");
        var scientist = _scientistsGway.GetById(id);
        if (scientist is null)
        {
            return View(new UserSingleList(scientist));
        }
        var partecipants = _participantsGway.GetAll().Where(p => p.ScientistId == scientist.Id).ToList();
        if (partecipants is null)
        {
            return View(new UserSingleList(scientist));
        }
        var experimentIds = partecipants.Select(p => p.ExperimentId).ToList();
        if (experimentIds is null || !experimentIds.Any())
        {
            return View(new UserSingleList(scientist));
        }
        var experimentsByScientist = _experimentsGway.GetByIdMulti(experimentIds);
        if (experimentsByScientist is null)
        {
            return View(new UserSingleList(scientist)); 
	    }
        return View(new UserSingleList(scientist, experimentsByScientist.OrderBy(e => e.DueDate).OrderBy(e => e.OrderingByPriority()).ToList()));
    }

    public IActionResult DetailMulti(int id)
    {
        _logger.LogInformation($"{nameof(UsersController)}.{nameof(UsersController.DetailMulti)} was called.");
        var scientist = _scientistsGway.GetById(id);
        if (scientist is null)
        {
            return View(new UserMultiList(scientist));
        }
        var partecipants = _participantsGway.GetAll().Where(p => p.ScientistId == scientist.Id).ToList();
        if (partecipants is null)
        {
            return View(new UserMultiList(scientist));
        }
        var experimentIds = partecipants.Select(p => p.ExperimentId).ToList();
        if (experimentIds is null || !experimentIds.Any())
        {
            return View(new UserMultiList(scientist));
        }
        var experimentsByScientist = _experimentsGway.GetByIdMulti(experimentIds);
        if (experimentsByScientist is null)
        {
            return View(new UserMultiList(scientist));
        }
        var experimentsByScientistList = experimentsByScientist.ToList();
        var expsByScIn0 = experimentsByScientistList.Where(e => e.State!.Name.Equals(DBSettings.GetStatesNames()[0], StringComparison.OrdinalIgnoreCase)).ToList() ?? new List<Experiment>();
        var expsByScIn1 = experimentsByScientistList.Where(e => e.State!.Name.Equals(DBSettings.GetStatesNames()[1], StringComparison.OrdinalIgnoreCase)).ToList() ?? new List<Experiment>();
        var expsByScIn2 = experimentsByScientistList.Where(e => e.State!.Name.Equals(DBSettings.GetStatesNames()[2], StringComparison.OrdinalIgnoreCase)).ToList() ?? new List<Experiment>();
        return View(new UserMultiList(scientist, 
			                          expsByScIn0.OrderBy(e => e.DueDate).OrderBy(e => e.OrderingByPriority()).ToList(),
                                      expsByScIn1.OrderBy(e => e.DueDate).OrderBy(e => e.OrderingByPriority()).ToList(), 
								      expsByScIn2.OrderBy(e => e.DueDate).OrderBy(e => e.OrderingByPriority()).ToList()));
                    
    }

    public IActionResult DetailTask(int id)
    {
        _logger.LogInformation($"{nameof(UsersController)}.{nameof(UsersController.DetailTask)} was called.");
        return RedirectToAction("Detail", "Tasks", new { id });
    }
}