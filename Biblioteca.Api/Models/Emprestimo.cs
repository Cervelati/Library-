
namespace Biblioteca.Api.Models;

public class Emprestimo {

    public int Id {get; set;}
    public int LivroId {get; set;}
    public required  Livro Livro {get; set;}
    public int UsuarioId {get; set;}
    public required Usuario Usuario {get; set;}
    public DateTime DataEmprestimo {get; set;}
    public DateTime DataDevolucaoPrevista {get; set;}
    public DateTime? DataDevolucaoReal {get; set;}
}