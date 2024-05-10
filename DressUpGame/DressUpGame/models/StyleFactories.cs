namespace DressUpGame.models
{
    public interface IAccessoryFactory
    {
        Shoes CreateShoes();
        Hat CreateHat();
        Earrings CreateEarrings();
    }

    public class CasualAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes() => new CasualShoes();
        public Hat CreateHat() => new CasualHat();
        public Earrings CreateEarrings() => new CasualEarrings();
    }

    public class CoolAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes() => new CoolShoes();
        public Hat CreateHat() => new CoolHat();
        public Earrings CreateEarrings() => new CoolEarrings();
    }


    public class FormalAccessoryFactory : IAccessoryFactory
    {
        public Shoes CreateShoes() => new FormalShoes();
        public Hat CreateHat() => new FormalHat();
        public Earrings CreateEarrings() => new FormalEarrings();
    }
}

