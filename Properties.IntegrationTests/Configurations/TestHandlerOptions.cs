using Microsoft.AspNetCore.Authentication;

namespace Properties.IntegrationTests.Configurations;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string UserName { get; set; } = "testuser123@mail.com";
    public List<string> Roles { get; set; } = [];
}