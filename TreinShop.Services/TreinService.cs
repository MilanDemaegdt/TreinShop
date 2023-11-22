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
    public class TreinService : IService<Trein>
    {
        private IDAO<Trein> _treinDAO;

        public TreinService(IDAO<Trein> treinDAO)
        {
            _treinDAO = treinDAO;
        }

        public async Task Add(Trein entity)
        {
            await _treinDAO.Add(entity);
        }

        public async Task Delete(Trein entity)
        {
            await _treinDAO.Delete(entity);
        }

        public async Task<Trein> FindById(int Id)
        {
            return await _treinDAO.FindById(Id);
        }

        public async Task<IEnumerable<Trein>> GetAll()
        {
            return await _treinDAO.GetAll();
        }

        public Task<IEnumerable<Trein>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IEnumerable<Trein>>> GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            return await _treinDAO.GetTrainsByStation(vertrekID, aankomstID, timeSpan);
        }

        public async Task Update(Trein entity)
        {
            await _treinDAO.Update(entity);
        }
    }
}
