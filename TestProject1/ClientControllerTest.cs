using ConsoleApp1.Controllers;
using ConsoleApp1.DataBase;
using ConsoleApp1.Model;
using Moq;

namespace ConsoleApp1.Controllers.Tests
{
    [TestClass]
    public class ClientControllerTest
    {
        [TestMethod]
        public void CreateClient_AddsClientToDatabase ()
        {
            // Arrange
            int clientId = 1;
            string name = "John";
            string secondName = "Doe";
            string email = "john.doe@example.com";

            var mockDatabaseService = new Mock<IDatabaseService> ();
            var clientController = new ClientController (mockDatabaseService.Object);

            // Act
            clientController.CreateClient (clientId, name, secondName, email);

            // Assert
            mockDatabaseService.Verify (db => db.AddClient (It.IsAny<Client> ()), Times.Once);
        }

        [TestMethod]
        public void GetClientById_ReturnsClientFromDatabase ()
        {
            // Arrange
            int clientId = 1;
            var expectedClient = new Client (clientId, "John", "Doe", "john.doe@example.com");

            var mockDatabaseService = new Mock<IDatabaseService> ();
            mockDatabaseService.Setup (db => db.GetClientByIdOrNull (clientId)).Returns (expectedClient);
            var clientController = new ClientController (mockDatabaseService.Object);

            // Act
            var result = clientController.GetClientById (clientId);

            // Assert
            Assert.AreEqual (expectedClient, result);
        }

        [TestMethod]
        public void GetAllClients_ReturnsListOfClients ()
        {
            // Arrange
            var expectedClients = new List<Client>
        {
            new Client(1, "John", "Doe", "john.doe@example.com"),
            new Client(2, "Jane", "Smith", "jane.smith@example.com"),
        };

            var mockDatabaseService = new Mock<IDatabaseService> ();
            mockDatabaseService.Setup (db => db.GetAllClients ()).Returns (expectedClients);
            var clientController = new ClientController (mockDatabaseService.Object);

            // Act
            var result = clientController.GetAllClients ();

            // Assert
            CollectionAssert.AreEqual (expectedClients, result);
        }

        [TestMethod]
        public void UpdateClient_UpdatesClientInDatabase ()
        {
            // Arrange
            int clientId = 1;
            string name = "UpdatedName";
            string secondName = "UpdatedSecondName";
            string email = "updated.email@example.com";

            var mockDatabaseService = new Mock<IDatabaseService> ();
            var existingClient = new Client (clientId, "OriginalName", "OriginalSecondName", "original.email@example.com");
            mockDatabaseService.Setup (db => db.GetClientByIdOrNull (clientId)).Returns (existingClient);
            var clientController = new ClientController (mockDatabaseService.Object);

            // Act
            clientController.UpdateClient (clientId, name, secondName, email);

            // Assert
            mockDatabaseService.Verify (db => db.UpdateClient (It.IsAny<Client> ()), Times.Once);
            Assert.AreEqual (name, existingClient.Name);
            Assert.AreEqual (secondName, existingClient.SecondName);
            Assert.AreEqual (email, existingClient.Email);
        }

        [TestMethod]
        public void DeleteClient_RemovesClientFromDatabase ()
        {
            // Arrange
            int clientId = 1;

            var mockDatabaseService = new Mock<IDatabaseService> ();
            var clientController = new ClientController (mockDatabaseService.Object);

            // Act
            clientController.DeleteClient (clientId);

            // Assert
            mockDatabaseService.Verify (db => db.RemoveClient (clientId), Times.Once);
        }
    }
}
