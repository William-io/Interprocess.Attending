# Sistema de Atendimento Médico

Sistema gerenciamento de pacientes, clínicas e atendimentos médicos desenvolvido em ASP.NET Core 8.0 com arquitetura limpa.

## Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- SQL Server (LocalDB ou Express)

### 1. Clone o Repositório
```bash
git clone https://github.com/William-io/Interprocess.Attending.git
cd Interprocess.Attending
```

### 2. Configure o Banco de Dados
O projeto usa **SQL Server LocalDB**. A string de conexão já está configurada no `appsettings.json`.

### 3. Execute as Migrações
```bash
dotnet ef database update --project Source/Interprocess.Attending.Infrastructure --startup-project Source/Interprocess.Attending.API
```

### 4. Inicie a API
```bash
dotnet run --project Source/Interprocess.Attending.API 
```
**API disponível em:** `http://localhost:5117`  
**Swagger:** `http://localhost:5117/swagger/index.html`

**Nota:** Sendo executado em HTTP puro, evitando problemas de certificado HTTPS em desenvolvimento local , em comunicação com o APP.

### 5. Inicie o Frontend
Em outro terminal:
```bash
dotnet run --project Source/Interprocess.Attending.App
```
**App disponível em:** `http://localhost:5000`

## Funcionalidades

### Pacientes
- Cadastrar novos pacientes
- Listar todos os pacientes
- Buscar por nome ou CPF
- Editar informações
- Inativar pacientes

### Clínicas - *opcional, relacionamento com atendimento e paciente.
- Listar clínicas disponíveis
- Visualizar detalhes
```Clinic > Attendance: Uma clínica pode ter muitos atendimentos (1:N)
Patient > Attendance: Um paciente pode ter muitos atendimentos (1:N)
Clinic > Patient: Relacionamento indireto através de atendimentos (N:N via Attendance)
```

### Atendimentos
- Registrar novos atendimentos
- Listar com nome do paciente e clínica
- Filtrar por data e status
- Editar atendimentos
- Inativar atendimentos

### Dashboard
- Visão geral com contadores
- Estatísticas em tempo real

## Endpoints da API

### Pacientes `/api/patients`

#### Criar Paciente
```http
POST /api/patients
Content-Type: application/json

{
  "name": "João Silva",
  "cpf": "12345678901",
  "dateBirth": "1990-05-15",
  "sex": "1",
  "street": "Rua das Flores, 123",
  "city": "São Paulo",
  "state": "SP",
  "zipCode": "01234-567",
  "district": "Centro",
  "complement": "Apto 45"
}
```

#### Listar Pacientes
```http
GET /api/patients
```

#### Buscar Pacientes
```http
GET /api/patients/filters?name=João&cpf=123&status=1
```

#### Atualizar Paciente
```http
PUT /api/patients/{id}
Content-Type: application/json

{
  "name": "João Carlos Silva",
  "cpf": "123.456.789-01",
  "dateBirth": "1990-05-15",
  "sex": "1",
  "street": "Rua das Flores, 456",
  "city": "São Paulo",
  "state": "SP",
  "zipCode": "01234-567",
  "district": "Centro",
  "complement": "Apto 123"
}
```

#### Inativar Paciente
```http
DELETE /api/patients/{id}
```

### Clínicas `/api/clinics`

#### Listar Clínicas
```http
GET /api/clinics
```

### Atendimentos `/api/attendances`

#### Criar Atendimento
```http
POST /api/attendances
Content-Type: application/json

{
  "clinicId": "9845bdfb-cad4-4a29-8e42-1b279a4e6693",
  "patientId": "9a0e6e5f-8289-42b3-8f0b-78a07ea9c6d8",
  "description": "Consulta de rotina",
  "startedDate": "2025-07-17T14:30:00"
}
```

#### Listar Atendimentos
```http
GET /api/attendances
```

#### Buscar Atendimentos
```http
GET /api/attendances/filters?startDate=2025-07-01&endDate=2025-07-31&status=Active
```

#### Atualizar Atendimento
```http
PUT /api/attendances/{id}
Content-Type: application/json

{
  "clinicId": "9845bdfb-cad4-4a29-8e42-1b279a4e6693",
  "patientId": "9a0e6e5f-8289-42b3-8f0b-78a07ea9c6d8",
  "description": "Consulta de retorno",
  "startedDate": "2025-07-17T15:00:00"
}
```

#### Inativar Atendimento
```http
DELETE /api/attendances/{id}
```

## Valores Válidos

### Sexo do Paciente:
- `"0"` = Não Informado
- `"1"` = Masculino  
- `"2"` = Feminino

### Status:
- `"Active"` = Ativo
- `"Inactive"` = Inativo

## Tecnologias Utilizadas

### Backend:
- **ASP.NET Core 8.0** - Framework principal
- **Entity Framework Core** - ORM para banco de dados
- **MediatR** - Padrão CQRS
- **Dapper** - Queries otimizadas
- **SQL Server** - Banco de dados
- **Swagger** - Documentação da API

### Frontend:
- **HTML5** - Estrutura
- **JavaScript ES6+** - Lógica
- **Bootstrap 5** - Interface responsiva
- **Bootstrap Icons** - Ícones

### Arquitetura:
- **Clean Architecture** - Separação de responsabilidades
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - Abstração de dados
- **Unit of Work** - Controle de transações, princial do saveChanges
- **DDD** - Uso Value objects e controle de criação de entidade.

## Estrutura do Projeto

```
Interprocess.Attending/
├── Source/
│   ├── Interprocess.Attending.API/          # Web API
│   ├── Interprocess.Attending.App/          # Frontend
│   ├── Interprocess.Attending.Application/  # Casos de uso
│   ├── Interprocess.Attending.Domain/       # Regras de negócio
│   └── Interprocess.Attending.Infrastructure/ # Dados e infra
└── README.md
```

## Como Usar o Sistema

### 1. Acessar o Frontend
1. Acesse `http://localhost:5000`
2. Use o menu de navegação
3. Gerencie pacientes, atendimentos e visualize o dashboard

### 2. Testar a API
1. Acesse `http://localhost:5117/swagger`
2. Teste os endpoints diretamente
3. Use as informações do README para criar requests

### 3. Dados de Exemplo
O sistema já vem com dados iniciais (seed data) para teste:
- **Pacientes** já cadastrados
- **Clínicas** disponíveis