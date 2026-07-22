# 📚 Biblioteca API

API REST para gestão de uma biblioteca — cadastro de livros, usuários e controle de empréstimos, com autenticação JWT para o bibliotecário. Desenvolvida como projeto de portfólio, com foco em arquitetura em camadas, regras de negócio reais e boas práticas de API.

## 🧰 Stack

- **ASP.NET Core Web API** (.NET 9)
- **Entity Framework Core** + **Npgsql** (provider PostgreSQL)
- **PostgreSQL**, rodando em container **Docker**
- **JWT** (autenticação manual, sem ASP.NET Core Identity) + **BCrypt.Net** (hash de senha)
- **Scalar** para documentação e testes interativos da API

## 🏗️ Arquitetura

Camadas: **Controller → Service → DbContext** (sem camada Repository — o `DbContext` já cumpre esse papel).

```
Biblioteca.Api/
├── Controllers/     # Endpoints HTTP
├── Services/        # Regras de negócio
├── Models/          # Entidades (mapeadas pelo EF Core)
├── DTOs/            # Objetos de entrada/saída da API
└── Data/            # BibliotecaDbContext
```

Namespaces explícitos por camada (`Biblioteca.Api.Models`, `Biblioteca.Api.Data`, etc).

## 🗂️ Entidades

- **Livro** — Título, Autor, ISBN, Ano de Publicação, Estoque
- **Usuario** — Nome, Email, Data de Cadastro (quem pega o livro emprestado)
- **Emprestimo** — vincula Livro e Usuario, com Data de Empréstimo, Data de Devolução Prevista e Real
- **Bibliotecario** — Nome, Email, Hash de Senha (autenticação)

## 🔐 Autenticação

Todos os endpoints de Livro, Usuario e Emprestimo exigem autenticação — não é um catálogo público, é um sistema de uso interno da biblioteca. Qualquer pessoa registrada é um bibliotecário com acesso total (sem sistema de papéis/permissões).

```
POST /api/Auth/registrar   → cria um bibliotecário (senha é hasheada com BCrypt)
POST /api/Auth/login       → retorna um token JWT
```

Endpoints protegidos exigem o header:
```
Authorization: Bearer <token>
```

## 📋 Regras de negócio

- Um usuário pode ter no máximo **3 empréstimos ativos** simultaneamente
- Um empréstimo só é criado se o livro tiver **estoque disponível**
- Ao criar um empréstimo, o bibliotecário confirma a identidade do usuário conferindo **Id + Nome + Email**
- Estoque é decrementado na criação do empréstimo e devolvido na devolução
- Erros de negócio (não encontrado, regra violada) retornam respostas HTTP limpas (404/400) via middleware global de exceções — nunca um 500 cru

## ▶️ Rodando localmente

**1. Suba o banco no Docker:**
```powershell
docker run --name biblioteca-db -e POSTGRES_PASSWORD=suasenha -p 5433:5432 -d postgres
```

**2. Configure a connection string** (User Secrets):
```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5433;Database=biblioteca-db;Username=postgres;Password=suasenha"
dotnet user-secrets set "Jwt:Key" "<chave-aleatória-de-32+-caracteres>"
dotnet user-secrets set "Jwt:Issuer" "BibliotecaApi"
```

**3. Aplique as migrations:**
```powershell
dotnet ef database update
```

**4. Rode a API:**
```powershell
dotnet run
```

**5. Acesse a documentação interativa (Scalar):**
```
http://localhost:5120/scalar/v1
```

## 📌 Endpoints principais

| Recurso | Rota base | Autenticação |
|---|---|---|
| Auth | `/api/Auth` (registrar, login) | Pública |
| Livro | `/api/Livro` (GET, POST, PUT, PATCH, DELETE) | Protegida |
| Usuario | `/api/Usuario` (GET, POST, PATCH, DELETE) | Protegida |
| Emprestimo | `/api/Emprestimo` (GET, POST, PATCH devolução) | Protegida |