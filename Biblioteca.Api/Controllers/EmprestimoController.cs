using Microsoft.AspNetCore.Mvc;
using Biblioteca.Api.Models;
using Biblioteca.Api.Services;
using Biblioteca.Api.DTOs;

namespace Biblioteca.Api.Controllers;

    [ApiController]
    [Route("api/[Controller]")]
    public class EmprestimoController : ControllerBase 
    {
        private readonly EmprestimoService _emprestimoService;

        public EmprestimoController (EmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }

        [HttpGet]
        public async Task <IActionResult> BuscarEmprestimoCAsync ()
        {
            var lista = await _emprestimoService.ListarTodosAsync();

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> BuscarUmEmprestimoCAsync (int id)
        {
            var lista = await _emprestimoService. BuscarPorIdAsync(id);

            return Ok(lista);
        }

        [HttpPost]
        public async Task <IActionResult> CriarCAsync (CriarEmprestimoRequestDTO EmprestimoDTO)
        {
            var emprestimoCriado = await _emprestimoService.CriarAsync(
                EmprestimoDTO.UsuarioId,
                EmprestimoDTO.Nome,
                EmprestimoDTO.Email,
                EmprestimoDTO.LivroId);

            return Ok (emprestimoCriado);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DevolucaoCAsync(int id)
        {
            var emprestimoDevolvido = await _emprestimoService.DevolverAsync(id);
            return Ok(emprestimoDevolvido);
        }

        
    }