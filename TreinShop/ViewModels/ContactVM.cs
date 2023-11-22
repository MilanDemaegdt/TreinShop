using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Xunit.Sdk;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace TreinShop.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "The Email field is required")]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Name field is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject field is required")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
    }
}
