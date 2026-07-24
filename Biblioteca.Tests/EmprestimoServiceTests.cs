using System.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;      
using Biblioteca.Api.Models;    
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;  
using Xunit;
using Xunit.Sdk;
using System.Security.Authentication;

namespace Biblioteca.Tests;

public class EmprestimoServiceTests
{
    private BibliotecaDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<BibliotecaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco novo e isolado a cada teste
            .Options;

        return new BibliotecaDbContext(options);
    }

    [Fact]
    public async Task CriarEmprestimo_DeveRetornarEmprestimo()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new EmprestimoService(context);
        var serviceL = new LivroService(context);
        var serviceU = new UsuarioService(context);

        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        var livroCriado = await serviceL.CriarAsync(livro);

        var usuarioNovo = new Usuario 
        { 
            Nome = "Novo Nome", 
            Email = "novo@email.com" 
        };
        var usuarioCriado = await serviceU.CriarAsync(usuarioNovo);

        var verificado = await service.CriarAsync
        (
            usuarioCriado.Id,
            usuarioCriado.Nome, 
            usuarioCriado.Email, 
            livroCriado.Id
        );

        //Act
        var novoEmprestimo = await service.BuscarPorIdAsync(verificado.Id);

       //Assert
       Assert.NotNull(novoEmprestimo);
       Assert.Equal(novoEmprestimo.Id, verificado.Id); 

    }

    [Fact]
    public async Task CriarEmprestimoComEstoqueZerado_DeveRetornarException()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new EmprestimoService(context);
        var serviceL = new LivroService(context);
        var serviceU = new UsuarioService(context);

        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 0
        };
        var livroCriado = await serviceL.CriarAsync(livro);

        var usuarioNovo = new Usuario 
        { 
            Nome = "Novo Nome", 
            Email = "novo@email.com" 
        };
        var usuarioCriado = await serviceU.CriarAsync(usuarioNovo);

        //Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.CriarAsync(usuarioCriado.Id, usuarioCriado.Nome, usuarioCriado.Email, livroCriado.Id)
        );

    }

        [Fact]
    public async Task CriarEmprestimoAMais_DeveRetornarException()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new EmprestimoService(context);
        var serviceL = new LivroService(context);
        var serviceU = new UsuarioService(context);

        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 10
        };
        var livroCriado = await serviceL.CriarAsync(livro);

        var usuarioNovo = new Usuario 
        { 
            Nome = "Novo Nome", 
            Email = "novo@email.com" 
        };
        var usuarioCriado = await serviceU.CriarAsync(usuarioNovo);

        await service.CriarAsync(usuarioCriado.Id, usuarioCriado.Nome, usuarioCriado.Email, livroCriado.Id);
        await service.CriarAsync(usuarioCriado.Id, usuarioCriado.Nome, usuarioCriado.Email, livroCriado.Id);
        await service.CriarAsync(usuarioCriado.Id, usuarioCriado.Nome, usuarioCriado.Email, livroCriado.Id);

        //Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.CriarAsync(usuarioCriado.Id, usuarioCriado.Nome, usuarioCriado.Email, livroCriado.Id)
        );

    }

    [Fact]
    public async Task DevolverEmprestimo_DeveRetornarEmprestimo()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new EmprestimoService(context);
        var serviceL = new LivroService(context);
        var serviceU = new UsuarioService(context);

        var livro = new Livro
        {
            Titulo = "Clean Code",
            Autor = "Robert Martin",
            Isbn = "9780132350884",
            AnoPublicacao = new DateTime(2008, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Estoque = 5
        };
        var livroCriado = await serviceL.CriarAsync(livro);

        var usuarioNovo = new Usuario 
        { 
            Nome = "Novo Nome", 
            Email = "novo@email.com" 
        };
        var usuarioCriado = await serviceU.CriarAsync(usuarioNovo);

        var verificado = await service.CriarAsync
        (
            usuarioCriado.Id,
            usuarioCriado.Nome, 
            usuarioCriado.Email, 
            livroCriado.Id
        );

        var novoEmprestimo = await service.BuscarPorIdAsync(verificado.Id);
        Assert.NotNull(novoEmprestimo);
        Assert.Equal(novoEmprestimo.Id, verificado.Id);

        var devolvido = await service.DevolverAsync(novoEmprestimo.Id);
        Assert.NotNull(devolvido.DataDevolucaoReal);

        var livroAtualizado = await serviceL.BuscarPorIdAsync(livroCriado.Id);
        Assert.Equal(5, livroAtualizado!.Estoque);
        
    }
}