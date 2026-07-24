using System.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;      
using Biblioteca.Api.Models;    
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;  
using Xunit;
using Xunit.Sdk;


namespace Biblioteca.Tests;

public class LivroServiceTests
{
    private BibliotecaDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<BibliotecaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco novo e isolado a cada teste
            .Options;

        return new BibliotecaDbContext(options);
    }

    [Fact]
    public async Task CriarAsync_DeveAdicionarLivroComSucesso()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);
        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };

        // Act
        var resultado = await service.CriarAsync(livro);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Clean Code", resultado.Titulo);
        Assert.Equal(1, context.Livros.Count());
    }

    [Fact]
    public async Task AtualizarEstoqueAsync_DeveAtualizarEstoqueDoLivro()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);

        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        // Act
        await service.AtualizarEstoqueAsync(livro.Id, 5);

        // Assert
        var livroAtualizado = await context.Livros.FindAsync(livro.Id);
        Assert.NotNull(livroAtualizado);
        Assert.Equal(5, livroAtualizado.Estoque);
    }

    [Fact]
    public async Task AtualizarEstoqueNegativo_DeveJogarThrows()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);
        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        context.Livros.Add(livro);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.AtualizarEstoqueAsync(livro.Id, -20)
        );
    }

    [Fact]
    public async Task BuscarLivroPorId_DeveVoltarUmLivro()
    {
         //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);
        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        context.Livros.Add(livro);
        await context.SaveChangesAsync();

        //Act
        var resultado = await service.BuscarPorIdAsync(livro.Id);

        //Assert
        Assert.NotNull(resultado);
        Assert.Equal(livro.Id, resultado.Id);
        Assert.Equal("Clean Code", resultado.Titulo);
    }

    [Fact]
    public async Task AtualizarLivro_DeveAtualizarTudoNoLivro()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);
        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        await service.CriarAsync(livro);
        await context.SaveChangesAsync();

        //Act 
        var livroNovo = new Livro
        {
            Titulo = "Pinoquio",
            Autor = "Robert",
            Isbn = "9780132350885",
            AnoPublicacao = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 10
        };
        var resultado = await service.AtualizarAsync(livro.Id, livroNovo);
        await context.SaveChangesAsync();

        //Assert
        Assert.Equal("Pinoquio", resultado.Titulo);
        Assert.Equal("Robert", resultado.Autor);

        var livroNoBanco = await context.Livros.FindAsync(livro.Id);
        Assert.Equal("Pinoquio", livroNoBanco!.Titulo);
    }

    [Fact]
    public async Task DeletarLivro_DeveExcluirOLivro()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new LivroService(context);
        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        await service.CriarAsync(livro);
        await context.SaveChangesAsync();

        //Act
        await service.DeletarAsync(livro.Id);

        //Assert
        var livroDeletado= await context.Livros.FindAsync(livro.Id);
        Assert.Null(livroDeletado);  
    }
}