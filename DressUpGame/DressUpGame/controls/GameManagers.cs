using DressUpGame.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.controls
{
    public class ClothingEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ClothingStyle Style { get; set; }
        //????
        public Weather Weather { get; set; }
        public Mood Mood { get; set; }
        //!!!!

        public ClothingEvent(string name, string description, ClothingStyle style, Weather weather, Mood mood)
        {
            Name = name;
            Description = description;
            Style = style;
            Weather = weather;
            Mood = mood;
        }
    }

    public class OutfitBuilder
    {
        private IClothing? shirt;
        private IClothing? pants;
        private IClothing? shoes;
        private IClothing? hat;
        private IClothing? earrings;

        public OutfitBuilder SetShirt(IClothing shirt)
        {
            this.shirt = shirt;
            return this;
        }

        public OutfitBuilder SetPants(IClothing pants)
        {
            this.pants = pants;
            return this;
        }

        public OutfitBuilder SetShoes(IClothing shoes)
        {
            this.shoes = shoes;
            return this;
        }

        public OutfitBuilder SetHat(IClothing hat)
        {
            this.hat = hat;
            return this;
        }

        public OutfitBuilder SetEarrings(IClothing earrings)
        {
            this.earrings = earrings;
            return this;
        }

        //????????

        public void Clear()
        {
            shirt = null;
            pants = null;
            hat = null;
            earrings = null;
            earrings = shoes;
        }

        public OutfitBuilder BeingSilly()
        {
            if (pants != null)
            {
                PantiesDecoration panties = new PantiesDecoration();
                pants = panties.Decorate(pants);
            }
            return this;
        }
        public OutfitBuilder BeingSerious()
        {
            if (shirt != null)
            {
                IroningDecoration ironing = new IroningDecoration();
                shirt = ironing.Decorate(shirt);
            }
            return this;
        }

        public OutfitBuilder WithSunglasses()//(Mood mood)
        {
            if (hat != null)
            {
                //if (mood == Mood.Happy || mood == Mood.Confident)
                //{
                SunglassesDecoration sunglasses = new SunglassesDecoration();
                //if (hat != null)
                //{
                //    throw new Exception("Cannot wear sunglasses with a hat!");
                //}
                //hat = sunglasses.Decorate(hat ?? new NoHat()); // Use NoHat class if hat is null

                hat = sunglasses.Decorate(hat); // Use NoHat class if hat is null

                //}
            }
            return this;
        }

        public OutfitBuilder WithScarf()//(Mood mood)
        {
            if (hat != null)
            {
                //if (mood == Mood.Sad)
                //{
                ScarfDecoration scarf = new ScarfDecoration();
                //hat = scarf.Decorate(hat ?? new NoHat()); // Use NoHat class if hat is null
                hat = scarf.Decorate(hat); // Use NoHat class if hat is null

                //}
            }
            return this;
        }

        public List<IClothing> Build()
        {
            List<IClothing> outfit = new();

            if (hat != null && earrings != null && shoes != null)
            {
                outfit.Add(hat);
                outfit.Add(earrings);
                outfit.Add(shoes);
            }

            if (shirt != null)
            {
                outfit.Add(shirt);
            }

            if (pants != null)
            {
                outfit.Add(pants);
            }

            return outfit;
        }
    }


    public class Player
    {
        private static Player? instance;

        private List<IClothing> currentOutfit = new();

        private int score = 0;

        private Player() { }

        public static Player GetInstance()
        {
            instance ??= new Player();
            return instance;
        }

        public List<IClothing> GetCurrentOutfit()
        {
            return currentOutfit;
        }

        public void SetCurrentOutfit(List<IClothing> outfit)
        {
            currentOutfit = outfit;
        }

        public string GetOutfitDescription()
        {
            string outfitDescription = "Current Outfit:\n";
            foreach (IClothing item in currentOutfit)
            {
                outfitDescription += $"{item.Name}: {item.Description}\n";
            }
            return outfitDescription;
        }

        public void DressUpForEvent(ClothingEvent clothingEvent)
        {
            score = 0;
            foreach (IClothing item in currentOutfit)
            {
                if (item.Style == clothingEvent.Style)
                {
                    score++;
                }
            }
        }

        public int GetScore()
        {
            return score;
        }
    }

    public class ClothingEventManager
    {
        private readonly List<ClothingEvent> events;

        public ClothingEventManager()
        {
            events = new List<ClothingEvent>
            {
                new ClothingEvent("Seeing Okscana Dmytrivna", "You want to look as smart as possible", ClothingStyle.Formal, Weather.Sunny, Mood.Silly),
                new ClothingEvent("Attending your wedding", "Actually you don't have time to be fancy", ClothingStyle.Casual, Weather.Sunny, Mood.Silly),
                new ClothingEvent("Going to give a lecture on behaviour to your niece", "You have to look older and colder.", ClothingStyle.Cool, Weather.Sunny, Mood.Silly),
                new ClothingEvent("Going to adoption centre for a cat", "You want to slightly impress your new buddy.", ClothingStyle.Formal, Weather.Sunny, Mood.Silly),
                new ClothingEvent("Going to take out a loan at the bank", "Better not be looking too rich or too cool.", ClothingStyle.Casual, Weather.Sunny, Mood.Silly),
                new ClothingEvent("Going to bless the Easter bread", "Don't look like a fool and dress only cool!", ClothingStyle.Cool, Weather.Sunny, Mood.Silly),
            };
        }

        public ClothingEvent GetRandomEvent()
        {
            Random rnd = new();
            int index = rnd.Next(events.Count);
            return events[index];
        }
    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public class DressUpFacade
    {
        private readonly ClothingEventManager game;
        private readonly Player player;
        private OutfitBuilder builder;
        private ClothingEvent currentEvent;

        public DressUpFacade(ClothingEventManager game, Player player)
        {
            this.game = game;
            this.player = player;
            builder = new OutfitBuilder();
            currentEvent = game.GetRandomEvent();
        }

        public void DressUp()
        {
            List<IClothing> outfit = builder.Build();
            player.SetCurrentOutfit(outfit);
            player.DressUpForEvent(game.GetRandomEvent());
        }

        public int GetScore()
        {
            return player.GetScore();
        }

        public string GetOutfitDescription(string title)
        {
            return $"{title}\n\n{player.GetOutfitDescription()}";
        }

        public void ClearOutfit()
        {
            builder.Clear();
            player.SetCurrentOutfit(new List<IClothing>());
        }

        public ClothingEvent GetRandomEvent()
        {
            currentEvent = game.GetRandomEvent();
            return currentEvent;
        }

        public string GetCurrentEventDescription()
        {
            ClothingEvent randomEvent = game.GetRandomEvent();
            return $"{randomEvent.Name} - {randomEvent.Description}";
        }

        public ClothingStyle GetCurrentEventStyle()
        {
            return game.GetRandomEvent().Style;
        }

        public void SetShirt(ClothingStyle style)
        {
            switch (style)
            {
                case ClothingStyle.Casual:
                    builder.SetShirt(new CasualShirt());
                    break;
                case ClothingStyle.Formal:
                    builder.SetShirt(new FormalShirt());
                    break;
                case ClothingStyle.Cool:
                    builder.SetShirt(new CoolShirt());
                    break;
                default:
                    builder.SetShirt(null);
                    break;
            }
        }

        public void SetPants(ClothingStyle style)
        {
            switch (style)
            {
                case ClothingStyle.Casual:
                    builder.SetPants(new CasualPants());
                    break;
                case ClothingStyle.Formal:
                    builder.SetPants(new FormalPants());
                    break;
                case ClothingStyle.Cool:
                    builder.SetPants(new CoolPants());
                    break;
                default:
                    builder.SetPants(null);
                    break;
            }
        }

        public void SetMoodDecorations(string mood)
        {
            switch (mood)
            {
                case "Silly":
                    builder.BeingSilly(); break;
                case "Serious":
                    builder.BeingSerious(); break;
                default:
                    break;
            }
        }

        public void SetWeatherDecorations(string weather)
        {
            switch (weather)
            {
                case "Sunny":
                    builder.WithSunglasses(); break;
                case "Cold":
                    builder.WithScarf(); break;
                default:
                    break;
            }
        }


        public void SetAccessories(ClothingStyle style)
        {
            IAccessoryFactory factory;
            switch (style)
            {
                case ClothingStyle.Casual:
                    factory = new CasualAccessoryFactory();
                    break;
                case ClothingStyle.Formal:
                    factory = new FormalAccessoryFactory();
                    break;
                case ClothingStyle.Cool:
                    factory = new CoolAccessoryFactory();
                    break;
                default:
                    factory = null;
                    break;
            }

            if (factory != null)
            {
                builder.SetShoes(factory.CreateShoes());
                builder.SetHat(factory.CreateHat());
                builder.SetEarrings(factory.CreateEarrings());
            }
            else
            {
                builder.SetShoes(null);
                builder.SetHat(null);
                builder.SetEarrings(null);
            }
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    }
}

