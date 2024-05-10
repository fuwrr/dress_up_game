using System.Collections.Generic;
using System.Diagnostics;

namespace DressUpGame.models
{
    public class CleanEditionShirtManager
    {
        private static readonly Dictionary<Shirt, int> Stock = new Dictionary<Shirt, int>();

        public static void InitializeStock()
        {
            Stock[new FormalShirt()] = 10;
            Stock[new CoolShirt()] = 10;
        }

        public static int GetStock(Shirt shirt)
        {
            return Stock.ContainsKey(shirt) ? Stock[shirt] : 0;
        }

        public static void ReduceStock(Shirt shirt)
        {
            if (Stock.ContainsKey(shirt))
            {
                int currentStock = Stock[shirt];
                if (currentStock > 0)
                {
                    Stock[shirt] = --currentStock;
                }
            }
        }
    }


    public class CleanEditionShirt : Shirt
    {
        private readonly Shirt _shirt;

        public CleanEditionShirt(Shirt shirt)
        {
            _shirt = shirt;
        }

        public override string Name => _shirt.Name;

        public override string Description => _shirt.Description;

        public override ClothingStyle Style => _shirt.Style;

        public override IClothing Decorate(IDecoration decoration)
        {
            return _shirt.Decorate(decoration);
        }

        public bool IsInStock()
        {
            return CleanEditionShirtManager.GetStock(_shirt) > 0;
        }

        public IClothing? AddToOutfit()
        {
            if (IsInStock())
            {
                CleanEditionShirtManager.ReduceStock(_shirt);
                return this;
            }
            else
            {
                Debug.WriteLine("Sorry, this Limited Edition Shirt is out of stock!");
                return null;
            }
        }
    }
}

