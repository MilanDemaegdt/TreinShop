using System;
using System.Collections.Generic;

namespace TreinShop.Domain.Entities
{
    public partial class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; } = null!;
        public int PrijsPerNacht { get; set; }
        public int StationId { get; set; }

        public virtual Station Station { get; set; } = null!;
    }
}
