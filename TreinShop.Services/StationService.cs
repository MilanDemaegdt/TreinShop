using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TreinShop.Domain.Entities;
using TreinShop.Repositories.Interfaces;
using TreinShop.Services.Interfaces;

namespace TreinShop.Services
{
    public class StationService : IService<Station>
    {
        private IDAO<Station> _stationDAO;

        public StationService(IDAO<Station> stationDAO)
        {
            _stationDAO = stationDAO;
        }

        public async Task Add(Station entity)
        {
            await _stationDAO.Add(entity);
        }

        public async Task Delete(Station entity)
        {
            await _stationDAO.Delete(entity);
        }

        public async Task<Station> FindById(int Id)
        {
            return await _stationDAO.FindById(Id);
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            return await _stationDAO.GetAll();
        }

        public Task<IEnumerable<Station>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Station entity)
        {
            await _stationDAO.Update(entity);
        }

        Task<IEnumerable<IEnumerable<Trein>>> IService<Station>.GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
