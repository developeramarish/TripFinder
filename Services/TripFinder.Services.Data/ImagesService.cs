﻿namespace TripFinder.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data.Common;

    public class ImagesService : IImagesService
    {
        private readonly Cloudinary cloudinary;
        private readonly IConfiguration configuration;
        private readonly IDeletableEntityRepository<Image> imagesRepository;

        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";

        public ImagesService(Cloudinary cloudinary, IConfiguration configuration, IDeletableEntityRepository<Image> imagesRepository)
        {
            this.cloudinary = cloudinary;
            this.configuration = configuration;
            this.imagesRepository = imagesRepository;
            this.cloudinary = cloudinary;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public async Task<Image> CreateAsync(IFormFile imageSource)
        {
            var compleateUrl = await ApplicationCloudinary.UploadFileAsync(this.cloudinary, imageSource);
            var url = compleateUrl.Replace(this.imagePathPrefix, string.Empty);
            var image = new Image { ImageUrl = url };

            await this.imagesRepository.AddAsync(image);
            await this.imagesRepository.SaveChangesAsync();

            return image;
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
