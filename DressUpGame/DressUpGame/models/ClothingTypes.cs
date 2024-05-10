namespace DressUpGame.models
{
    //interface for ALL other types of clothes
    public interface IClothing 
    {
        string Name { get; }
        string Description { get; }
        ClothingStyle Style { get; }

        IClothing Decorate(IDecoration decoration);
    }

    //Shirt + Pants + (Shoes + Hat + Earrings)
    public abstract class Shirt : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }

        public virtual IClothing Decorate(IDecoration decoration)
        {
            return decoration.Decorate(this);
        }
    }

    public abstract class Pants : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }

        public virtual IClothing Decorate(IDecoration decoration)
        {
            return decoration.Decorate(this);
        }
    }

    public abstract class Shoes : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }

        public virtual IClothing Decorate(IDecoration decoration)
        {
            return decoration.Decorate(this);
        }
    }

    public abstract class Hat : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }

        public virtual IClothing Decorate(IDecoration decoration)
        {
            return decoration.Decorate(this);
        }
    }

    public abstract class Earrings : IClothing
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ClothingStyle Style { get; }

        public virtual IClothing Decorate(IDecoration decoration)
        {
            return decoration.Decorate(this);
        }
    }
}

