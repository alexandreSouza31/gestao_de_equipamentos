using GestaoDeEquipamentosConsoleApp.Compartilhado;

namespace GestaoDeEquipamentosConsoleApp.Negocio
{
    public class Chamado : IEntidade
    {
        public static int numeroId = 1;
        public int id { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public DateTime dataAbertura { get; set; }
        public Equipamento equipamento { get; set; }

        public Chamado(string titulo, string descricao, DateTime dataAbertura,Equipamento equipamento)
        {
            this.titulo = titulo;
            this.descricao = descricao;
            this.dataAbertura = dataAbertura;
            this.equipamento = equipamento;
        }
    }
}