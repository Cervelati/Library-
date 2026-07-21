using Microsoft.AspNetCore.Mvc;
using Biblioteca.Api.Models;
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;

namespace Biblioteca.Api.Controllers;

    [ApiController]
    [Route("api/[Controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController (UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task <IActionResult> ListadeTodosCAsync ()
        {
            var usuarioBuscado = await _usuarioService.ListarTodosAsync();

            return Ok (usuarioBuscado);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> BuscarPorIdCAsync (int id)
        {
            var usuarioBuscado = await _usuarioService.BuscarPorIdAsync(id);


            return Ok (usuarioBuscado);
        }

        [HttpPost]
        public async Task <IActionResult> CriarCAsync (CriarUsuarioRequestDTO request)
        {
            var usuario = new Usuario {
                Nome = request.Nome,
                Email = request.Email
            };
            
            await _usuarioService.CriarAsync(usuario);
            return Created();
        }

        [HttpPatch("{id}")]
        public async Task <IActionResult> AtualizarCAsync (int id, Usuario usuario)
        {
            var usuarioAtualizado = await _usuarioService.AtualizarAsync(id, usuario);

            return Ok (usuarioAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task <IActionResult> DeletarCAsync (int id)
        {
            await _usuarioService.DeletarAsync(id);

            return NoContent();
        }

    }