using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.models
{
    public class ClothingEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ClothingStyle Style { get; set; }

        public ClothingEvent(string name, string description, ClothingStyle style)
        {
            Name = name;
            Description = description;
            Style = style;
        }
    }

    public enum ClothingStyle
    {
        Formal,
        Casual,
        Cool
    }

    public interface IClothing
    {
        string Name { get; }
        string Description { get; }
        ClothingStyle Style { get; }
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

    public class Game
    {
        private readonly List<ClothingEvent> events;

        public Game()
        {
            events = new List<ClothingEvent>
            {
                new ClothingEvent("Seeing Okscana Dmytrivna", "You want to look as smart as possible", ClothingStyle.Formal),
                new ClothingEvent("Attending your wedding", "Actually you don't have time to be fancy", ClothingStyle.Casual),
                new ClothingEvent("Going to give a lecture on behaviour to your niece", "You have to look older and colder.", ClothingStyle.Cool),
                new ClothingEvent("Going to adoption centre for a cat", "You want to slightly impress your new buddy.", ClothingStyle.Formal),
                new ClothingEvent("Going to take out a loan at the bank", "Better not be looking too rich or too cool.", ClothingStyle.Casual),
                new ClothingEvent("Going to bless the Easter bread", "Don't look like a fool and dress only cool!", ClothingStyle.Cool),

                //new ClothingEvent("Modeling and analyzing software lab!", "Serdyuk's looking, be cool!!.", ClothingStyle.Cool)
            };
        }

        public ClothingEvent GetRandomEvent()
        {
            Random rnd = new();
            int index = rnd.Next(events.Count);
            return events[index];
        }
    }

    public interface IAccessoryFactory
    {
        Shoes CreateShoes();
        Hat CreateHat();
        Earrings CreateEarrings();
    }
    public abstract class Shoes : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }
    }
    public abstract class Hat : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }
    }
    public abstract class Earrings : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }
    }
    public class CasualAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes()
        {
            return new CasualShoes();
        }
        public Hat CreateHat()
        {
            return (Hat)new CasualHat();
        }
        public Earrings CreateEarrings()
        {
            return new CasualEarrings();
        }
    }

    public class CoolAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes()
        {
            return new CoolShoes();
        }
        public Hat CreateHat()
        {
            return (Hat)new CoolHat();
        }
        public Earrings CreateEarrings()
        {
            return new CoolEarrings();
        }
    }

    public class FormalAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes()
        {
            return new FormalShoes();
        }
        public Hat CreateHat()
        {
            return new FormalHat();
        }
        public Earrings CreateEarrings()
        {
            return new FormalEarrings();
        }
    }

}

