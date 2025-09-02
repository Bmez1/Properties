using Properties.Api.Endpoints.Users;
using Properties.Domain.Errors;
using Properties.IntegrationTests.Configurations;

using System.Net;
using System.Net.Http.Json;

namespace Properties.IntegrationTests;

public class UserTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public UserTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UnregisteredUserLogin()
    {

        var client = _factory.CreateClient();
        var request = new Login.Request("testuser123@mail.com", "12345678");
        var msjExpected = UserError.NotFoundByEmail.Description;

        // Act
        var response = await client.PostAsJsonAsync("api/v1/users/login", request);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains(msjExpected, content);
    }

    [Fact]
    public async Task SuccessfulUserRegistration()
    {

        var client = _factory.CreateClient();
        var request = new Login.Request("testuser123@mail.com", "12345678");

        // Act
        var response = await client.PostAsJsonAsync("api/v1/users", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ExistingUserRegistration()
    {

        var client = _factory.CreateClient();
        var request = new Login.Request("mail@mail.com", "12345678");
        var msjExpected = UserError.EmailNotUnique.Description;

        // Act
        var response = await client.PostAsJsonAsync("api/v1/users", request);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Contains(msjExpected, content);
    }

    [Fact]
    public async Task RequestWithoutAuthentication()
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/owners");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}