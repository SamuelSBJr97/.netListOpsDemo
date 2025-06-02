using Microsoft.EntityFrameworkCore;

namespace NetListOpsDemo.grpc
{
    public class NetListOpsDbContext : DbContext
    {
        public NetListOpsDbContext(DbContextOptions<NetListOpsDbContext> options) : base(options) { }

        public DbSet<PessoaEntity> Pessoas { get; set; }
        public DbSet<FiscalizacaoEntity> Fiscalizacoes { get; set; }
        public DbSet<GovernoEntity> Governos { get; set; }
    }

    public class PessoaEntity
    {
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public int FiscalizacaoId { get; set; }
        public FiscalizacaoEntity Fiscalizacao { get; set; }
    }

    public class FiscalizacaoEntity
    {
        public int FiscalizacaoId { get; set; }
        public string Nome { get; set; }
        public int GovernoId { get; set; }
        public GovernoEntity Governo { get; set; }
        public ICollection<PessoaEntity> Pessoas { get; set; }
    }

    public class GovernoEntity
    {
        public int GovernoId { get; set; }
        public string Nome { get; set; }
        public ICollection<FiscalizacaoEntity> Fiscalizacoes { get; set; }
    }
}
