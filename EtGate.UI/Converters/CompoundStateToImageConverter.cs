using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using static Domain.Services.InService.InServiceMgr;

namespace EtGate.UI.Converters;

public class CompoundStateToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CompoundState state)
        {
            // Construct the image file path based on the EnumFruit value
            var fileName = $"{state}.png";
            var filePath = $"Assets/{fileName}"; // Update with your actual path
            return new Bitmap(filePath);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
