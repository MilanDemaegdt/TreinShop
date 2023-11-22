using System;
using System.Collections.Generic;

namespace TreinShop.Domain.Entities
{
    public partial class TicketItem
    {
        public int TicketItemId { get; set; }
        public int TicketId { get; set; }
        public int TreinId { get; set; }
        public DateTime Date { get; set; }

        public virtual Ticket Ticket { get; set; } = null!;
        public virtual Trein Trein { get; set; } = null!;
    }
}
