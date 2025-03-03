using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.DTOs.LoginModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nome é Obrigatório")]
        public string? NomeUsuario { get; set; }

        [Required(ErrorMessage = "A senha é Obrigatório")]
        public string? Senha { get; set; }
    }
}