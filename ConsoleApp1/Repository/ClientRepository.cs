using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;

namespace ConsoleApp1.Repository
{
    public class ClientRepository : IClientRepository
	{
		private List<Client> clients;

		public ClientRepository()
		{
			clients = new List<Client>();
		}

		public void AddClient(Client client)
		{
			clients.Add(client);
		}

		public List<Client> GetAllClients()
		{
			return clients.ToList();
		}

		public Client? GetClientById(int clientId)
		{
			return clients.FirstOrDefault(client => client.Id == clientId);
		}

		public void UpdateClient(Client updatedClient)
		{
			Client existingClient = clients.FirstOrDefault(client => client.Id == updatedClient.Id);

			if (existingClient != null)
			{
				existingClient.Name = updatedClient.Name;
				existingClient.SecondName = updatedClient.SecondName;
				existingClient.Email = updatedClient.Email;
			}
			else
			{
				throw new InvalidOperationException("Client not found");
			}
		}

		public void DeleteClient(int clientId)
		{
			int index = clients.FindIndex(client => client.Id == clientId);

			if (index != -1)
			{
				clients.RemoveAt(index);
			}
			else
			{
				throw new InvalidOperationException("Client not found");
			}
		}
	}
}
