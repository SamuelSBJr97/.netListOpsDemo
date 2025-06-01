using Grpc.Core;
using NetListOpsDemo.grpc;
using System.Collections.Concurrent;

namespace NetListOpsDemo.grpc.Services
{
    public class PessoaServiceImpl : PessoaService.PessoaServiceBase
    {
        private static readonly ConcurrentDictionary<int, Pessoa> Pessoas = new();

        public override Task<Pessoa> GetPessoa(PessoaIdRequest request, ServerCallContext context)
        {
            if (Pessoas.TryGetValue(request.Id, out var pessoa))
                return Task.FromResult(pessoa);
            throw new RpcException(new Status(StatusCode.NotFound, "Pessoa não encontrada"));
        }

        public override Task<PessoaList> ListPessoas(Empty request, ServerCallContext context)
        {
            var list = new PessoaList();
            list.Pessoas.AddRange(Pessoas.Values);
            return Task.FromResult(list);
        }

        public override Task<Pessoa> CreatePessoa(Pessoa request, ServerCallContext context)
        {
            Pessoas[request.Id] = request;
            return Task.FromResult(request);
        }

        public override Task<Empty> DeletePessoa(PessoaIdRequest request, ServerCallContext context)
        {
            Pessoas.TryRemove(request.Id, out _);
            return Task.FromResult(new Empty());
        }
    }
}