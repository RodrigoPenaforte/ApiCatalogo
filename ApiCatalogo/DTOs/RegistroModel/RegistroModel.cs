using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.DTOs.RegistroModel
{
    public class RegistroModel
    {
        [Required(ErrorMessage = "Nome é Obrigatório")]
        public string? NomeUsuario { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email é Obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é Obrigatório")]
        public string? Senha { get; set; }
    }
}