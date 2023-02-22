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
        void CreateGraduate(string gender, int age, string location, string vacation, List<string> specialization,
                            int expectedSalary, int experience, string yearGraduation, string faculty, string resumeLink)
        {
            CreateFaculty(faculty);
            CreateLocation(location);
            var s = new List<Specializations>();
            foreach (var item in specialization)
            {
                CreateSpecialization(item);
                s.Add(db.Specializations.FirstOrDefault(u => u.Specialization == item));
            }
            var f = db.Faculties.FirstOrDefault(u => u.Faculty == faculty);
            var l = db.Locations.FirstOrDefault(u => u.Location == location);
            Graduate g = new Graduate { Gender = gender, Age = age, Location = l , Vacation = vacation, Specialization = s, 
                                        ExpectedSalary = expectedSalary, Experience = experience, YearGraduation = yearGraduation, 
                                        Faculty = f, ResumeLink = resumeLink };
            db.Graduates.Add(g);
            db.SaveChanges();
        }
        void CreateFaculty(string faculty)
        {
            if (faculty == String.Empty)
                throw new ArgumentException();
            Faculties f = new Faculties { Faculty = faculty };
            db.Faculties.Add(f);
            db.SaveChanges();
        }
        void CreateLocation(string location)
        {
            if (location == String.Empty)
                throw new ArgumentException();
            Locations l = new Locations { Location = location };
            db.Locations.Add(l);
            db.SaveChanges();
        }
        void CreateSpecialization(string specialization)
        {
            if (specialization == String.Empty)
                throw new ArgumentException();
            Specializations s = new Specializations { Specialization = specialization };
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
        void UpdateGraduate(string gender, int age, string location, string vacation, List<string> specialization,
                            int expectedSalary, int experience, string yearGraduation, string faculty, string resumeLink)
        {
            var g = db.Graduates.FirstOrDefault(u => u.ResumeLink == resumeLink);
            var f = db.Faculties.FirstOrDefault(u => u.Faculty == faculty);
            var l = db.Locations.FirstOrDefault(u => u.Location == location);
            var s = new List<Specializations>();
            foreach (var item in specialization)
                s.Add(db.Specializations.FirstOrDefault(u => u.Specialization == item));
            g.Gender = gender;
            g.Age = age;
            g.Location = l;
            g.Vacation = vacation;
            g.Specialization = s;
            g.ExpectedSalary = expectedSalary;
            g.Experience = experience;
            g.YearGraduation = yearGraduation;
            g.Faculty = f;
            db.Entry(g).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }
    }
}
