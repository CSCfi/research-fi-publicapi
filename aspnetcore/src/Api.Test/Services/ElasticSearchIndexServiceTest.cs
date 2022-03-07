using Api.Services;
using Api.Test.TestHelpers;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test.Services
{
    public class ElasticSearchIndexServiceTest
    {
        private readonly ElasticSearchIndexService _service;

        private readonly ElasticClient _elasticClient;
        private readonly Mock<ILogger<ElasticSearchIndexService>> _loggerMock = new();
        private readonly List<IApiCallDetails> _elasticApiCalls = new();

        private readonly List<string> _mockedIndices = new();

        public ElasticSearchIndexServiceTest()
        {
            void handler(IApiCallDetails apiRequest) => _elasticApiCalls.Add(apiRequest);

            _elasticClient = ElasticTestClient.Create(handler, _mockedIndices);

            _service = new ElasticSearchIndexService(
                _elasticClient,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task Should_CreateV1Index_WhenIndicesDoNotExist()
        {
            // Arrange
            var indexName = $"someindex{Guid.NewGuid():N}";
            var entities = GetEntities();
            _mockedIndices.Clear();

            // Act
            await _service.IndexAsync(indexName, entities);

            // Assert
            var expectedIndexName = $"{indexName}_v1";
            var expectedAliasName = indexName;
            _elasticApiCalls.AssertCreateIndexCalled(expectedIndexName);
            _elasticApiCalls.AssertBulkAliasCalled(expectedAliasName, expectedIndexName, "*");
        }

        [Fact]
        public async Task Should_CreateV2Index_WhenV1Exists()
        {
            // Arrange
            var indexName = $"someindex{Guid.NewGuid():N}";
            var entities = GetEntities();
            _mockedIndices.Add($"/{indexName}_v1");

            // Act
            await _service.IndexAsync(indexName, entities);

            // Assert
            var expectedIndexName = $"{indexName}_v2";
            var expectedAliasName = indexName;
            _elasticApiCalls.AssertCreateIndexCalled(expectedIndexName);
            _elasticApiCalls.AssertBulkAliasCalled(expectedAliasName, expectedIndexName, "*");
        }

        [Fact]
        public async Task Should_CreateV1Index_WhenV2Exists()
        {
            // Arrange
            var indexName = $"someindex{Guid.NewGuid():N}";
            var entities = GetEntities();
            _mockedIndices.Add($"/{indexName}_v2");

            // Act
            await _service.IndexAsync(indexName, entities);

            // Assert
            var expectedIndexName = $"{indexName}_v1";
            var expectedAliasName = indexName;
            _elasticApiCalls.AssertCreateIndexCalled(expectedIndexName);
            _elasticApiCalls.AssertBulkAliasCalled(expectedAliasName, expectedIndexName, "*");
        }

        private static List<TestModel> GetEntities()
        {
            return new List<TestModel>
            {
                new TestModel{ Id = "model 1"}
            };
        }
    }
}
