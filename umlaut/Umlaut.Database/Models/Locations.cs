using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umlaut.Database.Models
{
    public class Locations
    {
        //Id города
        public int Id { get; set; }

        //Город
        public string Location { get; set; }

        //Все выпускники в конкретном городе
        public List<Graduate> Graduate { get; set; }
    }
}
