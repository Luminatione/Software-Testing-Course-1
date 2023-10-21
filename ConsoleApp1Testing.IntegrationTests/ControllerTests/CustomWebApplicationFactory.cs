using ConsoleApp1.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1Testing.IntegrationTests.ControllerTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock <IOrderRepository> OrderRepositoryMock { get; }

        public CustomWebApplicationFactory ()
        {
            OrderRepositoryMock = new Mock<IOrderRepository> ();
        }

        protected override void ConfigureWebHost (IWebHostBuilder builder)
        {
            base.ConfigureWebHost (builder);

            builder.ConfigureTestServices (Services =>
            {
                Services.AddSingleton (OrderRepositoryMock.Object);
            });
        }
    }
}
