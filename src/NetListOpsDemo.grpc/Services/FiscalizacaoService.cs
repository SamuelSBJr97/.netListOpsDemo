using Grpc.Core;
using NetListOpsDemo.grpc;
using System.Collections.Concurrent;

namespace NetListOpsDemo.grpc.Services
{
    // Renaming the class to avoid circular dependency with the generated base class
    public class FiscalizacaoServiceImpl : FiscalizacaoService.FiscalizacaoServiceBase
    {
        private static readonly ConcurrentDictionary<int, Fiscalizacao> Fiscalizacoes = new();

        public override Task<Fiscalizacao> GetFiscalizacao(FiscalizacaoIdRequest request, ServerCallContext context)
        {
            if (Fiscalizacoes.TryGetValue(request.Id, out var fiscalizacao))
                return Task.FromResult(fiscalizacao);
            throw new RpcException(new Status(StatusCode.NotFound, "Fiscalização não encontrada"));
        }

        public override Task<FiscalizacaoList> ListFiscalizacoes(Empty request, ServerCallContext context)
        {
            var list = new FiscalizacaoList();
            list.Fiscalizacoes.AddRange(Fiscalizacoes.Values);
            return Task.FromResult(list);
        }

        public override Task<Fiscalizacao> CreateFiscalizacao(Fiscalizacao request, ServerCallContext context)
        {
            Fiscalizacoes[request.Id] = request;
            return Task.FromResult(request);
        }

        public override Task<Empty> DeleteFiscalizacao(FiscalizacaoIdRequest request, ServerCallContext context)
        {
            Fiscalizacoes.TryRemove(request.Id, out _);
            return Task.FromResult(new Empty());
        }
    }
}