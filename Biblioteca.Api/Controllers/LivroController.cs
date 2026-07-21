using Microsoft.AspNetCore.Mvc;
using Biblioteca.Api.Models;
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;

namespace Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase 
{
    private readonly LivroService _livroService;

    public LivroController (LivroService livroService)
    {
            _livroService = livroService;
    }

    [HttpGet]
    public async Task <IActionResult> ListarTodosAsync()
    {
        var livros = await _livroService.ListarTodosAsync();
        return Ok(livros);
    }

    [HttpPost]
    public async Task<IActionResult> CriarLivroAsync(CriarLivroRequestDTO request)
    {
        var livro = new Livro
        {
            Titulo = request.Titulo,
            Autor = request.Autor,
            Isbn = request.Isbn,
            AnoPublicacao = request.AnoPublicacao,
            Estoque = request.Estoque
        };

        var livroCriado = await _livroService.CriarAsync(livro);
        return Created(string.Empty, livroCriado);
    }

    [HttpPatch("{id}")]
    public async Task <IActionResult> AtualizarEstoqueAsync (int id, AtualizarEstoqueRequestDTO request)
    {
        await _livroService.AtualizarEstoqueAsync(id, request.Estoque);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> AtualizarLivroAsync (int id, AtualizarLivroRequestDTO request)
    {
       var livro = new Livro {
            Titulo = request.Titulo,
            Autor = request.Autor,
            Isbn = request.Isbn,
            AnoPublicacao = request.AnoPublicacao,
            Estoque = request.Estoque
       };
       
       
        var livroAtualizado = await _livroService.AtualizarAsync(id, livro);
        return Ok(livroAtualizado);
    }

    [HttpDelete("{id}")]
    public async Task <IActionResult> DeleteLivroAsync (int id)
    {
        await _livroService.DeletarAsync(id);

        return NoContent();
    }
    
}
