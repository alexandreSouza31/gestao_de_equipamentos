﻿﻿# Gestão de Equipamentos 🛠️ 

Este é um sistema de controle de equipamentos, desenvolvido em C#, com foco no cadastro, visualização, edição e exclusão de registros de equipamentos, chamados, e fabricantes.

## Sumário

- [Visão geral](#visão-geral)
  - [Mídia](#mídia-)
  - [Funcionalidades](#funcionalidades)
  - [Desenvolvido com](#desenvolvido-com-)
  - [Estrutura do projeto](#estrutura-do-projeto-)
- [Como rodar o código?](#como-rodar-o-código-)
  - [Passo a passo - Clone ou baixe o projeto](#passo-a-passo---clone-ou-baixe-o-projeto--)
  - [Uso](#uso-)
- [Autor](#autor-)

## Visão geral

### Mídia 📷
##### GIF da aplicação - Clique no GIF para dar Play/Pause
![image](https://i.imgur.com/sdpXRJM.gif)

### Funcionalidades✅ 
- Cadastro de equipamentos com dados como:

   - Nome

   - Preço de aquisição

   - Número de série

   - Data de fabricação

   - Fabricante

- Cadastro de Chamados com dados como:

   - Título

   - Descrição

   - Data de abertura

   - associação de equipamento

- Cadastro de Fabricantes com dados como:

   - Nome

   - Email

   - Telefone


- Visualização em formato de tabela

- Edição de dados já cadastrados (com possibilidade de manter campos inalterados)

- Exclusão de registros por ID

- Geração automática de IDs sequenciais

- Mensagens interativas e prompts amigáveis no console


### Desenvolvido com 🚀

[![My Skills](https://skillicons.dev/icons?i=cs,dotnet,git&theme=light)](https://skillicons.dev)


### Estrutura do projeto 📁
```
├── GestaoDeEquipamentos.ConsoleApp
│   ├── Apresentacao
│   │   ├── TelaChamado.cs
│   │   ├── TelaEquipamento.cs
│   │   ├── TelaFabricante.cs
│   │   └── TelaMenu.cs
│   ├── Dados
│   │   ├── RepositorioChamado.cs
│   │   ├── RepositorioEquipamento.cs
│   │   └── RepositorioFabricante.cs
│   ├── Negocio
│   │   ├── Chamado.cs
│   │   ├── Equipamento.cs
│   │   └── Fabricante.cs
│   ├── Requisitos
│   │   └── requisitos_gestaoDeEquipamentos.txt
│   ├── Utils
│   │   ├── Validar
│   │   │   └── ValidarCampo.cs
│   │   ├── DesejaExcluir.cs
│   │   ├── DigitarEnterEContinuar.cs
│   │   ├── Direcionar.cs
│   │   └── ResultadoDirecionamento.cs
│   └── Program.cs
├── .gitattributes
├── .gitignore
├── GestaoDeEquipamentos.sln
└── README.md

```


### Como rodar o código? 🤖

#### ❗❗Obs: Há a necessidade de ter o .NET SDK instalado em sua máquina previamente!

#### Passo a passo - Clone ou baixe o projeto  👣👣

1. Abra o terminal do seu editor de código;
2. Navegue até a pasta onde deseja instalar o projeto;
3. Clone o projeto 
ex:``` git clone git@github.com:alexandreSouza31/gestao_de_equipamentos.git```
 ou se preferir, baixe clicando no botão verde chamado "Code" no repositório desse projeto, e depois "Download zip.


#### Uso 💻
1. Inicie o App:
Certifique-se de estar na pasta do projeto, e navegue pelo terminal até o caminho do arquivo Program.cs
```
GestaoDeEquipamentos.ConsoleApp
```
2. Compile e execute o programa: ```dotnet run```

    ou, com o arquivo Program.cs aberto clique no botão verde(Current Document(Program.cs)) para iniciar

3. Siga as instruções do menu interativo no terminal!


## Autor 😏 

<main>
<div style="display: flex; align-items: center; gap: 20px;padding-bottom: 2em">
  <img src="https://github.com/user-attachments/assets/74c712a4-9e48-4ae3-839c-46089b850a27" width="80" />
  <h3 style="margin: 0;"><i>Alexandre Mariano</i></h4>
</div>

  <p>
    <a href="https://www.linkedin.com/in/alexandresouza31/">
      <img src="https://skillicons.dev/icons?i=linkedin&theme=dark" width="50"/>
      LinkedIn
    </a> &nbsp;  |  &nbsp;
    <a href="https://github.com/alexandreSouza31">
      <img src="https://skillicons.dev/icons?i=github&theme=dark" width="50"/>
      GitHub
    </a>
  </p>
</main>


<a href="#gestão-de-equipamentos" 
   style="position: fixed; right: 10px; bottom: 20px; background-color:rgba(99, 102, 99, 0.32); color: white; padding: 1px 5px 5px; text-decoration: none; border-radius: 5px; font-size: 16px;">
   🔝Voltar ao topo🔝
</a>
