using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Models
{
    public class Culture
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool Default { get; set; }
        public ICollection<Translation> Translations { get; set; }
    }
}
