using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/user", () => new {Name = "Antonio", Age = 35} );

app.MapPost("/Saveprod", (Prod product) => {
    return product.Name + " - " + product.Cod;
});

app.MapGet("/AddHeader", (HttpResponse response) =>{
response.Headers.Add("Teste", "Testando header");
return "Todo mundo é gay";
});

//FromQuery url
app.MapGet("/getproduct",([FromQuery] string dateStart, [FromQuery] string dateEnd) =>{
  return dateStart + " - " + dateEnd;
});

//user/{code}
app.MapGet("/getproduct/{Code}", ([FromRoute]string Code) =>{
return Code;
});

//Usando Request Header key/value //tem q ser o mesmo nome daqui na key "product-code"
app.MapGet("/getprod", (HttpRequest request) =>{
    return request.Headers["product-code"].ToString();
});

app.MapPost("/SaveprodList", (Prod product) => {
  ProductRepository.AddProd(product);
});

app.MapGet("/getById/{Code}", ([FromRoute] string Code)=>{
    
    var produto = ProductRepository.GetProd(Code);
    return produto;
});


app.Run();

public static class ProductRepository
{
  public static List<Prod> MinhaLista { get; set; }

  public static void AddProd(Prod produto)
  {
    MinhaLista = new List<Prod>();
    MinhaLista.Add(produto);
  }

  public static Prod GetProd(string code)
  {
   return MinhaLista.First(p => p.Cod == code);
  }
}
public class Prod
{
public string Name { get; set; }
public string Cod { get; set; }
}
