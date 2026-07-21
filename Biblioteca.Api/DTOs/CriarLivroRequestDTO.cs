namespace Biblioteca.Api.DTOs;

public class CriarLivroRequestDTO
{
    public required string Titulo { get; set; }
    public required string Autor { get; set; }
    public required string Isbn { get; set; }
    public required DateTime AnoPublicacao { get; set; }
    public required int Estoque { get; set; }
}