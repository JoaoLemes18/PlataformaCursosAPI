# 📚 Plataforma de Cursos - API

**Status do projeto:** 🚧 Em construção 🚧

API desenvolvida em **C# (.NET Core)** para gerenciar uma plataforma de cursos, com funcionalidades de cadastro de usuários, matrículas, lançamento de notas e geração de relatórios de desempenho.

---

## 🚀 Funcionalidades previstas

- ✅ Cadastro de usuários: Aluno, Professor, Coordenador, Administrativo e Financeiro.
- ✅ Sistema de autenticação com troca obrigatória de senha no primeiro login.
- ✅ Cadastro e gerenciamento de cursos e turmas.
- ✅ Matrícula de alunos em múltiplos cursos, com controle de status (Ativa, Trancada, Cancelada).
- ⏳ Lançamento e consulta de notas.
- ⏳ Relatórios e gráficos de desempenho.
- ✅ Sistema de permissões baseado no tipo de usuário.

> **Nota:** O projeto ainda está em desenvolvimento. Algumas funcionalidades podem estar incompletas ou sujeitas a alterações.

---

## 🛠️ Tecnologias Utilizadas

- C# 12 / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- Swagger (OpenAPI)
---

## ⚙️ Como rodar o projeto localmente

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- SQL Server 
- Visual Studio ou VSCode
- Git

### 🔧 Passos para executar

1. **Clone o repositório:**

```bash
git clone https://github.com/seu-usuario/plataforma-cursos-api.git
```

2. **Configure o banco de dados:**

- Ajuste a `ConnectionStrings` no arquivo `appsettings.json` conforme seu ambiente.

3. **Execute as migrações:**

```bash
dotnet ef database update
```

4. **Rode a aplicação:**

```bash
dotnet run
```

5. **Acesse a documentação da API:**

```
https://localhost:{porta}/swagger
```

---

## 📝 Estrutura do Projeto

- **Controllers** → Endpoints da API.
- **Models** → Entidades e DTOs.
- **Middlewares** → Tratamento global, autenticação e autorização.

---

## 📫 Como contribuir

1. Fork este repositório.
2. Crie uma branch com sua feature:

```bash
git checkout -b minha-feature
```

3. Commit suas alterações:

```bash
git commit -m 'feat: Minha nova feature'
```

4. Push para a branch:

```bash
git push origin minha-feature
```

5. Abra um Pull Request.

---

## ⚠️ Observações importantes

- O projeto está em desenvolvimento contínuo.
- Algumas rotas podem não estar disponíveis ou plenamente implementadas.
- Feedbacks e sugestões são bem-vindos!


