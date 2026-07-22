using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;
using Biblioteca.Api.Models;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Biblioteca.Api.Services;

    public class BibliotecarioService 
    {
        private readonly BibliotecaDbContext _context;
        private readonly IConfiguration _configuration;

        public BibliotecarioService (BibliotecaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public string GerarToken (Bibliotecario bibliotecario)
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, bibliotecario.Id.ToString()),
                new Claim(ClaimTypes.Email, bibliotecario.Email)
                
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }