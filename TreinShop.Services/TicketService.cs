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
    public class TicketService : IService<Ticket>
    {
        private IDAO<Ticket> _ticketDAO;

        public TicketService(IDAO<Ticket> ticketDAO)
        {
            _ticketDAO = ticketDAO;
        }

        public async Task Add(Ticket entity)
        {
            await _ticketDAO.Add(entity);
        }

        public async Task Delete(Ticket entity)
        {
            await _ticketDAO.Delete(entity);
        }

        public async Task<Ticket> FindById(int Id)
        {
            return await _ticketDAO.FindById(Id);
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            return await _ticketDAO.GetAll();
        }

        public async Task<IEnumerable<Ticket>> GetAll(ClaimsPrincipal user)
        {
            return await _ticketDAO.GetAll(user);
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Ticket entity)
        {
            await _ticketDAO.Update(entity);
        }

        Task<IEnumerable<IEnumerable<Trein>>> IService<Ticket>.GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
