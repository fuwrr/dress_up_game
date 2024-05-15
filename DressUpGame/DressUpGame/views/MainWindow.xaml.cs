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
        List<string> selectedMoods;
        List<string> selectedWeather;
        IStreakObserver streakObserver = new StreakDisplayObserver();

        public MainWindow()
        {
            InitializeComponent();
            facade = new DressUpFacade(new ClothingEventManager(), Player.GetInstance(), streakObserver);
            selectedMoods = new List<string>();
            selectedWeather = new List<string>();
            RefreshUI(null, null);
        }

        private void RefreshUI(object? sender, RoutedEventArgs? e)
        {
            facade.GetRandomEvent();
            eventInfoTextBlock.Text = facade.GetCurrentEventDescription();
            ClearOutfitButton_Click(sender, e);
            selectedMoods = new List<string>();
            selectedWeather = new List<string>();
        }

        private void DressUpButton_Click(object sender, RoutedEventArgs e)
        {
            facade.DressUp();
            InfoWindow descriptionWindow = new(facade.GetOutfitDescription());
            descriptionWindow.ShowDialog();
        }

        private void ImReadyButton_Click(object sender, RoutedEventArgs e)
        {
            facade.DressUp();
            int score = facade.GetScore();
            InfoWindow scoreWindow = new($"Score: {score}");
            scoreWindow.ShowDialog();
            RefreshUI(null, null);
        }

        //Add showing mood and weather
        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow hintWindow = new($"Your style has to be {facade.GetCurrentEventStyle()}!! Please!!!\nAlso important notes:\n\nwhen you choose hat - it also automatically chooses earrings and shoes that are can be +1 point!\nand when you're choosing mood or weather, it will be counted in score no matter are you dressed or not, but description will be changed only if necessary clothes are worn");
            hintWindow.ShowDialog();
        }

        // Image click handlers for clothes and accessories
        private void ShirtImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetShirt(GetStyleFromImageName(imageName));
            shirtImage.Source = new BitmapImage(new Uri($"../assets/outfits/shirt_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
        }

        private void PantsImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetPants(GetStyleFromImageName(imageName));
            pantsImage.Source = new BitmapImage(new Uri($"../assets/outfits/pants_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
        }

        private void AccessoriesImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            facade.SetAccessories(GetStyleFromImageName(imageName));

            shoesImage.Source = new BitmapImage(new Uri($"../assets/outfits/shoes_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
            earringsImage.Source = new BitmapImage(new Uri($"../assets/outfits/earrings_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
            hatImage.Source = new BitmapImage(new Uri($"../assets/outfits/hat_{imageName[imageName.Length - 1]}.png", UriKind.Relative));
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
                string mood = clickedButton.Content.ToString();

                facade.SetMoodDecorations(mood);
                if (!selectedMoods.Contains(mood))
                {
                    selectedMoods.Add(mood);
                }
                moodTextBox.Text = string.Join(" + ", selectedMoods);
            }
        }

        private void WeatherButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string weather = clickedButton.Content.ToString();

                facade.SetWeatherDecorations(weather);

                if (!selectedWeather.Contains(weather))
                {
                    selectedWeather.Add(weather);
                }
                weatherTextBox.Text = string.Join(" + ", selectedWeather);
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
