using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Httpd
{
    public class Korisnik
    {
        public Korisnik(string ime, string prezime, string jmbg, string tipVakcine, bool prvaDoza)
        {
            Ime = ime;
            Prezime = prezime;
            Jmbg = jmbg;
            TipVakcine = tipVakcine;
            PrvaDoza = prvaDoza;
        }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Jmbg { get; set; }
        public string TipVakcine { get; set; }
        public bool PrvaDoza { get; set; }
    }
}
