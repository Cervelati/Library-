using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;
using Biblioteca.Api.Models;
using BCrypt.Net;

namespace Biblioteca.Api.Services;

    public class BibliotecarioService 
    {
        private readonly BibliotecaDbContext _context;

        public BibliotecarioService (BibliotecaDbContext context)
        {
            _context = context;
        }

        public async Task<Bibliotecario> CriarBibliotecarioAsync(Bibliotecario bibliotecario)
        {
            bibliotecario.HashSenha = BCrypt.Net.BCrypt.HashPassword(bibliotecario.HashSenha);

            _context.Bibliotecario.Add(bibliotecario);
            await _context.SaveChangesAsync();
            return bibliotecario;
        }

        public async Task <Bibliotecario> LoginBibliotecarioAsync(string email, string senha)
        {
           
            var verificador = await _context.Bibliotecario.FirstOrDefaultAsync(
            b => email == b.Email);

            if (verificador == null)
            {
                throw new KeyNotFoundException("Dados inválidos!");
            }
            
            bool isCorrect = BCrypt.Net.BCrypt.Verify(senha, verificador.HashSenha);
            if (isCorrect == false)
            {
                throw new KeyNotFoundException("Dados inválidos!");
            }

            return (verificador);
        }
    }