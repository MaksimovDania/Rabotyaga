using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.FacultyRepository
{
    public class FacultyRepository : BaseRepository, IFacultyRepository
    {
        public FacultyRepository(UmlautDBContext context) : base(context) { }

        public void CreateFaculty(Faculties newFaculty)
        {
            if (newFaculty.Faculty == String.Empty)
                throw new ArgumentException();
            if (_context.Faculties.Any(u => u.Faculty == newFaculty.Faculty))
                throw new InvalidOperationException("Such a faculty already exists");
            _context.Faculties.Add(newFaculty);
            _context.SaveChanges();

        }

        public void DeleteFaculty(string deleteFacultyStr)
        {
            if (!_context.Faculties.Any(u => u.Faculty == deleteFacultyStr))
                throw new InvalidOperationException("There is no such faculty");
            var deleteFaculty = _context.Faculties.FirstOrDefault(u => u.Faculty == deleteFacultyStr);
            _context.Faculties.Remove(deleteFaculty);
            _context.SaveChanges();

        }

        public IEnumerable<Faculties> GetFacultiesList()
        {
            var list = _context.Faculties.ToList();
            return list;
        }
    }
}
