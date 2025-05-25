using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Negocio
{
    public class Equipamento : IEntidade
    {
        private static int numeroId = 1;
        public int id { get; set; }
        public string nome { get; set; }
        public decimal precoAquisicao { get; set; }
        public string numeroSerie { get; set; }
        public DateTime dataFabricacao { get; set; }
        public Fabricante fabricante { get; set; }

        public override string ToString()
        {
            return $"Id: {id} - Nome: {nome}";
        }

        public Equipamento(string nome, decimal precoAquisicao, string numeroSerie,DateTime dataFabricacao,Fabricante fabricante)
        {
            this.id = numeroId++;
            this.nome = nome;
            this.precoAquisicao = precoAquisicao;
            this.numeroSerie = numeroSerie;
            this.dataFabricacao = dataFabricacao;
            this.fabricante = fabricante;
        }

        public Equipamento() {}
    }
}