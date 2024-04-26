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
//using static System.Net.Mime.MediaTypeNames;

namespace DressUpGame
{
    public partial class MainWindow : Window
    {
        private readonly Game game;
        private readonly Player player;
        private readonly OutfitBuilder builder;
        private IAccessoryFactory accessoryFactory;
        private ClothingEvent randomEvent;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            randomEvent = game.GetRandomEvent();
            eventInfoTextBlock.Text = $"{randomEvent.Name} - {randomEvent.Description}";
            builder = new OutfitBuilder();
            player = Player.GetInstance();
        }

        private void DressUpButton_Click(object sender, RoutedEventArgs e)
        {
            List<IClothing> outfit = builder.Build();
            player.SetCurrentOutfit(outfit);
            player.DressUpForEvent(randomEvent);

            ClothingDescriptionWindow descriptionWindow = new($"Ocassion: {randomEvent.Name}\n{randomEvent.Description}\n\n" + player.GetOutfitDescription());
            descriptionWindow.ShowDialog();
        }

        private void ImReadyButton_Click(object sender, RoutedEventArgs e)
        {
            List<IClothing> outfit = builder.Build();
            player.SetCurrentOutfit(outfit);
            player.DressUpForEvent(randomEvent);

            ClothingDescriptionWindow descriptionWindow = new($"\nScore: {player.GetScore()}");
            descriptionWindow.ShowDialog();
            randomEvent = game.GetRandomEvent();
            eventInfoTextBlock.Text = $"{randomEvent.Name} - {randomEvent.Description}";
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            ClothingDescriptionWindow descriptionWindow = new($"Your style has to be {randomEvent.Style}!! Please!!!\nAlso important note:\n\nwhen you choose hat - it also automatically chooses earrings and shoes that are can be +1 point!");
            descriptionWindow.ShowDialog();
        }

        private void ShirtImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            //string imageName = System.IO.Path.GetFileNameWithoutExtension(clickedImage.Source.ToString());
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);

            char styleDigit = imageName[imageName.Length - 1];
            switch (styleDigit)
            {
                case '2':
                    builder.SetShirt(new CasualShirt());
                    break;
                case '3':
                    builder.SetShirt(new FormalShirt());
                    break;
                case '4':
                    builder.SetShirt(new CoolShirt());
                    break;
                default:
                    break;
            }

            shirtImage.Source = new BitmapImage(new Uri($"../assets/outfits/shirt_{styleDigit}.png", UriKind.Relative));
        }
        private void PantsImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            //string imageName = Path.GetFileNameWithoutExtension(clickedImage.Source.ToString());
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);
            //MessageBox.Show($"imagename: {imageName}, l: {imageName.Length}", "Title of the Message Box", MessageBoxButton.OK);

            char styleDigit = imageName[imageName.Length - 1];
            switch (styleDigit)
            {
                case '2':
                    builder.SetPants(new CasualPants());
                    break;
                case '3':
                    builder.SetPants(new FormalPants());
                    break;
                case '4':
                    builder.SetPants(new CoolPants());
                    break;
                default:
                    break;
            }

            pantsImage.Source = new BitmapImage(new Uri($"../assets/outfits/pants_{styleDigit}.png", UriKind.Relative));
        }

        private void AccessoriesImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            //string imageName = System.IO.Path.GetFileNameWithoutExtension(clickedImage.Source.ToString());
            string imageName = CroppingHelper.GetOriginalSource(clickedImage);


            char styleDigit = imageName[imageName.Length - 1];

            switch (styleDigit)
            {
                case '2':
                    accessoryFactory = new CasualAccessoryFactory();
                    break;
                case '3':
                    accessoryFactory = new FormalAccessoryFactory();
                    break;
                case '4':
                    accessoryFactory = new CoolAccessoryFactory();
                    break;
                default:
                    break;
            }

            builder.SetShoes(accessoryFactory.CreateShoes());
            builder.SetHat(accessoryFactory.CreateHat());
            builder.SetEarrings(accessoryFactory.CreateEarrings());

            shoesImage.Source = new BitmapImage(new Uri($"../assets/outfits/shoes_{styleDigit}.png", UriKind.Relative));
            earringsImage.Source = new BitmapImage(new Uri($"../assets/outfits/earrings_{styleDigit}.png", UriKind.Relative));
            hatImage.Source = new BitmapImage(new Uri($"../assets/outfits/hat_{styleDigit}.png", UriKind.Relative));
        }
    }
}
