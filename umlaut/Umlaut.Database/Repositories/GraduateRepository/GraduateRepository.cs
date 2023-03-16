using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.GraduateRepository
{
    internal class GraduateRepository : BaseRepository, IGraduateRepository
    {
        public GraduateRepository(UmlautDBContext context) : base(context) { }

        public void CreateGraduate(Graduate newGraduate)
        {
            newGraduate = IsUnique(newGraduate);
            _context.Graduates.Add(newGraduate);
            _context.SaveChanges();
        }

        public Graduate IsUnique(Graduate graduate)
        {
            if (_context.Graduates.Where(u => u.ResumeLink == graduate.ResumeLink).Count() > 0)
                throw new InvalidOperationException("Such a graduate already exists");
            if (_context.Faculties.Where(u => u.Faculty == graduate.Faculty.Faculty).Count() > 0)
                graduate.Faculty = _context.Faculties.FirstOrDefault(u => u.Faculty == graduate.Faculty.Faculty);
            if (_context.Locations.Where(u => u.Location == graduate.Location.Location).Count() > 0)
                graduate.Location = _context.Locations.FirstOrDefault(u => u.Location == graduate.Location.Location);
            foreach (var item in graduate.Specialization)
                if (_context.Specializations.Where(u => u.Specialization == item.Specialization).Count() > 0)
                {
                    graduate.Specialization.Remove(item);
                    graduate.Specialization.Add(_context.Specializations.FirstOrDefault(u => u.Specialization == item.Specialization));
                }
            return graduate;
        }

        public void DeleteGraduate(string resumeLink)
        {
            var deleteGraduate = _context.Graduates.FirstOrDefault(u => u.ResumeLink == resumeLink);
            _context.Graduates.Remove(deleteGraduate);
            _context.SaveChanges();
        }

        public IEnumerable<Graduate> GetGraduatesList()
        {
            var list = _context.Graduates.ToList();
            return list;

        }

        public void UpdateGraduate(Graduate newG)
        {
            var g = _context.Graduates.FirstOrDefault(u => u.ResumeLink == newG.ResumeLink);
            g.Gender = newG.Gender;
            g.Age = newG.Age;
            g.Location = newG.Location;
            g.Vacation = newG.Vacation;
            g.Specialization = newG.Specialization;
            g.ExpectedSalary = newG.ExpectedSalary;
            g.Experience = newG.Experience;
            g.YearGraduation = newG.YearGraduation;
            g.Faculty = newG.Faculty;
            _context.Entry(g).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public bool IsAlreadyExists(string resumeLink)
        {
            return _context.Graduates.Where(u => u.ResumeLink == resumeLink).Any();
        }
    }
}
