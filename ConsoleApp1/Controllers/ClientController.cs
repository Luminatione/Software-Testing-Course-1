using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using System;
using System.Collections.Generic;

namespace ConsoleApp1.Controllers
{
    public class ClientController
    {
        private readonly IClientRepository clientRepository;

        public ClientController(IClientRepository clientRepo)
        {
            this.clientRepository = clientRepo;
        }

        public void CreateClient(int id, string name, string secondName, string email)
        {
            Client newClient = new Client(id, name, secondName, email);
            clientRepository.AddClient(newClient);
        }

        public Client? GetClientById(int clientId)
        {
            return clientRepository.GetClientById(clientId);
        }

        public List<Client> GetAllClients()
        {
            return clientRepository.GetAllClients();
        }

        public void UpdateClient(int clientId, string name, string secondName, string email)
        {
            Client existingClient = clientRepository.GetClientById(clientId);
            existingClient.Name = name;
            existingClient.SecondName = secondName;
            existingClient.Email = email;
            clientRepository.UpdateClient(existingClient);
        }

        public void DeleteClient(int clientId)
        {
            clientRepository.DeleteClient(clientId);
        }
    }
}
