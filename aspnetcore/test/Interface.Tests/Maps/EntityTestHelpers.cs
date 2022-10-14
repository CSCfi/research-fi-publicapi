using System.Collections.Generic;
using CSC.PublicApi.DatabaseContext.Entities;

namespace CSC.PublicApi.Tests.Maps;

public static class EntityTestHelpers
{

    public static DimCallProgramme With(this DimCallProgramme cp, DimCallProgramme parentProgramme)
    {
        parentProgramme.DimCallProgrammeId2s ??= new List<DimCallProgramme> { new DimCallProgramme { Id = -1 } };
        cp.DimCallProgrammeId2s = new[]
        {
            parentProgramme
        };

        return cp;
    }
}