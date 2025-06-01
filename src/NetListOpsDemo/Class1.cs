namespace NetListOpsDemo;

public struct Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
}

public struct Fiscalizacao
{
    public int Id { get; set; }
    public int PessoaId { get; set; }
    public string Area { get; set; }
    public DateTime Data { get; set; }
}

public struct Governo
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Estado { get; set; }
}

public static class ListHashSetOps
{
    // Exemplo: retorna elementos únicos de uma lista usando HashSet
    public static List<T> DistinctByHashSet<T>(List<T> list)
    {
        var set = new HashSet<T>(list);
        return set.ToList();
    }

    // Exemplo: interseção entre duas listas usando HashSet
    public static List<T> IntersectByHashSet<T>(List<T> list1, List<T> list2)
    {
        var set = new HashSet<T>(list1);
        set.IntersectWith(list2);
        return set.ToList();
    }

    // Exemplo: diferença entre duas listas usando HashSet
    public static List<T> ExceptByHashSet<T>(List<T> list1, List<T> list2)
    {
        var set = new HashSet<T>(list1);
        set.ExceptWith(list2);
        return set.ToList();
    }
}
