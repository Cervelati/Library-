using Biblioteca.Api.Models;
using Biblioteca.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Api.Services;

    public class LivroService 
    { 
        private readonly BibliotecaDbContext _context;
        public LivroService (BibliotecaDbContext context) 
        {
            _context = context;
        }

        public async Task <Livro> CriarAsync(Livro livro) 
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task <Livro?> BuscarPorIdAsync (int id)
        {
            return await _context.Livros.FindAsync(id);
        }

        public async Task <List<Livro>> ListarTodosAsync () 
        {
            return await _context.Livros.ToListAsync();
        }

        public async Task  AtualizarAsync (int id, Livro livroAtualizado) 
        {
            var atualizarLivro = await _context.Livros.FindAsync(id);
            if (atualizarLivro == null) {

                throw new KeyNotFoundException($"Livro {id} não encontrado!");
            }
            
            atualizarLivro.Titulo = livroAtualizado.Titulo;
            atualizarLivro.Autor = livroAtualizado.Autor;
            atualizarLivro.Isbn = livroAtualizado.Isbn;
            atualizarLivro.AnoPublicacao = livroAtualizado.AnoPublicacao;


            await _context.SaveChangesAsync();
        }

        public async Task AtualizarEstoque (int id, int estoqueAtualizado)
        {
            if (estoqueAtualizado < 0)
            {
                throw new ArgumentException ("Estoque não pode ser negativo1");
            }

            var atualizarEstoque = await _context.Livros.FindAsync(id);
            if (atualizarEstoque == null)
            {
                throw new KeyNotFoundException ($"Livro {id} não encontrado");
            }

            atualizarEstoque.Estoque = estoqueAtualizado;
            
            await _context.SaveChangesAsync();
        }

        public async Task  DeletarAsync (int id) 
        {
            var deleteLivro = await _context.Livros.FindAsync(id);
            if (deleteLivro == null){

                 throw new KeyNotFoundException($"Livro {id} não encontrado!");
            }

            _context.Livros.Remove(deleteLivro);
            await _context.SaveChangesAsync();
        }
        
    }
