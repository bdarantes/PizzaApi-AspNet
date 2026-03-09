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