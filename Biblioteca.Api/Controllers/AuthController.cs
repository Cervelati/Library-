using Microsoft.AspNetCore.Mvc;
using Biblioteca.Api.Models;
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;

namespace Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BibliotecarioService _bibliotecarioService;

    public AuthController(BibliotecarioService bibliotecarioService)
    {
        _bibliotecarioService = bibliotecarioService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(RegistrarBibliotecarioRequestDTO request)
    {
        var bibliotecario = new Bibliotecario
        {
            Nome = request.Nome,
            Email = request.Email,
            HashSenha = request.Senha
        };

        var criado = await _bibliotecarioService.CriarBibliotecarioAsync(bibliotecario);
        return Created(string.Empty, criado);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO request)
    {
        var bibliotecario = await _bibliotecarioService.LoginBibliotecarioAsync(request.Email, request.Senha);
        var token = _bibliotecarioService.GerarToken(bibliotecario);
        return Ok(new { token });
    }
}