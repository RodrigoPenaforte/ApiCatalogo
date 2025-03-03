📦 API Catálogo
📌 Descrição
Este projeto é uma API REST desenvolvida em ASP.NET Core para gerenciamento de um catálogo de produtos e categorias.

A API implementa boas práticas de desenvolvimento, como:
✅ DTOs (Data Transfer Objects) para desacoplar a lógica do sistema.
✅ Repositórios e serviços para maior modularidade e flexibilidade na manutenção do código.
✅ Logger para monitoramento e registro de atividades.
✅ Middleware para gestão de processos intermediários.
✅ Paginação para otimizar consultas em grandes volumes de dados.
✅ Autenticação JWT para proteger os endpoints e permitir testes via Swagger.

🚀 Tecnologias Utilizadas
ASP.NET Core
Entity Framework Core
Identity (Autenticação e Autorização)
JWT (JSON Web Token)
Swagger (Documentação da API)
AutoMapper (Mapeamento de DTOs)
MediatR (Mediador para separação de responsabilidades)

🔒 Autenticação e Segurança
Agora, os endpoints protegidos exigem um token JWT válido, e os usuários autenticados pelo Identity podem testar suas requisições diretamente no Swagger.
