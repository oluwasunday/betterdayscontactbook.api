using BetterDaysContactBook.Core.helper;
using BetterDaysContactBook.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterDaysContactBook.Core
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration configuration;
        private readonly Cloudinary cloudinary;
        private readonly ImageUploadSettings _imageUploadSetting;
        public ImageService(IConfiguration configuration, IOptions<ImageUploadSettings> imageUploadSetting)
        {
            _imageUploadSetting = imageUploadSetting.Value;
            this.configuration = configuration;
            this.cloudinary = new Cloudinary(
                new Account(_imageUploadSetting.CloudName, _imageUploadSetting.ApiKey, _imageUploadSetting.ApiSecret));

        }

        public async Task<UploadResult> UploadAsync(IFormFile image)
        {
            var pictureMaxLength = Convert.ToInt32(configuration.GetSection("PhotoSettings:Size").Get<string>());
            if(image.Length > pictureMaxLength){
                throw new ArgumentOutOfRangeException("Maximum Image size required is 3mb");
            }

            bool pictureFormat = false;
            var imageExtensions = configuration.GetSection("PhotoSettings:Formats").Get<List<string>>();

            foreach (var item in imageExtensions) {
                if (image.FileName.EndsWith(item)) {
                    pictureFormat = true;
                    break;
                }
            }

            if(pictureFormat == false)
                throw new BadImageFormatException("File format not supported");

            var uploadPic = new ImageUploadResult();

            using(var imageStream = image.OpenReadStream())
            {
                string fileName = Guid.NewGuid().ToString() + image.FileName;

                uploadPic = await cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(fileName, imageStream),
                    Transformation = new Transformation()
                        .Crop("thumb")
                        .Gravity("face")
                        .Width(150)
                        .Height(200)
                        .Radius(5)
                });
            }

            return uploadPic;
        }
    }
}
