using STR_CRL_API_COMALM.EL.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.EL
{
    public class Usuario
    {
        public int usuarioId { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
        public string username {get;set;}
        public string password { get;set;}
        public Complemento rol { get; set; }
        public Filial filial { get; set; }
    }
}
