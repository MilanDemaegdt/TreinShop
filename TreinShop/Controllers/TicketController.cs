using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreinShop.Domain.Entities;
using TreinShop.Repositories.Interfaces;
using TreinShop.Repositories;
using TreinShop.Services;
using System;
using TreinShop.ViewModels;
using TreinShop.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TreinShop.Controllers
{
    public class TicketController : Controller
    {
        private TicketService _ticketService;
        private TicketItemService _ticketItemService;
        private TreinService _treinService;
        private StationService _stationService;
        private IDAO<Ticket> _ticketDAO;
        private IDAO<TicketItem> _ticketItemDAO;
        private IDAO<Trein> _treinDAO;
        private IDAO<Station> _stationDAO;

        private readonly IMapper _mapper;

        public TicketController(IMapper mapper)
        {
            _mapper = mapper;
            _ticketDAO = new TicketDAO();
            _ticketItemDAO = new TicketItemDAO();
            _treinDAO = new TreinDAO();
            _stationDAO = new StationDAO();
            _ticketService = new TicketService(_ticketDAO);
            _ticketItemService = new TicketItemService(_ticketItemDAO);
            _treinService = new TreinService(_treinDAO);
            _stationService = new StationService(_stationDAO);
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.ErrorMessage = null;
            var errorMessage = "";
            if (TempData.Peek("ErrorMessage") != null) {
                errorMessage = (string)TempData.Peek("ErrorMessage");
            }
            
            if (errorMessage != null)
            {
                ViewBag.ErrorMessage = errorMessage;
                TempData["ErrorMessage"] = null;
            }
            var ticketList = await _ticketService.GetAll(User);  // Domain objects
            List<TicketVM> ticketVMs = new List<TicketVM>();
            if (ticketList != null)
            {
                ticketVMs = _mapper.Map<List<TicketVM>>(ticketList);
                foreach (TicketVM ticketVM in ticketVMs)
                {
                    var ticketItems = await GetByTicketId(ticketVM.TicketId);
                    if (ticketItems != null && ticketItems.Any())
                    {
                        DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                        TimeSpan timeOnly = DateTime.Now.TimeOfDay;
                        DateOnly firstTicketItem = DateOnly.FromDateTime(ticketItems.First().Date);
                        DateOnly lastTicketItem = DateOnly.FromDateTime(ticketItems.Last().Date);
                        var firstTrein = await _treinService.FindById(ticketItems.First().TreinId);
                        var lastTrein = await _treinService.FindById(ticketItems.Last().TreinId);

                        DateTimeOffset dateTimeOffset = new DateTimeOffset(firstTicketItem.Year, firstTicketItem.Month, firstTicketItem.Day, 0, 0, 0, TimeSpan.Zero);
                        TimeSpan timeSpan = firstTrein.Tijd;
                        DateTime dateTime = dateTimeOffset.Add(timeSpan).DateTime;
                        ticketVM.DateVertrek = dateTime;
                        if (ticketItems.Count() > 1)
                        {
                            DateTimeOffset dateTimeOffset2 = new DateTimeOffset(lastTicketItem.Year, lastTicketItem.Month, lastTicketItem.Day, 0, 0, 0, TimeSpan.Zero);
                            TimeSpan timeSpan2 = lastTrein.ReisTijd;
                            DateTime dateTime2 = dateTimeOffset2.Add(timeSpan2).DateTime;
                            ticketVM.DateAankomst = dateTime2;
                        }
                        else
                        {
                            TimeSpan timeSpan2 = firstTrein.ReisTijd;
                            DateTime dateTime2 = dateTimeOffset.Add(timeSpan2).DateTime;
                            ticketVM.DateAankomst = dateTime2;
                        }


                        ticketVM.vertrek = (await _stationService.FindById(firstTrein.VertrekId)).Name;
                        ticketVM.bestemming = (await _stationService.FindById(lastTrein.AankomstId)).Name;
                        if (firstTicketItem > now)
                        {
                            ticketVM.Status = "Available";
                        }
                        else if (lastTicketItem < now)
                        {
                            ticketVM.Status = "Completed";
                        }
                        else
                        {
                            if (timeOnly > lastTrein.ReisTijd)
                            {
                                ticketVM.Status = "Completed";
                            }
                            else if (timeOnly < firstTrein.Tijd)
                            {
                                ticketVM.Status = "Completed";
                            }
                            else
                            {
                                ticketVM.Status = "In use";
                            }
                        }
                    }
                }
            }
            ticketVMs = ticketVMs.OrderBy(t => t.Status == "Available" ? 1 : t.Status == "In use" ? 2 : 3).ToList();

            if (ticketVMs.Count() == 0)
            {
                ticketVMs = null;
            }
            return View(ticketVMs);
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

        public async Task<IActionResult> Details(int ticketId)
        {
            if (ticketId == null)
            {
                return NotFound();
            }
            var ticketItems = await GetByTicketId(ticketId);
            List<Trein> treins = new List<Trein>();
            foreach (TicketItem ticketItem in ticketItems)
            {
                treins.Add(await _treinService.FindById(ticketItem.TreinId));
            }
            List<TreinVM> treinVMs = _mapper.Map<List<TreinVM>>(treins);
            foreach (TreinVM treinVM in treinVMs)
            {
                Trein trein = await _treinService.FindById(treinVM.TreinId);
                Station station1 = await _stationService.FindById(trein.VertrekId);
                Station station2 = await _stationService.FindById(trein.AankomstId);
                treinVM.VertrekNaam = station1.Name;
                treinVM.AankomstNaam = station2.Name;
            }
            return View(treinVMs);
        }

        public async Task<IActionResult> Cancel(int ticketId)
        {
            if (ticketId == null)
            {
                return NotFound();
            }
            TicketItem ticketItem;
            while ((ticketItem = await _ticketItemService.FindById(ticketId)) != default(TicketItem))
            {
                if (ticketItem.Date > DateTime.Now.AddDays(3))
                {
                    TempData["ErrorMessage"] = null;
                    _ticketItemService.Delete(ticketItem);
                }
                else
                {
                    TempData["ErrorMessage"] = "You cannot cancel this ticket as the cancellation period has ended.\nPlease note that ticket cancellations are only permitted up to 3 days prior to the scheduled departure date."; return RedirectToAction("Index", "Ticket");
                }
            }
            Ticket ticket = await _ticketService.FindById(ticketId);
            _ticketService.Delete(ticket);
            return RedirectToAction("Index", "Ticket");
        }
        
    }
}
