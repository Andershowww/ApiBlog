namespace ApiBlog.DTO
{
    public class ResultadoAcao
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }

        public static ResultadoAcao Ok(string msg = "Sucesso") => new() { Sucesso = true, Mensagem = msg };
        public static ResultadoAcao Falha(string msg) => new() { Sucesso = false, Mensagem = msg };
    }
}
