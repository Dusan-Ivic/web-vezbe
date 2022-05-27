using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOV2.Models
{
    public class Avion
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public int BrojSedista { get; set; }
        public string RasponKrila { get; set; }
        public string Proizvodjac { get; set; }
        public string PrekookeanskiLet { get; set; }

        public Avion()
        {

        }
    }
}