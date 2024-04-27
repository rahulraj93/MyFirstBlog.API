using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST {url}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                //file upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //Domain to Dto

                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    FileName = blogImage.FileName,
                    FileExtension = blogImage.FileName,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    Url = blogImage.Url
                };

                return Ok(response);

            }
            return BadRequest(ModelState);

        }

        //GET {url}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await this.imageRepository.GetAll();

            if(images == null)
            {
                return NotFound();
            }

            var blogImageDtoList = new List<BlogImageDto>();
            //Domain to DTO
            foreach(var image in images)
            {
                blogImageDtoList.Add(new BlogImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    FileExtension = image.FileName,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    Url = image.Url
                });
            }
            return Ok(blogImageDtoList);

        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) 
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if(file.Length > 18585768)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10 MB");
            }
        }
    }
}
