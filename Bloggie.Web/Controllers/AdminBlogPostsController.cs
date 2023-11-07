using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers;

//[Authorize(Roles = "Admin")]
public class AdminBlogPostsController : Controller
{
	private readonly ITagRepository tagRepository;
	private readonly IBlogPostRepository blogPostRepository;

	public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
	{
		this.tagRepository = tagRepository;
		this.blogPostRepository = blogPostRepository;
	}

	//[HttpGet]
	//public async Task<IActionResult> List()
	//{
	//	var blogPost = await blogPostRepository.GetAllAsync();
	//	return View();
	//}

	[HttpGet]
    public async Task<IActionResult> Add()
	{
		var tags = await tagRepository.GetAllAsync();

		var model = new AddBlogPostRequest
		{
			Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
		};

		return View(model);
	}

	[HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
	{
		var blogPost = new BlogPost
		{
			Heading = addBlogPostRequest.Heading,
			PageTitle = addBlogPostRequest.PageTitle,
			Author = addBlogPostRequest.Author,
			Content = addBlogPostRequest.Content,
			FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
			PublishedDate = addBlogPostRequest.PublishedDate,
			Visible = addBlogPostRequest.Visible,
			ShortDescription = addBlogPostRequest.ShortDescription,
			UrlHandle = addBlogPostRequest.UrlHandle,
		};

        await blogPostRepository.AddAsync(blogPost);

        return RedirectToAction("Add");
    }

	[HttpGet]
	public async Task<IActionResult> List()
	{
		var blogPost = await blogPostRepository.GetAllAsync();
		return View(blogPost);
	}
}
