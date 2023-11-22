using Xunit.Abstractions;
using Xunit.Sdk;
using System.ComponentModel.DataAnnotations;

namespace TreinShop.ViewModels
{
    public class TreinVM
    {
        public int TreinId { get; set; }
        [Display(Name = "Departure")]
        public string VertrekNaam { get; set; }
        [Display(Name = "Destination")]
        public string AankomstNaam { get; set; }
        [Display(Name = "Departure Time")]
        public TimeSpan Tijd { get; set; }
        [Display(Name = "Arrival Time")]
        public TimeSpan ReisTijd { get; set; }
        public int MaxPassagiers { get; set; }
        public int VertrekId { get; set; }
        public int AankomstId { get; set; }
    }
}

