using Microsoft.AspNetCore.Mvc;
using Biblioteca.Api.Models;
using Biblioteca.Api.Services;

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
    public async Task <IActionResult> CriarLivroAsync(Livro livro)
    {
        var livroCriado = await _livroService.CriarAsync(livro);
        return Created(string.Empty, livroCriado);
    }

    [HttpPatch("{id}")]
    public async Task <IActionResult> AtualizarEstoqueAsync (int id, int estoque)
    {
        await _livroService.AtualizarEstoqueAsync(id, estoque);


        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> AtualizarLivroAsync (int id, Livro livro)
    {
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
