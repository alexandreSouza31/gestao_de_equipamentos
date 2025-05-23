using GestaoDeEquipamentosConsoleApp.Compartilhado;
using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using GestaoDeEquipamentosConsoleApp.Utils;

namespace GestaoDeEquipamentosConsoleApp.Apresentacao
{
    public class TelaEquipamento : TelaBase
    {
        public string pagina;
        private RepositorioEquipamento repositorioEquipamento;
        private RepositorioFabricante repositorioFabricante;
        public TelaFabricante telaFabricante;
        Direcionar direcionar = new Direcionar();

        public TelaEquipamento(RepositorioEquipamento repositorioEquipamento, RepositorioFabricante repositorioFabricante, TelaFabricante telaFabricante) : base("Equipamento")
        {
            this.repositorioEquipamento = repositorioEquipamento;
            this.repositorioFabricante = repositorioFabricante;
            this.telaFabricante = telaFabricante;
        }

        public bool ExecutarMenuEquipamento(TelaEquipamento telaEquipamento)
        {
            char opcaoEscolhida = telaEquipamento.ApresentarMenu();

            if (opcaoEscolhida == 'S') return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    bool continuar = telaEquipamento.Cadastrar();
                    if (!continuar) return false;
                    break;
                case '2':
                    telaEquipamento.Visualizar(true, true);
                    break;
                case '3':
                    telaEquipamento.Editar();
                    break;
                case '4':
                    telaEquipamento.Excluir();
                    break;
                default:
                    Console.WriteLine("Digite uma opção válida!");
                    DigitarEnterEContinuar.Executar(true);
                    break;
            }
            return true;
        }

        public bool Cadastrar()
        {
            pagina = "Cadastrar";
            ExibirCabecalho(pagina);

            bool haFabricante = repositorioFabricante.VerificarExistenciaRegistros();
            var resultado = direcionar.DirecionarParaMenu(haFabricante, true, "Fabricante");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Equipamento equipamento = new Equipamento("", 0, "", new DateTime(1975, 1, 1), null);
            var novosDados = ObterNovosDados(equipamento, false);
            if (novosDados == null) return false;

            AtualizarEquipamento(equipamento, novosDados);

            equipamento.id = Equipamento.numeroId++;
            repositorioEquipamento.CadastrarRegistro(equipamento);

            Console.WriteLine($"nome: {equipamento.nome} cadastrado com sucesso! id: {equipamento.id}");
            DigitarEnterEContinuar.Executar();
            return true;
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            pagina = "Visualizar";
            if (exibirCabecalho) ExibirCabecalho(pagina);

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarRegistros();
            int encontrados = 0;

            string tamanhoCabecalhoColunas = "{0, -5} | {1, -30} | {2, -15} | {3, -15} | {4, -15} | {5, -10}";

            for (int i = 0; i < equipamentos.Length; i++)
            {
                Equipamento e = equipamentos[i];
                if (e == null) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine(
                        tamanhoCabecalhoColunas,
                        "Id".ToUpper(), "Nome".ToUpper(), "Preço Aquisicao".ToUpper(), "Numero Série".ToUpper(), "Data Fabricação".ToUpper(), "Fabricante".ToUpper()
                    );
                }

                Console.WriteLine(
                    tamanhoCabecalhoColunas,
                    e.id, e.nome, e.precoAquisicao.ToString("C2"), e.numeroSerie, e.dataFabricacao.ToShortDateString(), e.fabricante.nome
                );

                encontrados++;
            }

            if (encontrados == 0 && msgAoCadastrar) Console.WriteLine("Ainda não há equipamentos! Faça um cadastro!");

