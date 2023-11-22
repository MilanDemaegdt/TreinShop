using TreinShop.Domain.Entities;

namespace TreinShop.ViewModels
{
    public class TicketItemVM
    {
        public int TicketItemId { get; set; }
        public int TicketId { get; set; }
        public int TreinId { get; set; }
        public DateTime Date { get; set; }
    }
}
