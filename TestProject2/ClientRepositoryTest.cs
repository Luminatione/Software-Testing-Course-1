using ConsoleApp1.Model;
using ConsoleApp1.Repository;
using Newtonsoft.Json;
using System.Collections;
using Xunit.Abstractions;

namespace TestProject2
{
    public class ClientRepositoryTest
    {
        private readonly ITestOutputHelper output;

        public ClientRepositoryTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        public class ClientDataLoader : IEnumerable<object[]>
        {
            private const string testDataJsonPath = "Data/data.json";

            public IEnumerator<object[]> GetEnumerator()
            {
                string cd = Directory.GetCurrentDirectory();
                string jsonPath = Path.GetRelativePath(cd, testDataJsonPath);
                if (!File.Exists(jsonPath))
                {
                    throw new FileNotFoundException(jsonPath);
                }
                string content = File.ReadAllText(jsonPath);
                var data = JsonConvert.DeserializeObject<IEnumerable<Client>>(content);
                var result = new List<object[]>();
                for (var client = data.GetEnumerator(); client.MoveNext();)
                {
                    var origin = client.Current;
                    client.MoveNext();
                    result.Add(new[] { origin, client.Current });
                }
                return result.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
       

        public static IEnumerable<object[]> GetClients()
        {
            yield return new[] { new Client(1, "John", "Doe", "john.doe@example.com") };
            yield return new[] { new Client(1, "Dave", "Ro", "dave.ro@example.com") };
            yield return new[] { new Client(1, "Mark", "Jo", "mark.jojo@example.com") };
        }

		[MemberData(nameof(GetClients))]
        [Theory]
        public void AddClient_ValidClient_AddsClient(Client client)
        {
            // Arrange
            var clientRepository = new ClientRepository();

            // Act
            clientRepository.AddClient(client);
            output.WriteLine("Adding client to repositry");

            // Assert
            Assert.Contains(client, clientRepository.GetAllClients());
        }

        [Theory]
        [ClassData(typeof(ClientDataLoader))]
        public void UpdateClient_ValidClient_UpdatesClient(Client client, Client updated)
        {
            // Arrange
            var clientRepository = new ClientRepository();
            clientRepository.AddClient(client);

            // Act
            clientRepository.UpdateClient(updated);
            output.WriteLine("Updating client in repositry");

            // Assert
            var result = clientRepository.GetClientById(client.Id);
            Assert.Equal(updated.Name, result.Name);
            Assert.Equal(updated.Email, result.Email);
            Assert.Equal(updated.SecondName, result.SecondName);
        }
    }
}