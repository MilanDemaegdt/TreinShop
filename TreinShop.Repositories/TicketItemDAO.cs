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
    public class TicketItemDAO : IDAO<TicketItem>
    {

        private readonly TreinDbContext _db;

        public TicketItemDAO()
        {
            _db = new TreinDbContext();
        }

        public async Task Add(TicketItem entity)
        {
            _db.Entry(entity).State = EntityState.Added;
            try
            {
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TicketItem ON;");
                await _db.SaveChangesAsync();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT TicketItem OFF;");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task Delete(TicketItem entity)
        {

            if (entity != null)
            {
                _db.TicketItems.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<TicketItem> FindById(int id)
        {
            try
            {
                TreinDbContext _db2 = new TreinDbContext();
                return await _db2.TicketItems.FirstOrDefaultAsync(b => b.TicketId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TicketItem>> GetAll()
        {
            try
            {
                return await _db.TicketItems
                               .ToListAsync(); // volgende Namespaces toevoegen bovenaan using System.Linq; using Microsoft.EntityFrameworkCore;
            }
            catch (Exception ex)
            { throw; }
        }

        public Task<IEnumerable<TicketItem>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<IEnumerable<Trein>>> IDAO<TicketItem>.GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountPassengersOfTrainAsync(int trainId, DateTime date)
        {
            var test = await _db.TicketItems
                            .Where(t => t.TreinId == trainId && t.Date == date)
                            .ToListAsync();
            return test.Count();
        }

        Task IDAO<TicketItem>.Update(TicketItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
