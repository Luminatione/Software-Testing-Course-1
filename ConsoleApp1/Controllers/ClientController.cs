using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using Database;
using System;
using System.Collections.Generic;

namespace ConsoleApp1.Controllers
{
    public class ClientController
    {
        private readonly IDatabaseService context;

        public ClientController(IDatabaseService context)
        {
            this.context = context;
        }

        public void CreateClient(int id, string name, string secondName, string email)
        {
            Client newClient = new Client(id, name, secondName, email);
            context.AddClient(newClient);
        }

        public Client? GetClientById(int clientId)
        {
            return context.GetClientByIdOrNull(clientId);
        }

        public List<Client> GetAllClients()
        {
            return context.GetAllClients();
        }

        public void UpdateClient(int clientId, string name, string secondName, string email)
        {
            Client existingClient = GetClientById(clientId);
            existingClient.Name = name;
            existingClient.SecondName = secondName;
            existingClient.Email = email;
            context.UpdateClient(existingClient);
        }

        public void DeleteClient(int clientId)
        {
            context.RemoveClient(clientId);
        }
    }
}
