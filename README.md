# DeskFlow API

API REST desenvolvida em ASP.NET Core para gerenciamento de chamados e atendimento de suporte técnico de TI.

O projeto foi desenvolvido como Projeto Final Avaliativo do módulo de Desenvolvimento Back End .NET, aplicando conceitos de arquitetura em camadas, Entity Framework Core, SQL Server, validação de dados, tratamento global de exceções e desenvolvimento de APIs RESTful.

---

## Sobre o projeto

O **DeskFlow API** é um sistema de gestão de chamados de suporte técnico.

A aplicação permite:

* cadastrar, consultar, atualizar e excluir categorias;
* cadastrar chamados de suporte;
* controlar o ciclo de vida dos chamados;
* iniciar chamados em atendimento;
* encerrar chamados com registro da solução;
* registrar interações entre solicitantes e equipe técnica;
* consultar chamados com categoria e histórico de interações;
* filtrar chamados por status, prioridade e categoria;
* impedir operações inválidas de acordo com as regras de negócio;
* utilizar tratamento global de exceções com respostas JSON padronizadas.

O projeto utiliza uma arquitetura separada em **Controllers, Services e Repositories**, mantendo as responsabilidades organizadas em diferentes camadas.

---

## Tecnologias utilizadas

* **C#**
* **.NET 10**
* **ASP.NET Core Web API**
* **Entity Framework Core 10**
* **SQL Server**
* **SQL Server Developer Edition**
* **OpenAPI**
* **Git**
* **GitHub**
* **Visual Studio Code / Editor de código**

### Pacotes principais

* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Design`
* `Microsoft.AspNetCore.OpenApi`

---

## Arquitetura

O projeto utiliza uma arquitetura em camadas:

```text
Cliente
   |
   v
Controllers
   |
   v
Services
   |
   v
Repositories
   |
   v
Entity Framework Core
   |
   v
SQL Server
```

### Controllers

Responsáveis por receber as requisições HTTP, chamar os serviços correspondentes e retornar as respostas da API.

Exemplos:

* `CategoriasController`
* `ChamadosController`
* `InteracoesController`

### Services

Contêm as regras de negócio da aplicação.

Exemplos:

* validação das informações;
* controle do ciclo de vida dos chamados;
* validação da existência de categorias;
* impedimento de interações em chamados fechados;
* impedimento da exclusão de categorias com chamados associados.

### Repositories

Responsáveis pelo acesso aos dados através do Entity Framework Core.

Exemplos:

* `CategoriaRepository`
* `ChamadoRepository`
* `InteracaoRepository`

### Data

Contém o `DeskFlowDbContext`, responsável pelo acesso ao banco de dados e pelo mapeamento das entidades.

### Middlewares

Contém o tratamento global de exceções da aplicação.

---

## Estrutura do projeto

```text
DeskFlow
├── src
│   └── DeskFlow.API
│       ├── Controllers
│       │   ├── CategoriasController.cs
│       │   ├── ChamadosController.cs
│       │   └── InteracoesController.cs
│       │
│       ├── Data
│       │   ├── DeskFlowDbContext.cs
│       │   └── Migrations
│       │
│       ├── Middlewares
│       │   └── ExceptionHandlingMiddleware.cs
│       │
│       ├── Models
│       │   ├── Entities
│       │   │   ├── Categoria.cs
│       │   │   ├── Chamado.cs
│       │   │   └── Interacao.cs
│       │   │
│       │   └── Dtos
│       │       ├── CategoriaDtos.cs
│       │       └── ChamadoDtos.cs
│       │
│       ├── Repositories
│       │   ├── CategoriaRepository.cs
│       │   ├── ChamadoRepository.cs
│       │   └── InteracaoRepository.cs
│       │
│       ├── Services
│       │   ├── CategoriaService.cs
│       │   ├── ChamadoService.cs
│       │   └── InteracaoService.cs
│       │
│       ├── Program.cs
│       ├── appsettings.json
│       └── DeskFlow.API.csproj
│
├── scripts
│   └── database.sql
│
├── .gitignore
├── README.md
└── DeskFlow.slnx
```

---

# Modelo de dados

O sistema possui três entidades principais.

## Categoria

Representa uma categoria utilizada para classificar os chamados.

Campos:

| Campo | Tipo   |
| ----- | ------ |
| Id    | int    |
| Nome  | string |

Uma categoria pode possuir vários chamados.

Relacionamento:

```text
Categoria 1 ───────── N Chamado
```

---

## Chamado

Representa uma solicitação de suporte técnico.

Campos:

| Campo           | Tipo      |
| --------------- | --------- |
| Id              | int       |
| Titulo          | string    |
| Descricao       | string    |
| Prioridade      | enum      |
| Status          | enum      |
| SolicitanteNome | string    |
| DataAbertura    | DateTime  |
| DataFechamento  | DateTime? |
| Solucao         | string?   |
| CategoriaId     | int       |

### Prioridade

```text
Baixa
Media
Alta
```

### Status

```text
Aberto
EmAndamento
Fechado
```

---

## Interacao

Representa uma interação registrada dentro de um chamado.

Campos:

| Campo        | Tipo     |
| ------------ | -------- |
| Id           | int      |
| ChamadoId    | int      |
| Autor        | string   |
| Mensagem     | string   |
| DataRegistro | DateTime |

Um chamado pode possuir várias interações.

Relacionamento:

```text
Chamado 1 ───────── N Interacao
```

---

# Regras de negócio

## Abertura de chamado

Ao criar um chamado:

* o status é automaticamente definido como `Aberto`;
* a data de abertura é registrada automaticamente;
* a categoria informada precisa existir;
* a prioridade precisa ser válida.

O cliente não precisa enviar o status nem a data de abertura.

---

## Início do atendimento

Um chamado somente pode ser iniciado quando estiver no status:

```text
Aberto
```

Após a operação:

```text
Aberto
   |
   v
