using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class LoginViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel 
        { 
            [Required(ErrorMessage = "El campo DNI es obligatorio.")]
            [DataType(DataType.Text)]
            public int DNI { get; set; }

            [Required(ErrorMessage = "El campo Password es obligatorio.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        
        }

    }
}
