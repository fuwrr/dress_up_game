namespace DressUpGame.models
{
    public interface IDecoration
    {
        IClothing Decorate(IClothing clothing);
    }
    // Mood-Based Accessories (using Decorator pattern)
    public class SunglassesDecoration : IDecoration //weather == sunny
    {
        public IClothing Decorate(IClothing clothing)
        {
            return new DecoratedClothing(clothing, "with cool sunglasses");
        }
    }

    public class ScarfDecoration : IDecoration //weather == cold
    {
        public IClothing Decorate(IClothing clothing)
        {
            return new DecoratedClothing(clothing, "with a cozy scarf");
        }
    }

    public class PantiesDecoration : IDecoration //mood == silly
    {
        public IClothing Decorate(IClothing clothing)
        {
            return new DecoratedClothing(clothing, "with minion panties underneath;)");
        }
    }

    public class IroningDecoration : IDecoration //mood == serious
    {
        public IClothing Decorate(IClothing clothing)
        {
            return new DecoratedClothing(clothing, ", fully ironied");
        }
    }

    public class DecoratedClothing : IClothing
    {
        private readonly IClothing _clothing;
        private readonly string _decorationDescription;

        public DecoratedClothing(IClothing clothing, string decorationDescription)
        {
            _clothing = clothing;
            _decorationDescription = decorationDescription;
        }

        public string Name => _clothing.Name + " " + _decorationDescription;
        public string Description => _clothing.Description + ", " + _decorationDescription;
        public ClothingStyle Style => _clothing.Style;

        public IClothing Decorate(IDecoration decoration)
        {
            // Can be used for stacking decorations
            return decoration.Decorate(this);
        }
    }
}

