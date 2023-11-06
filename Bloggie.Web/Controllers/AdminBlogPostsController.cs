using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

//[Authorize(Roles = "Admin")]
public class AdminBlogPostsController : Controller
{
	private readonly ITagInterface tagRepository;
	private readonly IBlogPostRepository blogPostRepository;

	public AdminBlogPostsController(ITagInterface tagRepository, IBlogPostRepository blogPostRepository)
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
    public IActionResult Add()
	{
		return View();
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
