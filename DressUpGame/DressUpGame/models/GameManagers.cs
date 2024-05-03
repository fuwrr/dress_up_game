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
        Cool,
        None
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

        public IClothing GetShirt()
        {
            return shirt;
        }

        public IClothing GetPants()
        {
            return pants;
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

    public class DressUpFacade
    {
        protected readonly Game game;
        protected readonly Player player;
        protected OutfitBuilder builder;
        private ClothingEvent currentEvent;

        public DressUpFacade(Game game, Player player)
        {
            this.game = game;
            this.player = player;
            builder = new OutfitBuilder();
            currentEvent = game.GetRandomEvent();
        }

        virtual public void DressUp()
        {
            List<IClothing> outfit = builder.Build();
            player.SetCurrentOutfit(outfit);
            player.DressUpForEvent(currentEvent);
        }

        public int GetScore()
        {
            return player.GetScore();
        }

        virtual public string GetOutfitDescription()
        {
            return $"\n{player.GetOutfitDescription()}";
        }

        public ClothingEvent GetRandomEvent()
        {
            currentEvent = game.GetRandomEvent();
            return currentEvent;
        }

        public string GetCurrentEventDescription()
        {
            return $"{currentEvent.Name} - {currentEvent.Description}";
        }

        public ClothingStyle GetCurrentEventStyle()
        {
            return currentEvent.Style;
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
    }

    //Decorators for 5 lab, actually unnecessary
    public class DesignerDecorator : DressUpFacade
    {
        private readonly IDesigner designer;

        public DesignerDecorator(IDesigner designer, Game game, Player player) : base(game, player)
        {
            this.designer = designer;
        }

        public override void DressUp()
        {
            base.DressUp();
            designer.RefineOutfit(builder);
        }

        public override string GetOutfitDescription()
        {
            return designer.GetDescription(base.GetOutfitDescription());
        }
    }

    public interface IDesigner
    {
        void RefineOutfit(OutfitBuilder builder);
        string GetDescription(string description);
    }

    public class ClassicDesigner : IDesigner
    {
        public void RefineOutfit(OutfitBuilder builder)
        {
            if (builder.GetShirt() is FormalShirt)
            {
                builder.SetEarrings(null); // Classic style avoids earrings with formal shirts
            }
        }

        public string GetDescription(string description)
        {
            return description + "\nDesigned with a classic touch.";
        }
    }

    public class TrendyDesigner : IDesigner
    {
        public void RefineOutfit(OutfitBuilder builder)
        {
            if (builder.GetPants() is CasualPants)
            {
                builder.SetHat(null); // Trendy style avoids hats with casual pants
            }
        }

        public string GetDescription(string description)
        {
            return description + "\nDesigned with a trendy flair.";
        }
    }

}
