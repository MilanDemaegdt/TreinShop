using TreinShop.Extensions;
using TreinShop.Services;
using TreinShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TreinShop.ViewModels;
using TreinShop.Domain.Entities;
using AutoMapper;
using TreinShop.Repositories.Interfaces;
using TreinShop.Repositories;
using TreinShop.Util.Mail;
using TreinShop.ViewModels;
using Microsoft.Extensions.Options;

namespace TreinShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private TicketService _ticketService;
        private TicketItemService _ticketItemService;
        private IDAO<Ticket> _ticketDAO;
        private IDAO<TicketItem> _ticketItemDAO;
        private readonly IMapper _mapper;
        private EmailSend _emailSend;

        public ShoppingCartController(IMapper mapper, IOptions<EmailSettings> emailSettings)
        {
            _mapper = mapper;
            _ticketDAO = new TicketDAO();
            _ticketItemDAO = new TicketItemDAO();
            _ticketService = new TicketService(_ticketDAO);
            _ticketItemService = new TicketItemService(_ticketItemDAO);
            _emailSend = new EmailSend(emailSettings);
        }
        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            return View(cartList);
        }

        public async Task<IActionResult> Delete()
        {
            ShoppingCartVM? cartList = null;

            HttpContext.Session.SetObject("ShoppingCart", cartList);

            return View("Index", cartList);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> IndexAsync(ShoppingCartVM carts)
        {
            // Check if user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ShoppingCartVM? shoppingCartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartList != null)
            {
                
                List<CartVM> cartList = shoppingCartList.Cart;
                if (cartList.First().Tijd < DateTime.Now.AddDays(14))
                {
                    return View(carts);
                }
                Contact(cartList);//Email sturen

                TicketCreateVM ticketCreateVM = new TicketCreateVM
                {
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Class = cartList.First().Class,
                };

                List<TicketItemCreateVM> ticketItems = new List<TicketItemCreateVM>();

                foreach (CartVM cart in cartList)
                {
                    TicketItemCreateVM item = new TicketItemCreateVM
                    {
                        TreinId = cart.TreinID,
                        Date = cart.Tijd,
                        Date2 = cart.ReisTijd,
                    };
                    ticketItems.Add(item);
                }
                var ticket = _mapper.Map<Ticket>(ticketCreateVM);
                await _ticketService.Add(ticket);

                foreach (TicketItemCreateVM ticketItemCreateVM in ticketItems)
                {
                    ticketItemCreateVM.TicketId = ticket.TicketId;

                    var ticketItem = _mapper.Map<TicketItem>(ticketItemCreateVM);
                    await _ticketItemService.Add(ticketItem);
                }
                ShoppingCartVM? empty = null;
                HttpContext.Session.SetObject("ShoppingCart", empty);
            }
            
            
            return RedirectToAction("Confirmation", "ShoppingCart");
        }

        public IActionResult Contact(List<CartVM> ticketItems)
        {
            string message = "Bedankt voor uw bestelling " + "op " + ticketItems.First().DateCreated + "<br>" + "<br>" + "Bestelling:" + "<br>";
            foreach (CartVM ticketItem in ticketItems)
            {
                message += " " + ticketItem.VertrekNaam;
                message += " naar " + ticketItem.AankomstNaam;
                message += " van " + ticketItem.Tijd;
                message += " tot " + ticketItem.ReisTijd;
                message += " " + ticketItem.Class;
                message += "<br>";
            }
            _emailSend.SendEmailAsync(User.Identity.Name, "Bedankt voor uw bestelling!", message);
            return View("Thanks");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
