using poc.api.mongodb.Model;

namespace poc.api.mongodb.Service;

public interface IProdutoService
{
    Task<List<Produto>> Get();
    Task<Produto> Get(int id);
    Task<Produto> Post(Produto entity);
    Task<Produto> Put(Produto entity);
    Task<Produto> Delete(int id);
}
