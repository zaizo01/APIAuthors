using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAPIAutores.Validations;

namespace WebAPIAutores.Entities
{
    public class Autor: IValidatableObject
    {
        public int Id { get; set; }
        //[Range(18, 100, ErrorMessage = "La {0} debe estar entre {1}-{2}")]
        //[NotMapped]
        //public int Age { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:120, ErrorMessage = "El campo {0} no debe de tener mas de {1} caracteres")]
        [FirstLetterUppercaseAttribute]
        public string Name { get; set; }
        public List<Libro> Libros { get; set; }

        //public int Minor { get; set; }
        //public int Bigger { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();

                if (firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new string[] { nameof(Name) });
                }
            }

            //if (Minor > Bigger)
            //{
            //    yield return new ValidationResult("Este valor no ouede ser mas grander que el campo Mayor",
            //        new string[] { nameof(Menor) });
            //}
        }
    }
}
