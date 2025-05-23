using System.Security.Cryptography.X509Certificates;

namespace GestaoDeEquipamentosConsoleApp.Compartilhado
{
    public abstract class TelaBase
    {
        private string nomeEntidade;

        protected TelaBase(string nomeEntidade)
        {
            this.nomeEntidade = nomeEntidade;
        }

        protected void ExibirCabecalho(string pagina)
        {
            string nomeSolucao = "Gestão de Equipamentos";
            Console.Clear();
            Console.WriteLine($"----- {nomeSolucao}/{pagina} -----");
            Console.WriteLine();
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho("");
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
    }
}
