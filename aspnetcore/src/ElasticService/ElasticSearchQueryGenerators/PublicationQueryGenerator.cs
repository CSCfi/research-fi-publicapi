﻿using CSC.PublicApi.ElasticService.SearchParameters;
using CSC.PublicApi.Service.Models;
using CSC.PublicApi.Service.Models.Publication;
using Nest;

namespace CSC.PublicApi.ElasticService.ElasticSearchQueryGenerators;

public class PublicationQueryGenerator : QueryGeneratorBase<PublicationSearchParameters, Publication>
{
    public PublicationQueryGenerator(IndexNameSettings configuration) : base(configuration)
    {
    }

    protected override Func<QueryContainerDescriptor<Publication>, QueryContainer> GenerateQueryForSearch(PublicationSearchParameters parameters)
    {
        var subQueries = CreateSubQueries(parameters);
        var filters = CreateFilters(parameters);

        return queryDescriptor => queryDescriptor
            .Bool(boolDescriptor => boolDescriptor
                .Must(subQueries)
                .Filter(filters)
            );
    }

    private static IEnumerable<Func<QueryContainerDescriptor<Publication>, QueryContainer>> CreateSubQueries(PublicationSearchParameters parameters)
    {
        var subQueries = new List<Func<QueryContainerDescriptor<Publication>, QueryContainer>>();

        if (!string.IsNullOrWhiteSpace(parameters.Name))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.Name)
                    .Query(parameters.Name)));
        }

        if (!string.IsNullOrWhiteSpace(parameters.OriginalPublicationId))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.OriginalPublicationId)
                    .Query(parameters.OriginalPublicationId)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.AuthorsText))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.AuthorsText)
                    .Query(parameters.AuthorsText)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.AuthorFirstNames) && string.IsNullOrWhiteSpace(parameters.AuthorLastName))
        {
            // Only first names
            subQueries.Add(
                q => q.Nested(
                    query => query
                        .Path(p => p.Authors)
                        .Query(
                            q => q.Match(m => m
                                .Field(f => f.Authors.Suffix(nameof(Author.FirstNames)))
                                .Query(parameters.AuthorFirstNames)))));
        }
        else if (string.IsNullOrWhiteSpace(parameters.AuthorFirstNames) && !string.IsNullOrWhiteSpace(parameters.AuthorLastName))
        {
            // Only last name
            subQueries.Add(
                q => q.Nested(
                    query => query
                        .Path(p => p.Authors)
                        .Query(
                            q => q.Match(m => m
                                .Field(f => f.Authors.Suffix(nameof(Author.LastName)))
                                .Query(parameters.AuthorLastName)))));
        }
        else if (!string.IsNullOrWhiteSpace(parameters.AuthorFirstNames) && !string.IsNullOrWhiteSpace(parameters.AuthorLastName))
        {
            // Both first names and last name
            subQueries.Add(
                q => q.Nested(
                    query => query
                        .Path(p => p.Authors)
                        .Query(
                            q => q.Bool(b => b
                                .Must(mu => mu
                                    .Match(m => m
                                        .Field(f => f.Authors.Suffix(nameof(Author.FirstNames))).Query(parameters.AuthorFirstNames)
                                    ), mu => mu
                                    .Match(m => m
                                        .Field(f => f.Authors.Suffix(nameof(Author.LastName))).Query(parameters.AuthorLastName)))))));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.ConferenceName))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.ConferenceName)
                    .Query(parameters.ConferenceName)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.JournalName))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.JournalName)
                    .Query(parameters.JournalName)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.ParentPublicationName))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.ParentPublication.Suffix(nameof(ParentPublication.Name)))
                    .Query(parameters.ParentPublicationName)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.ParentPublicationPublisher))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.ParentPublication.Suffix(nameof(ParentPublication.Publisher)))
                    .Query(parameters.ParentPublicationPublisher)));
        }
        
        if (!string.IsNullOrWhiteSpace(parameters.PublisherName))
        {
            subQueries.Add(t => 
                t.Match(query => query.Field(f => f.PublisherName)
                    .Query(parameters.PublisherName)));
        }
        
        if (parameters.Keywords is not null)
        {
            subQueries.Add(t =>
                t.Match(s => s.Field(f => f.Keywords.Suffix(nameof(Keyword.Value)))
                    .Query(parameters.Keywords)));
        }

        return subQueries;
    }
    
    private static IEnumerable<Func<QueryContainerDescriptor<Publication>, QueryContainer>> CreateFilters(PublicationSearchParameters parameters)
    {
        var filters = new List<Func<QueryContainerDescriptor<Publication>, QueryContainer>>();
        
        if (parameters.CreatedFrom is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Created)
                            .GreaterThanOrEquals(parameters.CreatedFrom))
                    ))));
        }

        if (parameters.CreatedTo is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Created)
                            .LessThanOrEquals(parameters.CreatedTo))
                    ))));
        }
        
        if (parameters.ModifiedFrom is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Modified)
                            .GreaterThanOrEquals(parameters.ModifiedFrom))
                    ))));
        }

        if (parameters.ModifiedTo is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                    (
                        s.DateRange(r => r
                            .Field(f => f.Modified)
                            .LessThanOrEquals(parameters.ModifiedTo))
                    ))));
        }
        
        if (parameters.PublicationYear is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                        s.DateRange(r => r
                            .Field(f => f.PublicationYear)
                            .LessThanOrEquals(parameters.PublicationYear)
                            .GreaterThanOrEquals(parameters.PublicationYear).Format("yyyy"))
                    )));
        }
        
        if (parameters.ReportingYear is not null)
        {
            filters.Add(x => x
                .Bool(b => b
                    .Should(s =>
                        s.DateRange(r => r
                            .Field(f => f.ReportingYear)
                            .LessThanOrEquals(parameters.ReportingYear)
                            .GreaterThanOrEquals(parameters.ReportingYear).Format("yyyy"))
                    )));
        }
        
        if (parameters.OrganizationId is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Organizations.Suffix(nameof(Organization.Id)))
                    .Value(parameters.OrganizationId)));
        }
        
        if (parameters.OrganizationUnitId is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Organizations.Suffix(nameof(Organization.Units)).Suffix(nameof(OrganizationUnit.Id)))
                    .Value(parameters.OrganizationUnitId))); 
        }

        if (parameters.AuthorOrcId is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Authors.Suffix(nameof(Author.Orcid)))
                    .Value(parameters.AuthorOrcId)));
        }
        
        if (parameters.TypeCode is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Type)
                    .Value(parameters.TypeCode)));
        }
        
        if (parameters.PublisherOpenAccess is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.PublisherOpenAccess)
                    .Value(parameters.PublisherOpenAccess)));
        }
        
        if (parameters.Issn is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Issn)
                    .Value(parameters.Issn)));
        }
        
        if (parameters.Isbn is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Isbn)
                    .Value(parameters.Isbn)));
        }
        
        
        if (parameters.JufoCode is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.JufoCode)
                    .Value(parameters.JufoCode)));
        }
        
        if (parameters.Doi is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Doi)
                    .Value(parameters.Doi)));
        }

        if (parameters.Status is not null)
        {
            filters.Add(t =>
                t.Term(s => s.Field(f => f.Status)
                    .Value(parameters.Status)));
        }

        return filters;
    }

    protected override Func<QueryContainerDescriptor<Publication>, QueryContainer> GenerateQueryForSingle(string id)
    {
        return queryContainerDescriptor => queryContainerDescriptor.Term(query => query.Field(f => f.Id).Value(id));
    }
}