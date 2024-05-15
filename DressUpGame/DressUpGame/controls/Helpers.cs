using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.controls
{
    /*You can usage of this func in MainWindow.xaml Image elements
     Helps to crop images and display parts of it
    
     Example: hats are only at the top of whole outfit, 
       so for shelf-display it crops it to top
    
     -Set+Get crop rect
     -Set+Get original image source path*/

    public static class CroppingHelper
    {
        public static readonly DependencyProperty CropRectangleProperty =
            DependencyProperty.RegisterAttached(
                "CropRectangle",
                typeof(Rect),
                typeof(CroppingHelper),
                new FrameworkPropertyMetadata(new Rect(), FrameworkPropertyMetadataOptions.AffectsRender, OnCropRectangleChanged));

        public static readonly DependencyProperty OriginalSourceProperty =
            DependencyProperty.RegisterAttached(
                "OriginalSource",
                typeof(string),
                typeof(CroppingHelper),
                new PropertyMetadata(null));

        public static void SetCropRectangle(UIElement element, Rect value)
        {
            element.SetValue(CropRectangleProperty, value);
        }

        public static Rect GetCropRectangle(UIElement element)
        {
            return (Rect)element.GetValue(CropRectangleProperty);
        }

        public static void SetOriginalSource(UIElement element, string value)
        {
            element.SetValue(OriginalSourceProperty, value);
        }

        public static string GetOriginalSource(UIElement element)
        {
            return System.IO.Path.GetFileNameWithoutExtension((string)element.GetValue(OriginalSourceProperty));
        }

        private static void OnCropRectangleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Image image)
            {
                if (image.Source is BitmapSource source)
                {
                    Rect cropRect = (Rect)e.NewValue;
                    CroppedBitmap croppedBitmap = new(source, new Int32Rect((int)cropRect.X, (int)cropRect.Y, (int)cropRect.Width, (int)cropRect.Height));
                    image.Source = croppedBitmap;

                    SetOriginalSource(image, source.ToString());
                }
            }
        }
    }

    //There's no .Count func in enums and flags
    public static class FlagHelper
    {
        public static int GetNumberOfOptions<T>() where T : Enum
        {
            int count = 0;
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                if (((int)(object)value) != 0 && (((int)(object)value) & (((int)(object)value) - 1)) == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }

    //helps to save+load new high strike 
    public class FileHelper
    {
        private const string streakFilePath = "streak.txt";

        public void LoadMaxScoreStreak(int[] maxScoreStreak)
        {
            if (File.Exists(streakFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(streakFilePath))
                    {
                        maxScoreStreak[0] = int.Parse(reader.ReadLine());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading max score streak: {ex.Message}");
                }
            }
        }

        public void SaveMaxScoreStreak(int maxScoreStreak)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(streakFilePath, false)) // Overwrite existing file
                {
                    writer.WriteLine(maxScoreStreak);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving max score streak: {ex.Message}");
            }
        }
    }
}