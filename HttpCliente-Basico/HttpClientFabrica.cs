using AL.Agencia.Repositorios.Base.ClientesRest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpCliente_Basico
{
    public static class HttpClientFabrica
    {
        private const int TEMPO_DE_EXPIRACAO_DO_CACHE_DNS_EM_MINUTOS = 1;
        private const int TIMEOUT_REQUISICAO_HTTP_EM_MINUTOS = 5;

        private static Dictionary<string, HttpClient> _clientes = new Dictionary<string, HttpClient>();

        public static HttpClient ObterCliente(string endpoint, List<CabecalhoRequiscaoHttpRest> cabecalhoNomeEValores)
        {
            HttpClient cliente = null;

            lock (_clientes)
            {
                string chave = ObterChave(endpoint, cabecalhoNomeEValores);
                if (!_clientes.TryGetValue(chave, out cliente))
                {
                    cliente = ObterNovoCliente(endpoint, cabecalhoNomeEValores);
                    _clientes.Add(chave, cliente);

                    ConfigurarTempoDeConexao(endpoint);
                }
            }

            return cliente;
        }

        private static string ObterChave(string endpoint, List<CabecalhoRequiscaoHttpRest> cabecalhoNomeEValores)
        {
            return $"{endpoint}_{string.Join(";", cabecalhoNomeEValores)}";
        }

        private static HttpClient ObterNovoCliente(string endpoint, List<CabecalhoRequiscaoHttpRest> cabecalhoNomeEValores)
        {
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(endpoint);
            cliente.Timeout = TimeSpan.FromMinutes(TIMEOUT_REQUISICAO_HTTP_EM_MINUTOS);

            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var cabelhoNomeEValor in cabecalhoNomeEValores)
            {
                cliente.DefaultRequestHeaders.Add(cabelhoNomeEValor.Chave, cabelhoNomeEValor.Valor);
            }

            return cliente;
        }

        private static void ConfigurarTempoDeConexao(string endpoint)
        {
            ServicePointManager.FindServicePoint(new Uri(endpoint)).ConnectionLeaseTimeout = (int)TimeSpan.FromMinutes(TEMPO_DE_EXPIRACAO_DO_CACHE_DNS_EM_MINUTOS).TotalMilliseconds;
        }
    }
}