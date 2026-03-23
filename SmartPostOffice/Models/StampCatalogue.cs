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
            new(){ Id=1,  Name="SRI LANKA AIR FORCE - 75TH ANNIVERSARY - 2026",     Theme="Stamps",   PricePerStamp=70m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/large/6875.jpg" },
            new(){ Id=2,  Name="NATIONAL INSTITUTE OF MENTAL HEALTH- CENTENARY (1926 - 2026) - 2026",     Theme="Stamps",   PricePerStamp=70m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6859.jpg" },
            new(){ Id=4,  Name="Centenary of Sri Lankan Cinema (1925 - 2025) ",     Theme="Stamps",   PricePerStamp=70m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6853.jpg" },
            new(){ Id=5,  Name="CHRISTMAS - 2025 (1/2)",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6820.jpg" },
            new(){ Id=6,  Name="CHRISTMAS - 2025 (2/2)",     Theme="Stamps",   PricePerStamp=110m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6821.jpg" },
            new(){ Id=7,  Name="Dr. Roland Silva - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6791.jpg" },
            new(){ Id=8,  Name="Deepavali - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6787.jpg" },
            new(){ Id=9,  Name="World Post Day - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6771.jpg" },
            new(){ Id=10,  Name="UNIVERSITY OF JAFFNA - 50 TH ANNIVERSARY - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6769.jpg" },
            new(){ Id=11,  Name="World Children's Day - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6765.jpg" },
            new(){ Id=12,  Name="THE WONDER OF THE UNIVERSE - 2025 (EAGLE NEBULA) 1/10",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6736.jpg" },
            new(){ Id=13,  Name="THE WONDER OF THE UNIVERSE - 2025 (SOLAR ECLIPSE) 2/10",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6737.jpg" },
            new(){ Id=14,  Name="THE WONDER OF THE UNIVERSE - 2025 (BLACK HOLE) 3/10",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6738.jpg" },
            new(){ Id=15,  Name="THE WONDER OF THE UNIVERSE - 2025 (ANDROMEDA) 4/10 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6739.jpg" },
            new(){ Id=16,  Name="THE WONDER OF THE UNIVERSE - 2025 (CRAB NEBULA) 5/10 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6740.jpg" },
            new(){ Id=17,  Name="THE WONDER OF THE UNIVERSE - 2025 (ORION NEBULA) 6/10 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6741.jpg" },
            new(){ Id=18,  Name="THE WONDER OF THE UNIVERSE - 2025 (RED SPOT) 7/10 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6742.jpg" },
            new(){ Id=19,  Name="THE WONDER OF THE UNIVERSE - 2025 (STEPHAN'S QUINTET) 8/10 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6743.jpg" },
            new(){ Id=20,  Name="THE WONDER OF THE UNIVERSE - 2025 (PLEIADES) 9/10",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6744.jpg" },
            new(){ Id=21,  Name="THE WONDER OF THE UNIVERSE - 2025 (RINGS OF SATURN) 10/10",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6745.jpg" },
            new(){ Id=22,  Name="THE WONDER OF THE UNIVERSE - 2025 (FDC) ",     Theme="Stamps",   PricePerStamp=700m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6757.jpg" },
            new(){ Id=23,  Name="THE WONDER OF THE UNIVERSE - 2025 (SHEETLET) ",     Theme="Stamps",   PricePerStamp=500m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6746.jpg" },
            new(){ Id=24,  Name="National Meelad - Un-Nabi - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6720.jpg" },
            new(){ Id=25,  Name="BICENTENARY OF COFFEE CULTIVATION IN SRI LANKA - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6660.jpg" },
            new(){ Id=26,  Name="SRI LANKA MEDICAL COUNCIL - (CENTENARY CELEBRATIONS) - 2025 ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6652.jpg" },
            new(){ Id=27,  Name="MINNERIYA NATIONAL PARK - 2025 (YELLOW-STRIPED CHEVROTAIN) (2/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6590.jpg" },
            new(){ Id=28,  Name="MINNERIYA NATIONAL PARK - 2025 (PAINTED STORK) (8/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6596.jpg" },
            new(){ Id=29,  Name="MINNERIYA NATIONAL PARK - 2025 (INDIAN STAR TORTOISE) (3/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6591.jpg" },
            new(){ Id=30,  Name="MINNERIYA NATIONAL PARK - 2025 (BRAHMINY KITE) (10/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6598.jpg" },
            new(){ Id=31,  Name="MINNERIYA NATIONAL PARK - 2025 (ASIAN OPENBILL) (9/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6597.jpg" },
            new(){ Id=32,  Name="MINNERIYA NATIONAL PARK - 2025 (SRI LANKAN ELEPHANT) (5/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6593.jpg" },
            new(){ Id=33,  Name=" MINNERIYA NATIONAL PARK - 2025 (SRI LANKAN JACKAL) (1/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6589.jpg" },
            new(){ Id=34,  Name="MINNERIYA NATIONAL PARK - 2025 (BLACK NAPED HARE) (6/10)",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6594.jpg" },
            new(){ Id=35,  Name="MINNERIYA NATIONAL PARK - 2025 (INDIAN PANGOLIN) (4/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6592.jpg" },
            new(){ Id=36,  Name="MINNERIYA NATIONAL PARK - 2025 (SRI LANKAN SLOTH BEAR) (7/10) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6595.jpg" },
            new(){ Id=37,  Name="SRI DALADA POSON PERAHERA ETHKANDA RAJA MAHA VIHARAYA - KURUNEGALA (2025) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6565.jpg" },
            new(){ Id=38,  Name="REVENUE STATE EMBLEM - 2025 ",     Theme="Stamps",   PricePerStamp=1000m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6561.jpg" },
            new(){ Id=39,  Name="STATE VESAK - 2569 - (2025) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6535.jpg" },
            new(){ Id=40,  Name="VESAK - 2569 - (2025) - (ARATHTHANA RAJA MAHA VIHARAYA PAINTINGS) ",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6531.jpg" },
            new(){ Id=41,  Name="VESAK - 2569 - (2025) - (LIHINIYAGALA RAJA MAHA VIHARAYA PAINTINGS)",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6532.jpg" },
            new(){ Id=42,  Name="VESAK - 2569 - (2025) - (POTHGUL RAJA MAHA VIHARAYA PAINTINGS)",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6533.jpg" },
            new(){ Id=43,  Name="PROF.SANGEETH NIPUN SANATH NANDASIRI (මහාචාර් ය සංගීත් නිපුන් සනත් නන්දසිරි) - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6472.jpg" },
            new(){ Id=44,  Name="EARTH SUMMIT - 2025",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6473.jpg" },
            new(){ Id=45,  Name="NATIONAL MEELAD - UN-NABI - 2024",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6449.jpg" },
            new(){ Id=46,  Name="CENTURY OF RADIO COMMUNICATION IN SRI LANKA - ශ්‍රී ලංකා ගුවන්විදුලි සන්නිවේදනයේ සියවස - 2024",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6436.jpg" },
            new(){ Id=47,  Name="CHRISTMAS - 2024 (2/2)",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6395.jpg" },
            new(){ Id=48,  Name="DEEPAVALI - 2024",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6380.jpg" },
            new(){ Id=49,  Name="150TH ANNIVERSARY OF UPU (UNIVERSAL POSTAL UNION) - 2024",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6324.jpg" },
            new(){ Id=50,  Name="WORLD POST DAY - 2024",     Theme="Stamps",   PricePerStamp=50m,  ImagePlaceholder=" ", ImageUrl="https://stapp.slpost.gov.lk/attachments/pcinventoryitemimages/6323.jpg" }

        };
    }}

