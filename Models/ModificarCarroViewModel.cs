using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class ModificarCarroViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {

            [Required]
            public int ID { get; set; }

            [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
            [Range(1, int.MaxValue)]
            public int Cantidad { get; set; }

        }


    }
}