            if (digitarEnterEContinuar) DigitarEnterEContinuar.Executar();
            return encontrados > 0;
        }


        public bool Editar()
        {
            pagina = "Editar";
            ExibirCabecalho(pagina);

            bool visualizarCadastrados = Visualizar(false, false, false);
            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaRegistros();
            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, false, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarRegistros();

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do equipamento para editar: ");
                int idEscolhido = Convert.ToInt32(Console.ReadLine()!);

                Equipamento equipamentoSelecionado = null;

                for (int i = 0; i < equipamentos.Length; i++)
                {
                    Equipamento e = equipamentos[i];
                    if (e == null) continue;

                    if (idEscolhido == e.id)
                    {
                        equipamentoSelecionado = e;
                        break;
                    }
                }

                if (equipamentoSelecionado != null)
                {
                    var novosDados = ObterNovosDados(equipamentoSelecionado, true);
                    AtualizarEquipamento(equipamentoSelecionado, novosDados);

                    Visualizar(true, false);
                    Console.WriteLine();
                    Console.WriteLine($"{equipamentoSelecionado.nome} editado com sucesso! id: {equipamentoSelecionado.id}");
                    DigitarEnterEContinuar.Executar();
                    return true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("ID inválido. Tente novamente.");
                }
            }
        }

        internal bool Excluir()
        {
            pagina = "Excluir";
            ExibirCabecalho(pagina);

            bool visualizarCadastrados = Visualizar(false, false, false);
            bool haEquipamentos = repositorioEquipamento.VerificarExistenciaRegistros();

            var resultado = direcionar.DirecionarParaMenu(haEquipamentos, false, "Equipamento");
            if (resultado != ResultadoDirecionamento.Continuar) return false;

            Equipamento[] equipamentos = repositorioEquipamento.SelecionarRegistros();

            while (true)
            {
                Console.WriteLine();
                Console.Write("Digite o Id do equipamento para excluir: ");

                if (!int.TryParse(Console.ReadLine(), out int idEscolhido))
                {
                    Console.WriteLine("ID inválido. Tente novamente.");
                    continue;
                }

                var equipamento = repositorioEquipamento.SelecionarRegistroPorId(idEscolhido);

                if (equipamento == null)
                {
                    Console.WriteLine("Equipamento não encontrado. Tente novamente.");
                    continue;
                }

                DesejaExcluir desejaExcluir = new DesejaExcluir();
                var vaiExcluir = desejaExcluir.DesejaMesmoExcluir(equipamento.nome);
                if (vaiExcluir != "S") return false;

                for (int i = 0; i < equipamentos.Length; i++)
                {
                    if (equipamentos[i] != null && equipamentos[i].id == idEscolhido)
                    {
                        equipamentos[i] = null;
                        Console.WriteLine();
                        Console.WriteLine($"Equipamento {equipamento.nome} excluído com sucesso! id: {idEscolhido}");
                        DigitarEnterEContinuar.Executar();
                        return true;
                    }
                }
            }
        }

        private Equipamento ObterNovosDados(Equipamento dadosOriginais, bool editar)
        {
            while (true)
            {
                pagina = "Cadastrar";
                ExibirCabecalho(pagina);

                if (editar)
                {
                    Console.WriteLine();
                    Console.WriteLine("************* Caso não queira alterar um campo, basta pressionar Enter para ignorá-lo");
                }

                string nome = RepositorioBase<Equipamento>.ObterEntrada("Nome", dadosOriginais.nome, editar);
                decimal precoAquisicao = RepositorioBase<Equipamento>.ObterEntrada("preço Aquisição", dadosOriginais.precoAquisicao, editar);
                string numeroSerie = RepositorioBase<Equipamento>.ObterEntrada("número Série", dadosOriginais.numeroSerie, editar);
                DateTime dataFabricacao = RepositorioBase<Equipamento>.ObterEntrada("data Fabricação", dadosOriginais.dataFabricacao, editar);

                bool haFabricantes = telaFabricante.Visualizar(true, false, false);
                var resultado = direcionar.DirecionarParaMenu(haFabricantes, true, "Fabricante");
                if (resultado != ResultadoDirecionamento.Continuar)
                    return null;

                Console.Write(editar ? $"ID do Fabricante ({dadosOriginais.fabricante?.id}): " : "ID do Fabricante: ");
                string inputFabricante = Console.ReadLine()!;

                Fabricante fabricante = string.IsNullOrWhiteSpace(inputFabricante)
                    ? dadosOriginais.fabricante!
                    : repositorioFabricante.SelecionarRegistroPorId(int.Parse(inputFabricante));

                if (fabricante == null)
                {
                    Console.WriteLine("Fabricante não encontrado! Pressione Enter para continuar...");
                    Console.ReadLine();
                    continue;
                }

                string[] nomesCampos = { "nome", "preço aquisição", "numero série", "data abertura", "fabricante" };
                string[] valoresCampos = { nome, precoAquisicao.ToString(), numeroSerie, dataFabricacao.ToString(), fabricante.ToString()! };

                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);

                if (!string.IsNullOrEmpty(erros))
                {
                    Console.WriteLine("\nErros encontrados:");
                    Console.WriteLine(erros);
                    DigitarEnterEContinuar.Executar();
                    Console.Clear();
                    continue;
                }

                return new Equipamento(nome, precoAquisicao, numeroSerie, dataFabricacao, fabricante);
            }
        }


        public void AtualizarEquipamento(Equipamento dadosOriginais, Equipamento novosDados)
        {
            dadosOriginais.nome = novosDados.nome;
            dadosOriginais.precoAquisicao = novosDados.precoAquisicao;
            dadosOriginais.numeroSerie = novosDados.numeroSerie;
            dadosOriginais.dataFabricacao = novosDados.dataFabricacao;
            dadosOriginais.fabricante = novosDados.fabricante;
        }
    }
}