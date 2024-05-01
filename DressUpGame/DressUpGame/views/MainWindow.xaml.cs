﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using DressUpGame.models;
//using static System.Net.Mime.MediaTypeNames;

namespace DressUpGame
{
    public partial class MainWindow : Window
    {
        private readonly DressUpFacade facade;

        public MainWindow()
        {
            InitializeComponent();
            facade = new DressUpFacade(new Game(), Player.GetInstance());
            RefreshUI();
        }

        private void RefreshUI()
        {
            facade.GetRandomEvent();
            eventInfoTextBlock.Text = facade.GetCurrentEventDescription();
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
            RefreshUI();
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDescriptionWindow($"Your style has to be {facade.GetCurrentEventStyle()}!! Please!!!\nAlso important note:\n\nwhen you choose hat - it also automatically chooses earrings and shoes that are can be +1 point!");
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
    }
}
