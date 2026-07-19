using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Models;

namespace Biblioteca.Api.Data;

public class BibliotecaDbContext : DbContext {

    public BibliotecaDbContext(DbContextOptions <BibliotecaDbContext> options) : base(options) {}

    public DbSet<Livro> Livros {get; set;}
    public DbSet<Usuario> Usuarios {get; set;} 
    public DbSet<Emprestimo> Emprestimos {get; set;}
}