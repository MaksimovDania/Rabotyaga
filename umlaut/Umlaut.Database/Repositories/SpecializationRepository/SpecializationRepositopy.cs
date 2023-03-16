using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.SpecializationRepository
{
    public class SpecializationRepositopy : BaseRepository, ISpecializationRepository
    {
        public SpecializationRepositopy(UmlautDBContext context) : base(context) { }

        public void CreateSpecialization(Specializations specialization)
        {
            if (specialization.Specialization == String.Empty)
                throw new ArgumentException();
            if (_context.Specializations.Any(u => u.Specialization == specialization.Specialization))
                throw new InvalidOperationException("Such a specialization already exists");
            _context.Specializations.Add(specialization);
            _context.SaveChanges();

        }

        public void DeleteSpecialization(string deleteSpecializationStr)
        {
            if (!_context.Specializations.Any(u => u.Specialization == deleteSpecializationStr))
                throw new InvalidOperationException("There is no such specialization");
            var deleteSpecialization = _context.Specializations.FirstOrDefault(u => u.Specialization == deleteSpecializationStr);
            _context.Specializations.Remove(deleteSpecialization);
            _context.SaveChanges();

        }

        public IEnumerable<Specializations> GetSpecializationsList()
        {
            var list = _context.Specializations.ToList();
            return list;

        }
    }
}
