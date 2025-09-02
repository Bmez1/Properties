using Newtonsoft.Json.Linq;

using Properties.Application.UseCases.Owners.Dtos;
using Properties.IntegrationTests.Configurations;

using System.Net;

namespace Properties.IntegrationTests;

public class OwnerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public OwnerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOwners()
    {
        var client = _factory.CreateAuthenticatedClient();

        // Act
        var response = await client.GetAsync("api/v1/owners");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        JObject jObj = JObject.Parse(content);

        var owners = jObj["data"]!.ToObject<List<OwnerResponseDto>>() ?? [];
        var totalData = jObj["totalData"]?.ToObject<int>();

        // Assert
        Assert.NotEmpty(owners);
        Assert.True(totalData >= 2);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
