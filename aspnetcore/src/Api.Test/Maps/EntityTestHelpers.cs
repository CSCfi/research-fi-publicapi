using Api.Models.Entities;

namespace Api.Test.TestHelpers
{
    public static class EntityTestHelpers
    {

        public static DimCallProgramme With(this DimCallProgramme cp, DimCallProgramme parentProgramme)
        {
            cp.DimCallProgrammeId2s = new[]
            {
                parentProgramme
            };

            return cp;
        }
    }
}