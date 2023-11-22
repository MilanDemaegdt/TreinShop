using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TreinShop.Domain.Data;
using TreinShop.Domain.Entities;
using TreinShop.Repositories.Interfaces;


namespace TreinShop.Repositories
{
    public class TicketDAO : IDAO<Ticket>
    {

        private readonly TreinDbContext _db;

        public TicketDAO()
        {
            _db = new TreinDbContext();
        }

        public async Task Add(Ticket entity)
        {
            _db.Entry(entity).State = EntityState.Added;
            try
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Ticket ON;");
                await _db.SaveChangesAsync();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Ticket OFF;");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task Delete(Ticket entity)
        {
            if (entity != null)
            {
                _db.Tickets.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Ticket> FindById(int id)
        {
            try
            {
                return await _db.Tickets.Where(b => b.TicketId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO");
            }
        }

        public async Task<IEnumerable<Ticket>> GetAll(ClaimsPrincipal user)
        {
            try
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return await _db.Tickets
                            .Where(t => t.UserId == userId)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<Ticket>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEnumerable<Trein>>> GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
