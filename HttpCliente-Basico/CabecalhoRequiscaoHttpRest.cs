using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL.Agencia.Repositorios.Base.ClientesRest
{
    public class CabecalhoRequiscaoHttpRest
    {
        public string Chave { get; private set; }
        public string Valor { get; private set; }

        public CabecalhoRequiscaoHttpRest(string chave, string valor)
        {
            Chave = chave;
            Valor = valor;
        }
    }
}
