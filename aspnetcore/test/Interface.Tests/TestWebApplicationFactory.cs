using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CSC.PublicApi.Interface.Tests;

public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public TestWebApplicationFactory()
    {
        // Provide required config values that are mandatory at startup
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Test");

        // Serilog
        Environment.SetEnvironmentVariable("Serilog__WriteTo__HttpSink__Name", null);
        Environment.SetEnvironmentVariable("Serilog__WriteTo__HttpSink__Args__httpClient", null);
        Environment.SetEnvironmentVariable("Serilog__WriteTo__HttpSink__Args__textFormatter", null);

        // ElasticSearch
        Environment.SetEnvironmentVariable("ElasticSearch__Url", "http://localhost:9200");

        // Keycloak
        Environment.SetEnvironmentVariable("Keycloak__audience", "test-audience");
        Environment.SetEnvironmentVariable("Keycloak__authority", "http://localhost:8080/realms/test-realm");
        Environment.SetEnvironmentVariable("Keycloak__metadataaddress", "http://localhost:8080/realms/test-realm/.well-known/openid-configuration");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(".");
        

        // Mock authentication.
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });

        base.ConfigureWebHost(builder);
    }
}