using Microsoft.EntityFrameworkCore;
using Biblioteca.Api.Models;
using Biblioteca.Api.Data;

namespace Biblioteca.Api.Services;

public class UsuarioService {

    private readonly BibliotecaDbContext _context;
    public UsuarioService (BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task <Usuario> CriarAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task <List<Usuario>> ListarTodosAsync()
    {
        var resultado = await _context.Usuarios.ToListAsync();
        return resultado;
        
    }

    public async Task<Usuario> BuscarPorIdAsync(int id) 
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) 
        {
            throw new KeyNotFoundException ($"Usuário {id} não encontrado");
        }

        return usuario;
    }

    public async Task <Usuario> AtualizarAsync (int id, Usuario usuarioAtualizado)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) 
        {
            throw new KeyNotFoundException ($"Usuário {id} não encontrado");
        }
        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Email = usuarioAtualizado.Email;

        await _context.SaveChangesAsync();
        return usuario;
    }
    
    public async Task DeletarAsync (int id) 
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) 
        {
            throw new KeyNotFoundException ($"Usuário {id} não encontrado");
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }

}