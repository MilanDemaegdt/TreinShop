using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TreinShop.Domain.Entities;
using TreinShop.Extensions;
using TreinShop.Repositories;
using TreinShop.Repositories.Interfaces;
using TreinShop.Services;
using TreinShop.ViewModels;

namespace TreinShop.Controllers
{
    public class BookingController : Controller
    {
        private TreinService _treinService;
        private StationService _stationService;
        private TicketService _ticketService;
        private TicketItemService _ticketItemService;
        private IDAO<Trein> _treinDAO;
        private IDAO<Station> _stationDAO;
        private IDAO<Ticket> _ticketDAO;
        private IDAO<TicketItem> _ticketItemDAO;
        private readonly IMapper _mapper;

        public BookingController(IMapper mapper)
        {
            _mapper = mapper;
            _treinDAO = new TreinDAO();
            _stationDAO = new StationDAO();
            _ticketDAO = new TicketDAO();
            _ticketItemDAO = new TicketItemDAO();
            _treinService = new TreinService(_treinDAO);
            _stationService = new StationService(_stationDAO);
            _ticketService = new TicketService(_ticketDAO);
            _ticketItemService = new TicketItemService(_ticketItemDAO);
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Select(int? vertrekID, int? aankomstID, DateTime timeSpan, bool business)
        {
            if (vertrekID == null || aankomstID == null)
            {
                return NotFound();
            }
            var timeSpan2 = timeSpan.TimeOfDay;
            var treinList = await _treinService.GetTrainsByStation(Convert.ToInt32(vertrekID), Convert.ToInt32(aankomstID), timeSpan2);
            List<Trein?> treins = (List<Trein?>)treinList.First();
            var Class = "";
            var prijs = 0;
            if (business)
            {
                Class = "Business";
                prijs = 30;
            }
            else
            {
                Class = "Economic";
                prijs = 20;
            }
            if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") == null)
            { 

                foreach (Trein trein in treins)
                {
                    
                    CartVM item = new CartVM
                    {
                        TreinID = trein.TreinId,
                        VertrekNaam = trein.Vertrek.Name,
                        AankomstNaam = trein.Aankomst.Name,
                        Tijd = timeSpan.Date + trein.Tijd,
                        ReisTijd = timeSpan.Date + trein.ReisTijd,
                        VrijePlaatsen = (trein.MaxPassagiers),
                        Prijs = prijs,
                        DateCreated = DateTime.Now,
                        Class = Class,
                    };

                    ShoppingCartVM? shopping;

                    if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
                    {
                        shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                    }
                    else
                    {
                        shopping = new ShoppingCartVM();
                        shopping.Cart = new List<CartVM>();
                    }
                    shopping?.Cart?.Add(item);
                    HttpContext.Session.SetObject("ShoppingCart", shopping);
                }
                return RedirectToAction("Index", "ShoppingCart");
            }
            return RedirectToAction("GetTrainsByStations", "Booking", new
            {
                vertrekId = vertrekID,
                bestemmingId = aankomstID,
                dateTime = timeSpan,
                business = business
            });
        }

        public async Task<IActionResult> GetTrainsByStations()
        {
            ViewBag.banner = Banners();
            ViewBag.vertrek = 0;
            ViewBag.bestemming = 0;
            ViewBag.dateTime = 0;
            ViewBag.treinList = 0;
            ViewBag.business = false;
            ViewBag.lstStation = new SelectList(await _stationService.GetAll(), "StationId", "Name");
            ViewBag.lstStation2 = new SelectList(await _stationService.GetAll(), "StationId", "Name");
            if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
            {
                var shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                if (shopping.Cart.Count() != 0)
                {
                    ViewBag.ErrorMessage = "There is already a ticket in your shoppingcart!";
                    return View();
                }
            }
            return View(); // pass an empty list of TreinVM objects as the model
        }

        [HttpPost]
        public async Task<IActionResult> GetTrainsByStations(int? vertrekId, int? bestemmingId, DateTime dateTime, bool business)
        {
            ViewBag.lstStation = new SelectList(await _stationService.GetAll(), "StationId", "Name", vertrekId);
            ViewBag.lstStation2 = new SelectList(await _stationService.GetAll(), "StationId", "Name", bestemmingId);
            ViewBag.banner = Banners();
            ViewBag.dateTime = 0;
            ViewBag.treinList = 0;
            ViewBag.vertrek = 0;
            ViewBag.bestemming = 0;
            ViewBag.business = business;
            if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
            {
                var shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                if (shopping.Cart.Count() != 0)
                {
                    ViewBag.ErrorMessage = "There is already a ticket in your shoppingcart!";
                    return View();
                }
            }
            if (vertrekId == null || bestemmingId == null)
            {
                ViewBag.ErrorMessage = "Please select the stations first";
                return View();
            }
            else if (vertrekId == bestemmingId)
            {
                ViewBag.ErrorMessage = "Please select 2 different stations";
                return View();
            }
            else {
                TimeSpan timeSpan = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
                var bierList = await _treinService.GetTrainsByStation(Convert.ToInt16(vertrekId), Convert.ToInt16(bestemmingId), timeSpan);
                ViewBag.vertrek = vertrekId;
                ViewBag.bestemming = bestemmingId;
                ViewBag.dateTime = dateTime;
                var bierList2 = new List<List<Trein>>();
                if (bierList != null) {
                    foreach (List<Trein> treinList in bierList)
                    {
                        var boolean = true;
                        foreach (Trein trein in treinList)
                        {
                            var count = await _ticketItemService.GetCountPassengersOfTrainAsync(trein.TreinId, dateTime.Date);
                            if (count >= trein.MaxPassagiers)
                            {
                                boolean = false;
                                await GetTrainsByStations(vertrekId, bestemmingId, dateTime.AddMinutes(15), business);
                            }
                        }
                        if (boolean)
                        {
                            bierList2.Add(treinList);
                        }
                    }
                }
                
                if (bierList2 != null) {
                    if (bierList2.Any() && bierList2.First() != null)
                    {
                        ViewBag.treinList = 1;
                        List<TreinVM> listVM = _mapper.Map<List<TreinVM>>(bierList2.First());
                        return View(listVM);
                    }
                }
                List<TreinVM> listVM2 = new List<TreinVM>();
                return View(listVM2);
            }
        }


        [HttpPost]
        public JsonResult ValidateStations(int StationIdVertrek, int StationIdBestemming)
        {
            System.Diagnostics.Debug.WriteLine(StationIdVertrek);
            System.Diagnostics.Debug.WriteLine(StationIdBestemming);
            if (StationIdVertrek == StationIdBestemming)
            {
                return Json("Vertrek station en bestemming station kunnen niet hetzelfde zijn.");
            }
            else
            {
                return Json(true);
            }
        }

        public async Task<List<TicketItem>> GetByTicketId(int ticketId)
        {
            var ticketItemList = await _ticketItemService.GetAll();
            var newTicketItemList = new List<TicketItem>();
            foreach (TicketItem ticketItem in ticketItemList)
            {
                if (ticketItem.TicketId == ticketId)
                {
                    newTicketItemList.Add(ticketItem);
                }
            }
            return newTicketItemList;
        }

        public async Task<string> Banners()
        {
            var ticketList = await _ticketService.GetAll(User);
            var ticketVMs = new List<TicketVM>();
            if (ticketList != null)
            {
                ticketVMs = _mapper.Map<List<TicketVM>>(ticketList);
                foreach (var ticketVM in ticketVMs)
                {
                    var ticketItems = await GetByTicketId(ticketVM.TicketId);
                    if (ticketItems != null && ticketItems.Any())
                    {
                        var firstTrein = await _treinService.FindById(ticketItems.First().TreinId);
                        var lastTrein = await _treinService.FindById(ticketItems.Last().TreinId);
                        ticketVM.vertrek = (await _stationService.FindById(firstTrein.VertrekId)).Name;
                        ticketVM.bestemming = (await _stationService.FindById(lastTrein.AankomstId)).Name;
                    }
                }
            }

            var groups = ticketVMs.GroupBy(t => t.bestemming)
                                  .OrderByDescending(g => g.Count());

            var bestemmingBanner = groups.FirstOrDefault()?.Key ?? "Trein";

            return bestemmingBanner.Trim();
        }

    }
}
