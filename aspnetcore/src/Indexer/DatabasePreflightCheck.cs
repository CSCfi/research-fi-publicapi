using CSC.PublicApi.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CSC.PublicApi.Indexer;

public class DatabasePreflightCheck
{
    private readonly ApiDbContext? _context;
    private readonly ILogger<DatabasePreflightCheck>? _logger;
    private readonly string _logPrefix = "Database preflight check: ";

    public DatabasePreflightCheck(ApiDbContext context, ILogger<DatabasePreflightCheck> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Constructor without dependencies for unit testing
    public DatabasePreflightCheck()
    {
    }

    // Most of publications should have author information linked via fact_contribution.
    // Exact ratio cannot be determined, 80% is used as a baseline.
    public bool FactContributionNumberOfDistinctReferencesToDimPublicationIsGood(int dimPublicationCount, int factContributionDistinctReferencesToDimPublicationCount)
    {
        if (factContributionDistinctReferencesToDimPublicationCount >= dimPublicationCount * 0.8)
        {
            return true;
        }
        return false;
    }
    public bool IsGood()
    {
        bool isGood = true;
        if (_context != null && _logger != null)
        {
            _logger.LogInformation(_logPrefix + "Check that required database tables contain data for indexing");

            // Publication count
            int dimPublication_Count = _context.DimPublications.AsNoTracking().Where(dp => dp.Id > 0).Count();
            _logger.LogInformation(_logPrefix + "publications: dim_publication count = {DimPublicationCount}", dimPublication_Count);
            if (dimPublication_Count == 0)
            {
                _logger.LogError(_logPrefix + "publications: Table dim_publication is empty");
                isGood = false;
            }

            // Funding call count (dim_call_programmme in database)
            int dimCallProgramme_Count = _context.DimCallProgrammes.AsNoTracking().Where(dcp => dcp.Id > 0).Count();
            _logger.LogInformation(_logPrefix + "funding calls: dim_call_programme count = {DimCallProgramme}", dimCallProgramme_Count);
            if (dimCallProgramme_Count == 0)
            {
                _logger.LogError(_logPrefix + "funding calls: Table dim_call_programme is empty");
                isGood = false;
            }

            // Funding decision count
            int dimFundingDecision_Count = _context.DimFundingDecisions.AsNoTracking().Where(dfd => dfd.Id > 0).Count();
            _logger.LogInformation(_logPrefix + "funding decisions: dim_funding_decision count = {DimFundingDecision}", dimFundingDecision_Count);
            if (dimFundingDecision_Count == 0)
            {
                _logger.LogError(_logPrefix + "funding decisions: Table dim_funding_decision is empty");
                isGood = false;
            }

            // Research dataset count
            int dimResearchDataset_Count = _context.DimResearchDatasets.AsNoTracking().Where(drd => drd.Id > 0).Count();
            _logger.LogInformation(_logPrefix + "research datasets: dim_research_dataset count = {DimResearchDataset}", dimResearchDataset_Count);
            if (dimResearchDataset_Count == 0)
            {
                _logger.LogError(_logPrefix + "research datasets: Table dim_research_dataset is empty");
                isGood = false;
            }

            // Publication related fact_contribution count.
            // Count distinct dim_publication references in fact_contribution.
            int distinctDimPublicationReferencesInFactContribution_Count =
                _context.FactContributions.AsNoTracking().Where(fc => fc.DimPublicationId > 0).Select(fc => fc.DimPublicationId).Distinct().Count();
            _logger.LogInformation(_logPrefix + "publications: Number of distinct dim_publication references in fact_contribution = {DistinctDimPublicationReferencesInFactContributionCount}", distinctDimPublicationReferencesInFactContribution_Count);
            if (!FactContributionNumberOfDistinctReferencesToDimPublicationIsGood(dimPublication_Count, distinctDimPublicationReferencesInFactContribution_Count))
            {
                _logger.LogError(_logPrefix + "publications: Possibly too few of dim_publication references in fact_contribution");
                isGood = false;
            }

            if (isGood)
            {
                _logger.LogInformation(_logPrefix + "status OK");
            }
            else if (!isGood)
            {
                _logger.LogError(_logPrefix + "indexing aborted");
            }
        }
        return isGood;
    }
}