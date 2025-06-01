using NetListOpsDemo;
using System.Diagnostics;

namespace NetListOpsDemo.Tests
{

    public class ListHashSetOpsTests
    {
        [Fact]
        public void TestDistinctIntersectExceptWithLargeDataset()
        {
            // Gerar milhões de structs
            int total = 2_000_000;
            var pessoas = new List<Pessoa>(total);
            var fiscalizacoes = new List<Fiscalizacao>(total);
            var governos = new List<Governo>(total);
            var baseDate = new DateTime(2020, 1, 1);
            for (int i = 0; i < total; i++)
            {
                pessoas.Add(new Pessoa { Id = i, Nome = $"Pessoa {i}", Idade = i % 100 });
                fiscalizacoes.Add(new Fiscalizacao { Id = i, PessoaId = i, Area = $"Area {i % 10}", Data = baseDate.AddDays(i % 365) });
                governos.Add(new Governo { Id = i, Nome = $"Governo {i % 5}", Estado = $"Estado {i % 27}" });
            }

            // Operações de lista para lista usando HashSet
            var pessoasUnicas = ListHashSetOps.DistinctByHashSet(pessoas);
            var intersecao = ListHashSetOps.IntersectByHashSet(pessoas, pessoasUnicas);
            var diferenca = ListHashSetOps.ExceptByHashSet(pessoas, intersecao);

            Assert.Equal(pessoasUnicas.Count, pessoas.Count); // Todos são únicos
            Assert.Equal(intersecao.Count, pessoas.Count); // Interseção total
            Assert.Empty(diferenca); // Não sobra nada
        }
    }
}