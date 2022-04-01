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
		public static ArrayList users = new ArrayList();

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
				{
					// Ako nema parametara u resource, ispisujemo sve registrovane korisnike

					string responseText = "HTTP/1.0 200 OK\r\n\r\n";
					sw.Write(responseText);

					sw.WriteLine("<html><body><ol>");
					foreach (string user in users)
					{
						sw.WriteLine($"<li>{user}</li>\n");
					}
					sw.WriteLine("</ol></body></html>");
				}
				else
				{
					Console.WriteLine("Request from " + socket.RemoteEndPoint + ": "
					+ resource + "\n");

					if (resource.Contains("dodaj?usrname="))
					{
						resource = resource.Substring(6);           // Preskacemo "dodaj?"
						string[] data = resource.Split('&');        // Delimo parametre na delove
						string usrname = data[0].Split('=')[1];     // Iz "usrname=korisnickoIme" izvlacimo samo "korisnickoIme"
						string firstName = data[1].Split('=')[1];   // Isto se radi za firstName
						string lastName = data[2].Split('=')[1];    // i za lastName

						Console.WriteLine($"Username: {usrname}");
						Console.WriteLine($"First name: {firstName}");
						Console.WriteLine($"Last name: {lastName}");

						string responseText = "HTTP/1.0 200 OK\r\n\r\n";
						sw.Write(responseText);

						usrname = Uri.UnescapeDataString(usrname);
						usrname = usrname.Replace("+", " ");

						sw.Write("<html><body>");
						if (users.Contains(usrname))
						{
							sw.Write($"<h1>Korisnik '{usrname}' vec postoji!</h1>");
						}
						else
						{
							users.Add(usrname);
							sw.Write($"<h1>Korisnik '{usrname}' je uspesno dodat!</h1>");
						}
						sw.WriteLine("</body></html>");
					}
					else if (resource.Contains("pronadji?usrname="))
					{
						resource = resource.Substring(9);			// Preskacemo "pronadji?"
						string usrname = resource.Split('=')[1];    // Iz "usrname=korisnickoIme" izvlacimo samo "korisnickoIme"

						Console.WriteLine($"Username: {usrname}");

						string responseText = "HTTP/1.0 200 OK\r\n\r\n";
						sw.Write(responseText);

						usrname = Uri.UnescapeDataString(usrname);
						usrname = usrname.Replace("+", " ");

						sw.Write("<html><body>");
						if (users.Contains(usrname))
						{
							sw.Write($"<h1>Korisnik '{usrname}' je pronadjen!</h1>");
						}
						else
						{
							sw.Write($"<h1>Korisnik '{usrname}' nije pronadjen!</h1>");
						}
						sw.WriteLine("</body></html>");
					}
					else if (resource.Contains("obrisi?usrname="))
					{
						resource = resource.Substring(7);			// Preskacemo "obrisi?"
						string usrname = resource.Split('=')[1];    // Iz "usrname=korisnickoIme" izvlacimo samo "korisnickoIme"

						Console.WriteLine($"Username: {usrname}");

						string responseText = "HTTP/1.0 200 OK\r\n\r\n";
						sw.Write(responseText);

						usrname = Uri.UnescapeDataString(usrname);
						usrname = usrname.Replace("+", " ");

						sw.Write("<html><body>");
						if (users.Contains(usrname))
						{
							users.Remove(usrname);
							sw.Write($"<h1>Korisnik '{usrname}' je uspesno obrisan!</h1>");
						}
						else
						{
							sw.Write($"<h1>Korisnik '{usrname}' nije pronadjen!</h1>");
						}
						sw.WriteLine("</body></html>");
					}
					else
					{
						SendResponse(resource, socket, sw);
					}
				}
			}
			sr.Close();
			sw.Close();
			stream.Close();

			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
			//return 0;
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
			responseText = "HTTP/1.0 200 OK\r\n\r\n";
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
