//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACC.Models
{
    using System;
    
    public partial class OrderItems_Select_Result
    {
        public long PK_OrderItem { get; set; }
        public long FK_Order { get; set; }
        public int FK_Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal LineValue { get; set; }
    }
}