EmAndamento
```

Não é permitido iniciar novamente um chamado que já esteja em atendimento ou fechado.

---

## Encerramento

Um chamado somente pode ser encerrado quando estiver:

```text
EmAndamento
```

Além disso, uma solução deve ser informada.

Após o encerramento:

```text
EmAndamento
      |
      v
   Fechado
```

Também são registradas:

* a solução;
* a data de fechamento.

---

## Interações

É possível registrar interações enquanto o chamado não estiver fechado.

Não é permitido adicionar uma interação a um chamado com status:

```text
Fechado
```

---

## Exclusão de categorias

Uma categoria não pode ser excluída caso possua chamados associados.

Essa regra é validada na camada de serviço e também é protegida pelo relacionamento configurado no Entity Framework Core.

---

# Endpoints

A API utiliza rotas RESTful com nomes no plural e em letras minúsculas.

## Categorias

### Criar categoria

```http
POST /api/categorias
```

Exemplo:

```json
{
  "nome": "Hardware e Equipamentos"
}
```

Resposta esperada:

```text
201 Created
```

---

### Listar categorias

```http
GET /api/categorias
```

---

### Consultar categoria

```http
GET /api/categorias/{id}
```

Exemplo:

```http
GET /api/categorias/1
```

---

### Atualizar categoria

```http
PUT /api/categorias/{id}
```

Exemplo:

```json
{
  "nome": "Hardware"
}
```

Resposta:

```text
204 No Content
```

---

### Excluir categoria

```http
DELETE /api/categorias/{id}
```

Resposta:

```text
204 No Content
```

Caso a categoria possua chamados associados, a API retorna erro de regra de negócio.

---

# Chamados

## Criar chamado

```http
POST /api/chamados
```

Exemplo:

```json
{
  "titulo": "Computador não liga",
  "descricao": "O computador não apresenta sinais de funcionamento.",
  "prioridade": "Alta",
  "solicitanteNome": "João da Silva",
  "categoriaId": 1
}
```

Ao criar o chamado, a API automaticamente define:

```text
Status = Aberto
DataAbertura = data/hora atual
```

Resposta:

```text
201 Created
```

---

## Listar chamados

```http
GET /api/chamados
```

A resposta inclui os dados da categoria associada.

---

## Consultar chamado

```http
GET /api/chamados/{id}
```

Exemplo:

```http
GET /api/chamados/1
```

A consulta retorna:

* dados do chamado;
* categoria;
* interações registradas.

---

## Iniciar chamado

```http
POST /api/chamados/{id}/iniciar
```

Exemplo:

```http
POST /api/chamados/1/iniciar
```

Altera:

```text
Aberto → EmAndamento
```

Resposta:

```text
204 No Content
```

---

## Encerrar chamado

```http
POST /api/chamados/{id}/encerrar
```

Exemplo:

```json
{
  "solucao": "Foi substituído o cabo de energia."
}
```

Altera:

```text
EmAndamento → Fechado
```

Também registra a data de fechamento.

Resposta:

```text
204 No Content
```

---

# Interações

## Criar interação

```http
POST /api/chamados/{id}/interacoes
```

Exemplo:

```json
{
  "autor": "Técnico",
  "mensagem": "Foi identificado um problema no componente."
}
```

Resposta:

```text
201 Created
```

A API não permite registrar novas interações em chamados fechados.

---

# Filtros de chamados

O endpoint de listagem aceita filtros opcionais.

## Por status

```http
GET /api/chamados?status=Fechado
```

## Por prioridade

```http
GET /api/chamados?prioridade=Alta
```

## Por categoria

```http
GET /api/chamados?categoriaId=1
```

## Combinação de filtros

Os filtros podem ser utilizados simultaneamente.

Exemplo:

```http
GET /api/chamados?status=Fechado&prioridade=Baixa&categoriaId=1
```

Nesse caso, a consulta retorna somente chamados que atendam a todos os filtros informados.

---

# Códigos HTTP

A API utiliza códigos HTTP de acordo com o resultado da operação.

| Código | Utilização                                  |
| ------ | ------------------------------------------- |
| 200    | Consulta realizada com sucesso              |
| 201    | Recurso criado                              |
| 204    | Operação realizada sem conteúdo de retorno  |
| 400    | Dados inválidos ou regra de negócio violada |
| 404    | Recurso não encontrado                      |
| 500    | Erro interno inesperado                     |

---

# Validação e tratamento de erros

A aplicação utiliza:

* `DataAnnotations`;
* validações adicionais na camada de Services;
* `[ApiController]`;
* `ExceptionHandlingMiddleware`.

O middleware realiza o tratamento global das exceções e retorna respostas JSON sem expor stack trace ou detalhes internos da aplicação.

Exemplo de erro:

```json
{
  "mensagem": "Chamado não encontrado."
}
```

Outro exemplo:

```json
{
  "mensagem": "Não é possível adicionar interações a um chamado fechado."
}
```

---

# Injeção de dependência

Os Services e Repositories são registrados no container de dependências do ASP.NET Core utilizando `AddScoped`.

Exemplo:

```csharp
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CategoriaService>();

