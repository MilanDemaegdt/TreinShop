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
    public class UserDAO : IDAO<AspNetUser>
    {

        private readonly TreinDbContext _db;

        public UserDAO()
        {
            _db = new TreinDbContext();
        }

        public async Task Add(AspNetUser entity)
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

        public Task Delete(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task<AspNetUser> FindById(int id)
        {
            try
            {
                return await _db.AspNetUsers.Where(b => b.Id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("error DAO");
            }
        }

        public async Task<IEnumerable<AspNetUser>?> GetAll()
        {
            try
            {
                return await _db.AspNetUsers
                               .ToListAsync(); // volgende Namespaces toevoegen bovenaan using System.Linq; using Microsoft.EntityFrameworkCore;
            }
            catch (Exception ex)
            { throw; }
        }

        public Task<IEnumerable<AspNetUser>> GetAll(ClaimsPrincipal user)
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

        public Task Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
