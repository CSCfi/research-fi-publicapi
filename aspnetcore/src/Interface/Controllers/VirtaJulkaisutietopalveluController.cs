using Microsoft.AspNetCore.Mvc;
using CSC.PublicApi.Interface.Models;
using Microsoft.AspNetCore.Authorization;
using ResearchFi.Query;
using Microsoft.EntityFrameworkCore;
using ResearchFi.VirtaJtp.Duplikaatit;
using ResearchFi.VirtaJtp.Tilanneraportti;
using ResearchFi.VirtaJtp.Virheraportti;
using ResearchFi.VirtaJtp.YhteisjulkaisutRistiriitaiset;

namespace CSC.PublicApi.Interface.Controllers;

[ApiController]
[Route("[controller]")]

public class VirtaJulkaisutietopalveluController : ControllerBase
{
    private const string ApiVersion = "1.0";
    private readonly VirtaJtpDbContext _virtaJtpDbContext;
    private readonly ILogger<VirtaJulkaisutietopalveluController> _logger;

    public VirtaJulkaisutietopalveluController(ILogger<VirtaJulkaisutietopalveluController> logger, VirtaJtpDbContext virtaJtpDbContext)
    {
        _logger = logger;
        _virtaJtpDbContext = virtaJtpDbContext;
    }

