using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Web.Models
{
    /// <summary>
    /// bảng khách hàng
    /// CreateBy:Lê thanh ngọc (23/11/2021)
    /// </summary>
    public class Customer
    {
        #region Declare
        #endregion

        #region Contructor
       
        #endregion

        #region Method
        #endregion

        #region property
        /// <summary>
        /// khoóa chính
        /// </summary>
    
        
        public Guid CustomerId { get; set; }
        /// <summary>
        /// mã khách hàng
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// tên đầu
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// tên cuối
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// họ và tên
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// giới tính 
        /// </summary>
        public int? Gender { get; set; }
        /// <summary>
        /// địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// mã nhóm khách hàng,khóa ngoại
        /// </summary>
        /// misacukcuk_development
        public Guid CustomerGroupId { get; set; }
        public double DebitAmount { get; set; }
        /// <summary>
        /// mã thẻ thành viên
        /// </summary>
        public string MemberCardCode { get; set; }
        /// <summary>
        /// tên công ty
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// mã số thuế công ty
        /// </summary>
        public string CompanyTaxCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        #endregion

    }
}
