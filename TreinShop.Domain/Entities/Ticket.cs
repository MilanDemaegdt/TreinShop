using System;
using System.Collections.Generic;

namespace TreinShop.Domain.Entities
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketItems = new HashSet<TicketItem>();
        }

        public int TicketId { get; set; }
        public string UserId { get; set; } = null!;
        public string Class { get; set; } = null!;

        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<TicketItem> TicketItems { get; set; }
    }
}
