using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using DressUpGame.models;
using DressUpGame.controls;

namespace DressUpGame
{
    public partial class MainWindow : Window
    {
        private readonly DressUpFacade facade;
        private int style;

        public MainWindow()
        {
            InitializeComponent();
            facade = new DressUpFacade(new ClothingEventManager(), Player.GetInstance());
            RefreshUI(null, null);
            style = 1;
        }

        private void RefreshUI(object sender, RoutedEventArgs e)
        {
            facade.GetRandomEvent();
            eventInfoTextBlock.Text = facade.GetCurrentEventDescription();
            ClearOutfitButton_Click(sender, e);
        }

        private void DressUpButton_Click(object sender, RoutedEventArgs e)
        {
            facade.DressUp();
            ShowDescriptionWindow("Outfit Description");
        }

        private void ImReadyButton_Click(object sender, RoutedEventArgs e)
        {
            facade.DressUp();
            int score = facade.GetScore();
            ShowDescriptionWindow($"Score: {score}");
            RefreshUI(null, null);
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDescriptionWindow($"Your style has to be {facade.GetCurrentEventStyle()}!! Please!!!\nAlso important notes:\n\nwhen you choose hat - it also automatically chooses earrings and shoes that are can be +1 point!\nand when you're choosing mood or wether, it's better to be fully dressed, or it will not have any impact!");
        }

        private void ShowDescriptionWindow(string title)
        {
            ClothingDescriptionWindow descriptionWindow = new(facade.GetOutfitDescription(title));
            descriptionWindow.ShowDialog();
        }

        // Image click handlers for clothes and accessories
        private void ShirtImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetShirt(GetStyleFromImageName(imageName));
            shirtImage.Source = new BitmapImage(new Uri($"../assets/outfits{style}/shirt_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
        }

        private void PantsImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetPants(GetStyleFromImageName(imageName));
            pantsImage.Source = new BitmapImage(new Uri($"../assets/outfits{style}/pants_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
        }

        private void AccessoriesImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetAccessories(GetStyleFromImageName(imageName));

            shoesImage.Source = new BitmapImage(new Uri($"../assets/outfits{style}/shoes_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
            earringsImage.Source = new BitmapImage(new Uri($"../assets/outfits{style}/earrings_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
            hatImage.Source = new BitmapImage(new Uri($"../assets/outfits{style}/hat_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
        }

        private ClothingStyle GetStyleFromImageName(string imageName)
        {
            char styleDigit = imageName[imageName.Length - 1];
            switch (styleDigit)
            {
                case '2':
                    return ClothingStyle.Casual;
                case '3':
                    return ClothingStyle.Formal;
                case '4':
                    return ClothingStyle.Cool;
                default:
                    return ClothingStyle.None;
            }
        }

        private void MoodButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string buttonContent = clickedButton.Content.ToString();

                facade.SetMoodDecorations(buttonContent);
                moodTextBox.Text = buttonContent;
            }
        }

        private void WeatherButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string buttonContent = clickedButton.Content.ToString();

                facade.SetWeatherDecorations(buttonContent);
                weatherTextBox.Text = buttonContent;
            }
        }

        private void ClearOutfitButton_Click(object sender, RoutedEventArgs e)
        {
            facade.ClearOutfit();
            shirtImage.Source = null;
            pantsImage.Source = null;
            hatImage.Source = null;
            earringsImage.Source = null;
            shoesImage.Source = null;
            moodTextBox.Text = string.Empty;
            weatherTextBox.Text = string.Empty;
        }

    }
}