    [HttpGet("Latausraportit/duplikaatit")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<DuplikaatitAPI>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async IAsyncEnumerable<DuplikaatitAPI> GetDuplikaatit([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
        ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
        var duplikaatits = _virtaJtpDbContext.Duplikaatits
            .OrderBy(b => b.DuplikaattiId)
            .AsNoTracking();

        if (virtaQueryParameters.organisaatiotunnus != null)
        {
            duplikaatits = duplikaatits.Where(b =>
                b.Organisaatiotunnus == virtaQueryParameters.organisaatiotunnus);
        }
        duplikaatits = duplikaatits
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

        await foreach (var duplikaatti in duplikaatits.AsAsyncEnumerable())
        {
            // Map from DB model to API model
            yield return new DuplikaatitAPI
            {
                DuplikaattiId = duplikaatti.DuplikaattiId,
                Organisaatiotunnus = duplikaatti.Organisaatiotunnus,
                Organisaationimi = duplikaatti.Organisaationimi,
                Julkaisuntunnus = duplikaatti.Julkaisuntunnus,
                Kuvaus = duplikaatti.Kuvaus,
                Julkaisunorgtunnus = duplikaatti.Julkaisunorgtunnus,
                Duplikaattijulkaisunorgtunnus = duplikaatti.Duplikaattijulkaisunorgtunnus,
                Julkaisunnimi = duplikaatti.Julkaisunnimi,
                Duplikaattijulkaisunnimi = duplikaatti.Duplikaattijulkaisunnimi,
                Julkaisutyyppikoodi = duplikaatti.Julkaisutyyppikoodi,
                Tila = duplikaatti.Tila,
                TarkistusId = duplikaatti.TarkistusId,
                Luontipaivamaara = duplikaatti.Luontipaivamaara,
                Ilmoitusvuosi = duplikaatti.Ilmoitusvuosi,
                Julkaisuvuosi = duplikaatti.Julkaisuvuosi
            };
        }
    }


    [HttpGet("Latausraportit/tilanne")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<TilanneraporttiAPI>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async IAsyncEnumerable<TilanneraporttiAPI> Get([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
        ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
        var tilanneraporttis = _virtaJtpDbContext.Tilanneraporttis
            .OrderBy(a => a.TilanneraporttiId)
            .AsNoTracking();

        if (virtaQueryParameters.organisaatiotunnus != null)
        {
            tilanneraporttis = tilanneraporttis.Where(b =>
            b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus);
        }
        tilanneraporttis = tilanneraporttis
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

        await foreach (var tilanneraportti in tilanneraporttis.AsAsyncEnumerable())
        {
            // Map from DB model to API model
            yield return new TilanneraporttiAPI
            {
                TilanneraporttiId = tilanneraportti.TilanneraporttiId,
                Organisaationimi = tilanneraportti.Organisaationimi,
                OrganisaatioTunnus = tilanneraportti.OrganisaatioTunnus,
                JulkaisuVuosi = tilanneraportti.JulkaisuVuosi,
                IlmoitusVuosi = tilanneraportti.IlmoitusVuosi,
                JulkaisunNimi = tilanneraportti.JulkaisunNimi,
                JulkaisuTyyppi = tilanneraportti.JulkaisuTyyppi,
                JulkaisunTila = tilanneraportti.JulkaisunTila,
                JulkaisunTunnus = tilanneraportti.JulkaisunTunnus,
                OrganisaationJulkaisuTunnus = tilanneraportti.OrganisaationJulkaisuTunnus,
                Luontipaivamaara = tilanneraportti.Luontipaivamaara,
                Paivityspaivamaara = tilanneraportti.Paivityspaivamaara,
                JufoTunnus = tilanneraportti.JufoTunnus,
                JufoLuokkaKoodi = tilanneraportti.JufoLuokkaKoodi,
                AvoinSaatavuusJulkaisumaksu = tilanneraportti.AvoinSaatavuusJulkaisumaksu,
                AvoinSaatavuusJulkaisumaksuVuosi = tilanneraportti.AvoinSaatavuusJulkaisumaksuVuosi,
                JulkaisuKanavaOa = tilanneraportti.JulkaisuKanavaOa,
                AvoinSaatavuusKytkin = tilanneraportti.AvoinSaatavuusKytkin,
                LisenssiKoodi = tilanneraportti.LisenssiKoodi,
                MuotoKoodi = tilanneraportti.MuotoKoodi,
                YleisoKoodi = tilanneraportti.YleisoKoodi,
                EmojulkaisuntyyppiKoodi = tilanneraportti.EmojulkaisuntyyppiKoodi,
                ArtikkelityyppiKoodi = tilanneraportti.ArtikkelityyppiKoodi,
                VertaisarvioituKytkin = tilanneraportti.VertaisarvioituKytkin,
                RaporttiKytkin = tilanneraportti.RaporttiKytkin,
                OpinnayteKoodi = tilanneraportti.OpinnayteKoodi,
                TaidetyyppiKoodi = tilanneraportti.TaidetyyppiKoodi,
                AvsovellusTyyppiKoodi = tilanneraportti.AvsovellusTyyppiKoodi,
                RinnakkaistallennettuKytkin = tilanneraportti.RinnakkaistallennettuKytkin
            };
        }
    }

    [HttpGet("Latausraportit/virheet")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<VirheraporttiAPI>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async IAsyncEnumerable<VirheraporttiAPI> GetVirheet([FromQuery] GetVirtaQueryParameters virtaQueryParameters, [FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
        ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
        var virheraporttis = _virtaJtpDbContext.Virheraporttis
            .OrderBy(b => b.VirheraporttiId)
            .AsNoTracking();

        if (virtaQueryParameters.organisaatiotunnus != null)
        {
            virheraporttis = virheraporttis.Where(b =>
                b.OrganisaatioTunnus == virtaQueryParameters.organisaatiotunnus);
        }
        virheraporttis = virheraporttis
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

        await foreach (var virheraportti in virheraporttis.AsAsyncEnumerable())
        {
            // Map from DB model to API model
            yield return new VirheraporttiAPI
            {
                VirheraporttiId = virheraportti.VirheraporttiId,
                OrganisaatioTunnus = virheraportti.OrganisaatioTunnus,
                Nimi = virheraportti.Nimi,
                Kuvaus = virheraportti.Kuvaus,
                JulkaisunOrgTunnus = virheraportti.JulkaisunOrgTunnus,
                JulkaisunNimi = virheraportti.JulkaisunNimi,
                Julkaisutyyppikoodi = virheraportti.Julkaisutyyppikoodi,
                Tila = virheraportti.Tila,
                TarkistusId = virheraportti.TarkistusId,
                Koodi = virheraportti.Koodi,
                Virheviesti = virheraportti.Virheviesti,
                Luontipaivamaara = virheraportti.Luontipaivamaara,
                IlmoitusVuosi = virheraportti.IlmoitusVuosi,
                JulkaisuVuosi = virheraportti.JulkaisuVuosi
            };
        }
    }

    [HttpGet("Yhteisjulkaisut/ristiriitaiset")]
    [Authorize(Policy = ApiPolicies.Publication.Read)]
    [MapToApiVersion(ApiVersion)]
    [Produces(ApiConstants.ContentTypeJson)]
    [Consumes(ApiConstants.ContentTypeJson)]
    [ProducesResponseType(typeof(IEnumerable<YhteisjulkaisutRistiriitaisetApi>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async IAsyncEnumerable<YhteisjulkaisutRistiriitaisetApi> Get([FromQuery] VirtaPaginationQueryParameters queryParameters)
    {
        ResponseHelper.AddVirtaPaginationResponseHeaders(HttpContext, queryParameters.PageNumber, queryParameters.PageSize);
        var YhteisjulkaisutRistiriitaisets = _virtaJtpDbContext.YhteisjulkaisutRistiriitaisets
            .OrderBy(b => b.RrId)
            .AsNoTracking();

        YhteisjulkaisutRistiriitaisets = YhteisjulkaisutRistiriitaisets
            .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize);

        await foreach (var YhteisjulkaisutRistiriitaiset in YhteisjulkaisutRistiriitaisets.AsAsyncEnumerable())
        {
            // Map from DB model to API model
            // Database property "Julkaisutyyppi" is mapped to API property "RistiriitainenTieto"
            yield return new YhteisjulkaisutRistiriitaisetApi
            {
                RrId = YhteisjulkaisutRistiriitaiset.RrId,
                YhteisjulkaisunTunnus = YhteisjulkaisutRistiriitaiset.YhteisjulkaisunTunnus,
                JulkaisunTunnus = YhteisjulkaisutRistiriitaiset.JulkaisunTunnus,
                Organisaationimi = YhteisjulkaisutRistiriitaiset.Organisaationimi,
                OrganisaatioTunnus = YhteisjulkaisutRistiriitaiset.OrganisaatioTunnus,
                JulkaisuVuosi = YhteisjulkaisutRistiriitaiset.JulkaisuVuosi,
                IlmoitusVuosi = YhteisjulkaisutRistiriitaiset.IlmoitusVuosi,
                JulkaisunNimi = YhteisjulkaisutRistiriitaiset.JulkaisunNimi,
                RistiriitainenTieto = YhteisjulkaisutRistiriitaiset.Julkaisutyyppi, // Map from "Julkaisutyyppi" in the DB to "RistiriitainenTieto"
                JulkaisunOrgTunnus = YhteisjulkaisutRistiriitaiset.JulkaisunOrgTunnus,
                LiittyvaJulkaisunOrgTunnus = YhteisjulkaisutRistiriitaiset.LiittyvaJulkaisunOrgTunnus,
                Luontipaivamaara = YhteisjulkaisutRistiriitaiset.Luontipaivamaara,
                Koodi = YhteisjulkaisutRistiriitaiset.Koodi,
                Kuvaus = YhteisjulkaisutRistiriitaiset.Kuvaus,
                Tila = YhteisjulkaisutRistiriitaiset.Tila
            };
        }
    }
}
