using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Net;

namespace CoachFrika.APIs.Domin.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
    };
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await Task.Run(() => _cloudinary.Upload(uploadParams));

            if (uploadResult.StatusCode == HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }

            return null;
        }
    }

}
