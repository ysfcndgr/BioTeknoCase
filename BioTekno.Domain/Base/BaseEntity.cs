using System;
using System.ComponentModel.DataAnnotations;

namespace BioTekno.Domain.Base
{
    public abstract class BaseEntity
    {
        #region Base Properties
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }

        #endregion Base Properties
    }
}

