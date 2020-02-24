using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using Moq;

using NetCoreV22.Interfaces;

using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace NetCoreV22.Tests
{
    public class WeatherForecastIntegrationTests
    {
        private const string _apiUrl = "api/weatherforecast";
        private readonly IWebHostBuilder _hostBuilder;

        public WeatherForecastIntegrationTests()
        {
            _hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services => services.AddAutofac());
        }

        [Fact]
        public async Task Get_Returns_Weather_Forecast()
        {
            // Arrange
                    _hostBuilder.ConfigureTestContainer<ContainerBuilder>(builder =>
                    {
                        var mockRequestValidationService = new Mock<IRequestValidationService>();
                        mockRequestValidationService.Setup(service =>
                                service.RequestCanBeProcessed())
                            .Returns(true);

                        builder.Register(c => mockRequestValidationService.Object)
                            .As<IRequestValidationService>();
                    });

            var server = new TestServer(_hostBuilder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync(_apiUrl);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Returns_BadRequest()
        {
            // Arrange
                 _hostBuilder.ConfigureTestContainer<ContainerBuilder>(builder =>
                    {
                        var mockRequestValidationService = new Mock<IRequestValidationService>();
                        mockRequestValidationService.Setup(service =>
                                service.RequestCanBeProcessed())
                            .Returns(false);

                        builder.Register(c => mockRequestValidationService.Object)
                            .As<IRequestValidationService>();
                    });

            var server = new TestServer(_hostBuilder);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync(_apiUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
