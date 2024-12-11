namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Alerts = new HashSet<Alert>();
            this.Logs = new HashSet<Log>();
            this.Products = new HashSet<Product>();
        }

        [Key]  // Definir como clave primaria (opcional si se usa convenciones por defecto)
        public int UserID { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder los 100 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, ErrorMessage = "La contraseña no puede exceder los 255 caracteres.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no puede exceder los 50 caracteres.")]
        public string Role { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de intentos fallidos debe ser un número positivo.")]
        public int? FailedLoginAttempts { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "La fecha de bloqueo debe tener un formato válido.")]
        public DateTime? AccountLockedUntil { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "La fecha de creación debe tener un formato válido.")]
        public DateTime? CreatedAt { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Status { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
