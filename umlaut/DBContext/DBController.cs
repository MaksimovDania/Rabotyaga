using DBModels;

namespace DBContext
{
    public class DBController
    {
        private DataBaseContext db;

        public DBController()
        {
            db = new DataBaseContext();
        }
        IEnumerable<Graduate> GetGraduatesList()
        {
            List<Graduate> list = db.Graduates.ToList();
            return list;
        }
        IEnumerable<Faculties> GetFacultiesList()
        {
            List <Faculties> list = db.Faculties.ToList();
            return list;
        }
        IEnumerable<Locations> GetLocationsList()
        {
            List<Locations> list = db.Locations.ToList();
            return list;
        }
        IEnumerable<Specializations> GetSpecializationsList()
        {
            List<Specializations> list = db.Specializations.ToList();
            return list;
        }
        void CreateGraduate(Graduate g)
        {
            CreateFaculty(g.Faculty);
            CreateLocation(g.Location);
            foreach (var item in g.Specialization)
                CreateSpecialization(item);
            db.Graduates.Add(g);
            db.SaveChanges();
        }
        void CreateFaculty(Faculties f)
        {
            if (f.Faculty == String.Empty)
                throw new ArgumentException();
            db.Faculties.Add(f);
            db.SaveChanges();
        }
        void CreateLocation(Locations l)
        {
            if (l.Location == String.Empty)
                throw new ArgumentException();
            db.Locations.Add(l);
            db.SaveChanges();
        }
        void CreateSpecialization(Specializations s)
        {
            if (s.Specialization == String.Empty)
                throw new ArgumentException();
            db.Specializations.Add(s);
            db.SaveChanges();
        }
        void DeleteGraduate(int id)
        {
            var g = db.Graduates.FirstOrDefault(u => u.Id == id);
            db.Graduates.Remove(g);
            db.SaveChanges();
        }
        void DeleteFaculty(string faculty)
        {
            var f = db.Faculties.FirstOrDefault(u => u.Faculty == faculty);
            db.Faculties.Remove(f);
            db.SaveChanges();
        }
        void DeleteLocation(string location)
        {
            var l = db.Locations.FirstOrDefault(u => u.Location == location);
            db.Locations.Remove(l);
            db.SaveChanges();
        }
        void DeleteSpecialization(string specialization)
        {
            var s = db.Specializations.FirstOrDefault(u => u.Specialization == specialization);
            db.Specializations.Remove(s);
            db.SaveChanges();
        }
        void UpdateGraduate(Graduate newG)
        {
            var g = db.Graduates.FirstOrDefault(u => u.ResumeLink == newG.ResumeLink);
            g.Gender = newG.Gender;
            g.Age = newG.Age;
            g.Location = newG.Location;
            g.Vacation = newG.Vacation;
            g.Specialization = newG.Specialization;
            g.ExpectedSalary = newG.ExpectedSalary;
            g.Experience = newG.Experience;
            g.YearGraduation = newG.YearGraduation;
            g.Faculty = newG.Faculty;
            db.Entry(g).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
