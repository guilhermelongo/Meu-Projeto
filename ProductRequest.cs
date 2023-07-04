
//representaçao do payload
public record ProductRequest(string Cod, string Name, string Description, int CategoryId, List<string> tags){

}