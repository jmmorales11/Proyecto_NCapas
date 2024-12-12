namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Column(TypeName = "decimal(10,2)")] // Especificar decimal(10,2)
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock no puede ser negativa.")]
        public int UnitsInStock { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int UserID { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
