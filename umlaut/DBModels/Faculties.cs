using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels
{
    public class Faculties
    {
        //Id факультета
        public int Id { get; set; }

        //Факультет
        public string? Faculty { get; set; }

        //Все выпускники конкретного факультета
        public List<Graduate> Graduate { get; set; }
    }
}
