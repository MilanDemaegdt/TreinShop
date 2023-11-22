using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TreinShop.Domain.Entities;

namespace TreinShop.Repositories.Interfaces
{
    public interface IDAO<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);
        Task<T> FindById(int Id);
        Task<IEnumerable<IEnumerable<Trein>>> GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan);
        Task<IEnumerable<T>> GetAll(ClaimsPrincipal user);
        Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date);
    }
}
