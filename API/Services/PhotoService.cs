namespace API.Services;

using System.Threading.Tasks;
using API.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using DotNetEnv;

public class PhotoService: IPhotoService{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config){
        Env.Load();
        //var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        
        string CloudName = Env.GetString("CloudName");
        string ApiKey = Env.GetString("ApiKey");
        string ApiSecret = Env.GetString("ApiSecret");

        var acc = new Account(CloudName,ApiKey,ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file){
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0){
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams{
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "spa2025"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
}
