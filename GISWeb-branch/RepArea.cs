//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GISWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class RepArea
    {
        public int RepAreaId { get; set; }
        public string RepName { get; set; }
        public int SalesRepId { get; set; }
        public int PostalCodeID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<bool> Archived { get; set; }
    }
}
