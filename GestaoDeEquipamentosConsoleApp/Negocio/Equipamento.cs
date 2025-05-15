namespace GestaoDeEquipamentosConsoleApp.Negocio
{
    public class Equipamento
    {
        public static int numeroId = 1;
        public int id;
        public string nome;
        public decimal precoAquisicao;
        public string numeroSerie;
        public DateTime dataFabricacao;
        public Fabricante fabricante;

        public override string ToString()
        {
            return $"Id: {id} - Nome: {nome}";
        }

        public Equipamento(string nome, decimal precoAquisicao, string numeroSerie,DateTime dataFabricacao,Fabricante fabricante)
        {
            this.nome = nome;
            this.precoAquisicao = precoAquisicao;
            this.numeroSerie = numeroSerie;
            this.dataFabricacao = dataFabricacao;
            this.fabricante = fabricante;
        }
    }
}