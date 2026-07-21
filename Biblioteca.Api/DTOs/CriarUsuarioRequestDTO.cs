namespace Biblioteca.Api.DTOs;

public class CriarUsuarioRequestDTO
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
}