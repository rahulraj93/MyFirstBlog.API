namespace CodePulse.API.Models.DTO
{
    public class CreateBLogPostRequestDto
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public List<Guid> Categories { get; set; }
    }
}
