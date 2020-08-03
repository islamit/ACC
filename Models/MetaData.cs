using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACC.Models
{
   
public  class AccountMetadata
        {
        
            [Key]
            [Display(Name = "الرقم")]
            public string PK_Account;
            [Display(Name = "الحساب")]
            public string Account_Name ;
            [Display(Name = "الرصيد")]
            public string Balance;
            public decimal DP_sum;
            public decimal cr_sum;
            [Display(Name = "النوع")]
            public byte FK_Account_Type ;
            [Display(Name = "التصنيف")]
            public byte FK_Account_Category ;
            [Display(Name = "المسنخدم")]
            public string FK_Insert_User;
            [Display(Name = "تاريخ الادخال")]
            public Nullable<System.DateTime> Insert_Date;
            [Display(Name = "تاريخ التعديل")]
            public Nullable<System.DateTime> Update_Date;
            [Display(Name = "مستخدم التعديل")]
            public string FK_Update_User;
            [Display(Name = "محذوف")]
            public Nullable<bool> Is_Delete;
            [Display(Name = "تاريخ الحذف")]
            public Nullable<System.DateTime> Delete_Date;
            [Display(Name = "مستخدم الحذف")]
            public string FK_Delete_User;

        }
public class VoucherMetadata
    {
      
        [Key]
        [Display(Name = "الرقم")]
        public long PK_Voucher;
        [Display(Name = "النوع")]
        public byte FK_Voucher_Type;
        [Display(Name = "السند")]
        public int Voucher_Num;
        [Display(Name = "التاريخ")]
        [DataType(DataType.Date)]
        public System.DateTime Voucher_Date;
        [Display(Name = "الحساب")]
        public string FK_Account;
        [Display(Name = "البيان")]
        public string Voucher_Name;
        [Display(Name = "المبلغ")]
        public decimal Amount;
        [Display(Name = "رقم الحركة")]
        public Nullable<long> FK_Transaction;
        [Display(Name = "نوع الحركة")]
        public Nullable<byte> FK_Transaction_Type;
        [Display(Name = "الحالة")]
        public Nullable<byte> FK_Voucher_Status;
        [Display(Name = "المسنخدم")]
        public string FK_Insert_User;
        [Display(Name = "تاريخ الادخال")]
        public Nullable<System.DateTime> Insert_Date;
        [Display(Name = "تاريخ التعديل")]
        public Nullable<System.DateTime> Update_Date;
        [Display(Name = "مستخدم التعديل")]
        public string FK_Update_User;
        [Display(Name = "محذوف")]
        public Nullable<bool> Is_Delete;
        [Display(Name = "تاريخ الحذف")]
        public Nullable<System.DateTime> Delete_Date;
        [Display(Name = "مستخدم الحذف")]
        public string FK_Delete_User;

    }
    public   class Voucher_TypesMetadata
    {
        [Display(Name = "الرقم")]
        public byte PK_Voucher_Type;
        [Display(Name = "نوع السند")]
        public string Voucher_Type;
    }
    public class OrderMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public long PK_Order;
        [Display(Name = "البائع")]
        public byte FK_Sales_Man;
        [Display(Name = "الفاتورة")]
        public string OrderNo;
        [Display(Name = "الحساب")]
        public string FK_Account;
        [Display(Name = "التاريخ")]
        [DataType(DataType.Date)]
        public System.DateTime Order_Date;
        [Display(Name = "الكفيل")]
        public string Sponsor;
        [Display(Name = "معومات الكفيل")]
        public string Sponsor_Info;
        [Display(Name = "الاجمالي")]
        public Nullable<decimal> GTotal;
        [Display(Name = "الحركة")]
        public Nullable<long> FK_Transaction;
        [Display(Name = "دفعة اولى")]
        public Nullable<decimal> Payment;
        [Display(Name = "ملاحظات")]
        public Nullable<long> Notes;
        [Display(Name = "رقم السند")]
        public Nullable<int> Voucher_Num;
        [Display(Name = "المسنخدم")]
        public string FK_Insert_User;
        [Display(Name = "تاريخ الادخال")]
        public Nullable<System.DateTime> Insert_Date;
        [Display(Name = "تاريخ التعديل")]
        public Nullable<System.DateTime> Update_Date;
        [Display(Name = "مستخدم التعديل")]
        public string FK_Update_User;
        [Display(Name = "محذوف")]
        public Nullable<bool> Is_Delete;
        [Display(Name = "تاريخ الحذف")]
        public Nullable<System.DateTime> Delete_Date;
        [Display(Name = "مستخدم الحذف")]
        public string FK_Delete_User;
    }
