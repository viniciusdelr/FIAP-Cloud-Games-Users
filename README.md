# FIAP-Cloud-Games-Users

Serviço responsável pelo cadastro, login e gerenciamento de usuários da plataforma FIAP Cloud Games.


# Tecnologias

.NET 8 (ou versão usada)

Entity Framework Core

SQL Server / Azure SQL

Docker

# Funcionalidades

CRUD de usuários

Autenticação JWT

Validação de e-mail e senha

Registro de logs

Integração com outros microsserviços

# Instalação

1. Clone o repositório:
git clone https://github.com/seu-usuario/nome-repositorio.git

2. Acesse o diretório do projeto:
cd nome-repositorio

3. Instale as dependências:
dotnet restore

# Configuração

Configure o appsettings.json ou variáveis de ambiente com:

String de conexão do banco de dados

Chaves JWT

Configurações do Docker

Exemplo:

<img width="822" height="225" alt="image" src="https://github.com/user-attachments/assets/f23a91b5-dd26-41dd-bf1a-3c100970acc5" />

# Execução

dotnet run

Docker:
docker build -t nome-servico:latest .
docker run -d -p 5000:80 --name nome-servico nome-servico:latest

# Endpoints

<img width="474" height="269" alt="image" src="https://github.com/user-attachments/assets/c0093ce1-a534-4678-98a0-1c2cde716354" />

POST /register:	Cadastro de usuário

POST /auth/login:	Login e geração de token JWT

# Testes

Rodar testes unitários:

dotnet test


