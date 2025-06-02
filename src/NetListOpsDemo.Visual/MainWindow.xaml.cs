using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using NetListOpsDemo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace NetListOpsDemo.Visual;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Pessoa> pessoasA = new();
    private List<Pessoa> pessoasB = new();
    private List<Pessoa> lastResult = new();
    private Channel<List<Governo>> _mensageriaChannel = Channel.CreateUnbounded<List<Governo>>();
    private List<Governo> _governosGrpc = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += Window_Loaded;
        TreeGov.SelectedItemChanged += TreeGov_SelectedItemChanged;
        InitLists();
        ShowLists();
        ShowVennDiagram();
        StartMensageriaListener();
    }

    private void InitLists()
    {
        // Exemplo de uso com 10 governos, 5 fiscalizações por governo e 100 pessoas por fiscalização
        var random = new Random();
        var allGovernos = new List<Governo>();
        var allPessoas = new List<Pessoa>();
        int pessoaId = 1;
        for (int g = 1; g <= 10; g++)
        {
            var fiscalizacoes = new List<Fiscalizacao>();
            for (int f = 1; f <= 5; f++)
            {
                var pessoas = new List<Pessoa>();
                for (int p = 1; p <= 100; p++)
                {
                    var pessoa = new Pessoa
                    {
                        Id = pessoaId++,
                        Nome = $"Pessoa_{g}_{f}_{p}",
                        Idade = random.Next(18, 70)
                    };
                    pessoas.Add(pessoa);
                    allPessoas.Add(pessoa);
                }
                fiscalizacoes.Add(new Fiscalizacao
                {
                    Id = (g - 1) * 5 + f,
                    PessoaId = 0, // não usado neste exemplo
                    Area = $"Area_{f}",
                    Data = DateTime.Now.AddDays(-f),
                    Pessoas = pessoas
                });
            }
            allGovernos.Add(new Governo
            {
                Id = g,
                Nome = $"Governo_{g}",
                Estado = $"Estado_{g}",
                Fiscalizacoes = fiscalizacoes
            });
        }
        // Exemplo: pessoasA = pessoas da primeira fiscalização do primeiro governo
        // pessoasB = pessoas da segunda fiscalização do segundo governo
        pessoasA = allGovernos[0].Fiscalizacoes[0].Pessoas;
        pessoasB = allGovernos[1].Fiscalizacoes[1].Pessoas;
    }

    private void ShowLists()
    {
        if (ListA == null || ListB == null || ListResult == null) return;
        ListA.ItemsSource = null;
        ListB.ItemsSource = null;
        ListA.ItemsSource = pessoasA?.ConvertAll(p => $"Id: {p.Id}, Nome: {p.Nome}, Idade: {p.Idade}") ?? new List<string>();
        ListB.ItemsSource = pessoasB?.ConvertAll(p => $"Id: {p.Id}, Nome: {p.Nome}, Idade: {p.Idade}") ?? new List<string>();
        ListResult.ItemsSource = null;
    }

    private void ShowResult(IEnumerable<Pessoa> pessoas)
    {
        lastResult = pessoas?.ToList() ?? new List<Pessoa>();
        ListResult.ItemsSource = lastResult.Select(p => $"Id: {p.Id}, Nome: {p.Nome}, Idade: {p.Idade}").ToList();
        UpdateVennDiagramFromResult();
    }

    private void UpdateVennDiagramFromResult()
    {
        if (TxtGovernoCount == null || TxtFiscalizacaoCount == null || TxtIntersecaoCount == null) return;
        var pessoasFiscalizacao = pessoasA?.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
        var pessoasGoverno = pessoasB?.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
        var pessoasResultado = lastResult?.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
        var intersecFiscalizacao = pessoasResultado.Intersect(pessoasFiscalizacao).Count();
        var intersecGoverno = pessoasResultado.Intersect(pessoasGoverno).Count();
        var intersecAmbos = pessoasResultado.Intersect(pessoasFiscalizacao.Intersect(pessoasGoverno)).Count();
        TxtGovernoCount.Text = intersecGoverno.ToString();
        TxtFiscalizacaoCount.Text = intersecFiscalizacao.ToString();
        TxtIntersecaoCount.Text = intersecAmbos.ToString();
    }

    private void BtnSortResult_Click(object sender, RoutedEventArgs e)
    {
        if (lastResult == null || lastResult.Count == 0) return;
        var ordered = lastResult.OrderBy(p => p.Nome).ThenBy(p => p.Idade).ThenBy(p => p.Id).ToList();
        ListResult.ItemsSource = ordered.Select(p => $"Id: {p.Id}, Nome: {p.Nome}, Idade: {p.Idade}").ToList();
        lastResult = ordered;
    }

    private void BtnDistinct_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.DistinctByHashSet(pessoasA);
        ShowResult(result);
    }

    private void BtnIntersect_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.IntersectByHashSet(pessoasA, pessoasB);
        ShowResult(result);
    }

    private void BtnExcept_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.ExceptByHashSet(pessoasA, pessoasB);
        ShowResult(result);
    }

    private void BtnUnion_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.UnionByHashSet(pessoasA, pessoasB);
        ShowResult(result);
    }

    private void ShowVennDiagram()
    {
        if (TxtGovernoCount == null || TxtFiscalizacaoCount == null || TxtIntersecaoCount == null) return;
        var pessoasFiscalizacao = pessoasA?.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
        var pessoasGoverno = pessoasB?.Select(p => p.Id).ToHashSet() ?? new HashSet<int>();
        var pessoasAmbos = pessoasGoverno.Intersect(pessoasFiscalizacao).Count();
        TxtGovernoCount.Text = pessoasGoverno.Count.ToString();
        TxtFiscalizacaoCount.Text = pessoasFiscalizacao.Count.ToString();
        TxtIntersecaoCount.Text = pessoasAmbos.ToString();
    }

    // Simulação de obtenção de dados via gRPC
    private async Task<List<Governo>> ObterGovernosViaGrpcAsync()
    {
        // Aqui você substituiria pelo seu client gRPC real
        var random = new Random();
        var lista = new List<Governo>();
        int pessoaId = 1;
        for (int g = 1; g <= 3; g++)
        {
            var fiscalizacoes = new List<Fiscalizacao>();
            for (int f = 1; f <= 2; f++)
            {
                var pessoas = new List<Pessoa>();
                for (int p = 1; p <= 5; p++)
                {
                    pessoas.Add(new Pessoa { Id = pessoaId++, Nome = $"Pessoa_{g}_{f}_{p}", Idade = random.Next(18, 70) });
                }
                fiscalizacoes.Add(new Fiscalizacao
                {
                    Id = (g - 1) * 2 + f,
                    PessoaId = 0,
                    Area = $"Area_{f}",
                    Data = DateTime.Now.AddDays(-f),
                    Pessoas = pessoas
                });
            }
            lista.Add(new Governo
            {
                Id = g,
                Nome = $"Governo_{g}",
                Estado = $"Estado_{g}",
                Fiscalizacoes = fiscalizacoes
            });
        }
        return lista;
    }

    private async void BtnAtualizarGrpc_Click(object sender, RoutedEventArgs e)
    {
        var governos = await ObterGovernosViaGrpcAsync();
        await _mensageriaChannel.Writer.WriteAsync(governos);
    }

    private async void StartMensageriaListener()
    {
        _ = Task.Run(async () =>
        {
            await foreach (var governos in _mensageriaChannel.Reader.ReadAllAsync())
            {
                Dispatcher.Invoke(() => PopularTreeView(governos));
            }
        });
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var governos = await ObterGovernosViaGrpcAsync();
        await _mensageriaChannel.Writer.WriteAsync(governos);
    }

    private void PopularTreeView(List<Governo> governos)
    {
        _governosGrpc = governos;
        TreeGov.Items.Clear();
        foreach (var gov in governos)
        {
            var govNode = new TreeViewItem { Header = $"Governo: {gov.Nome} ({gov.Estado})", Tag = gov };
            foreach (var fisc in gov.Fiscalizacoes)
            {
                var fiscNode = new TreeViewItem { Header = $"Fiscalização: {fisc.Area} ({fisc.Data:dd/MM/yyyy})", Tag = fisc };
                foreach (var pessoa in fisc.Pessoas)
                {
                    fiscNode.Items.Add(new TreeViewItem { Header = $"Pessoa: {pessoa.Nome} (Idade: {pessoa.Idade})", Tag = pessoa });
                }
                govNode.Items.Add(fiscNode);
            }
            TreeGov.Items.Add(govNode);
        }
    }

    private void TreeGov_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (TreeGov.SelectedItem is TreeViewItem item)
        {
            if (item.Tag is Governo gov)
            {
                pessoasA = gov.Fiscalizacoes.SelectMany(f => f.Pessoas).ToList();
                pessoasB = new List<Pessoa>();
                ShowLists();
                ShowVennDiagram();
            }
            else if (item.Tag is Fiscalizacao fisc)
            {
                pessoasA = fisc.Pessoas;
                pessoasB = new List<Pessoa>();
                ShowLists();
                ShowVennDiagram();
            }
            else if (item.Tag is Pessoa pessoa)
            {
                pessoasA = new List<Pessoa> { pessoa };
                pessoasB = new List<Pessoa>();
                ShowLists();
                ShowVennDiagram();
            }
        }
    }
}