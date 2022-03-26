using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationServer
{
	class Program
	{
		public static ArrayList users = new ArrayList();

		public static void StartListening()
		{
			//Establish the local endpoint for the socket.
			//Dns.GetHostName returns the name of the host running the application.
			//IPHostEntry ipHostInfo = Dns.GetHostEntry("192.168.1.208");//Dns.Resolve(Dns.GetHostName());
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());

			//IPAddress ipAddress = ipHostInfo.AddressList[8];
			IPAddress ipAddress = IPAddress.Loopback; //ipHostInfo.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

			//Create a TCP/IP socket.
			Socket serverSocket = new Socket(
				ipAddress.AddressFamily,//AddressFamily.InterNetwork
				SocketType.Stream,
				ProtocolType.Tcp
			);

			// Bind the socket to the local endpoint and listen for incoming connections.
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

					Task<int> t = Task.Factory.StartNew(() => Run(socket));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			Console.WriteLine("\nPress ENTER to continue...");
			Console.ReadLine();
		}

		private static int Run(Socket socket)
		{
			NetworkStream stream = new NetworkStream(socket);
			StreamReader sr = new StreamReader(stream);
			StreamWriter sw = new StreamWriter(stream)
			{
				NewLine = "\r\n",
				AutoFlush = true
			};

			while (true)
			{
				Console.WriteLine("Receiving data...");
				string line = sr.ReadLine();
				Console.WriteLine("Stiglo od klijenta: {0}", line);

				if (line.ToUpper().StartsWith("EXIT"))
				{
					break;
				}
				else if (line.ToUpper().StartsWith("ADD"))
				{
					string[] usernames = line.Substring(line.IndexOf(' ') + 1).Split(',');
					foreach (string username in usernames)
					{
						if (username.Trim().Length > 0)
						{
							if (!users.Contains(username.Trim()))
							{
								users.Add(username.Trim());
								sw.WriteLine("KORISNIK '{0}' JE USPESNO REGISTROVAN!", username.Trim());
							}
							else
							{
								sw.WriteLine("KORISNIK '{0}' VEC POSTOJI!", username.Trim());
							}
						}
					}
					sw.WriteLine("END");
				}
				else if (line.ToUpper().StartsWith("LIST"))
				{
					foreach (string user in users)
					{
						sw.WriteLine(user);
					}
					sw.WriteLine("END");
				}
				else if (line.ToUpper().StartsWith("REMOVE"))
				{
					string username = line.Split(' ')[1].Trim();
					if (users.Contains(username))
					{
						users.Remove(username);
						sw.WriteLine("KORISNIK '{0}' JE OBRISAN!", username);
					}
					else
					{
						sw.WriteLine("KORISNIK '{0}' NE POSTOJI!", username);
					}
				}
				else if (line.ToUpper().StartsWith("FIND"))
				{
					string username = line.Split(' ')[1].Trim();
					if (users.Contains(username))
					{
						sw.WriteLine("KORISNIK '{0}' JE REGISTROVAN!", username);
					}
					else
					{
						sw.WriteLine("KORISNIK '{0}' NE POSTOJI!", username);
					}
				}
			}

			sr.Close();
			sw.Close();
			stream.Close();

			socket.Shutdown(SocketShutdown.Both);
			socket.Close();
			return 0;
		}

		static void Main(string[] args)
		{
			StartListening();
		}
	}
}
