using ConsoleApp1.Model;
using ConsoleApp1.Repository.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace ConsoleApp1.Repository.Tests
{
	[TestClass]
	public class ClientRepositoryTests
	{
		[TestMethod]
		[DataRow(1, "John", "Doe", "john.doe@example.com")]
		[DataRow(1, "Dave", "Ro", "dave.ro@example.com")]
		[DataRow(1, "Mark", "Jo", "mark.jojo@example.com")]
		public void AddClient_ValidClient_AddsClient(int id, string name, string secondName, string email)
		{
			// Arrange
			var clientRepository = new ClientRepository();
			var clientToAdd = new Client(id, name, secondName, email);

			// Act
			clientRepository.AddClient(clientToAdd);

			// Assert
			CollectionAssert.Contains(clientRepository.GetAllClients(), clientToAdd);
		}

		[TestMethod]
		public void GetAllClients_EmptyRepository_ReturnsEmptyList()
		{
			// Arrange
			var clientRepository = new ClientRepository();

			// Act
			var result = clientRepository.GetAllClients();

			// Assert
			CollectionAssert.AreEqual(new List<Client>(), result);
		}

		[TestMethod]
		public void GetClientById_ExistingClient_ReturnsClient()
		{
			// Arrange
			var clientRepository = new ClientRepository();
			var clientToAdd = new Client(1, "John", "Doe", "john.doe@example.com");
			clientRepository.AddClient(clientToAdd);

			// Act
			var result = clientRepository.GetClientById(1);

			// Assert
			result.Should().Be(clientToAdd);
		}

		[TestMethod]
		public void GetClientById_NonexistentClient_ReturnsNull()
		{
			// Arrange
			var clientRepository = new ClientRepository();

			// Act
			var result = clientRepository.GetClientById(1);

			// Assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void UpdateClient_ExistingClient_UpdatesClient()
		{
			// Arrange
			var clientRepository = new ClientRepository();
			var existingClient = new Client(1, "John", "Doe", "john.doe@example.com");
			clientRepository.AddClient(existingClient);

			var updatedClient = new Client(1, "Jane", "Dow", "jane.dow@example.com");

			// Act
			clientRepository.UpdateClient(updatedClient);
			Console.WriteLine($"Updated client");

			// Assert
			var result = clientRepository.GetClientById(1);
			Assert.AreEqual(updatedClient.Name, result.Name);
			Assert.AreEqual(updatedClient.Email, result.Email);
			Assert.AreEqual(updatedClient.SecondName, result.SecondName);
		}

		[TestMethod]
		public void UpdateClient_NonexistentClient_ThrowsInvalidOperationException()
		{
			// Arrange
			var clientRepository = new ClientRepository();
			var updatedClient = new Client(1, "Jane", "Doe", "jane.doe@example.com");

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => clientRepository.UpdateClient(updatedClient));
		}

		[TestMethod]
		public void DeleteClient_ExistingClient_RemovesClient()
		{
			// Arrange
			var clientRepository = new ClientRepository();
			var clientToRemove = new Client(1, "John", "Doe", "john.doe@example.com");
			clientRepository.AddClient(clientToRemove);

			// Act
			clientRepository.DeleteClient(1);

			// Assert
			CollectionAssert.DoesNotContain(clientRepository.GetAllClients(), clientToRemove);
		}

		[TestMethod]
		public void DeleteClient_NonexistentClient_ThrowsInvalidOperationException()
		{
			// Arrange
			var clientRepository = new ClientRepository();

			// Act and Assert
			Assert.ThrowsException<InvalidOperationException>(() => clientRepository.DeleteClient(1));
		}
	}
}
