

public class Livro  {

    public int Id {get; set;}
    public required string Titulo {get; set;}
    public required string Autor {get; set;}
    public required string Isbn {get; set;}
    public DateTime AnoPublicacao {get; set;}
    public int Estoque {get; set;}
}