public  class ItemMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public int PK_Item;
        [Display(Name = "الصنف")]
        public string Item_Name;
        [Display(Name = "السعر")]
        public decimal Item_Price; 
        
    }
    public class OrderItemMetadata
    {

        [Key]
        [Display(Name = "الرقم")]
        public long PK_OrderItem ;
        [Display(Name = "الفاتورة")]
        public long FK_Order ;
        [Display(Name = "الصنف")]
        public int FK_Item ;
        [Display(Name = "العدد")]
        public int Quantity ;
        [Display(Name = "السعر")]
        public decimal Price ;
        [Display(Name = "الاجمالي")]
        public decimal LineValue ;

    }
    public class CustomerMetadata
    {
        public CustomerMetadata()
        {
            this.Is_Delete = false;
        }
        [Key]
        [Display(Name = "الرقم")]
        public long PK_Customer;
        [Display(Name = "الملف")]
        public Nullable<long> FileNum;
        [Display(Name = "الاسم")]
        public string Customer_Name;
        [Display(Name = "الهوية")]
        public Nullable<long> IDCard;
        [Display(Name = "الرصيد")]
        [Column(TypeName = "decimal(8,0)")]
        public Nullable<decimal> Balance;
        [Display(Name = "محمول ")]
        public string Mobile1;
        [Display(Name = "محمول 2")]
        public string Mobile2;
        [Display(Name = "العنوان")]
        public string Address;
        [Display(Name = "ملاحظات")]
        public string Notes;
        [Display(Name = "الحساب")]
        public string FK_Account;
        [Display(Name = "تصنيف الزبون")]
        public Nullable<byte> FK_Customer_Type;
        [Display(Name = "المسنخدم")]
        public string FK_Insert_User;
        [Display(Name = "تاريخ الادخال")]
        public Nullable<System.DateTime> Insert_Date;
        [Display(Name = "تاريخ التعديل")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Update_Date;
        [Display(Name = "مستخدم التعديل")]
        public string FK_Update_User;
        [Display(Name = "محذوف")]
        public Nullable<bool> Is_Delete;
        [Display(Name = "تاريخ الحذف")]
        public Nullable<System.DateTime> Delete_Date;
        [Display(Name = "مستخدم الحذف")]
        public string FK_Delete_User;
       
    }

    public class Sales_ManMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public int PK_Sales_Man;
        [Display(Name = "البائع")]
        public string Sales_Man_Name;
    }

    public  class TransactionMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public long PK_Transaction;
        [Display(Name = "النوع")]
        public byte FK_Transaction_Type;
        [Display(Name = "التاريخ")]
        [DataType(DataType.Date)]
        public System.DateTime Transaction_Date;
        [Display(Name = "الحساب")]
        public string FK_Account;
        [Display(Name = "البيان")]
        public string Transaction_Name;
        [Display(Name = "المبلغ")]
        public decimal Amount;
        [Display(Name = "الحالة")]
        public Nullable<byte> FK_Transaction_Status;
        [Display(Name = "المسنخدم")]
        public string FK_Insert_User;
        [Display(Name = "تاريخ الادخال")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Insert_Date;
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ التعديل")]
        public Nullable<System.DateTime> Update_Date;
        [DataType(DataType.Date)]
        [Display(Name = "مستخدم التعديل")]
        public string FK_Update_User;
        [Display(Name = "محذوف")]
        public Nullable<bool> Is_Delete;
        [Display(Name = "تاريخ الحذف")]
        public Nullable<System.DateTime> Delete_Date;
        [Display(Name = "مستخدم الحذف")]
        public string FK_Delete_User;



    }

    public  class Transaction_TypesMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public byte PK_Transaction_Type;
        [Display(Name = "النوع")]
        public string Transaction_Type;
    }

   

    public  class AspNetUserMetadata
    {
        [Key]
        [Display(Name = "الرقم")]
        public string Id ;
        [Display(Name = "البريد")]
        public string Email ;
        [Display(Name = "تاكيد البريد")]
        public bool EmailConfirmed ;
        public string PasswordHash ;
        public string SecurityStamp ;
        [Display(Name = "المحمول")]
        public string PhoneNumber ;
        public bool PhoneNumberConfirmed ;
        public bool TwoFactorEnabled ;
        public Nullable<System.DateTime> LockoutEndDateUtc ;
        public bool LockoutEnabled ;
        public int AccessFailedCount ;
        [Display(Name = "اسم المستخدم")]
        public string UserName ;

    }
}