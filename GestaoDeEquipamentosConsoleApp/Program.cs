using System.Runtime.Intrinsics.X86;

namespace GestaoDeEquipamentosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Equipamento equipamento=new Equipamento();
        }
    }

    internal class Equipamento
    {
        public int id;
        public string nome;//nome.length>6
        public decimal precoAquisicao;
        public string numeroSerie;
        public DateTime dataFabricacao;
        public string fabricante;
    }
}
