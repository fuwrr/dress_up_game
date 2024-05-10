using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.models
{
    public class CasualShirt : Shirt
    {
        public override string Name => "Favourite childhood T-shirt";
        public override string Description => "XXL size, with holes and has car drawing on it";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CoolShirt : Shirt
    {
        public override string Name => "Dad's jacket";
        public override string Description => "Looking excentric since '95";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class FormalShirt : Shirt
    {
        public override string Name => "Suit";
        public override string Description => "Suit up! for formal meetings";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }

    public class CasualPants : Pants
    {
        public override string Name => "Old sweatpants";
        public override string Description => "The most comfortable thing ever";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }

    public class CoolPants : Pants
    {
        public override string Name => "New baggy blue jeans";
        public override string Description => "Turkish quality, all youngsters are wearing these, I promise";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }

    public class FormalPants : Pants
    {
        public override string Name => "Suit Pants";
        public override string Description => "I almost can't even see that they're second-hand";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }

    public class CasualShoes : Shoes
    {
        public override string Name => "Crocs";
        public override string Description => "Squecking with casuality";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CoolShoes : Shoes
    {
        public override string Name => "StingSteps";
        public override string Description => "Bee shoes. Cool";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }

    public class FormalShoes : Shoes
    {
        public override string Name => "Suede Shoes";
        public override string Description => "Elegant shoes for formal occasions that mom bought you in 8th grade";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class CasualHat : Hat
    {
        public override string Name => "Knitted beanie";
        public override string Description => "Quite worn out but very warm";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CoolHat : Hat
    {
        public override string Name => "Long hair";
        public override string Description => "Girls like it. And you look like a girl too";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class FormalHat : Hat
    {
        public override string Name => "Cyllinder hat";
        public override string Description => "You've robbed MadHatter from Alice in Wonderland";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class CasualEarrings : Earrings
    {
        public override string Name => "One ear pierced";
        public override string Description => "You mom starts looking for your pirate ship";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CoolEarrings : Earrings
    {
        public override string Name => "Both ears pierced";
        public override string Description => "Girls like it too. And you still look like a girl too";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class FormalEarrings : Earrings
    {
        public override string Name => "Eyeglasess";
        public override string Description => "You pretend to look smart. Unexpectedly successfully";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }

}

