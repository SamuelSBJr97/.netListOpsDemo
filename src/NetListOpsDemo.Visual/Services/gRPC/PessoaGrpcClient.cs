using Grpc.Net.Client;
using NetListOpsDemo.grpc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetListOpsDemo.Visual.Services;

public class PessoaGrpcClient
{
    private readonly PessoaService.PessoaServiceClient _client;

    public PessoaGrpcClient(string grpcAddress)
    {
        var channel = GrpcChannel.ForAddress(grpcAddress);
        _client = new PessoaService.PessoaServiceClient(channel);
    }

    public async Task<List<Pessoa>> ListPessoasAsync()
    {
        var response = await _client.ListPessoasAsync(new Empty());
        return new List<Pessoa>(response.Pessoas);
    }

    public async Task<Pessoa> CreatePessoaAsync(Pessoa pessoa)
    {
        return await _client.CreatePessoaAsync(pessoa);
    }

    public async Task DeletePessoaAsync(int id)
    {
        await _client.DeletePessoaAsync(new PessoaIdRequest { Id = id });
    }
}