using SlimFitGym_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimFitGym_Mobile.Models
{
    public class MachineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public string[]? ImageUrls { get; set; } = null;

        public static List<MachineModel> machines = new()
        {
            new MachineModel
            {
                Id = 1,
                Name = "Treadmill",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 2,
                Name = "Elliptical",
                Description = "sdkwalfbawwf",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 3,
                Name = "Stationary Bike",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 4,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 5,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 6,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 7,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 8,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            },
            new MachineModel
            {
                Id = 9,
                Name = "Rowing Machine",
                Description = "fdghfhdgfsfasdgas",
                ImageUrls = ["https://picsum.photos/id/1018/80/150", "https://picsum.photos/id/1025/80/150"]
            }
        };
    }
}
