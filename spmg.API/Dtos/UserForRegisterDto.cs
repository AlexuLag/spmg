using System.ComponentModel.DataAnnotations;

namespace spmg.API.Dtos
{
    public class UserForRegisterDto
    {

        [Required]
        public string   Username { get; set; }
        
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Debe ingresar un password entre 8 y 4 caracteres")]
        public string Password { get; set; }
    }
}