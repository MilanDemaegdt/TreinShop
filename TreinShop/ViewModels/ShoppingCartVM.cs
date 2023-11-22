namespace TreinShop.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartVM>? Cart { get; set; }
    }

    public class CartVM
    {
        public int TreinID { get; set; }
        public string VertrekNaam { get; set; }
        public string AankomstNaam { get; set; }
        public DateTime Tijd { get; set; }
        public DateTime ReisTijd { get; set; }
        public int VrijePlaatsen { get; set; }
        public float Prijs { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Class { get; set; }
    }
}
