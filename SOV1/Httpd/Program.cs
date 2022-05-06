using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Httpd
{
    class Program
    {
        public static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();

        public static void StartListening()
        {
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Create a TCP/IP socket.
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket socket = serverSocket.Accept();

                    Task t = Task.Factory.StartNew(() => Run(socket));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private static void Run(Socket socket)
        {

            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };

            string resource = GetResource(sr);
            if (resource != null)
            {
                if (resource.Equals(""))
                    resource = "index.html";

                Console.WriteLine("Request from " + socket.RemoteEndPoint + ": "
                        + resource + "\n");

                // TODO: Add your code here...
                if (resource.StartsWith("index.html"))
                {
                    SendResponse(resource, socket, sw);
                }
                else if (resource.StartsWith("prijava"))
                {
                    string[] podaci = resource.Split(new string[] { "ime=", "prezime=", "jmbg=", "tipVakcine=", "prvaDoza=" }, StringSplitOptions.None);

                    string ime = podaci[1].Split('&')[0];
                    string prezime = podaci[2].Split('&')[0];
                    string jmbg = podaci[3].Split('&')[0];
                    string tipVakcine = podaci[4].Split('&')[0].Replace("+", " ");
                    bool prvaDoza = podaci[5].Split('&')[0] == "on";

                    string responseText = "HTTP/1.0 200 OK\r\n\r\n";
                    sw.Write(responseText);

                    sw.Write("<html><body>");

                    if (korisnici.ContainsKey(jmbg))
                    {
                        sw.Write("<h1>Korisnik sa unetim JMBG je vec prijavljen!</h1>");
                    }
                    else
                    {
                        korisnici.Add(jmbg, new Korisnik(ime, prezime, jmbg, tipVakcine, prvaDoza));

                        sw.Write("<table border=\"black\">");
                        sw.Write("<tr><th colspan=\"5\">Spisak prijavljenih korisnika</th></tr>");
                        sw.Write("<tr><td>JMBG</td><td>Ime</td><td>Prezime</td><td>Tip vakcine</td><td>Prva doza?</td></tr>");

                        foreach (Korisnik korisnik in korisnici.Values)
                        {
                            string pDoza = korisnik.PrvaDoza ? "Da" : "Ne";

                            sw.Write($"<tr><td>{korisnik.Jmbg}</td><td>{korisnik.Ime}</td><td>{korisnik.Prezime}</td><td>{korisnik.TipVakcine}</td><td>{pDoza}</td></tr>");
                        }

                        sw.Write("</table>");
                    }

                    sw.Write("<a href=\"http://localhost:8080/\">Nazad</a>");

                    sw.Write("</body></html>");
                }
                else if (resource.StartsWith("pretraga"))
                {
                    string[] podaci = resource.Split(new string[] { "prezime=" }, StringSplitOptions.None);
                    
                    string prezime = podaci[1];

                    string responseText = "HTTP/1.0 200 OK\r\n\r\n";
                    sw.Write(responseText);

                    sw.Write("<html><body>");

                    sw.Write("<table border=\"black\">");
                    sw.Write("<tr><td>JMBG</td><td>Ime</td><td>Prezime</td><td>Tip vakcine</td><td>Prva doza?</td></tr>");

                    foreach (Korisnik korisnik in korisnici.Values)
                    {
                        if (korisnik.Prezime.Contains(prezime))
                        {
                            string pDoza = korisnik.PrvaDoza ? "Da" : "Ne";

                            sw.Write($"<tr><td>{korisnik.Jmbg}</td><td>{korisnik.Ime}</td><td>{korisnik.Prezime}</td><td>{korisnik.TipVakcine}</td><td>{pDoza}</td></tr>");
                        }
                    }

                    sw.Write("</table>");

                    sw.Write("<a href=\"http://localhost:8080/\">Nazad</a>");

                    sw.Write("</body></html>");
                }
                else
                {
                    SendResponse(resource, socket, sw);
                }
            }
            sr.Close();
            sw.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            //return 0;
        }

        private static string GetPropertyValue(string field)
        {
            var newField = field.Split('&')[0];
            newField = Uri.UnescapeDataString(newField);
            newField = newField.Replace("+", " ");

            return newField;
        }

        private static string GetResource(StreamReader sr)
        {
            string line = sr.ReadLine();

            if (line == null)
                return null;

            String[] tokens = line.Split(' ');

            // prva linija HTTP zahteva: METOD /resurs HTTP/verzija
            // obradjujemo samo GET metodu
            string method = tokens[0];
            if (!method.Equals("GET"))
            {
                return null;
            }

            string rsrc = tokens[1];

            // izbacimo znak '/' sa pocetka
            rsrc = rsrc.Substring(1);

            // ignorisemo ostatak zaglavlja
            string s1;
            while (!(s1 = sr.ReadLine()).Equals(""))
                Console.WriteLine(s1);
            Console.WriteLine("Request: " + line);
            return rsrc;
        }

        private static void SendResponse(string resource, Socket socket, StreamWriter sw)
        {
            // ako u resource-u imamo bilo šta što nije slovo ili cifra, možemo da
            // konvertujemo u "normalan" oblik
            //resource = Uri.UnescapeDataString(resource);

            // pripremimo putanju do našeg web root-a
            resource = "../../../" + resource;
            FileInfo fi = new FileInfo(resource);

            string responseText;
            if (!fi.Exists)
            {
                // ako datoteka ne postoji, vratimo kod za gresku
                responseText = "HTTP/1.0 404 File not found\r\n"
                        + "Content-type: text/html; charset=UTF-8\r\n\r\n<b>404 Нисам нашао:"
                        + fi.Name + "</b>";
                sw.Write(responseText);
                Console.WriteLine("Could not find resource: " + fi.Name);
                return;
            }

            // ispisemo zaglavlje HTTP odgovora
            responseText = "HTTP/1.0 200 OK\r\nContent-type: text/html; charset=UTF-8\r\n\r\n";
            sw.Write(responseText);

            // a, zatim datoteku
            socket.SendFile(resource);
        }

        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}
