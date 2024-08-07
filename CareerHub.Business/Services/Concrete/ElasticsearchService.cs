using CareerHub.Business.Services.Abstract;
using Nest;

public class ElasticsearchService: IElasticsearchService
{
    private readonly IElasticClient _elasticClient;

    public ElasticsearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task IndexJobAsync(CareerHub.Entities.Entities.Job job)
    {
        var response = await _elasticClient.IndexDocumentAsync(job);

        if (!response.IsValid)
        {
            throw new Exception($"Failed to index document: {response.OriginalException.Message}");
        }
    }

    public async Task<IEnumerable<CareerHub.Entities.Entities.Job>> SearchJobsAsync(DateTime? start, DateTime? end)
    {
        var searchResponse = await _elasticClient.SearchAsync<CareerHub.Entities.Entities.Job>(s => s
            .Query(q => q
                .DateRange(r => r
                    .Field(f => f.PublicationDuration)
                    .GreaterThanOrEquals(start)
                    .LessThanOrEquals(end)
                )
            )
        );

        if (!searchResponse.IsValid)
        {
            throw new Exception($"Search failed: {searchResponse.OriginalException.Message}");
        }

        return searchResponse.Documents;
    }
}
