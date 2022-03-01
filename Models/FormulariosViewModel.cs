using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Plataformas_De_Desarrollo.Models
{
    public class FormulariosViewModel
    {
        [BindProperty]
        public LoginInputModel LoginInput { get; set; }

        [BindProperty]
        public RegisterInputModel RegisterInput { get; set; }

        [BindProperty]
        public AgregarInputModel AgregarInput { get; set; }        

        [BindProperty]
        public ModificarInputModel ModificarInput { get; set; }

        [BindProperty]
        public ModificarDatosInputModel ModificarDatosInput { get; set; }
        
        [BindProperty]
        public CambiarPasswordInputModel ModificarPasswordInput { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        public class LoginInputModel
        {
            [Required(ErrorMessage = "El campo DNI es obligatorio.")]
            [DataType(DataType.Text)]
            public int DNI { get; set; }

            [Required(ErrorMessage = "El campo Password es obligatorio.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }
        public class RegisterInputModel
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

        public class AgregarInputModel
        {

            [Required]
            public int ID { get; set; }

            [Required(ErrorMessage = "Ingrese cantidad de unidades para agregar al carro.")]
            public int Cantidad { get; set; }
        
        }
        
        public class ModificarInputModel
        {
            [Required]
            public int ID { get; set; }

            [Required(ErrorMessage = "El campo Cantidad debe contener un numero mayor a 0.")]
            public int Cantidad { get; set; }

        }

        public class ModificarDatosInputModel
        {

            [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
            [DataType(DataType.Text)]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
            [DataType(DataType.Text)]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo Email es obligatorio.")]
            [DataType(DataType.Text)] 
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo CUIT/CUIL es obligatorio.")]
            //[ StringLength(20, MinimumLength = 2)]
            [DataType(DataType.Text)]
            public int CUIT_CUIL { get; set; }

        }

        public class CambiarPasswordInputModel
        {
            [Required(ErrorMessage = "El campo contraseña anterior es obligatorio.")]
            [DataType(DataType.Password)]
            public string PasswordAnterior { get; set; }

            [Required(ErrorMessage = "El campo contraseña nueva es obligatorio.")]
            [DataType(DataType.Password)]
            public string PasswordNueva { get; set; }

            [Required(ErrorMessage = "El campo contraseña nueva es obligatorio.")]
            [DataType(DataType.Password)]
            public string PasswordNueva2 { get; set; }

        }

    }
}
