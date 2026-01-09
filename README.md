# FIAP Cloud Games - Users API

Microsserviço responsável pela gestão de identidade (Identity Provider) do ecossistema FIAP Cloud Games. Este serviço gerencia o ciclo de vida dos usuários e provê autenticação via tokens JWT para os serviços de Games e Payments.

## Arquitetura e Lógica

Esta API atua como a porta de entrada de segurança.
* **Banco de Dados**: Utiliza SQL Server (hospedado no Azure SQL Database ou container local) para persistência dos dados de cadastro.
* **Segurança**: Gera tokens JWT (JSON Web Tokens) assinados. Os outros microsserviços (Games e Payments) validam esse token para autorizar requisições.
* **Hospedagem**: A aplicação roda em containers Linux orquestrados por Kubernetes (AKS) na Azure.

## Tecnologias

* .NET 8
* Entity Framework Core (Code First)
* SQL Server
* Docker
* Swashbuckle (Swagger)

## Configuração (Variáveis de Ambiente)

Para rodar localmente ou no cluster, as seguintes configurações são obrigatórias no `appsettings.json` ou como variáveis de ambiente:

* `ConnectionStrings:DefaultConnection`: String de conexão com o SQL Server.
* `Jwt:Key`: Chave secreta para assinatura do token (deve ser a mesma configurada nos outros microsserviços).
* `Jwt:Issuer`: Emissor do token (Ex: FCG_Auth).
* `Jwt:Audience`: Público do token (Ex: FCG_Gamers).

## Execução Local (Docker)

Certifique-se de que o SQL Server esteja rodando e acessível.

1. Construir a imagem:
   docker build -t fcg-users .

2. Rodar o container (exemplo injetando conexão):
   docker run -d -p 8080:80 \
     -e "ConnectionStrings:DefaultConnection=Server=host.docker.internal;Database=FcgUsersDb;User Id=sa;Password=SuaSenhaForte;" \
     --name fcg-users \
     fcg-users

## Deploy na Azure (Kubernetes)

No ambiente de produção (Azure), o funcionamento segue este fluxo:
1. A imagem Docker é enviada para o Azure Container Registry (ACR).
2. O cluster AKS (Azure Kubernetes Service) baixa a imagem.
3. As configurações sensíveis (Connection String e Chave JWT) são injetadas via **Kubernetes Secrets**.
4. O serviço é exposto internamente no cluster para validar tokens, mas acessível externamente apenas via Ingress/LoadBalancer para Login e Cadastro.

## Endpoints

A documentação completa está disponível no Swagger (/swagger/index.html) quando a aplicação está em modo Development.

### Account
* `POST /register`: Cria um novo usuário no sistema.
  * **Input**: Nome, Email, Senha.
  * **Lógica**: Criptografa a senha antes de salvar no banco.

### Auth
* `POST /Auth/Login`: Autentica um usuário existente.
  * **Input**: Email, Senha.
  * **Output**: Retorna um Token JWT (Bearer) contendo o ID do usuário (NameIdentifier) e Email.
  * **Uso**: Este token deve ser enviado no Header `Authorization` para acessar rotas protegidas nos serviços de Games e Payments.

## Testes

O projeto contém testes unitários para validar as regras de negócio e geração de token.

dotnet test