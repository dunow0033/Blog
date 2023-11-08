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

		var selectedTags = new List<Tag>();
		foreach(var selectedTagId in addBlogPostRequest.SelectedTags)
		{
			var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
			var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

			if(existingTag != null) 
			{
				selectedTags.Add(existingTag);
			}
		}

		blogPost.Tags = selectedTags;

		await blogPostRepository.AddAsync(blogPost);

		return RedirectToAction("List");
    }

	[HttpGet]
	public async Task<IActionResult> List()
	{
		var blogPost = await blogPostRepository.GetAllAsync();
		return View(blogPost);
	}

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
		var blogPost = await blogPostRepository.GetAsync(id);
		var tagsDomainModel = await tagRepository.GetAllAsync();

		if (blogPost != null)
		{
			var model = new EditBlogPostRequest
			{
				Id = blogPost.Id,
				Heading = blogPost.Heading,
				PageTitle = blogPost.PageTitle,
				Content = blogPost.Content,
				Author = blogPost.Author,
				ShortDescription = blogPost.ShortDescription,
				FeaturedImageUrl = blogPost.FeaturedImageUrl,
				PublishedDate = blogPost.PublishedDate,
				Visible = blogPost.Visible,
				Tags = tagsDomainModel.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				}),
				SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
			};

			return View(model);
		}


        return View();
    }

	[HttpPost]
	public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
	{
		var blogPostDomainModel = new BlogPost
		{
			Id = editBlogPostRequest.Id,
			Heading = editBlogPostRequest.Heading,
			PageTitle = editBlogPostRequest.PageTitle,
			Content = editBlogPostRequest.Content,
			Author = editBlogPostRequest.Author,
			ShortDescription = editBlogPostRequest.ShortDescription,
			FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
			PublishedDate = editBlogPostRequest.PublishedDate,
			UrlHandle = editBlogPostRequest.UrlHandle,
			Visible = editBlogPostRequest.Visible
		};

		var selectedTags = new List<Tag>();
		foreach(var selectedTag in editBlogPostRequest.SelectedTags)
		{
			if(Guid.TryParse(selectedTag, out var tag))
			{
				var foundTag = await tagRepository.GetAsync(tag);

				if(foundTag != null)
				{
					selectedTags.Add(foundTag);
				}
			}
		}

		blogPostDomainModel.Tags = selectedTags;

		var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

		if(updatedBlog != null)
		{
			return RedirectToAction("List");
		}

        return RedirectToAction("Edit");
    }

	[HttpPost]
	public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
	{
		var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

		if(deletedBlogPost != null)
		{
			return RedirectToAction("List");
		}

		return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
	}
}
