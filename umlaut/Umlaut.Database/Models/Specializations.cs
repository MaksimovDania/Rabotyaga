using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umlaut.Database.Models
{
    public class Specializations
    {
        //Id специализации
        public int Id { get; set; }

        //Специализация
        public string Specialization { get; set; }

        //Все выпускники конкретной специализации
        public List<Graduate> Graduate { get; set; }
    }
}
