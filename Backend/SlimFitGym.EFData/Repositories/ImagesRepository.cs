using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SlimFitGym.Models.Models;
using SlimFitGym.Models.Requests;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SlimFitGym.EFData.Interfaces;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace SlimFitGym.EFData.Repositories
{
    public class ImagesRepository:IImagesRepository
    {
        private readonly SlimFitGymContext context;
        private readonly Cloudinary cloudinary;

        public ImagesRepository(SlimFitGymContext slimFitGymContext, IConfiguration configuration)
        {
            this.context = slimFitGymContext;

            var cloudinaryAccount = new CloudinaryDotNet.Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );

            cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public Image? UploadImageToAccount(string imageUri, int accountId) 
        {
            string[] splitUri = imageUri.Split(',');

            if (!Base64.IsValid(splitUri[1]))
                throw new Exception("Érvénytelen Base64.");
            //TODO filesize
            IFormFile file = ConvertBase64ToIFormFile(splitUri[1],"Asd", splitUri[0].Split(':')[1]);

            UploadResult? cloudinaryResult = UploadToCloudinary(file);

            if (cloudinaryResult == null)
                throw new Exception("Hiba történt a kép feltöltése során.");

            Image img = new Image()
            {
                CloudinaryId = cloudinaryResult.PublicId.ToString(),
                AccountId = accountId,
                Url = cloudinaryResult.SecureUrl.ToString(),
                IsHighlighted = true
            };
            context.Set<Image>().Add(img);
            context.SaveChanges();

            return img;
        }

        public List<Image> UploadImagesToMachine(List<string> images, int machineId)
        {

            List<Image> result = new List<Image>();
            if (images.Count==0)
            {
                DeleteImagesByMachineId(machineId);
                context.SaveChanges();
                return result;
            }
            if (images.Count != 2)
                throw new Exception("Két kép állítható be gépekhez.");

            for (int i = 0; i < images.Count; i++)
            {
                string image = images[i];
                if (!image.StartsWith("https://res.cloudinary.com"))
                {
                    string[] splitUri = image.Split(',');
                    if (splitUri.Length!=2)
                        throw new Exception("Érvénytelen Base64.");
                    if (!Base64.IsValid(splitUri[1]))
                        throw new Exception("Érvénytelen Base64.");
                    IFormFile file = ConvertBase64ToIFormFile(splitUri[1], "Asd", splitUri[0].Split(':')[1]);

                    UploadResult? cloudinaryResult = UploadToCloudinary(file);
                    
                    if (cloudinaryResult == null)
                        throw new Exception("Hiba történt a kép feltöltése során.");

                    Image img = new Image()
                    {
                        CloudinaryId = cloudinaryResult.PublicId.ToString(),
                        MachineId = machineId,
                        Url = cloudinaryResult.SecureUrl.ToString(),
                        IsHighlighted = false
                    };
                    context.Set<Image>().Add(img);

                    result.Add(img);


                }
                else 
                {
                    Image? img = context.Set<Image>().SingleOrDefault(i => i.Url == image.Trim() && i.MachineId == machineId);
                    if (img == null)
                        throw new Exception("Érvénytelen url: külső url-ek nem elfogadottak.");
                    result.Add(img);
                }
            }
            //TODO id alapjan, mert full ugyanolyan nem lesz a highlight maitt
            List<Image> allImageUrlsToAMachine = this.context.Set<Image>().Where(i=>i.MachineId==machineId).ToList();
            foreach (Image img in allImageUrlsToAMachine)
            {
                if (!result.Contains(img))
                {
                    DeleteImageByPublicId(img.CloudinaryId);
                    context.Set<Image>().Remove(img);

                }
            }
            context.SaveChanges();
            if (HighlightMachineImage(result[0].CloudinaryId, machineId))
            {
                context.SaveChanges();
                return result;
            }
            throw new Exception("Hiba történt");
        }

        public Image UploadImageToRoom(string image, int roomId)
        {
            string[] splitUri = image.Split(',');

            if (!Base64.IsValid(splitUri[1]))
                throw new Exception("Érvénytelen Base64.");
            //TODO filesize
            IFormFile file = ConvertBase64ToIFormFile(splitUri[1], "Asd", splitUri[0].Split(':')[1]);

            UploadResult? cloudinaryResult = UploadToCloudinary(file);

            if (cloudinaryResult == null)
                throw new Exception("Hiba történt a kép feltöltése során.");

            Image img = new Image()
            {
                CloudinaryId = cloudinaryResult.PublicId.ToString(),
                RoomId = roomId,
                Url = cloudinaryResult.SecureUrl.ToString(),
                IsHighlighted = true
            };
            context.Set<Image>().Add(img);
            context.SaveChanges();

            return img;
        }

        public Image? DeleteImageByAccountId(int accountId)
        {
            var imageToRemove = context.Set<Image>().FirstOrDefault(i => i.AccountId == accountId);

            if (imageToRemove!=null)
            {
                var res = DeleteImageByPublicId(imageToRemove.CloudinaryId);
                if (res == true)
                {
                    context.Set<Image>().Remove(imageToRemove);
                    context.SaveChanges();                
                    return imageToRemove;
                    
                }
            }return null;
        }

        public List<Image> DeleteImagesByMachineId(int machineId)
        {
            List<Image> imagesToRemove = context.Set<Image>().Where(i => i.MachineId == machineId).ToList();
            if (imagesToRemove.Count>0)
            {
                foreach (Image image in imagesToRemove)
                {
                    DeleteImageByPublicId(image.CloudinaryId);
                    context.Set<Image>().Remove(image);
                }              
            }
            return imagesToRemove;

        }

        public Image DeleteImageByRoomId(int roomId)
        {
            Image imageToRemove = context.Set<Image>().SingleOrDefault(i => i.RoomId == roomId);
            if (imageToRemove!=null)
            {
                    DeleteImageByPublicId(imageToRemove.CloudinaryId);
                    context.Set<Image>().Remove(imageToRemove);
                    context.SaveChanges();
            }
            return imageToRemove;
        }

        public string GetImageUrlByAccountId(int accountId)
        {
            Image? image = context.Set<Image>().FirstOrDefault(i => i.AccountId == accountId);
            if (image !=null)
            {
                return image.Url;
            }return "";
        }

        public List<string> GetImageUrlsByMachineId(int machineId)
        {
            return context.Set<Image>().Where(i => i.MachineId == machineId).OrderByDescending(i=>i.IsHighlighted).Select(i => i.Url).ToList();
        }

        public string GetImageUrlByRoomId(int roomId)
        {
            Image? image = context.Set<Image>().FirstOrDefault(i => i.RoomId == roomId);
            if (image != null)
            {
                return image.Url;
            }
            return "";
        }

        public string GetImageByPublicId(string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId);
            var resource = cloudinary.GetResource(getResourceParams);

            if (resource == null)
                return "";

            return resource.SecureUrl;
        }

        private IFormFile ConvertBase64ToIFormFile(string base64String, string fileName, string contentType)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                var stream = new MemoryStream(fileBytes);

                return new FormFile(stream, 0, fileBytes.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
            }
            catch (Exception)
            {
                throw new Exception("Hiba a Base64-es konverzió során");
            }
        }
        private UploadResult? UploadToCloudinary(IFormFile file)
        {
            //TODO
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "SlimFitGym",
                    UseFilename = false, 
                    UniqueFilename = true 
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult;
            }
        }

        private bool DeleteImageByPublicId(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var deletionResult = cloudinary.Destroy(deletionParams);

            return deletionResult.Result == "ok"; 

        }

        private bool HighlightMachineImage(string publicId,int machineId)
        {
            this.context.Set<Image>().Where(i => i.MachineId == machineId).ExecuteUpdate<Image>(setters=>setters.SetProperty(i=>i.IsHighlighted,false));

            Image? img = this.context.Set<Image>().SingleOrDefault(i => i.CloudinaryId == publicId);
            if (img == null)
                return false;
            img.IsHighlighted=true;

            this.context.Entry(img).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //this.context.SaveChanges();
            return true; // Can only return when no errors occur at modifying
        }
    }
}
