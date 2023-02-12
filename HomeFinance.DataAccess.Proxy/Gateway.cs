using System.Net.Http.Headers;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Requests;
using HomeFinance.Domain.Services;
using HomeFinance.Domain.Utils;
using System.Text;
using System.Text.Json;

namespace HomeFinance.DataAccess.Proxy;

public class Gateway : IGateway
{
    public IUserDependentRepository<Tag, string> TagRepository { get; }
    public IOperationRepository OperationRepository { get; }
    public IUserDependentRepository<Wallet, Guid> WalletRepository { get; }
    public IUserDependentRepository<RepeatableOperation, Guid> RepeatableOperationRepository { get; }
    public IUserDependentRepository<TransientOperation, Guid> TransientOperationRepository { get; }
    public IMergeTagsService MergeTagsService => throw new NotImplementedException();

    private readonly HttpClient _client = new HttpClient();

   

    public Gateway(string baseUrl, string userName, string password)
    {
        var request = new UserLoginRequest()
        {
            UserNameOrEmail = userName,
            Password = password
        };
        var json = JsonSerializer.Serialize(request);
        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var jsonResponse = this._client.PostAsync(baseUrl+"User/Login", stringContent).Result.Content.ReadAsStringAsync().Result;

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var token = JsonSerializer.Deserialize<UserLoginResponse>(jsonResponse, options)!.Token;
        this._client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        

        TagRepository = new UserDependentRepository<Tag, string>(_client, baseUrl + "tag");
        WalletRepository = new UserDependentRepository<Wallet, Guid>(_client, baseUrl + "wallet");
        TransientOperationRepository =
            new UserDependentRepository<TransientOperation, Guid>(_client, baseUrl + "transient");
        OperationRepository = new OperationRepository(_client, baseUrl + "operation");
    }
}
