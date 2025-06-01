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

namespace NetListOpsDemo.Visual;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Pessoa> pessoasA;
    private List<Pessoa> pessoasB;

    public MainWindow()
    {
        InitializeComponent();
        InitLists();
        ShowLists();
    }

    private void InitLists()
    {
        pessoasA = new List<Pessoa>
        {
            new Pessoa { Id = 1, Nome = "Ana", Idade = 30 },
            new Pessoa { Id = 2, Nome = "Bruno", Idade = 25 },
            new Pessoa { Id = 3, Nome = "Carlos", Idade = 40 },
            new Pessoa { Id = 4, Nome = "Ana", Idade = 30 }, // Duplicado
        };
        pessoasB = new List<Pessoa>
        {
            new Pessoa { Id = 2, Nome = "Bruno", Idade = 25 },
            new Pessoa { Id = 3, Nome = "Carlos", Idade = 40 },
            new Pessoa { Id = 5, Nome = "Diana", Idade = 22 },
        };
    }

    private void ShowLists()
    {
        ListA.ItemsSource = null;
        ListB.ItemsSource = null;
        ListA.ItemsSource = pessoasA;
        ListB.ItemsSource = pessoasB;
        ListResult.ItemsSource = null;
    }

    private void BtnDistinct_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.DistinctByHashSet(pessoasA);
        ListResult.ItemsSource = result;
    }

    private void BtnIntersect_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.IntersectByHashSet(pessoasA, pessoasB);
        ListResult.ItemsSource = result;
    }

    private void BtnExcept_Click(object sender, RoutedEventArgs e)
    {
        var result = ListHashSetOps.ExceptByHashSet(pessoasA, pessoasB);
        ListResult.ItemsSource = result;
    }
}