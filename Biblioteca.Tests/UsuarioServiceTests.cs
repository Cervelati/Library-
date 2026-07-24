using System.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;      
using Biblioteca.Api.Models;    
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;  
using Xunit;
using Xunit.Sdk;


namespace Biblioteca.Tests;

public class UsuarioServiceTests
{
        private BibliotecaDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<BibliotecaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco novo e isolado a cada teste
            .Options;

        return new BibliotecaDbContext(options);
    }

    [Fact]
    public async Task CriarAsync_DeveDefinirDataCadastroAutomaticamente()
    {
        using var context = CriarContextoEmMemoria();
        var service = new UsuarioService(context);
        var usuario = new Usuario 
        { 
            Nome = "Usuario", 
            Email = "email@email.com" 
        };

        var antes = DateTime.UtcNow;
        var resultado = await service.CriarAsync(usuario);
        var depois = DateTime.UtcNow;

        Assert.NotNull(resultado);
        Assert.True(resultado.DataCadastro >= antes && resultado.DataCadastro <= depois);
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornarUsuario()
    {
        //Arrange
        using var context = CriarContextoEmMemoria();
        var service = new UsuarioService(context);
        var usuario = new Usuario
        {
            Nome = "Usuario",
            Email = "email@email.com",

        };
        await service.CriarAsync(usuario);

        //Act
        var usuarioBuscado = await service.BuscarPorIdAsync(usuario.Id);

        //Assert
        Assert.NotNull(usuarioBuscado);
        Assert.Equal(usuario.Id, usuarioBuscado.Id);
        Assert.Equal(usuario.Nome, usuarioBuscado.Nome);
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarUsuario()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var service = new UsuarioService(context);
        var usuario = new Usuario { Nome = "Usuario", Email = "email@email.com" };
        await service.CriarAsync(usuario);

        var usuarioNovo = new Usuario { Nome = "Novo Nome", Email = "novo@email.com" };

        // Act
        var resultado = await service.AtualizarAsync(usuario.Id, usuarioNovo);

        // Assert
        Assert.Equal("Novo Nome", resultado.Nome);
        Assert.Equal("novo@email.com", resultado.Email);

        var usuarioNoBanco = await context.Usuarios.FindAsync(usuario.Id);
        Assert.Equal("Novo Nome", usuarioNoBanco!.Nome);
    }

    [Fact]
    public async Task DeletarAsync_DeveExcluirUsuario()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var service = new UsuarioService(context);
        var usuario = new Usuario { Nome = "Usuario", Email = "email@email.com" };
        await service.CriarAsync(usuario);

        // Act
        await service.DeletarAsync(usuario.Id);

        // Assert
        var usuarioDeletado = await context.Usuarios.FindAsync(usuario.Id);
        Assert.Null(usuarioDeletado);
    }
}