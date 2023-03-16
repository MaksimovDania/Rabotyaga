using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.LocationRepository
{
    public interface ILocationRepository
    {
        IEnumerable<Locations> GetLocationsList();
        void CreateLocation(Locations location);
        void DeleteLocation(string location);
    }
}
