using NetListOpsDemo.grpc;
using NetListOpsDemo.grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Replace the static PessoaService with the actual implementation class.
app.MapGrpcService<PessoaService.PessoaServiceBase>();
app.MapGrpcService<FiscalizacaoService.FiscalizacaoServiceBase>();
app.MapGrpcService<GovernoService.GovernoServiceBase>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
