using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CSC.PublicApi.Interface;
using CSC.PublicApi.Interface.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace CSC.PublicApi.Interface.Tests;

public class VirtaHttpContractTest
{
    [Fact]
    public async Task VirtaRoutes_ReturnPopulatedMappedRecords()
    {
        var databaseName = $"virta-contract-{Guid.NewGuid():N}";
        using var factory = new TestWebApplicationFactory<Program>
        {
            AuthenticationMode = TestAuthenticationMode.AuthenticatedWithRoles,
            Roles = new[] { ApiPolicies.PolicyRoleMap[ApiPolicies.Publication.Read] }
        };
        using var host = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<VirtaJtpDbContext>();
            services.AddScoped(_ => new VirtaJtpDbContext(
                new DbContextOptionsBuilder<VirtaJtpDbContext>()
                    .UseInMemoryDatabase(databaseName)
                    .Options));
        }));

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<VirtaJtpDbContext>();
            db.Duplikaatits.Add(new Duplikaatit
            {
                DuplikaattiId = 11,
                Organisaatiotunnus = "org-1",
                Organisaationimi = "Synthetic organization",
                Julkaisuntunnus = "duplicate-publication",
                Julkaisunorgtunnus = "org-publication-1",
                Duplikaattijulkaisunorgtunnus = "org-publication-2",
                Julkaisunnimi = "Original title",
                Duplikaattijulkaisunnimi = "Duplicate title"
            });
            db.Tilanneraporttis.Add(new Tilanneraportti
            {
                TilanneraporttiId = 12,
                OrganisaatioTunnus = "org-1",
                Organisaationimi = "Synthetic organization",
                JulkaisuVuosi = 2024,
                JulkaisunTunnus = "status-publication",
                OrganisaationJulkaisuTunnus = "local-status-publication",
                JulkaisuTyyppi = "A1"
            });
            db.Virheraporttis.Add(new Virheraportti
            {
                VirheraporttiId = 13,
                OrganisaatioTunnus = "org-1",
                Nimi = "Synthetic organization",
                TarkistusId = 130,
                Koodi = "invalid-title",
                Virheviesti = "Synthetic validation message",
                JulkaisunNimi = "Publication with an error"
            });
            db.YhteisjulkaisutRistiriitaisets.Add(new YhteisjulkaisutRistiriitaiset
            {
                RrId = 14,
                YhteisjulkaisunTunnus = "joint-publication",
                JulkaisunTunnus = "publication",
                OrganisaatioTunnus = "org-1",
                JulkaisunNimi = "Conflicting publication",
                Julkaisutyyppi = "type-conflict"
            });
            await db.SaveChangesAsync();
        }

        using var client = host.CreateClient();
        var duplicates = await GetFirstRecordAsync(
            client,
            "/VirtaJulkaisutietopalvelu/Latausraportit/duplikaatit?organisaatiotunnus=org-1");
        Assert.Equal(11, duplicates.GetProperty("duplikaattiId").GetInt64());
        Assert.Equal("duplicate-publication", duplicates.GetProperty("julkaisuntunnus").GetString());
        Assert.Equal("Duplicate title", duplicates.GetProperty("duplikaattijulkaisunnimi").GetString());

        var status = await GetFirstRecordAsync(
            client,
            "/VirtaJulkaisutietopalvelu/Latausraportit/tilanne?organisaatiotunnus=org-1");
        Assert.Equal(2024, status.GetProperty("julkaisuVuosi").GetInt32());
        Assert.Equal("local-status-publication", status.GetProperty("organisaationJulkaisuTunnus").GetString());
        Assert.Equal("A1", status.GetProperty("julkaisuTyyppi").GetString());

        var errors = await GetFirstRecordAsync(
            client,
            "/VirtaJulkaisutietopalvelu/Latausraportit/virheet?organisaatiotunnus=org-1");
        Assert.Equal(13, errors.GetProperty("virheraporttiId").GetInt64());
        Assert.Equal("invalid-title", errors.GetProperty("koodi").GetString());
        Assert.Equal("Synthetic validation message", errors.GetProperty("virheviesti").GetString());

        var conflicts = await GetFirstRecordAsync(
            client,
            "/VirtaJulkaisutietopalvelu/Yhteisjulkaisut/ristiriitaiset");
        Assert.Equal("joint-publication", conflicts.GetProperty("yhteisjulkaisunTunnus").GetString());
        Assert.Equal("type-conflict", conflicts.GetProperty("ristiriitainenTieto").GetString());
        Assert.False(conflicts.TryGetProperty("julkaisutyyppi", out _));
    }

    private static async Task<JsonElement> GetFirstRecordAsync(HttpClient client, string path)
    {
        using var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.Equal(1, document.RootElement.GetArrayLength());
        return document.RootElement[0].Clone();
    }
}
