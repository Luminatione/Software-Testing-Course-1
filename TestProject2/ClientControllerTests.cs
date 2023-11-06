using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using ConsoleApp1.DataBase;
using Moq;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Database;
using ConsoleApp1.Controllers;

namespace ConsoleApp1.Tests
{
    public class ClientControllerTests
    {
        [Fact]
        public void CreateClient_ValidClientAndProducts_AddsClient ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            // Act
            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Assert
            Assert.Equal<Client> (
                new Client (clientId, clientName, clientSecondName, clientEmail),
                detabaseServiecve.GetClientByIdOrNull (clientId)
            );
        }

        [Fact]
        public void CreateClient_ValidClientAndProducts_AddsXlientThatExists ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            // Act
            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);
            Client sameClient = new Client (clientId, clientName, clientSecondName, clientEmail);

            // Assert
            Assert.Throws<ClientAlreadyExistsException> (() => detabaseServiecve.AddClient (sameClient));
        }

        [Fact]
        public void CreateClient_InvalidName_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            string clientName = null;
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            // Act and Assert
            Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException> (
                () => clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail)
            );
        }

        [Fact]
        public void CreateClient_InvalidSecondName_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            string clientSecondName = null;
            var clientEmail = "john.smith@example.mail";

            // Act and Assert
            Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException> (
                () => clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail)
            );
        }

        [Fact]
        public void CreateClient_InvalidEmail_ThrowsArgumentException ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            string clientEmail = null;

            // Act and Assert
            Assert.Throws<Microsoft.EntityFrameworkCore.DbUpdateException> (
                () => clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail)
            );
        }

        [Fact]
        public void ReadClient_ExistingData_GetsAllClient ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);
            List<Client> clientList = new List<Client> ()
            {
                new Client(clientId, clientName, clientSecondName, clientEmail)
            };

            // Act and Assert
            Assert.Equal<List<Client>> (
                Enumerable.Repeat (clientList, 1),
                Enumerable.Repeat (clientController.GetAllClients (), 1)
            );
        }

        [Fact]
        public void ReadClient_ExistingData_GetsById ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act and Assert
            Assert.Equal<Client> (
                new Client (clientId, clientName, clientSecondName, clientEmail),
                detabaseServiecve.GetClientByIdOrNull (clientId)
            );
        }

        [Fact]
        public void ReadClient_NoData_GetsAllClients ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            List<Client> clientList = new List<Client> ();

            // Act and Assert
            Assert.Equal<List<Client>> (
                Enumerable.Repeat (clientList, 1),
                Enumerable.Repeat (clientController.GetAllClients (), 1)
            );
        }

        [Fact]
        public void ReadClient_NotExistingData_GetsById ()
        {
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            // Act and Assert
            Assert.Null (detabaseServiecve.GetOrderByIdOrNull (2));
        }

        [Fact]
        public void UpdateClient_ValidData_UpdatesClient ()
        {
            // Arrange
            DatabaseService detabaseServiecve = new DatabaseService ();
            detabaseServiecve.CelarDatabse ();

            var clientController = new ClientController (detabaseServiecve);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act
            var newName = "Tom";
            clientController.UpdateClient (clientId, newName, clientSecondName, clientEmail);

            // Assert
            Assert.Equal<Client> (
                new Client (clientId, newName, clientSecondName, clientEmail),
                detabaseServiecve.GetClientByIdOrNull (clientId)
            );
        }

        [Fact]
        public void UpdateClient_InvalidDataId_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act and Assert
            Assert.Throws<NullReferenceException> (
                () =>
                    clientController.UpdateClient (
                        99,
                        clientName,
                        clientSecondName,
                        clientEmail
                    )
            );
        }

        [Fact]
        public void UpdateClient_InvalidDataName_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act and Assert
            Assert.Throws<InvalidOperationException> (
                () =>
                    clientController.UpdateClient (
                        clientId,
                        null,
                        clientSecondName,
                        clientEmail
                    )
            );
        }

        [Fact]
        public void UpdateClient_InvalidDataSecondName_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act and Assert
            Assert.Throws<InvalidOperationException> (
                () =>
                    clientController.UpdateClient (
                        clientId,
                        clientName,
                        null,
                        clientEmail
                    )
            );
        }

        [Fact]
        public void UpdateClient_InvalidDataEmail_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act and Assert
            Assert.Throws<InvalidOperationException> (
                () =>
                    clientController.UpdateClient (
                        clientId,
                        clientName,
                        clientSecondName,
                        null
                    )
            );
        }

        [Fact]
        public void DelateClient_ValidClient_DelateClient ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            var clientId = 1;
            var clientName = "John";
            var clientSecondName = "Smith";
            var clientEmail = "john.smith@example.mail";

            clientController.CreateClient (clientId, clientName, clientSecondName, clientEmail);

            // Act:
            clientController.DeleteClient (clientId);

            // Assert:
            var removedOrder = databaseService.GetClientByIdOrNull (clientId);
            Assert.Null (removedOrder);
        }

        [Fact]
        public void DelateClient_InvalidData_ThrowsException ()
        {
            // Arrange
            DatabaseService databaseService = new DatabaseService ();
            databaseService.CelarDatabse ();

            var clientController = new ClientController (databaseService);

            // Act and Assert
            Assert.Throws<ClientNotFoundException> (() => clientController.DeleteClient (1));
        }
    }
}