builder.Services.AddScoped<ChamadoRepository>();
builder.Services.AddScoped<ChamadoService>();

builder.Services.AddScoped<InteracaoRepository>();
builder.Services.AddScoped<InteracaoService>();
```

Dessa forma, os Controllers recebem seus Services por injeção de dependência, e os Services recebem os Repositories necessários.

---

# Entity Framework Core

O acesso ao banco de dados é realizado exclusivamente através do Entity Framework Core.

O projeto utiliza:

```text
Entity Framework Core 10
SQL Server
```

O `DeskFlowDbContext` configura:

* entidades;
* chaves primárias;
* propriedades obrigatórias;
* tamanhos máximos;
* chaves estrangeiras;
* relacionamentos;
* comportamento de exclusão.

---

# Banco de dados

O banco utilizado pelo projeto é:

```text
DeskFlow
```

Servidor:

```text
localhost
```

Autenticação:

```text
Windows Authentication
```

As atualizações do banco são realizadas através das migrations do Entity Framework Core.

## Criar/atualizar o banco

Na pasta raiz do projeto:

```powershell
dotnet ef database update --project ".\src\DeskFlow.API\DeskFlow.API.csproj"
```

O comando aplica as migrations existentes e cria ou atualiza o banco de dados.

---

# Migrations

A migration inicial foi criada utilizando o Entity Framework Core.

Para criar uma nova migration após uma alteração no modelo:

```powershell
dotnet ef migrations add NomeDaMigration --project ".\src\DeskFlow.API\DeskFlow.API.csproj"
```

Depois:

```powershell
dotnet ef database update --project ".\src\DeskFlow.API\DeskFlow.API.csproj"
```

As migrations ficam armazenadas em:

```text
src/DeskFlow.API/Data/Migrations
```

---

# Script SQL

O projeto também possui um script SQL gerado a partir das migrations do Entity Framework Core:

```text
scripts/database.sql
```

Esse arquivo contém o script necessário para criação da estrutura do banco de dados a partir das migrations.

O fluxo oficial de atualização do banco durante o desenvolvimento permanece sendo:

```text
Entity Framework Core
        ↓
Migrations
        ↓
dotnet ef database update
        ↓
