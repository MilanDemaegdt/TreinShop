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
    public class TreinDAO : IDAO<Trein>
    {
        private readonly TreinDbContext _db;

        public TreinDAO()
        {
            _db = new TreinDbContext();
        }

        public async Task Add(Trein entity)
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

        public Task Delete(Trein entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Trein> FindById(int id)
        {
            try
            {
                TreinDbContext _db2 = new TreinDbContext();
                return await _db2.Treins.Where(b => b.TreinId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Trein>?> GetAll()
        {
            try
            {
                return await _db.Treins
                               .Include(t => t.Vertrek)
                               .Include(t => t.Aankomst)
                               .ToListAsync(); // volgende Namespaces toevoegen bovenaan using System.Linq; using Microsoft.EntityFrameworkCore;
            }
            catch (Exception ex)
            { throw; }
        }

        public Task<IEnumerable<Trein>> GetAll(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IEnumerable<Trein>>> GetAllPossibleRoutes(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            var possibleRoutes = new List<List<Trein>>();
            var startingTrains = await _db.Treins
                .Include(t => t.Vertrek)
                .Include(t => t.Aankomst)
                .OrderBy(t => t.Tijd)
                .Where(t => t.VertrekId == vertrekID &&
                            t.AankomstId != aankomstID &&
                            t.Tijd >= timeSpan)
                .ToListAsync();
            foreach (var train1 in startingTrains)
            {
                var intermediateStationID1 = train1.AankomstId;
                var endingTrains = await _db.Treins
                    .Include(t => t.Vertrek)
                    .Include(t => t.Aankomst)
                    .OrderBy(t => t.Tijd)
                    .Where(t => t.VertrekId == intermediateStationID1 && t.AankomstId == aankomstID)
                    .ToListAsync();
                foreach (var endingTrain in endingTrains)
                {
                    if (endingTrain.Tijd >= train1.ReisTijd.Add(TimeSpan.FromMinutes(1)))
                    {
                        var route = new List<Trein> { train1, endingTrain };
                        possibleRoutes.Add(route);
                    }
                }
                var intermediateTrains = await _db.Treins
                    .Include(t => t.Vertrek)
                    .Include(t => t.Aankomst)
                    .OrderBy(t => t.Tijd)
                    .Where(t => t.VertrekId == intermediateStationID1 && t.AankomstId != aankomstID)
                    .ToListAsync();
                foreach (var train2 in intermediateTrains)
                {
                    var intermediateStationID2 = train2.AankomstId;
                    var endingTrains2 = await _db.Treins
                        .Include(t => t.Vertrek)
                        .Include(t => t.Aankomst)
                        .OrderBy(t => t.Tijd)
                        .Where(t => t.VertrekId == intermediateStationID2 && t.AankomstId == aankomstID)
                        .ToListAsync();
                    foreach (var endingTrain in endingTrains2)
                    {
                        if (endingTrain.Tijd >= train2.ReisTijd.Add(TimeSpan.FromMinutes(1)) && train2.Tijd >= train1.ReisTijd.Add(TimeSpan.FromMinutes(1)))
                        {
                            var route = new List<Trein> { train1, train2, endingTrain };
                            possibleRoutes.Add(route);
                        }
                    }
                }
            }
            possibleRoutes = possibleRoutes.OrderBy(l => l.Count).ToList();
            if (possibleRoutes.Count != 0) {
                return possibleRoutes;
            }
            return null;
        }

        public Task<int> GetCountPassengersOfTrainAsync(int treinId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IEnumerable<Trein>>> GetTrainsByStation(int vertrekID, int aankomstID, TimeSpan timeSpan)
        {
            try
            {
                // Zoeken achter één trein tussen de 2 stations
                var directTrain = await _db.Treins
                    .Include(t => t.Vertrek)
                    .Include(t => t.Aankomst)
                    .OrderBy(t => t.Tijd)
                    .FirstOrDefaultAsync(t => t.VertrekId == vertrekID &&
                                              t.AankomstId == aankomstID &&
                                              t.Tijd >= timeSpan);

                if (directTrain != null)
                {
                    var directTrainList = new List<Trein> { directTrain };
                    var possibleRoutes = new List<List<Trein>>();
                    possibleRoutes.Add(directTrainList);
                    return possibleRoutes;
                }

                // No direct train, use the code to find all possible connections
                var connections = await GetAllPossibleRoutes(vertrekID, aankomstID, timeSpan);
                if (connections != null) {
                    return connections;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task Update(Trein entity)
        {
            throw new NotImplementedException();
        }
    }
}
