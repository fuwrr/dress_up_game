namespace DressUpGame.models
{
    [System.Flags]
    public enum WeatherFlags
    {
        Sunny = 1 << 0,
        Cold = 1 << 1
    }

    [System.Flags]
    public enum MoodFlags
    {
        Silly = 1 << 0,
        Serious = 1 << 1
    }

    public enum ClothingStyle
    {
        Formal,
        Casual,
        Cool,
        None
    }
}

