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
    
    public partial class HardwareFailure
    {
        public int Fail_id { get; set; }
        public Nullable<int> Equipment_id { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
    
        public virtual Equipment Equipment { get; set; }
    }
}
