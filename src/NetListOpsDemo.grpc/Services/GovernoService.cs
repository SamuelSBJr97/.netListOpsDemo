using Grpc.Core;
using NetListOpsDemo.grpc;
using System.Collections.Concurrent;

namespace NetListOpsDemo.grpc.Services
{
    public class GovernoService : NetListOpsDemo.grpc.GovernoService.GovernoServiceBase
    {
        private static readonly ConcurrentDictionary<int, Governo> Governos = new();

        public override Task<Governo> GetGoverno(GovernoIdRequest request, ServerCallContext context)
        {
            if (Governos.TryGetValue(request.Id, out var governo))
                return Task.FromResult(governo);
            throw new RpcException(new Status(StatusCode.NotFound, "Governo não encontrado"));
        }

        public override Task<GovernoList> ListGovernos(Empty request, ServerCallContext context)
        {
            var list = new GovernoList();
            list.Governos.AddRange(Governos.Values);
            return Task.FromResult(list);
        }

        public override Task<Governo> CreateGoverno(Governo request, ServerCallContext context)
        {
            Governos[request.Id] = request;
            return Task.FromResult(request);
        }

        public override Task<Empty> DeleteGoverno(GovernoIdRequest request, ServerCallContext context)
        {
            Governos.TryRemove(request.Id, out _);
            return Task.FromResult(new Empty());
        }
    }
}