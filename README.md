# GestorPro

Sistema de gestão empresarial desenvolvido com .NET 10, seguindo os princípios de Clean Architecture e Domain-Driven Design (DDD). O projeto foi criado com foco em escalabilidade, manutenibilidade e boas práticas de desenvolvimento de software.

---

## 🎯 Objetivo

O GestorPro é uma API RESTful voltada para a gestão de operações empresariais, cobrindo desde o controle de usuários e permissões até o cadastro de clientes, produtos e estoque. O sistema é projetado para servir como backend de aplicações de gestão (ERP), com suporte a múltiplos perfis de acesso e regras de negócio bem definidas.

---

## ✅ Funcionalidades Atuais

### Autenticação e Autorização
- Login com e-mail e senha via JWT Bearer Token
- Controle de acesso baseado em papéis (RBAC) com os perfis: **Admin**, **Manager**, **Employee** e **Viewer**
- Tokens com expiração configurável e validação de audience/issuer

### Gestão de Usuários
- Criação, edição, listagem e desativação (soft delete) de usuários
- Regras de senha: mínimo 8 caracteres, letra maiúscula, número e caractere especial
- Unicidade de e-mail validada antes do cadastro
- Hash de senha com SHA-256

### Gestão de Clientes
- Cadastro de clientes com suporte a CPF (PF) e CNPJ (PJ), com validação completa dos dígitos verificadores
- Múltiplos endereços por cliente (cobrança, entrega ou ambos)
- Múltiplos contatos por cliente, com regra de exatamente um contato primário
- Atualização com sincronização inteligente de endereços e contatos (adiciona, atualiza ou remove)
- Desativação de clientes (soft delete)

### Gestão de Unidades de Medida
- Cadastro, listagem, edição e remoção de unidades de medida
- Associação futura com o módulo de produtos

### Módulo de Produtos (estrutura criada)
- Entidades `Product`, `ProductCategory`, `Inventory` e `InventoryMovement` já modeladas
- Configurações de banco de dados e migrations aplicadas
- Implementação de serviços prevista para próximas versões

---

## 🏗️ Arquitetura

O projeto segue uma **Clean Architecture** em camadas bem definidas:

```
GestorPro.Api          → Controllers, Middlewares, Filtros, configuração da API
GestorPro.Application  → Serviços, DTOs, ViewModels, InputModels, Validators, Mappers
GestorPro.Domain       → Entidades, Value Objects, Enums, Interfaces de repositório
GestorPro.Infra        → DbContext, Repositórios, Migrations, Unit of Work, Interceptors
GestorPro.Tests        → Testes unitários (xUnit + NSubstitute + FluentAssertions + Bogus)
```

### Padrões utilizados
- **Unit of Work** para controle de transações
- **Repository Pattern** com `BaseRepository<T>` genérico
- **Value Objects** para `Email` e `Document` (CPF/CNPJ) com validação no domínio
- **Soft Delete** para usuários e clientes
- **Global Exception Handler** com respostas padronizadas em `ProblemDetails`
- **Validation Filter** com FluentValidation integrado ao pipeline do ASP.NET Core

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | .NET 10 / ASP.NET Core |
| ORM | Entity Framework Core 10 |
| Banco de Dados | SQL Server 2022 |
| Autenticação | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) |
| Validação | FluentValidation 12 |
| Logging | Serilog (Console + File) |
| Documentação | Scalar (OpenAPI) |
| Containerização | Docker / Docker Compose |
| Testes | xUnit, NSubstitute, FluentAssertions, Bogus, AutoFixture |

---

## 🚀 Como executar

### Pré-requisitos
- [Docker](https://www.docker.com/) e Docker Compose instalados
- .NET 10 SDK (para rodar localmente sem Docker)

### Variáveis de ambiente

Copie o arquivo de exemplo e preencha os valores:

```bash
cp env.example .env
```

```env
SQL_SA_PASSWORD=SuaSenhaForte@123
GESTOR_PRO_ISSUER=GestorProIssuer
GESTOR_PRO_AUDIENCE=GestorProAudience
GESTOR_PRO_KEY=uma_chave_secreta_longa_com_pelo_menos_50_caracteres_aqui
```

### Subindo com Docker Compose

```bash
docker compose up -d
```

A API ficará disponível em `http://localhost:6000`. A documentação interativa (Scalar) estará em:

```
http://localhost:6000/scalar/v1
```

### Rodando localmente (sem Docker)

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations (com SQL Server local ou em container)
dotnet ef database update --project GestorPro.Infra --startup-project GestorPro.Api

# Rodar a API
dotnet run --project GestorPro.Api
```

---

## 🧪 Testes

```bash
dotnet test
```

A suíte de testes cobre:

- **Value Objects**: `Email` e `Document` (CPF/CNPJ)
- **Entidades de domínio**: `UnitOfMeasure`
- **Serviços de aplicação**: `UserService`, `CustomerService`, `UnitOfMeasureService`, `LoginService`, `AuthService`
- **Validators**: `CreateUserValidator`, `LoginValidator`, `CreateCustomerValidator`, `UpdateCustomerValidator`, `CreateUnitOfMeasureValidator`, `UpdateUnitOfMeasureValidator`

---

## 📋 Endpoints principais

| Método | Rota | Descrição | Acesso mínimo |
|---|---|---|---|
| POST | `/api/auth/login` | Autenticação | Público |
| POST | `/api/users` | Criar usuário | Público |
| GET | `/api/users` | Listar usuários | Manager |
| GET | `/api/users/{id}` | Buscar usuário | Manager |
| PUT | `/api/users/{id}` | Atualizar usuário | Manager |
| PUT | `/api/users/delete/{id}` | Desativar usuário | Manager |
| POST | `/api/customers` | Criar cliente | Employee |
| GET | `/api/customers` | Listar clientes | Viewer |
| GET | `/api/customers/{id}` | Buscar cliente | Viewer |
| PUT | `/api/customers/{id}` | Atualizar cliente | Employee |
| DELETE | `/api/customers/{id}` | Desativar cliente | Manager |
| POST | `/api/unitofmeasures` | Criar unidade de medida | Employee |
| GET | `/api/unitofmeasures` | Listar unidades | Viewer |
| GET | `/api/unitofmeasures/{id}` | Buscar unidade | Viewer |
| PUT | `/api/unitofmeasures/{id}` | Atualizar unidade | Autenticado |
| DELETE | `/api/unitofmeasures/{id}` | Remover unidade | Autenticado |

---

## 🗺️ Próximos passos

- [ ] Implementar serviços e endpoints do módulo de Produtos
- [ ] Implementar serviços e endpoints de Categorias de Produtos
- [ ] Implementar movimentações de estoque
- [ ] Adicionar testes de integração com banco em memória
- [ ] Implementar paginação nas listagens
- [ ] CI/CD pipeline

---

## 📄 Licença

Este projeto está licenciado sob a licença MIT. Consulte o arquivo [LICENSE.txt](LICENSE.txt) para mais detalhes.
