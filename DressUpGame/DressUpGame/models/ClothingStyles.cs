using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace DressUpGame.models
{
    //Shirt + Pants + (Shoes + Hat + Earrings)


    //-----------------------------------CASUAL STYLE------------------------------------------------
    public class CasualShirt : Shirt
    {
        public override string Name => "Hoodie";
        public override string Description => "XXL size, sticky, but feels right (you're discusting)";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CasualPants : Pants
    {
        public override string Name => "Old sweatpants";
        public override string Description => "Turkish quality for all these decades";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CasualShoes : Shoes
    {
        public override string Name => "Pink crocs";
        public override string Description => "Squecking with casuality";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CasualHat : Hat
    {
        public override string Name => "Knitted beanie";
        public override string Description => "Quite worn out, reminds salmon slice, but very warm";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }
    public class CasualEarrings : Earrings
    {
        public override string Name => "Silver chain and bracelet";
        public override string Description => "You've worn it everyday for last 3 years";
        public override ClothingStyle Style => ClothingStyle.Casual;
    }

    //----------------------------------------FORMAL STYLE------------------------------------------------
    public class FormalShirt : Shirt
    {
        public override string Name => "Suit";
        public override string Description => "Looks rich (you will be eaten first)";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class FormalPants : Pants
    {
        public override string Name => "Suit Pants";
        public override string Description => "Checked straigh pants for aristocrates";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class FormalShoes : Shoes
    {
        public override string Name => "Suede Shoes";
        public override string Description => "Elegant shoes for formal occasions that mom bought you in 8th grade";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class FormalHat : Hat
    {
        public override string Name => "Cyllinder hat";
        public override string Description => "Old-fashioned but exquisite";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }
    public class FormalEarrings : Earrings
    {
        public override string Name => "Pipe";
        public override string Description => "Quite pretencious, but you look tough";
        public override ClothingStyle Style => ClothingStyle.Formal;
    }




    //--------------------------------------------COOL STYLE------------------------------------------------
    public class CoolShirt : Shirt
    {
        public override string Name => "Dad's punk shirt";
        public override string Description => "Fainted skull print and sleeves absence makes you look sick";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class CoolPants : Pants
    {
        public override string Name => "DIY trousers";
        public override string Description => "You feel bold and creative and cringe";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class CoolShoes : Shoes
    {
        public override string Name => "Pink pidkradulyas";
        public override string Description => "You look like fantasy-book background character";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class CoolHat : Hat
    {
        public override string Name => "Mohawk";
        public override string Description => "All elders are afraid and you're out of gel now";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
    public class CoolEarrings : Earrings
    {
        public override string Name => "Makeup, tattoes and piercings";
        public override string Description => "Girls like it. And David Bowie too";
        public override ClothingStyle Style => ClothingStyle.Cool;
    }
}

