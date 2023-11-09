using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
	private readonly IBlogPostRepository blogPostRepository;
	private readonly IBlogPostLikeRepository blogPostLikeRepository;

	public BlogsController(IBlogPostRepository blogPostRepository)
	{
		this.blogPostRepository = blogPostRepository;
	}

	[HttpGet]
	public async Task<IActionResult> Index(string urlHandle)
	{
		var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

		return View(blogPost);
	}
}
