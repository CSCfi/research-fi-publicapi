﻿namespace CSC.PublicApi.Repositories;

/// <summary>
/// Responsible for fetching entities from database and mapping them to api models which are indexed to ElasticSearch.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIndexRepository<out T> : IIndexRepository where T : class
{
    /// <summary>
    /// Gets all database entities for indexing, projected as T.
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// Gets a chunk of database entities for indexing, projected as T.
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetChunk(int skipAmount, int takeAmount);
}

/// <summary>
/// Responsible for fetching entities from database and mapping them to api models which are indexed to ElasticSearch.
/// Used as a marker interface for dependency injection.
/// </summary>
public interface IIndexRepository
{
    /// <summary>
    /// The type of the model this repository provides.
    /// </summary>
    Type ModelType { get; }

    /// <summary>
    /// Returns models of the type ModelType.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<object> GetAllAsync();

    /// <summary>
    /// Returns models of the type ModelType.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<object> GetChunkAsync(int skipAmount, int takeAmount);

    /// <summary>
    /// Perform data manipulations which are hard to do in the db query phase.
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    List<object> PerformInMemoryOperations(List<object> objects);

    /// <summary>
    /// Perform to single entity data manipulations which are hard to do in the db query phase.
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    object PerformInMemoryOperation(object entity);
}