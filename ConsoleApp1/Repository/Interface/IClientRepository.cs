using ConsoleApp1.Model;

namespace ConsoleApp1.Repository.Interface
{
    public interface IClientRepository
    {
        void AddClient(Client client);
        void DeleteClient(int clientId);
        List<Client> GetAllClients();
        Client GetClientById(int clientId);
        void UpdateClient(Client updatedClient);
    }
}