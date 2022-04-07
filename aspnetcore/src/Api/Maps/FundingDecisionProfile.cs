using Api.Models.Entities;
using Api.Models.FundingDecision;
using AutoMapper;

namespace Api.Maps
{
    public class FundingDecisionProfile : Profile
    {
        public FundingDecisionProfile()
        {
            CreateProjection<DimFundingDecision, FundingDecision>(MemberList.None)
                .ForMember(dst => dst.Id, conf => conf.MapFrom(src => src.Id))
                .ForMember(dst => dst.CallProgramme, conf => conf.MapFrom(src => src.DimCallProgramme))
                .ForMember(dst => dst.ApprovalDate, conf => conf.MapFrom(src => src.DimDateIdApprovalNavigation))
                .ForMember(dst => dst.FundingStartDate, conf => conf.MapFrom(src => src.DimDateIdStartNavigation))
                .ForMember(dst => dst.FundingEndDate, conf => conf.MapFrom(src => src.DimDateIdEndNavigation))
                .ForMember(dst => dst.ContactPerson, conf => conf.MapFrom(src => src.DimNameIdContactPersonNavigation))
                .ForMember(dst => dst.Geo, conf => conf.MapFrom(src => src.DimGeo.Id != -1 ? src.DimGeo : null))
                .ForMember(dst => dst.CallProgrammes, conf => conf.MapFrom(src => src.DimFundingDecisionFroms))
                .ForMember(dst => dst.FrameworkProgramme, conf => conf.MapFrom(src => src.DimFundingDecisionIdParentDecisionNavigation));

            CreateProjection<DimOrganization, FunderOrganization>(MemberList.None);
            CreateProjection<DimSector, Sector>();
            CreateProjection<DimCallProgramme, CallProgramme>();
            CreateProjection<DimDate, Date?>();
            CreateProjection<DimName, ContactPerson>();
            CreateProjection<DimGeo, Geo>();
            CreateProjection<DimFundingDecision, CallProgramme>();
            CreateProjection<DimFieldOfScience, FieldOfScience>();
            CreateProjection<DimPublication, Models.FundingDecision.Publication>();

            CreateMap<int?, bool>().ConvertUsing(src => src == 1);
        }
    }
}
