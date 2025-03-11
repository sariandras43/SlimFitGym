using SlimFitGym.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimFitGym.Models.Models;

namespace SlimFitGym.EFData.Interfaces
{
    public interface IImagesRepository
    {
        Image? UploadImageToAccount(string imageUri, int accountId);
        List<Image> UploadImagesToMachine(List<ImageRequest> images, int machineId);
        Image UploadImageToRoom(string image, int roomId);
        Image? DeleteImageByAccountId(int accountId);
        List<Image> DeleteImagesByMachineId(int machineId);
        Image DeleteImageByRoomId(int roomId);
        string GetImageUrlByAccountId(int accountId);
        List<string> GetImageUrlsByMachineId(int machineId);
        List<string> GetImageUrlsByRoomId(int roomId);
        string GetImageByPublicId(string publicId);
    }
}
