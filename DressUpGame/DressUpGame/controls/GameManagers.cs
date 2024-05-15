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
    -StrikeObserver
     */

    public class ClothingEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ClothingStyle Style { get; set; }
        public WeatherFlags Weather { get; set; }
        public MoodFlags Mood { get; set; }

        public ClothingEvent(string name, string description, ClothingStyle style, WeatherFlags weather, MoodFlags mood)
        {
            Name = name;
            Description = description;
            Style = style;
            Weather = 0;
            Weather = weather;
            Mood = 0;
            Mood = mood;
        }
    }

    public class ClothingEventManager
    {
        private readonly List<ClothingEvent> events;
        private static readonly Random rnd = new Random();
        //private int currentIndex = -1;

        //add lore window before dressing up, cause too much text
        public ClothingEventManager()
        {
            events = new List<ClothingEvent>
            {
                new ClothingEvent("Seeing Okscana Dmytrivna", "You want to look as smart as possible, but balance on dorky side too", ClothingStyle.Formal, 0, MoodFlags.Silly|MoodFlags.Serious),
                new ClothingEvent("Attending your wedding", "Actually you don't have time to be fancy or feel overwhelmed", ClothingStyle.Casual, 0, MoodFlags.Silly),
                new ClothingEvent("Going to give a lecture on behaviour to your niece", "You have to look older and colder, despite the weather.", ClothingStyle.Cool, WeatherFlags.Sunny, MoodFlags.Serious),
                new ClothingEvent("Going to adoption centre for a cat", "You want to slightly impress your new buddy on this winter sunny day.", ClothingStyle.Formal, WeatherFlags.Sunny|WeatherFlags.Cold, 0),
                new ClothingEvent("Going to take out a loan at the bank", "Better not be looking too rich or too cool.", ClothingStyle.Casual, 0, MoodFlags.Serious),
                new ClothingEvent("Going to bless the Easter bread", "Don't look like a fool and dress only cool! But не забудь шапку!", ClothingStyle.Cool, WeatherFlags.Cold, MoodFlags.Serious),


                //for testing purposes, delete later
                /*new ClothingEvent("Silly", " ", ClothingStyle.Formal, 0, MoodFlags.Silly),
                new ClothingEvent("Silly And Serious", " ", ClothingStyle.Casual, 0, MoodFlags.Silly|MoodFlags.Serious),
                new ClothingEvent("Silly And Sunny", " ", ClothingStyle.Cool, WeatherFlags.Sunny, MoodFlags.Silly),
                new ClothingEvent("Silly And Serious And Sunny", " ", ClothingStyle.Cool, WeatherFlags.Sunny, MoodFlags.Silly|MoodFlags.Serious),
                new ClothingEvent("Silly And Serious And Sunny And Cold", " ", ClothingStyle.Cool, WeatherFlags.Sunny|WeatherFlags.Cold, MoodFlags.Silly|MoodFlags.Serious)*/
            };
        }

        public ClothingEvent GetRandomEvent()
        {
            int index = rnd.Next(events.Count);
            return events[index];
        }

        /*public ClothingEvent GetNextEvent()
        {
            currentIndex = (currentIndex + 1) % events.Count; // Increment index cyclically
            return events[currentIndex];
        }*/
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
        private MoodFlags mood;
        private WeatherFlags weather;
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
        public void SetMood(MoodFlags mood) => this.mood = mood;
        public void SetWeather(WeatherFlags weather) => this.weather = weather;

        public string GetCurrentOutfitDescription()
        {
            StringBuilder outfitDescription = new StringBuilder("Current Outfit:\n");
            foreach (IClothing item in currentOutfit)
            {
                outfitDescription.Append($"{item.Name}: {item.Description}\n");
            }
            return outfitDescription.ToString();
        }

        //On mood and weather
        //if i will only increase point when player guessed
        //you can cheat by choosing all options
        //so im also decreasing if answer is wrong
        
        //BUT fix suggestion: player can score higher points if event has more flags
        //need to normalize score data

        //okay, I compare mismatched ? matched and ++ or -- separately for mood and weather
        //treat as additional points, nothing happens if not chosen
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

            // Count matching and mismatched flags for Mood
            int matchingMoodFlags = 0;
            int mismatchedMoodFlags = 0;
            foreach (MoodFlags moodFlag in Enum.GetValues(typeof(MoodFlags)))
            {
                bool isMoodFlagSetInUserChoice = (mood & moodFlag) != 0;
                bool isMoodFlagSetInEvent = (clothingEvent.Mood & moodFlag) != 0;

                if (isMoodFlagSetInUserChoice && isMoodFlagSetInEvent)
                {
                    matchingMoodFlags++; // User correctly chose a mood present in the event
                }
                else if (isMoodFlagSetInUserChoice && !isMoodFlagSetInEvent)
                {
                    mismatchedMoodFlags++; // User chose a mood not present in the event
                }
                // No action needed for cases where the flag is not set in the user's choice
            }
            if (mismatchedMoodFlags != 0 && matchingMoodFlags != 0)
            {
                score = matchingMoodFlags > mismatchedMoodFlags ? score++ : score--;
            }
            //score += matchingMoodFlags;
            //score -= mismatchedMoodFlags;

            int matchingWeatherFlags = 0;
            int mismatchedWeatherFlags = 0;
            foreach (WeatherFlags weatherFlag in Enum.GetValues(typeof(WeatherFlags)))
            {
                bool isWeatherFlagSetInUserChoice = (weather & weatherFlag) != 0;
                bool isWeatherFlagSetInEvent = (clothingEvent.Weather & weatherFlag) != 0;

                if (isWeatherFlagSetInUserChoice && isWeatherFlagSetInEvent)
                {
                    matchingWeatherFlags++;
                }
                else if (isWeatherFlagSetInUserChoice && !isWeatherFlagSetInEvent)
                {
                    mismatchedWeatherFlags++;
                }
            }

            score += matchingWeatherFlags;
            score -= mismatchedWeatherFlags;
        }

        public int GetScore() => score;
    }

    public class DressUpFacade
    {
        private readonly ClothingEventManager game;
        private readonly Player player;
        private OutfitBuilder builder;
        private ClothingEvent currentEvent;
        private MoodFlags mood;
        private WeatherFlags weather;

        private int maxScoreStreak = 0;
        private int currentScoreStreak = 0;
        private IStreakObserver streakObserver; // Observer interface for streak tracking
        private FileHelper fileHelper = new FileHelper();

        public DressUpFacade(ClothingEventManager game, Player player, IStreakObserver observer)
        {
            this.game = game;
            this.player = player;
            builder = new OutfitBuilder();
            currentEvent = game.GetRandomEvent();
            mood = 0;
            weather = 0;
            streakObserver = observer;

            int[] maxScoreStreak = new int[1]; // Array to store the streak value
            fileHelper.LoadMaxScoreStreak(maxScoreStreak);
            this.maxScoreStreak = maxScoreStreak[0]; // Assign to class variable
        }

        public void DressUp()
        {
            List<IClothing> outfit = builder.Build();
            player.SetMood(mood);
            player.SetWeather(weather);
            player.SetCurrentOutfit(outfit);
            player.CalculateScore(currentEvent);
            Debug.WriteLine($"mood: {mood}, weather: {weather}");
            if (player.GetScore() == 5)
            {
                currentScoreStreak++;
                streakObserver.OnStreakUpdate(currentScoreStreak); // Notify observer about streak

                if (currentScoreStreak > maxScoreStreak)
                {
                    maxScoreStreak = currentScoreStreak;
                    streakObserver.OnNewHighStreak(maxScoreStreak); // Notify about new high score streak
                                                                    // Write new high streak to file (implementation details not shown)
                    fileHelper.SaveMaxScoreStreak(maxScoreStreak);  // ... write to file ...
                }
            }
            else
            {
                currentScoreStreak = 0;
                streakObserver.OnStreakBroken(); // Notify about broken streak
            }
        }

        public int GetScore() => player.GetScore();

        public string GetOutfitDescription() => $"\n\n{player.GetCurrentOutfitDescription()}";

        public void ClearOutfit()
        {
            builder.Clear();
            player.SetCurrentOutfit(new List<IClothing>());
            player.SetMood(mood = 0);
            player.SetWeather(weather = 0);
        }

        public ClothingEvent GetRandomEvent() 
        { 
            currentEvent = game.GetRandomEvent(); 
            return currentEvent;
        }

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

        //not sure if its the best way of implementing mood+weathers, but if you
        //are not wearing clothes, builder will not call the decorator => no
        //decription will be changes, but here in facade parameters mood+weather
        //still will be used for counting the score
        //----------------i've mentioned it in hint button text------------------
        public void SetMoodDecorations(string moodName)
        {
            switch (moodName)
            {
                case "Silly":
                    builder.BeingSilly(); mood |= MoodFlags.Silly; break;
                case "Serious":
                    builder.BeingSerious(); mood |= MoodFlags.Serious; break;
                default:
                    break;
            }
        }

        public void SetWeatherDecorations(string weatherName)
        {
            switch (weatherName)
            {
                case "Sunny":
                    builder.WithSunglasses(); weather |= WeatherFlags.Sunny; break;
                case "Cold":
                    builder.WithScarf(); weather |= WeatherFlags.Cold; break;
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

    public interface IStreakObserver
    {
        void OnStreakUpdate(int currentStreak);
        void OnNewHighStreak(int newHighStreak);
        void OnStreakBroken();
    }

    // Example implementation of IStreakObserver (can be in a separate class)
    public class StreakDisplayObserver : IStreakObserver
    {
        public void OnStreakUpdate(int currentStreak)
        {
            Debug.WriteLine($"Current Score Streak: {currentStreak}"); // Update UI display
        }

        public void OnNewHighStreak(int newHighStreak)
        {
            Debug.WriteLine($"New High Score Streak: {newHighStreak}! You're the best!"); // Display congratulatory message
        }

        public void OnStreakBroken()
        {
            Debug.WriteLine($"Score Streak Broken. Try again!"); // Display streak broken message
        }
    }

}

