using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Repositories;

namespace Bloggie.Web.Controllers;

public class AdminTagsController : Controller
{
    private readonly ITagInterface tagRepository;

    public AdminTagsController(ITagInterface tagRepository)
    {
        this.tagRepository = tagRepository;
    }


    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

		//bloggieDbContext.Tags.Add(tag);
		//bloggieDbContext.SaveChanges();
		await tagRepository.AddAsync(tag);

		return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var tags = await tagRepository.GetAllAsync();
        return View(tags);
    }
}
