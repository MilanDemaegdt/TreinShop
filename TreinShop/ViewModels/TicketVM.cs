using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TreinShop.Domain.Entities;

namespace TreinShop.ViewModels
{
    public class TicketVM
    {
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }
        [Display(Name = "Departure Time")]
        public DateTime DateVertrek { get; set; }
        [Display(Name = "Arrival Time")]
        public DateTime DateAankomst { get; set; }
        [Display(Name = "Departure")]
        public string vertrek { get; set; }
        [Display(Name = "Destination")]
        public string bestemming { get; set; }
    }
}
