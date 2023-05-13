using System.Text;
using System.Text.Json;
using HomeFinance.Domain.Repositories;

namespace HomeFinance.DataAccess.Proxy;

class UserDependentRepository<T, TKey>: IUserDependentCollectionRepository<T, TKey>
{
    private readonly HttpClient _client;
    private readonly string _path;
    public UserDependentRepository(HttpClient client, string path)
    {
        this._client=client;
        this._path=path;
    }


    public async Task<List<T>> GetAll()
    {
        var response = await this._client.GetAsync(this._path );
        var jsonResponse = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return (JsonSerializer.Deserialize<IEnumerable<T>>(jsonResponse, options)!).ToList();
    }

    public Task<T?> GetByKey(TKey key)
    {
        throw new NotImplementedException();
    }

    public Task<T> Add(T domain)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> AddRange(IEnumerable<T> domain)
    {
        var json = JsonSerializer.Serialize(domain);
        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var response=await this._client.PostAsync(this._path.TrimEnd('/') + "/range", stringContent);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<IEnumerable<T>>(jsonResponse, options)!;
    }

    public Task<T> Update(T domain)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> Update(IEnumerable<T> domain)
    {
        throw new NotImplementedException();
    }

    public Task Remove(TKey key)
    {
        throw new NotImplementedException();
    }
}