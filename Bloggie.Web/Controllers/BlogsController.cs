using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Repositories;
using Bloggie.Web.Models.ViewModels;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
	private readonly IBlogPostRepository blogPostRepository;
	private readonly IBlogPostLikeRepository blogPostLikeRepository;

	public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository)
	{
		this.blogPostRepository = blogPostRepository;
		this.blogPostLikeRepository = blogPostLikeRepository;
	}

	public IActionResult Index()
	{
		return View();
	}
}
