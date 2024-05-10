using DressUpGame.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.controls
{
    /*
    -Event + EventManager
    -OutfitBuilder
    -Player
    -Facade
     */

    public class ClothingEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ClothingStyle Style { get; set; }
        public Weather Weather { get; set; }
        public Mood Mood { get; set; }

        public ClothingEvent(string name, string description, ClothingStyle style, Weather weather, Mood mood)
        {
            Name = name;
            Description = description;
            Style = style;
            Weather = weather;
            Mood = mood;
        }
    }

    public class ClothingEventManager
    {
        private readonly List<ClothingEvent> events;
        private static readonly Random rnd = new Random();

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
            int index = rnd.Next(events.Count);
            return events[index];
        }
    }

    public class OutfitBuilder
    {
        private IClothing? shirt;
        private IClothing? pants;
        private IClothing? shoes;
        private IClothing? hat;
        private IClothing? earrings;

        public OutfitBuilder SetShirt(IClothing? shirt) => Assign(ref this.shirt, value: shirt);
        public OutfitBuilder SetPants(IClothing? pants) => Assign(ref this.pants, value: pants);
        public OutfitBuilder SetShoes(IClothing? shoes) => Assign(ref this.shoes, value: shoes);
        public OutfitBuilder SetHat(IClothing? hat) => Assign(ref this.hat, value: hat);
        public OutfitBuilder SetEarrings(IClothing? earrings) => Assign(ref this.earrings, value: earrings);

        public void Clear()
        {
            shirt = pants = hat = earrings = shoes = null;
        }

        public OutfitBuilder BeingSilly() => pants != null ? Decorate(ref pants, new PantiesDecoration()) : this;
        public OutfitBuilder BeingSerious() => shirt != null ? Decorate(ref shirt, new IroningDecoration()) : this;
        public OutfitBuilder WithSunglasses() => hat != null ? Decorate(ref hat, new SunglassesDecoration()) : this;
        public OutfitBuilder WithScarf() => hat != null ? Decorate(ref hat, new ScarfDecoration()) : this;

        public List<IClothing> Build()
        {
            var outfit = new List<IClothing>();
            AddIfNotNull(outfit, hat, earrings, shoes);
            AddIfNotNull(outfit, shirt);
            AddIfNotNull(outfit, pants);
            return outfit;
        }

        private static void AddIfNotNull(List<IClothing> outfit, params IClothing?[] items)
        {
            foreach (var item in items)
            {
                if (item != null)
                    outfit.Add(item);
            }
        }

        private static OutfitBuilder Assign(ref IClothing? field, IClothing value)
        {
            field = value;
            return new OutfitBuilder { shirt = field, pants = field, shoes = field, hat = field, earrings = field };
        }

        private static OutfitBuilder Decorate(ref IClothing? clothing, IDecoration decoration)
        {
            clothing = decoration.Decorate(clothing);
            return new OutfitBuilder { shirt = clothing, pants = clothing, shoes = clothing, hat = clothing, earrings = clothing };
        }
    }

    public class Player
    {
        private static Player? instance;
        private List<IClothing> currentOutfit = new List<IClothing>();
        private int score = 0;

        private Player() { }

        public static Player GetInstance()
        {
            if (instance == null)
            {
                lock (typeof(Player))
                {
                    instance ??= new Player();
                }
            }
            return instance;
        }

        public List<IClothing> GetCurrentOutfit() => currentOutfit;

        public void SetCurrentOutfit(List<IClothing> outfit) => currentOutfit = outfit;

        public string GetCurrentOutfitDescription()
        {
            StringBuilder outfitDescription = new StringBuilder("Current Outfit:\n");
            foreach (IClothing item in currentOutfit)
            {
                outfitDescription.Append($"{item.Name}: {item.Description}\n");
            }
            return outfitDescription.ToString();
        }

        public void CalculateScore(ClothingEvent clothingEvent)
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

        public int GetScore() => score;
    }

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
            player.CalculateScore(currentEvent);
        }

        public int GetScore() => player.GetScore();

        public string GetOutfitDescription() => $"\n\n{player.GetCurrentOutfitDescription()}";

        public void ClearOutfit()
        {
            builder.Clear();
            player.SetCurrentOutfit(new List<IClothing>());
        }

        public ClothingEvent GetRandomEvent() => game.GetRandomEvent();

        public string GetCurrentEventDescription() => $"{currentEvent.Name} - {currentEvent.Description}";

        public ClothingStyle GetCurrentEventStyle() => currentEvent.Style;

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
            IAccessoryFactory? factory;
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
                case ClothingStyle.None:
                    factory = null;
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
    }
}

