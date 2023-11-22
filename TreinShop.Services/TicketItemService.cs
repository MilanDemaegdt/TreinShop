using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TreinShop.Domain.Entities;
using TreinShop.Repositories.Interfaces;
using TreinShop.Services.Interfaces;

namespace TreinShop.Services
{
    public class TicketItemService : IService<TicketItem>
    {
        private IDAO<TicketItem> _ticketItemDAO;

        public TicketItemService(IDAO<TicketItem> ticketItemDAO)
        {
            _ticketItemDAO = ticketItemDAO;
        }

        public async Task Add(TicketItem entity)
        {
            await _ticketItemDAO.Add(entity);
        }

        public async Task Delete(TicketItem entity)
        {
            await _ticketItemDAO.Delete(entity);
        }

        public async Task<TicketItem> FindById(int Id)
        {
            return await _ticketItemDAO.FindById(Id);
        }

        public async Task<IEnumerable<TicketItem>> GetAll()
        {
            return await _ticketItemDAO.GetAll();
        }

        public Task<IEnumerable<TicketItem>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task Update(TicketItem entity)
        {
            await _ticketItemDAO.Update(entity);
        }

        Task<IEnumerable<IEnumerable<Trein>>> IService<TicketItem>.GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            return await _ticketItemDAO.GetCountPassengersOfTrainAsync(treinId, date);
        }
    }
}
