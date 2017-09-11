using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Models
{
    public class Translation
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        [ForeignKey("CultureId")]
        public int CultureId { get; set; }
        [StringLength(4000)]
        public string Text { get; set; }
        public virtual Culture Culture { get; set; }
        [StringLength(20)]
        public string CultureName { get; set; }
        public DateTime Modify_DT { get; set; }
    }
}
