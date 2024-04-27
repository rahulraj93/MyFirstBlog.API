using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetPostById(Guid id);
        Task<BlogPost?> GetPostByUrlHandle(string urlHandle);
        Task<BlogPost?> UpdateBlogPost(BlogPost blogPost);
        Task<BlogPost?> DeleteBlogPost(Guid id);
    }
}
