using Inventory.Domain.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Utilities
{
    public class ImageResizerUtility : IImageResizerUtility
    {
        public async Task ResizeImageAsync(string imagePath)
        {
            using var image = await Image.LoadAsync(imagePath);
            IImageFormat format;
            await using (var detectStream = File.OpenRead(imagePath))
            {
                format = await Image.DetectFormatAsync(detectStream);
            }
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(300, 300),
                Mode = ResizeMode.Max 
            }));

            await using var outputStream = File.Open(imagePath, FileMode.Create);
            await image.SaveAsync(outputStream, format);

        }
    }
}
