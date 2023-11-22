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
    public class UserService : IService<AspNetUser>
    {
        private IDAO<AspNetUser> _userDAO;

        public UserService(IDAO<AspNetUser> userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task Add(AspNetUser entity)
        {
            await _userDAO.Add(entity);
        }

        public async Task Delete(AspNetUser entity)
        {
            await _userDAO.Delete(entity);
        }

        public async Task<AspNetUser> FindById(int Id)
        {
            return await _userDAO.FindById(Id);
        }

        public async Task<IEnumerable<AspNetUser>> GetAll()
        {
            return await _userDAO.GetAll();
        }

        public Task<IEnumerable<AspNetUser>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task Update(AspNetUser entity)
        {
            await _userDAO.Update(entity);
        }

        Task<IEnumerable<IEnumerable<Trein>>> IService<AspNetUser>.GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
