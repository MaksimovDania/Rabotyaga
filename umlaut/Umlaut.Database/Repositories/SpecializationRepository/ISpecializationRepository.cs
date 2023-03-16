using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umlaut.Database.Models;

namespace Umlaut.Database.Repositories.SpecializationRepository
{
    public interface ISpecializationRepository
    {
        IEnumerable<Specializations> GetSpecializationsList();
        void CreateSpecialization(Specializations specialization);
        void DeleteSpecialization(string specialization);
    }
}
