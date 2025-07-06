# ApiBlog

API RESTful desenvolvida em **ASP.NET Core** para gerenciamento de usuários, posts, tags, curtidas e comentários em um sistema de blog.

---

## Prefácio

Este projeto é uma API RESTful desenvolvida em **ASP.NET Core**, que serve como backend para um sistema de blog com funcionalidades de usuários, posts, tags, curtidas e comentários.
### Padrões aplicados
Repository Pattern
Acesso ao banco de dados é abstraído por meio de interfaces e classes Repository. Isso desacopla a lógica de negócio do acesso direto ao Entity Framework Core, facilitando testes e manutenção.

DTOs (Data Transfer Objects)
Classes específicas para transferência de dados entre client e servidor, separando a estrutura da base da estrutura usada na API.

Controllers RESTful
Controllers organizados por recurso/entidade, expondo endpoints claros e padronizados para operações CRUD e outras ações específicas.

Separação por responsabilidade
Cada pasta e classe têm responsabilidade única, facilitando entendimento e manutenção do código.
---

## 🚀 Como rodar o projeto localmente

### 1. Clonar o repositório
```bash
git clone https://seurepositorio.git
cd ApiBlog
```

### 2. Configurar o arquivo appsettings.json
Configure a conexão com seu banco de dados SQL Server editando o arquivo appsettings.json. Exemplo:
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SEU_SERVIDOR;Initial Catalog=SEU_BANCO;User ID=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_AQUI",
    "Issuer": "ApiBlog",
    "Audience": "ApiBlogUser",
    "ExpireMinutes": 60
  }
}

### 3. O banco disponibilizado ja contem informacoes de usuarios, posts, tags, tudo para um teste com informações de exemplo

### 4. As rotas de login e cadastro não estão nas rotas protegidas, as demais precisam estar autenticadas com token jwt, podendo autenticar via swagger.

