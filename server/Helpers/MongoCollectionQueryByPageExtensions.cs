using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace server.Helpers
{
  public static class MongoCollectionQueryByPageExtensions
  {
    public static async Task<(int totalPages, IReadOnlyList<TDocument> data)> AggregateByPage<TDocument>(
      this IMongoCollection<TDocument> collection,
      SortDefinition<TDocument> sortDefinition,
      int page,
      int pageSize)
    {
      var countFacet = AggregateFacet.Create("count",
        PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Count<TDocument>()
        }));

      var dataFacet = AggregateFacet.Create("data",
        PipelineDefinition<TDocument, TDocument>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Sort(sortDefinition),
          PipelineStageDefinitionBuilder.Skip<TDocument>(page * pageSize),
          PipelineStageDefinitionBuilder.Limit<TDocument>(pageSize),
        }));


      var aggregation = await collection.Aggregate()
        .Facet(countFacet, dataFacet)
        .ToListAsync();

      var count = aggregation.First()
        .Facets.First(x => x.Name == "count")
        .Output<AggregateCountResult>()
        .First()
        .Count;

      var totalPages = (int) Math.Ceiling((double) count / pageSize);

      var data = aggregation.First()
        .Facets.First(x => x.Name == "data")
        .Output<TDocument>();

      return (totalPages, data);
    }
  }
}
