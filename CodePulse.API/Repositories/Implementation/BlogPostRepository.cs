using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteBlogPost(Guid id)
        {
            var IsExistingBlogPost = await this.dbContext.BlogPosts.FirstOrDefaultAsync(blog => blog.Id == id);

            if(IsExistingBlogPost == null)
            {
                return null;
            }

            this.dbContext.BlogPosts.Remove(IsExistingBlogPost);
            await dbContext.SaveChangesAsync();
            return IsExistingBlogPost;
            
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetPostById(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<BlogPost?> GetPostByUrlHandle(string urlHandle)
        {
            return await this.dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x=>x.UrlHandle == urlHandle);  
        }

        public async Task<BlogPost?> UpdateBlogPost(BlogPost blogPost)
        {
            var ExistingBlogPost = await dbContext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id == blogPost.Id);

            if (ExistingBlogPost == null)
            {
                return null;
            }

            dbContext.Entry(ExistingBlogPost).CurrentValues.SetValues(blogPost);

            ExistingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }
    }
}
