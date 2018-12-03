using System;
using System.Net.Http;

namespace HttpCliente_Basico
{
    class Program
    {
        private const string url = "https://jsonplaceholder.typicode.com/posts";
        static void Main(string[] args)
        {
            long tempoMs = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                watch.Restart();
                RealizarChamadaComFabricaInstancia();
                watch.Stop();
                Console.WriteLine($"Tempo gasto : {watch.ElapsedMilliseconds}");

                tempoMs += watch.ElapsedMilliseconds;
            }
            Console.WriteLine($"Tempo total gasto: {tempoMs}");
            Console.ReadKey();
        }

        private static void RealizarChamadaComMultiplaInstancia()
        {
            var cliente = new HttpClient();
            using (HttpResponseMessage resposta = cliente.GetAsync(url).Result)
            {
            }
        }

        private static void RealizarChamadaComFabricaInstancia()
        {
            var cliente = new ClienteRest(url);
            cliente.FazerRequisicaoGet(null);
        }
    }
}
