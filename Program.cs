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
app.MapGet("/prod", (HttpRequest request) =>{
    return request.Headers["product-code"].ToString();
});

//Insere Produto post
app.MapPost("/prod", (Prod product) => {
  ProductRepository.AddProd(product);
 return Results.Created($"/prod/{product.Cod}", product.Cod );
});
//Getby ID
app.MapGet("/prod/{Code}", ([FromRoute] string Code)=>{
    
    var produto = ProductRepository.GetProd(Code);
    if (produto != null)
    return Results.Ok(produto);
    else
    return Results.NotFound();
});
//Delete
app.MapDelete("/prod/{Code}",([FromRoute] string Code) =>{
   var deleteprod = ProductRepository.GetProd(Code);
       ProductRepository.DeleteProd(deleteprod);
       return Results.Ok();
});
//Update
app.MapPut("/prod",(Prod produto) => {
    var prodSaved = ProductRepository.GetProd(produto.Cod);
     prodSaved.Name = produto.Name;
     return Results.Ok();
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
   return MinhaLista.FirstOrDefault(p => p.Cod == code);
  }

  public static Prod DeleteProd(Prod produto){
    MinhaLista.Remove(produto);
    return produto;

  }
}
public class Prod
{
public string Name { get; set; }
public string Cod { get; set; }
}
