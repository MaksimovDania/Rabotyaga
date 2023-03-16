using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.LocationRepository
{
    public class LocationRepositopy : BaseRepository, ILocationRepository
    {
        public LocationRepositopy(UmlautDBContext context) : base(context) { }

        public void CreateLocation(Locations newLocation)
        {
            if (newLocation.Location == String.Empty)
                throw new ArgumentException();
            if (_context.Locations.Where(u => u.Location == newLocation.Location).Count() > 0)
                throw new InvalidOperationException("Such a location already exists");
            _context.Locations.Add(newLocation);
            _context.SaveChanges();

        }

        public void DeleteLocation(string deleteLocationStr)
        {
            if (_context.Locations.Where(u => u.Location == deleteLocationStr).Count() == 0)
                throw new InvalidOperationException("There is no such location");
            var deleteLocation = _context.Locations.FirstOrDefault(u => u.Location == deleteLocationStr);
            _context.Locations.Remove(deleteLocation);
            _context.SaveChanges();

        }

        public IEnumerable<Locations> GetLocationsList()
        {
            var list = _context.Locations.ToList();
            return list;

        }
    }
}
