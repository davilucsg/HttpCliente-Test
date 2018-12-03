using AL.Agencia.Repositorios.Base.ClientesRest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HttpCliente_Basico
{
    public class ClienteRest
    {
        public string EndPoint { get; set; }
        public string TipoConteudo { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public bool Sucesso { get; set; }
        public string ConteudoResposta { get; set; }

        private readonly List<CabecalhoRequiscaoHttpRest> cabecalhoRequiscaoHttpRest;


        public ClienteRest(string url)
        {
            EndPoint = url;
            TipoConteudo = "application/json";

            cabecalhoRequiscaoHttpRest = new List<CabecalhoRequiscaoHttpRest>();
        }

        public void FazerRequisicaoPost<T>(string recursoUri, T corpoMensagem)
        {
            HttpClient cliente = ObterCliente();

            using (HttpResponseMessage resposta = cliente.PostAsJsonAsync<T>(recursoUri, corpoMensagem).Result)
            {
                TratamentoDeResposta(resposta);
            }
        }

        public void FazerRequisicaoDelete(string recursoUri)
        {
            HttpClient cliente = ObterCliente();

            using (HttpResponseMessage resposta = cliente.DeleteAsync(recursoUri).Result)
            {
                TratamentoDeResposta(resposta);
            }
        }

        public void FazerRequisicaoPut<T>(string recursoUri, T corpoMensagem)
        {
            HttpClient cliente = ObterCliente();

            using (HttpResponseMessage resposta = cliente.PutAsJsonAsync<T>(recursoUri, corpoMensagem).Result)
            {
                TratamentoDeResposta(resposta);
            }
        }

        public void FazerRequisicaoGet(string uriRecurso)
        {
            HttpClient cliente = ObterCliente();

            using (HttpResponseMessage resposta = cliente.GetAsync(uriRecurso).Result)
            {
                TratamentoDeResposta(resposta);
            }
        }

        public T ObterResposta<T>()
        {
            if (ConteudoResposta.GetType() == typeof(T))
                return (T)(object)ConteudoResposta;

            return JsonConvert.DeserializeObject<T>(ConteudoResposta);
        }

        public T Cast<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void AdicionarValoreEmCabecalho(string nome, string valor)
        {
            cabecalhoRequiscaoHttpRest.Add(new CabecalhoRequiscaoHttpRest(nome, valor));
        }

        private HttpClient ObterCliente()
        {
            return HttpClientFabrica.ObterCliente(EndPoint, cabecalhoRequiscaoHttpRest);
        }

        private void TratamentoDeResposta(HttpResponseMessage response)
        {
            HttpStatus = response.StatusCode;
            Sucesso = response.IsSuccessStatusCode;
            ConteudoResposta = response.Content.ReadAsStringAsync().Result;
        }
    }
}
