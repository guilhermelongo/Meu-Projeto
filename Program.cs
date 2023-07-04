using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionStrings:Mysql"]);
var app = builder.Build();




app.MapGet("/", () => "Hello World!");

app.MapGet("/user", () => new { Name = "Antonio", Age = 35 });

app.MapPost("/Saveprod", (Prod product) =>
{
    return product.Name + " - " + product.Cod;
});

app.MapGet("/AddHeader", (HttpResponse response) =>
{
    response.Headers.Add("Teste", "Testando header");
    return "Todo mundo é gay";
});

//get para saber a base de dados q estamos usando
app.MapGet("/Configuratio/database", (IConfiguration configuration) =>
{
    return Results.Ok(configuration["database:connection"]);
});
//FromQuery url
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return dateStart + " - " + dateEnd;
});

//user/{code}
app.MapGet("/getproduct/{Code}", ([FromRoute] string Code) =>
{
    return Code;
});

//Usando Request Header key/value //tem q ser o mesmo nome daqui na key "product-code"
app.MapGet("/prod", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});

//Insere Produto post
app.MapPost("/prod", (ProductRequest productRequest, ApplicationDbContext context) =>{
    Category Category = context.Categories.Where(q => q.Id == productRequest.CategoryId).FirstOrDefault();
    var produto = new Prod(){
       Cod = productRequest.Cod,
       Name = productRequest.Name,
       Description = productRequest.Description,
       Category = Category
    };
    context.Products.Add(produto);
    context.SaveChanges();
    return Results.Created($"/products/{produto.Id}", produto.Id);
    
 
});
//Getby ID
app.MapGet("/prod/{Code}", ([FromRoute] string Code, ApplicationDbContext context) =>
{

    var produto = context.Products.Where(q => q.Cod == Code).FirstOrDefault();
    if (produto != null)
        return Results.Ok(produto);
    else
        return Results.NotFound();
});
//Delete
app.MapDelete("/prod/{Code}", ([FromRoute] string Code, ApplicationDbContext context) =>
{
    var deleteprod = context.Products.Where(q=> q.Cod == Code).FirstOrDefault();
    context.Products.Remove(deleteprod);
    context.SaveChanges();

    return Results.Ok();
});
//Update
app.MapPut("/prod", (ProductRequest productRequest, ApplicationDbContext context)  =>
{
    var prodSaved = context.Products.Where(q=> q.CategoryId == productRequest.CategoryId).FirstOrDefault();
    prodSaved.Name = productRequest.Name;
    prodSaved.Description = productRequest.Description;
    prodSaved.Cod = productRequest.Cod;
    context.SaveChanges();
    return Results.Ok();
});
app.Run();
