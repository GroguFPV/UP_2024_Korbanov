//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UP_2024.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Street
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> Id_City { get; set; }
    
        public virtual City City { get; set; }
    }
}
