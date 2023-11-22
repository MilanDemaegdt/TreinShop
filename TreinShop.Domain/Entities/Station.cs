using System;
using System.Collections.Generic;

namespace TreinShop.Domain.Entities
{
    public partial class Station
    {
        public Station()
        {
            Hotels = new HashSet<Hotel>();
            TreinAankomsts = new HashSet<Trein>();
            TreinVertreks = new HashSet<Trein>();
        }

        public int StationId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Trein> TreinAankomsts { get; set; }
        public virtual ICollection<Trein> TreinVertreks { get; set; }
    }
}