SQL Server
```

---

# Como executar o projeto

## Pré-requisitos

Para executar o projeto, é necessário possuir instalado:

* .NET SDK 10;
* SQL Server;
* Git.

---

## 1. Clonar o projeto

Depois que o repositório estiver publicado no GitHub:

```powershell
git clone https://github.com/sKDTs/deskflow-final.git
```

Entrar na pasta:

```powershell
cd deskflow-final
```

---

## 2. Restaurar as dependências

```powershell
dotnet restore
```

---

## 3. Atualizar o banco

```powershell
dotnet ef database update --project ".\src\DeskFlow.API\DeskFlow.API.csproj"
```

---

## 4. Executar a API

```powershell
dotnet run --project ".\src\DeskFlow.API\DeskFlow.API.csproj"
```

A aplicação será iniciada na URL informada pelo terminal.

---

# OpenAPI

Em ambiente de desenvolvimento, a API disponibiliza o documento OpenAPI através da configuração do ASP.NET Core.

O projeto utiliza:

```csharp
builder.Services.AddOpenApi();
```

e:

```csharp
app.MapOpenApi();
```

Isso permite consultar a especificação da API durante o desenvolvimento.

---

# Testes realizados

Durante o desenvolvimento foram realizados testes envolvendo:

* criação de categorias;
* consulta de categorias;
* atualização de categorias;
* exclusão de categorias;
* tentativa de exclusão de categoria com chamados associados;
* criação de chamados;
* consulta de chamados;
* consulta de chamado inexistente;
* filtros por status;
* filtros por prioridade;
* filtros por categoria;
* combinação de filtros;
* início do ciclo de vida do chamado;
* encerramento do chamado;
* validação de solução obrigatória;
* criação de interações;
* bloqueio de interação em chamado fechado;
* validação de dados obrigatórios;
* validação de identificadores;
* tratamento global de exceções;
* respostas HTTP 200, 201, 204, 400 e 404.

---

# Controle de versão

O projeto utiliza Git para controle de versão.

O histórico foi construído de forma incremental, registrando etapas reais do desenvolvimento, incluindo:

* criação da estrutura inicial;
* configuração do Entity Framework Core;
* criação das entidades;
* implementação do CRUD de categorias;
* implementação dos chamados;
* implementação do ciclo de vida;
* implementação das interações;
* implementação dos filtros;
* implementação do middleware;
* reforço das validações;
* criação do script SQL;
* documentação do projeto.

---

# Branches

O projeto utiliza branches para organização do desenvolvimento.

A branch principal é:

```text
main
```

Uma branch de desenvolvimento também é utilizada:

```text
develop
```

A branch `develop` é utilizada para alterações antes da integração com a branch principal.

---

# Requisitos implementados

O projeto contempla os principais requisitos funcionais definidos para o sistema:

* [x] Cadastro de categorias
* [x] Consulta de categorias
* [x] Atualização de categorias
* [x] Exclusão de categorias
* [x] Proteção contra exclusão de categoria com chamados
* [x] Cadastro de chamados
* [x] Status automático `Aberto`
* [x] Registro automático da data de abertura
* [x] Início do atendimento
* [x] Encerramento com solução obrigatória
* [x] Registro da data de fechamento
* [x] Cadastro de interações
* [x] Bloqueio de interações em chamados fechados
* [x] Consulta detalhada do chamado
* [x] Consulta de categoria do chamado
* [x] Consulta das interações do chamado
* [x] Filtro por status
* [x] Filtro por prioridade
* [x] Filtro por categoria
* [x] Combinação de filtros
* [x] Entity Framework Core 10
* [x] SQL Server
* [x] Migrations
* [x] Arquitetura Controllers → Services → Repositories
* [x] Injeção de dependência
* [x] Validações
* [x] Middleware global de exceções
* [x] Respostas HTTP adequadas
* [x] Script SQL
* [x] `.gitignore`

---

# Melhorias futuras

Algumas funcionalidades podem ser adicionadas em versões futuras:

* autenticação e autorização com ASP.NET Core Identity;
* autenticação JWT;
* utilização de `[Authorize]`;
* gerenciamento de usuários;
* níveis de acesso;
* paginação;
* ordenação avançada;
* pesquisa textual;
* logs estruturados;
* testes automatizados;
* documentação mais completa dos endpoints;
* integração com frontend;
* notificações de atualização de chamados.

A autenticação JWT não faz parte da implementação obrigatória atual e pode ser adicionada posteriormente como melhoria.

---

# Vídeo de apresentação

O vídeo de apresentação do projeto será disponibilizado no link abaixo:

```text
LINK_DO_VIDEO
```

O vídeo apresenta:

* objetivo do projeto;
* arquitetura;
* estrutura de pastas;
* banco de dados;
* execução da API;
* principais endpoints;
* regras de negócio;
* testes;
* fluxo de desenvolvimento;
* desafios encontrados;
* aprendizados;
* possíveis melhorias futuras.

---

# Repositório

Repositório público do projeto:

```text
https://github.com/sKDTs/deskflow-final.git
```

---

# Autor

**Kim de Tarso Vieira Cunha**

Projeto desenvolvido como parte da formação em **Desenvolvimento Back End .NET**.
