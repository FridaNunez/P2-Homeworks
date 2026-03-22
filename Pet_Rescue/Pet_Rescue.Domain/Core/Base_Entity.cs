using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pet_Rescue.Domain.Core

{
    public abstract class Base_Entity
    {
        [Key]

        public int Id { get; set; }
    }

}

