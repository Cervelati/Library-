

namespace Biblioteca.Api.DTOs;

    public class CriarEmprestimoRequest
    {
        public required int UsuarioId {get; set;}
        public required string Nome {get; set;}
        public required string Email {get; set;}
        public required int LivroId {get; set;}

    }