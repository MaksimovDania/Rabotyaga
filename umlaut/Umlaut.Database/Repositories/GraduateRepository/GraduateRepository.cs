using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.GraduateRepository
{
    public class GraduateRepository : BaseRepository, IGraduateRepository
    {
        public GraduateRepository(UmlautDBContext context) : base(context) { }

        public void CreateGraduate(Graduate newGraduate)
        {
            var graduate = IsUnique(newGraduate);
            _context.Graduates.Add(graduate);
            _context.SaveChanges();
        }

        private Graduate IsUnique(Graduate graduate)
        {
            if (_context.Graduates.Any(u => u.ResumeLink == graduate.ResumeLink))
                throw new InvalidOperationException("Such a graduate already exists");
            if (_context.Faculties.Any(u => u.Faculty == graduate.Faculty.Faculty))
                graduate.Faculty = _context.Faculties.FirstOrDefault(u => u.Faculty == graduate.Faculty.Faculty);
            if (_context.Locations.Any(u => u.Location == graduate.Location.Location))
                graduate.Location = _context.Locations.FirstOrDefault(u => u.Location == graduate.Location.Location);
            List<Specializations> specializations = new List<Specializations>();
            foreach (var item in graduate.Specialization)
                if (!_context.Specializations.Any(u => u.Specialization == item.Specialization))
                {
                    
                    _context.Specializations.Add(item);
                    specializations.Add(item);
                } else
                    specializations.Add(_context.Specializations.FirstOrDefault(u => u.Specialization == item.Specialization));
            graduate.Specialization = specializations;
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
