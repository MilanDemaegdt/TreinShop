using System;
using System.Collections.Generic;

namespace TreinShop.Domain.Entities
{
    public partial class Trein
    {
        public Trein()
        {
            TicketItems = new HashSet<TicketItem>();
        }

        public int TreinId { get; set; }
        public int VertrekId { get; set; }
        public int AankomstId { get; set; }
        public TimeSpan Tijd { get; set; }
        public TimeSpan ReisTijd { get; set; }
        public int MaxPassagiers { get; set; }

        public virtual Station Aankomst { get; set; } = null!;
        public virtual Station Vertrek { get; set; } = null!;
        public virtual ICollection<TicketItem> TicketItems { get; set; }
    }
}
