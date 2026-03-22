namespace SmartPostOffice.Models
{
    public class StampStyle
    {
        public int     Id               { get; set; }
        public string  Name             { get; set; } = string.Empty;
        public string  Theme            { get; set; } = string.Empty;
        public decimal PricePerStamp    { get; set; }
         public string  ImageUrl         { get; set; } = string.Empty;
        public string  ImagePlaceholder { get; set; } = string.Empty;  
    }

    public static class StampCatalogue
    {
        public const decimal FixedServiceCharge = 1000m;  
        public const int     MaxStampsPerStyle  = 25;

        public static List<StampStyle> All => new()
        {
            new(){ Id=1,  Name="Wild Elephants",     Theme="Wildlife",   PricePerStamp=70m,  ImagePlaceholder="🐘" },
            new(){ Id=2,  Name="Blue Whale",         Theme="Wildlife",   PricePerStamp=70m,  ImagePlaceholder="🐋" },
            new(){ Id=3,  Name="Sigiriya Rock",      Theme="Heritage",   PricePerStamp=80m,  ImagePlaceholder="🏯" },
            new(){ Id=4,  Name="Tooth Relic Temple", Theme="Heritage",   PricePerStamp=80m,  ImagePlaceholder="🛕" },
            new(){ Id=5,  Name="Tea Plantation",     Theme="Nature",     PricePerStamp=65m,  ImagePlaceholder="🍃" },
            new(){ Id=6,  Name="Stilt Fishermen",    Theme="Culture",    PricePerStamp=75m,  ImagePlaceholder="🎣" },
            new(){ Id=7,  Name="Kandyan Dancer",     Theme="Culture",    PricePerStamp=75m,  ImagePlaceholder="💃" },
            new(){ Id=8,  Name="SL Post 225 Years",  Theme="Philatelic", PricePerStamp=90m,  ImagePlaceholder="📮" },
            new(){ Id=9,  Name="Independence 2024",  Theme="National",   PricePerStamp=85m,  ImagePlaceholder="🇱🇰" },
            new(){ Id=10, Name="Coral Reef",         Theme="Marine",     PricePerStamp=70m,  ImagePlaceholder="🪸" },
        };
    }
}

