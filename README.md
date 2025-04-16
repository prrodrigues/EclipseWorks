#  Task Manager API

API RESTful para gerenciamento de tarefas e projetos com foco em produtividade, colaboração e controle de atividades da equipe.

---

##  Como rodar o projeto

### 1. Pré-requisitos

- Docker + Docker Compose
- .NET 8 SDK (apenas se for rodar local sem container)

### 2. Subir com Docker

```bash
export DOCKER_BUILDKIT=0
docker-compose up --build
```

- API disponível em: http://localhost:5000/swagger
- Banco de dados: PostgreSQL (porta 5432)

---

##  Testes

Para rodar os testes unitários:

```bash
dotnet test
```

- Testes localizados em: `TaskManagerApi.Tests`
- Framework: xUnit + Moq
- Cobertura com Coverlet

---

##  Estrutura do Projeto

```
TaskManagerApi/
├── Controllers/
├── Models/
├── DTOs/
├── Services/
├── Interfaces/
├── Data/
├── Enums/
├── Program.cs
├── Startup.cs
├── TaskManagerApi.csproj
TaskManagerApi.Tests/
├── Services/
├── TaskManagerApi.Tests.csproj
```

---

##  Funcionalidades

- [x] Listar, criar e remover projetos
- [x] Adicionar, atualizar, listar e remover tarefas por projeto
- [x] Adicionar comentários a tarefas
- [x] Histórico de alterações de tarefas
- [x] Validações e regras de negócio

---

##  Regras de Negócio

-  Prioridade da tarefa é imutável após criação
-  Um projeto não pode ser excluído com tarefas pendentes
-  Cada projeto possui no máximo 20 tarefas
-  Atualizações de tarefa registram histórico (quem, quando, o quê)
-  Comentários nas tarefas também ficam no histórico
-  Endpoints de relatório visíveis apenas para gerentes

---

##  Tecnologias Utilizadas

- .NET 8
- Entity Framework Core + PostgreSQL
- Docker + Docker Compose
- xUnit + Moq (testes)
- Coverlet (cobertura)
- Swagger (documentação)

---

##  Fase 2 – Refinamento

###  Perguntas para o PO

- Os comentários podem ser editados?
- Os relatórios devem conter tempo médio por tarefa?
- Vai existir hierarquia de permissões por projeto?
- Deseja notificações por tarefa vencida?

---

##  Fase 3 – Melhorias Futuras

- Autenticação via JWT
- Perfis de usuários (gerente, colaborador)
- Suporte a multiusuário com ownership por projeto
- Dashboard de produtividade
- CI/CD com GitHub Actions + Deploy Railway/Azure
- Separação por camadas + Clean Architecture

---

## Docker

### Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]
```

### docker-compose.yml

```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "5000:80"
    depends_on:
      - postgres
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=taskmanagerdb;Username=postgres;Password=postgres

  postgres:
    image: postgres:15
    restart: always
    environment:
      POSTGRES_DB: taskmanagerdb
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"
```

---

## Contato
Desenvolvido por Paulo Rodrigues
Para mais informações, e-mail para p.roberto.rodrigues@gmai.com.