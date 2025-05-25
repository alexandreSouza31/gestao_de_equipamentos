using GestaoDeEquipamentosConsoleApp.Dados;
using GestaoDeEquipamentosConsoleApp.Negocio;
using System.Security.Cryptography.X509Certificates;

namespace GestaoDeEquipamentosConsoleApp.Compartilhado
{
    public abstract class TelaBase<T> where T : IEntidade
    {
        protected string nomeEntidade;
        protected RepositorioBase<T> repositorio;

        protected TelaBase(string nomeEntidade, RepositorioBase<T> repositorio)
        {
            this.nomeEntidade = nomeEntidade;
            this.repositorio = repositorio;
        }

        protected void ExibirCabecalho()
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{nomeEntidade} -----");
            Console.WriteLine();
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho();
            Console.WriteLine();
            Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
            Console.WriteLine($"2 - Visualizar {nomeEntidade}");
            Console.WriteLine($"3 - Editar {nomeEntidade}");
            Console.WriteLine($"4 - Excluir {nomeEntidade}");
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

            return opcaoEscolhida;
        }

        public bool Cadastrar()
        {
            ExibirCabecalho();

            T dadosIniciais = CriarInstanciaVazia();

            var novosDados = ObterNovosDados(dadosIniciais, false);

            if (novosDados == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cadastro cancelado.");
                Console.ResetColor();
                return false;
            }

            repositorio.CadastrarRegistro(novosDados);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n {nomeEntidade} cadastrado com sucesso! ID: {novosDados.id}");
            Console.ResetColor();
            Console.Write("Digite [Enter] para continuar...");
            Console.ReadLine();
            return true;
        }

        public abstract T CriarInstanciaVazia();

        protected abstract T ObterNovosDados(T dadosIniciais, bool editar);
    }
}
