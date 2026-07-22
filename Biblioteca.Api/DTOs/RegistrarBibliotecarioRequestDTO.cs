namespace Biblioteca.Api.DTOs;

public class RegistrarBibliotecarioRequestDTO
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
}