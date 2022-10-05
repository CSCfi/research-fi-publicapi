using AutoMapper;
using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Interface.Models;
using CSC.PublicApi.Interface.Models.Publication;

namespace CSC.PublicApi.Interface.Maps;

public class PublicationProfile : Profile
{
    public PublicationProfile()
    {
        CreateMap<GetPublicationsQueryParameters, PublicationSearchParameters>();
        CreateMap<Service.Models.Publication.Publication, Publication>();
        CreateMap<Service.Models.Publication.Author, Author>();
        CreateMap<Service.Models.Publication.FieldOfEducation, FieldOfEducation>();
        CreateMap<Service.Models.Publication.FieldOfScience, FieldOfScience>();
        CreateMap<Service.Models.Publication.Organization, Organization>();
        CreateMap<Service.Models.Publication.OrganizationUnit, OrganizationUnit>();
        CreateMap<Service.Models.Publication.ParentPublicationType, ParentPublicationType>();
        CreateMap<Service.Models.Publication.PeerReviewed, PeerReviewed>();
        CreateMap<Service.Models.Publication.Person, Person>();
        CreateMap<Service.Models.Publication.SelfArchived, SelfArchived>();
        CreateMap<Service.Models.Publication.SelfArchivedData, SelfArchivedData>();
    }
}