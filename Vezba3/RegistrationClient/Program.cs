using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationClient
{
	class Program
	{
		public static void StartClient()
		{
			//Connect to a remote device.
			try
			{
				//Establish the remote endpoint for the socket.
				//This example uses port 11000 on the local computer.
				//IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");//Dns.Resolve(Dns.GetHostName())

				//IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPAddress ipAddress = IPAddress.Loopback;
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

				//Create a TCP/IP socket.
				Socket socket = new Socket(
					AddressFamily.InterNetwork,
					SocketType.Stream,
					ProtocolType.Tcp
				);

				//Connect the socket to the remote endpoint. Catch any errors.
				try
				{
					socket.Connect(remoteEP);

					Console.WriteLine("Socket connected to {0}", socket.RemoteEndPoint.ToString());

					NetworkStream stream = new NetworkStream(socket);
					StreamReader sr = new StreamReader(stream);
					StreamWriter sw = new StreamWriter(stream)
					{
						NewLine = "\r\n",
						AutoFlush = true
					};

					Console.WriteLine("(EXIT / LIST / ADD username,... / REMOVE username / FIND username)");
					while (true)
					{
						Console.Write(">> ");
						string command = Console.ReadLine();

						if (command.ToUpper().StartsWith("EXIT"))
						{
							sw.WriteLine(command);
							break;
						}
						else if (command.ToUpper().StartsWith("ADD"))
						{
							sw.WriteLine(command);
							while (true)
							{
								string response = sr.ReadLine();
								if (response == "END")
								{
									break;
								}
								Console.WriteLine(response);
							}
						}
						else if (command.ToUpper().StartsWith("LIST"))
						{
							sw.WriteLine(command);
							while (true)
							{
								string response = sr.ReadLine();
								if (response == "END")
								{
									break;
								}
								Console.WriteLine(response);
							}
						}
						else if (command.ToUpper().StartsWith("REMOVE"))
						{
							sw.WriteLine(command);
							Console.WriteLine(sr.ReadLine());
						}
						else if (command.ToUpper().StartsWith("FIND"))
						{
							sw.WriteLine(command);
							Console.WriteLine(sr.ReadLine());
						}
					}

					//Release the socket.
					sr.Close();
					sw.Close();
					stream.Close();
					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
				}
				catch (ArgumentNullException ane)
				{
					Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
				}
				catch (SocketException se)
				{
					Console.WriteLine("SocketException : {0}", se.ToString());
				}
				catch (Exception e)
				{
					Console.WriteLine("Unexpected exception : {0}", e.ToString());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		static void Main(string[] args)
		{
			StartClient();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}
	}
}
