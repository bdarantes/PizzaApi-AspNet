# 🍕 Pizzaria API - Sistema de Gestão

Projeto de back-end desenvolvido em **ASP.NET Core 8** para gerenciamento de uma pizzaria, utilizando **MariaDB** como banco de dados.

## 🚀 Tecnologias
* .NET 8.0
* Entity Framework Core 8.0
* MariaDB (Pomelo Entity Framework Core)
* Git / GitHub

## 🛠️ Como rodar o projeto
1. Clone o repositório.
2. Certifique-se de ter o MariaDB instalado e rodando.
3. Configure a string de conexão no `appsettings.json` (ver seção de configuração).
4. Execute `dotnet restore` para instalar as dependências.
5. Execute `dotnet run` para iniciar a API.

## 📋 Status das Sprints
- [x] **Sprint 0:** Setup inicial e Git.
- [x] **Sprint 1:** Infraestrutura de Dados e Segurança (Models e Usuário Dedicado).
- [x] **Sprint 2:** Configuração do Program.cs e Injeção de Dependência.
- [x] **Sprint 3:** Implementação dos Controllers (CRUD de Clientes e Produtos).
- [x] **Sprint 4:** Refatoração com DTOs (Data Transfer Objects).
- [x] **Sprint 5:** Lógica de Cálculo de Total (Business Logic).
- [x] **Sprint 6:** Implementação do Ciclo de Vida do Pedido (Fluxo de Status).
- [x] **Sprint 6.1:** Validação de Integridade de Status (Domain Validation).
- [x] **Sprint 7:** CRUD Completo de Produtos (Update/Delete).
- [x] **Sprint 8:** Implementação de Soft Delete (Conformidade com dados históricos).
- [x] **Sprint 9:** Filtros dinâmicos em Pedidos.
- [x] **Sprint 10:** Relatório Simples (Soma total de vendas).
- [x] **Sprint 11:** Desacoplamento de código (Service Layer para Pedidos, Produtos e Clientes).
- [x] **Sprint 12:** Documentação com Swagger Annotations (XML Comments).
- [x] **Sprint 13:** Padronização de Tipagem com Enums (Status de Pedido, Produto e Cliente).
- [x] **Sprint 14:** Gestão de Exceções Customizadas (NotFoundException e BusinessException).
- [x] **Sprint 15:** Otimização de Performance com Queries No-Tracking (EF Core).
- [x] **Sprint 16:** Refatoração de Controllers para Clean Controller Pattern (Tratamento Semântico de Erros).
- [x] **Sprint 17:** Implementação de Global Exception Handling (Middleware customizado).
- [x] **Sprint 18:** Exposição de novos Endpoints (GetById para Clientes) e limpeza total de blocos try/catch nas Controllers.

## 💎 Diferenciais do Projeto (O que aprendi/apliquei)
* **Tratamento Semântico de Erros:** A API não retorna apenas "Erro 500". Ela diferencia erros de negócio (400), recursos não encontrados (404) e erros de validação.
* **Performance First:** Uso estratégico de `.AsNoTracking()` em métodos de leitura para reduzir o consumo de memória e CPU.
* **Domain Integrity:** Uso de Enums para garantir que o ciclo de vida de um pedido (Pendente -> Em Preparo -> Entregue) seja respeitado, evitando estados inválidos no banco de dados.
* **Encapsulamento:** Toda a lógica de decisão foi movida para a Service Layer, mantendo as Controllers limpas e focadas apenas em responder ao protocolo HTTP.