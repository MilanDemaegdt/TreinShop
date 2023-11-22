using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TreinShop.ViewModels
{
    public class StationTreinen
    {
        [Required(ErrorMessage = "Vertrek station is niet geselecteerd.")]
        [Remote("ValidateStations", "Booking", ErrorMessage = "Vertrek station en bestemming station kunnen niet hetzelfde zijn.")]
        public int StationIdVertrek { get; set; }
        [Required(ErrorMessage = "Bestemming station is niet geselecteerd.")]
        public int StationIdBestemming { get; set; }
        public IEnumerable<SelectListItem>? StationsVertrek { get; set; }
        public IEnumerable<SelectListItem>? StationsBestemming { get; set; }
        
        public DateTime? DateTime { get; set; }
        public IEnumerable<TreinVM>? Treinen { get; set; }

    }
}
