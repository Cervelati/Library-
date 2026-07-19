using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Data;
using Biblioteca.Api.Models;

namespace Biblioteca.Api.Services;

    public class EmprestimoService {
        
        private readonly BibliotecaDbContext _context;
        private const int LIMITE_EMPRESTIMO = 3;

        public EmprestimoService (BibliotecaDbContext context)
        {
            _context = context;
        }
        
        public async Task <Emprestimo> CriarAsync(int usuarioId, string nome, string email, int livroId)
        {
            var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == usuarioId && u.Nome == nome && u.Email == email);
            
            if (usuario == null) 
            {
                throw new KeyNotFoundException("Dados do usuário não conferem com o cadastro.");
            }

            var emprestimosAtivo = await _context.Emprestimos
            .CountAsync(e => e.UsuarioId == usuarioId && e.DataDevolucaoReal == null);

            if (emprestimosAtivo >= LIMITE_EMPRESTIMO) 
            {
                throw new InvalidOperationException(
                $"Usuário já possui {emprestimosAtivo} empréstimos ativos (limite: {LIMITE_EMPRESTIMO}).");
            }

            var livro = await _context.Livros.FindAsync(livroId);
            if (livro == null || livro.Estoque <= 0) 
            {
                throw new KeyNotFoundException ("Livro indisponível para empréstimo!");
            }

            var emprestimo = new Emprestimo 
            {
                UsuarioId = usuarioId,
                Usuario = usuario,
                LivroId = livroId,
                Livro = livro,
                DataEmprestimo = DateTime.UtcNow,
                DataDevolucaoPrevista = DateTime.UtcNow.AddDays(14)

            };

            livro.Estoque --;
            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();

            return emprestimo;
        }
    }

    
