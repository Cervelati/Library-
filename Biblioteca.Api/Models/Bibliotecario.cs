namespace Biblioteca.Api.Models;

    public class Bibliotecario 
    {
        public  int Id { get; set; }
        public required string Nome {get; set; }
        public required string Email {get; set; }
        public required string HashSenha {get; set; }
    }