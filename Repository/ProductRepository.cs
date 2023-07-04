public static class ProductRepository
{
    public static List<Prod> MinhaLista { get; set; } = MinhaLista = new List<Prod>();

    public static void init(IConfiguration configuration)
    {
        var products = configuration.GetSection("Prod").Get<List<Prod>>();
        MinhaLista = products;
    }
    public static void AddProd(Prod produto)
    {
        MinhaLista.Add(produto);
    }

    public static Prod GetProd(string code)
    {
        return MinhaLista.FirstOrDefault(p => p.Cod == code);
    }

    public static Prod DeleteProd(Prod produto)
    {
        MinhaLista.Remove(produto);
        return produto;

    }
}

