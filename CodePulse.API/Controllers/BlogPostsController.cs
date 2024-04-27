using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Implementation;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        public ICategoryRepository CategoryRepository { get; }

        //POST : /api/blogposts/
        [HttpPost]
        public async Task<IActionResult> CreateBlobPost([FromBody] CreateBLogPostRequestDto request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                Content = request.Content,
                IsAvailable = request.IsAvailable,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            //Get all Categories
            foreach (var category in request.Categories) {
                var categoryById = await categoryRepository.GetById(category);
                if (categoryById != null)
                {
                    blogPost.Categories.Add(categoryById);
                }
            }


            blogPost = await blogPostRepository.CreateAsync(blogPost);

            var reponse = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Author = blogPost.Author,
                Content = blogPost.Content,
                IsAvailable = blogPost.IsAvailable,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };

            return Ok(reponse);

        }

        //GET : /api/blogposts/

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            //AppDomain to DTO
            var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    IsAvailable = blogPost.IsAvailable,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }

            if (response.Count > 0)
            {
                return Ok(response);
            }

            return NotFound();
        }

        //GET : /api/blogposts/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid id)
        {
            var blogPost = await this.blogPostRepository.GetPostById(id);

            if (blogPost == null)
            {
                return NotFound();
            }
            //Domain to Dto

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Author = blogPost.Author,
                Content = blogPost.Content,
                IsAvailable = blogPost.IsAvailable,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
                
            };
            return Ok(response);
        }

        //GET : /api/blogposts/{urlHandl}

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetPostByUrlHandle([FromRoute]string urlHandle)
        {
            var blogPost = await this.blogPostRepository.GetPostByUrlHandle(urlHandle);
            if (blogPost == null)
            {
                return NotFound();
            }
            //Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Author = blogPost.Author,
                Content = blogPost.Content,
                IsAvailable = blogPost.IsAvailable,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };
            return Ok(response);

        }

        //PUT : /api/blogposts/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute]Guid id, UpdateBlogPostRequestDto request)
        {
            //DTO to domain 
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach(var categoryId in request.Categories)
            {
                var IsExistingCategory = await this.categoryRepository.GetById(categoryId);
                if(IsExistingCategory != null)
                {
                    blogPost.Categories.Add(IsExistingCategory);
                }
            }

            blogPost = await this.blogPostRepository.UpdateBlogPost(blogPost);

            if(blogPost == null)
            {
                return NotFound();
            }

            //Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsAvailable = blogPost.IsAvailable,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle

                }).ToList()
            };

            return Ok(response);

        }


        //DELETE : /api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute]Guid id)
        {
            var blogPost = await this.blogPostRepository.DeleteBlogPost(id);

            if(blogPost == null)
            {
                return NotFound();
            }

            //Domain to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsAvailable = blogPost.IsAvailable,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle                
            };

            return Ok(response);

        }
    }
}
