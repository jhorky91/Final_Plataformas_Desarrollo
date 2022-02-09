using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class RegisterViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo DNI es obligatorio.")]
            public int DNI { get; set; }

            [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
            [DataType(DataType.Text)]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
            [DataType(DataType.Text)]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo Email es obligatorio.")]
            [DataType(DataType.Text)]
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo Password es obligatorio.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "El campo CUIT/CUIL es obligatorio.")]
            //[ StringLength(20, MinimumLength = 2)]
            [DataType(DataType.Text)]
            public int CUIT_CUIL { get; set; }

            public bool esEmpresa { get; set; }

        }

    }
}
