using System;
using System.ComponentModel.DataAnnotations;
namespace ACC.Models
{
    [MetadataType(typeof(ItemMetadata))]
    public partial class Item
    {
    }
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    {
    }
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }
    [MetadataType(typeof(OrderItemMetadata))]
    public partial class OrderItem
    {
    }
    [MetadataType(typeof(AccountMetadata))]
    public partial class Account
    {
    }

    [MetadataType(typeof(VoucherMetadata))]
    public partial class Voucher 
    {
    }

    [MetadataType(typeof(Sales_ManMetadata))]
    public partial class Sales_Man
    {
    }
    [MetadataType(typeof(TransactionMetadata))]
    public partial class Transaction
    {
    }
    [MetadataType(typeof(Transaction_TypesMetadata))]
    public partial class Transaction_Types
    {
    }
    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
    }
    [MetadataType(typeof(Voucher_TypesMetadata))]
    public partial class Voucher_Types
    {
    }
}