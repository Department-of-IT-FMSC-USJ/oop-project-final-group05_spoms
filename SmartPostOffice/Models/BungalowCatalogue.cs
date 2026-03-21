namespace SmartPostOffice.Models
{

    public class BungalowLocation
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Emoji { get; set; } = "🏠";
        public List<BungalowRoom> Rooms { get; set; } = new();
    }

    public class BungalowRoom
    {
        public string RoomLabel { get; set; } = string.Empty;
        public decimal OutsiderRate { get; set; }
        public decimal StaffRate { get; set; }
        public decimal ServiceCharge { get; set; } = 300m;
        public int MaxGuests { get; set; }
    }

    public static class BungalowCatalogue
    {
        public static List<BungalowLocation> All => new()
        {
            new(){ Id="anuradhapura",  Name="Anuradhapura",   Province="North Central", Emoji="🏛️",
                   Phone="025-2222250",
                   Description="Holiday experience in the city of the Ancient. 2 A/C rooms, dining room, satellite TV, catering.",
                   Rooms=new(){ new(){ RoomLabel="A/C Room", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=10 } } },

            new(){ Id="trincomalee-a", Name="Trincomalee 'A'", Province="Eastern",       Emoji="🏖️",
                   Phone="026-2222250",
                   Description="Holiday in the city of sunshine. 1 A/C room + 3 furnished rooms.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=12 } } },

            new(){ Id="trincomalee-b", Name="Trincomalee 'B'", Province="Eastern",       Emoji="🌊",
                   Phone="026-2222251",
                   Description="Beachside postal holiday home in Trincomalee.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=10 } } },

            new(){ Id="nuwaraeliya",   Name="Nuwara Eliya",    Province="Central",        Emoji="🌿",
                   Phone="052-2222250",
                   Description="Colonial style holiday home in heart of the city. Three rooms with hot water.",
                   Rooms=new(){ new(){ RoomLabel="Room (Hot Water)", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=10 } } },

            new(){ Id="ella",          Name="Ella",             Province="Uva",            Emoji="🌄",
                   Phone="057-2222250",
                   Description="Tea-carpeted slopes, misty peaks, Nine Arch Bridge train views. Hike Little Adam's Peak.",
                   Rooms=new(){
                       new(){ RoomLabel="Room 1 — 1 Double Bed",  OutsiderRate=6000m,  StaffRate=0m, ServiceCharge=300m, MaxGuests=2 },
                       new(){ RoomLabel="Room 2 — 3 Single Beds", OutsiderRate=8000m,  StaffRate=0m, ServiceCharge=300m, MaxGuests=3 },
                       new(){ RoomLabel="Room 3 — 4 Single Beds", OutsiderRate=10000m, StaffRate=0m, ServiceCharge=300m, MaxGuests=4 },
                   } },

            new(){ Id="sigiriya-a",    Name="Sigiriya 'A'",    Province="Central",        Emoji="🦁",
                   Phone="066-2286250",
                   Description="Relaxing environment with wildlife experiences. 2 furnished A/C rooms.",
                   Rooms=new(){ new(){ RoomLabel="A/C Room", OutsiderRate=5000m, StaffRate=1875m, ServiceCharge=300m, MaxGuests=4 } } },

            new(){ Id="sigiriya-b",    Name="Sigiriya 'B'",    Province="Central",        Emoji="🦅",
                   Phone="066-2286251",
                   Description="Wildlife experience near Sigiriya Rock Fortress.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=5000m, StaffRate=1875m, ServiceCharge=300m, MaxGuests=6 } } },

            new(){ Id="mannar",        Name="Mannar",           Province="Northern",       Emoji="🦩",
                    Phone="023-2222250",
                   Description="Peaceful northern coastal escape with flamingo watching.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=10 } } },

            new(){ Id="karainagar",    Name="Karainagar",       Province="Northern",       Emoji="🐚",
                   Phone="021-2222250",
                   Description="Island holiday home accessible by causeway, near Jaffna.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=8 } } },

            new(){ Id="chullipuram",   Name="Chullipuram",      Province="Northern",       Emoji="🌅",
                   Phone="021-2222251",
                   Description="Serene Northern Province postal holiday home.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=8 } } },

            new(){ Id="mihintalaya",   Name="Mihintalaya",      Province="North Central",  Emoji="🐘",
                   Phone="025-2222251",
                   Description="Buddhist pilgrimage town, elephant sightings and misty hills.",
                   Rooms=new(){ new(){ RoomLabel="Main Unit", OutsiderRate=8000m, StaffRate=3000m, ServiceCharge=300m, MaxGuests=10 } } },
        };
    }
}
