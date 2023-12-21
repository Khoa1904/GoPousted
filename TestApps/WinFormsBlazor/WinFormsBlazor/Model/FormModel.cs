using System.ComponentModel.DataAnnotations;

namespace WinFormsBlazor.Model
{
    public class FormModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string? UserName { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Mobile is too long.")]
        public string? UserMobile { get; set; }
    }
}
