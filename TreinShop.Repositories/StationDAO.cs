using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TreinShop.Domain.Data;
using TreinShop.Domain.Entities;
using TreinShop.Repositories.Interfaces;

namespace TreinShop.Repositories
{
    public class StationDAO : IDAO<Station>
    {
        private readonly TreinDbContext _db;

        public StationDAO()
        {
            _db = new TreinDbContext();
        }

        public async Task Add(Station entity)
        {
            _db.Entry(entity).State = EntityState.Added;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public Task Delete(Station entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Station> FindById(int id)
        {
            try
            {
                return await _db.Stations.Where(b => b.StationId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO");
            }
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            try
            {
                return await _db.Stations.ToListAsync(); // volgende Namespaces toevoegen bovenaan using System.Linq; using Microsoft.EntityFrameworkCore;
            }
            catch (Exception ex)
            { throw; }
        }

        public Task<IEnumerable<Station>> GetAll(ClaimsPrincipal user)
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

        public Task Update(Station entity)
        {
            throw new NotImplementedException();
        }
    }
}
