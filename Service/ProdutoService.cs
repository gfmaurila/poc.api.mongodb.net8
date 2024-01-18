using MongoDB.Driver;
using poc.api.mongodb.Configuration;
using poc.api.mongodb.Model;

namespace poc.api.mongodb.Service;

public class ProdutoService : IProdutoService
{
    private readonly IMongoCollection<Produto> _produto;
    public ProdutoService(MongoDatabaseFactory dbFactory)
    {
        _produto = dbFactory.GetDatabase().GetCollection<Produto>("Produto");
    }

    public async Task<List<Produto>> Get()
    {
        return await _produto.Find(_ => true).ToListAsync();
    }

    public async Task<Produto> Get(int id)
    {
        var filter = Builders<Produto>.Filter.Eq(p => p.Id, id);
        return await _produto.Find(filter).FirstOrDefaultAsync();
    }


    public async Task<Produto> Post(Produto entity)
    {
        // Obter a coleção de contadores
        var counters = _produto.Database.GetCollection<Counter>("counters");

        // Obter o próximo ID
        entity.Id = await GetNextId(counters, "produtos");

        // Inserir o produto
        await _produto.InsertOneAsync(entity);

        return entity;
    }

    public async Task<Produto> Put(Produto entity)
    {
        var filter = Builders<Produto>.Filter.Eq(p => p.Id, entity.Id);
        var updateResult = await _produto.ReplaceOneAsync(filter, entity);

        if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            return entity;

        return null;
    }


    public async Task<Produto> Delete(int id)
    {
        var filter = Builders<Produto>.Filter.Eq(p => p.Id, id);
        var result = await _produto.DeleteOneAsync(filter);

        if (result.IsAcknowledged && result.DeletedCount > 0)
            return null;

        return null;
    }

    private async Task<int> GetNextId(IMongoCollection<Counter> counters, string collectionName)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, collectionName);
        var update = Builders<Counter>.Update.Inc(c => c.Seq, 1);
        var options = new FindOneAndUpdateOptions<Counter, Counter>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var counter = await counters.FindOneAndUpdateAsync(filter, update, options);
        return counter.Seq;
    }

}
