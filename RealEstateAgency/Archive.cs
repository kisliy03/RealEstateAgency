//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstateAgency
{
    using System;
    using System.Collections.Generic;
    
    public partial class Archive
    {
        public int id { get; set; }
        public System.DateTime date_sale { get; set; }
        public int idOwner { get; set; }
        public int idClient { get; set; }
        public int idApartment { get; set; }
        public Nullable<System.DateTime> CertainDate { get; set; }
    }
}