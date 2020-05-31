using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConnectingApps.IntegrationFixture;
using ConnectingApps.MultiDelegatingHandler.Controllers;
using Microsoft.AspNetCore.Mvc;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ConnectingApps.MultiDelegatingHandler.Integrationtests
{
    public class SearchEngineControllerTests
    {
        [Fact]
        public async Task TestDelegate()
        {
            // arrange
            await using (var fixture = new Fixture<Startup>())
            {
                using (var searchEngineServer = fixture.FreezeServer("Google"))
                {
                    SetupUnStableServer(searchEngineServer, "Response");
                    var controller = fixture.Create<SearchEngineController>();

                    // act
                    var response = await controller.GetNumberOfCharacters("Hoi");

                    // assert, external
                    var externalResponseMessages = searchEngineServer.LogEntries.Select(l => l.ResponseMessage).ToList();
                    Assert.Equal(2, externalResponseMessages.Count);
                    Assert.Equal((int)HttpStatusCode.InternalServerError, externalResponseMessages.First().StatusCode);
                    Assert.Equal((int)HttpStatusCode.OK, externalResponseMessages.Last().StatusCode);


                    // assert, internal
                    var loggedResponse = fixture.LogSource.GetLoggedObjects<HttpResponseMessage>().ToList();
                    Assert.Single(loggedResponse);
                    var externalResponseContent = await loggedResponse.Single().Value.Content.ReadAsStringAsync();
                    Assert.Equal("Response", externalResponseContent);
                    Assert.Equal(HttpStatusCode.OK, loggedResponse.Single().Value.StatusCode);
                    Assert.Equal(8, ((OkObjectResult)response.Result).Value);
                }
            }
        }

        private void SetupUnStableServer(FluentMockServer fluentMockServer, string response)
        {
            fluentMockServer.Given(Request.Create().UsingGet())
                .InScenario("UnstableServer")
                .WillSetStateTo("FIRSTCALLDONE")
                .RespondWith(Response.Create().WithBody(response, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.InternalServerError));

            fluentMockServer.Given(Request.Create().UsingGet())
                .InScenario("UnstableServer")
                .WhenStateIs("FIRSTCALLDONE")
                .RespondWith(Response.Create().WithBody(response, encoding: Encoding.UTF8)
                    .WithStatusCode(HttpStatusCode.OK));
        }

    }
}
