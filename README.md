# GlobalSolutionsApp
---

## ReadMe Global Solutions - C-SHARP SOFTWARE DEVELOPMENT e ARQUITETURA ORIENTADA A SERVIÇOS (SOA) E WEB SERVICE
---
### ❗Problemática que queremos resolver

Eventos climáticos extremos como chuvas intensas, vendavais e tempestades têm causado frequentes quedas de energia elétrica em áreas urbanas, comprometendo serviços públicos essenciais — entre eles, os **semáforos**. Com a interrupção do funcionamento desses equipamentos, observam-se:

- *Aumento significativo de acidentes de trânsito, especialmente em cruzamentos movimentados.*
- *Congestionamentos severos, causando atrasos, estresse populacional e impacto econômico.*
- *Dependência de "flanelinhas" ou agentes improvisados para orientar o trânsito, muitas vezes sem preparo adequado.*

Nós viemos com o objetivo de atacar esse problema de semáforos afetados pela falta de energia.

---

### 🎯 Finalidade do Sistema
O objetivo principal deste sistema é simular um app para gerenciamento da nossa solução global.
Ao longo do desenvolvimento das entregas da Global Solutions tivemos como objetivo integrar ao máximo as matérias e suas entregas.

---

### 🔌 Integração com a API ViaCEP

Uma das funcionalidades centrais deste sistema é a sua capacidade de consumir dados de uma API externa para converter um **CEP (Código de Endereçamento Postal)** em um endereço completo.

### 🌐 API Utilizada

- **Nome:** ViaCEP  
- **Website Oficial:** [https://viacep.com.br](https://viacep.com.br)

### ⚙️ Como a Integração Funciona

#### 🔗 Endpoint Consumido

O sistema realiza requisições HTTP do tipo `GET` utilizando o seguinte formato de URL:

```
https://viacep.com.br/ws/{cep}/json/
```

> Onde `{cep}` é o número do CEP fornecido pelo usuário (somente os 8 dígitos).


#### 🕒 Momento da Consulta

A API ViaCEP é consultada em dois momentos principais:

- ✅ Ao **adicionar um novo farol**.
- ✅ Ao **atualizar o CEP** de um farol existente.


#### 📥 Manipulação dos Dados

- A resposta da API é um objeto JSON contendo dados como `logradouro`, `bairro`, `localidade` e `UF`.
- Esses dados são **desserializados** para um objeto C# da classe `Endereco.cs`.
- A interface do console então exibe as informações completas do endereço, enriquecendo os dados do farol.


### 🛡️ Tratamento de Erros

A implementação no arquivo `ViaCepService.cs` está preparada para tratar os seguintes cenários de erro:

- ❌ **CEP com formato inválido**
- ❌ **CEPs inexistentes** (a API retorna `"erro": true`)
- ❌ **Falhas na requisição HTTP** (ex.: problemas de conexão)


---

### ⚙️ Funções Principais do Sistema de Gerenciamento de Faróis

Este documento descreve as funcionalidades essenciais do sistema, projetado para ser uma ferramenta de console interativa e intuitiva.

#### 🔐 Sistema de Login

O acesso ao sistema é controlado por um mecanismo de login simples e direto. Para garantir a segurança e o controle do acesso, o sistema utiliza um usuário fixo e pré-configurado.

**Credenciais de Acesso:**
- **Usuário:** admin  
- **Senha:** 0000

**Acesso Controlado:**  
O login bem-sucedido é o único meio de acessar o menu principal e as funcionalidades de gerenciamento de faróis.


#### ➕ Adicionar Novo Farol

Esta função permite o cadastro de novos faróis no sistema de monitoramento. Ao selecionar esta opção, o usuário é guiado para fornecer as seguintes informações:

- **Nome:** Um identificador único para o farol.  
- **Status:** O estado operacional atual, selecionado de uma lista de opções (ex: *Online*, *Offline*, *Em Manutenção*).  
- **Nível de Energia:** Um valor percentual (0-100%) que indica a carga da bateria do farol.  
- **CEP:** O Código de Endereçamento Postal da localização do farol. Ao fornecer o CEP, o sistema automaticamente consome a **API ViaCEP** para buscar e associar o endereço completo ao farol.


#### 📋 Listar e Consultar Faróis

Para visualizar os dados, o sistema oferece duas funções principais:

- **Listar Todos os Faróis:**  
  Exibe um resumo de todos os faróis cadastrados na memória, mostrando suas informações essenciais de forma consolidada.

- **Ver Detalhes de um Farol Específico:**  
  Permite ao usuário buscar um farol pelo seu **Nome** ou **ID** único. O sistema exibe todos os detalhes do farol selecionado, incluindo o endereço completo obtido através da consulta ao CEP.


#### ✏️ Atualizar Farol

A funcionalidade de atualização oferece flexibilidade para gerenciar as informações dos faróis já cadastrados. O usuário pode buscar um farol pelo seu **nome** ou **ID** e modificar qualquer um de seus atributos:

- Nome  
- Status  
- Nível de Energia  
- CEP *(ao alterar o CEP, uma nova consulta à API ViaCEP é realizada para atualizar o endereço)*


#### ❌ Remover Farol

De forma simples e rápida, esta função permite a exclusão de um farol do sistema. O usuário seleciona o farol a ser removido através de seu **Nome** ou **ID**, e o registro é permanentemente apagado da lista em memória da sessão atual.

---

### 📚 Tecnologias e Conceitos Utilizados

- **Linguagem:** C#
- **Plataforma:** .NET (desenvolvimento realizado na versão 9.0)
- `System.Net.Http.Json`: Utilizado para facilitar a comunicação e a desserialização de JSON em chamadas HTTP para a API ViaCEP.

---

### Estrutura do Projeto

```
├── ViaCepConsumerApp.csproj    # Arquivo de configuração do projeto C#.
├── Program.cs                  # Ponto de entrada da aplicação, controla a UI e o fluxo do menu.
│
├── Models/                     # Contém as classes que representam os dados da aplicação.
│   ├── Endereco.cs             # Modelo para os dados de endereço retornados pela API ViaCEP.
│   ├── Farol.cs                # Modelo para os dados de um farol.
│   └── User.cs                 # Modelo para os dados do usuário do sistema.
│
└── Services/                   # Contém as classes com a lógica de negócios e acesso a dados/serviços.
    ├── AuthService.cs          # Serviço responsável pela lógica de autenticação do usuário.
    ├── FarolService.cs         # Serviço que gerencia as operações de CRUD dos faróis.
    └── ViaCepService.cs        # Serviço que encapsula a lógica para consumir a API ViaCEP.
```

---

### 🚀 Instruções de Execução

Siga os passos abaixo para compilar e executar o projeto em sua máquina local.

#### ✅ Pré-requisitos

É necessário ter o SDK do .NET instalado. Recomenda-se a versão **6.0 LTS ou superior**.  
Você pode baixá-lo em: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

#### ▶️ Passos para Execução

##### 1. Clone o Repositório

Abra um terminal (PowerShell, Command Prompt, etc.) e clone o repositório do projeto:


```
git clone https://github.com/JoaoLucasYudi/GlobalSolutionsApp.git
```

##### 2. Execute a Aplicação

```
dotnet run
```

#### 🧭 Como Usar

Ao iniciar, o aplicativo solicitará o login. Utilize as seguintes credenciais fixas:

Usuário: admin

Senha: 0000

Após o login bem-sucedido, o menu principal será exibido, permitindo o acesso a todas as funcionalidades de gerenciamento de faróis.

Siga as instruções exibidas em cada menu para interagir com o sistema.