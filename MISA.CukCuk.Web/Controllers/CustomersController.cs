using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Web.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// CreateBy:Lê thanh ngọc (23/11/2021)
        /// </summary>
        /// <returns>
        ///  200 - trả lại 1 danh sách khách hàng
        ///  204 - không có dữ liệu
        /// </returns>
        // GET: api/<CustomersController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                //khai báo thông tin kết nối.
                var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database=MISA.CukCuk_Demo_NVMANH_copy;Password=manhmisa;Character Set=utf8";
                //khởi tạo chuỗi kết nối
                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                //using IDbConnection dbConnection = new MySqlConnection(connectionString);
                ////thực thi lấy dữ liệu db
                var customers = mySqlConnection.Query<object>(sql: "SELECT * FROM Customer");
                //var customers = dbConnection.Query<Customer>(sql: "Proc_GetCustomers", commandType: CommandType.Text);
                //trả về cho client
                return Ok(customers);
            }
            catch(Exception ex)
            {
                var result = new
                {
                    devMsg = ex.Message,
                    useMsg="có lỗi xảy ra,vui lòng liên hệ dev:Lê thanh Ngọc để được hỗ trợ",
                    data=DBNull.Value,
                    moreInfo=""
                };
                return StatusCode(500,result);
            }
           
        }
        
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <returns>danh sách khách hàng</returns>
        /// createBy:Lê thanh ngọc (23/11/2021)
        // GET api/<CustomersController>/5
        [HttpGet("{customerId}")]
        public IActionResult Get(Guid customerId)
        {
            try
            {
                //khai báo thông tin kết nối.
                var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database=MISA.CukCuk_Demo_NVMANH_copy;Password=manhmisa;Character Set=utf8";
                //khởi tạo chuỗi kết nối
                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                ////thực thi lấy dữ liệu db
                var sql = $"SELECT * FROM Customer WHERE CustomerId = @CustomerId";
                DynamicParameters parameters = new DynamicParameters();//khơi tạo dynamic để tránh lỗi injection    
                parameters.Add("CustomerId", customerId);
                var customers = mySqlConnection.Query<object>(sql,param:parameters);
                //using IDbConnection dbConnection = new MySqlConnection(connectionString);
                //var customers = dbConnection.Query<Customer>("Proc_GetCustomerById", new {CustomerId=id }, commandType: CommandType.StoredProcedure);
                return Ok(customers);
            }
            catch(Exception ex)
            {
                var result = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xảy ra vui lòng liên hệ dev:Lê thanh Ngọc để được hỗ trợ.",
                    data = DBNull.Value,
                    moreInfo = ""
                };
                return StatusCode(500, result);
            }
        }

        // POST api/<CustomersController>
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                //validate dữ liệu
                //check bắt buộc nhập:
                var customerCode = customer.CustomerCode;
                if (string.IsNullOrEmpty(customerCode))
                {
                    var msg = new
                    {
                        devMsg = new { fieldName = "CustomerCode", msg = "mã khách hàng không để trống" },
                        useMsg = "mã khách hàng không được để trống",
                        Code = 999,
                    };
                    return BadRequest(msg);
                }
                //check trùng mã :
                var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database=MISA.CukCuk_Demo_NVMANH_copy;Password=manhmisa;Character Set=utf8";
                //using IDbConnection dbConnection = new MySqlConnection(connectionString);
                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
                var res = mySqlConnection.Query<Customer>("Proc_GetCustomerByCode", new { CustomerCode = customerCode }, commandType: CommandType.StoredProcedure);
                if (res.Count() > 0)
                {
                    return BadRequest("Mã khách hàng đã tồn tại");
                }
                var colums = "";
                var columsParams = "";

                DynamicParameters parameters = new DynamicParameters();//khơi tạo dynamic để tránh lỗi injection    
                // Lấy ra các property của đối tượng:                                               
                var props = typeof(Customer).GetProperties();
                //lấy ra name của đối tượng
                var className = typeof(Customer).Name;

                //duyệt từng property
                foreach (var prop in props)
                {
                    // Lấy ra tên của property
                    var propName = prop.Name;
                    //lấy giá trị của property tương ứng đối tượng.
                    var propValue = prop.GetValue(customer);
                    //tạo ra mã khách hàng mới ngẫu nhiên
                    if (propName == $"{className}Id" && prop.PropertyType == typeof(Guid))
                    {
                        propValue = Guid.NewGuid();
                    }
                    //cập nhập chuỗi lệnh thêm mới và add tham số tương ứng.
                    colums += $"{propName},";
                    columsParams += $"@{propName},";
                    parameters.Add($"@{propName}", propValue);
                }
                //thực hiện trừ đi kí tự (,) ở cuối cùng.
                colums = colums.Substring(0, colums.Length - 1);
                columsParams = columsParams.Substring(0, columsParams.Length - 1);
                var sql = $"INSERT INTO Customer ({colums}) VALUES ({columsParams})";
                // thực thi thêm
                var rowAffect = mySqlConnection.Execute(sql, param: parameters);
                return StatusCode(201, rowAffect);
                //var rowAffect = dbConnection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);// excute đọc hàm
                //if (rowAffect > 0)
                //{
                //    return Created("add successfull", customer);
                //}
            }
            catch(Exception ex)
            {
                var result = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xảy ra vui lòng liên hệ dev:ngọc lê để được hỗ trợ",
                    data = DBNull.Value,
                    moreInfo = ""
                };
                return StatusCode(500, result);
            }
          
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Customer customer)
        {
            try
            {
                //validate dữ liệu
                //check bắt buộc nhập:
                var customerCode = customer.CustomerCode;
                if (string.IsNullOrEmpty(customerCode))
                {
                    var msg = new
                    {
                        devMsg = new { fieldName = "CustomerCode", msg = "mã khách hàng không để trống" },
                        useMsg = "mã khách hàng không được để trống",
                        Code = 999,
                    };
                    return BadRequest(msg);
                }
                if (id ==null)
                {
                    return BadRequest();
                }
                //check trùng mã :
                var connectionString = "User Id=dev;Host=47.241.69.179;Port=3306;Database=MISA.CukCuk_Demo_NVMANH_copy;Password=manhmisa;Character Set=utf8";
                //using IDbConnection dbConnection = new MySqlConnection(connectionString);
                MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
              
                var colums = "";
                var columsParams = "";

                DynamicParameters parameters = new DynamicParameters();//khơi tạo dynamic để tránh lỗi injection    
                // Lấy ra các property của đối tượng:                                               
                var props = typeof(Customer).GetProperties();
                //lấy ra name của đối tượng
                var className = typeof(Customer).Name;

                //duyệt từng property
                foreach (var prop in props)
                {
                    // Lấy ra tên của property
                    var propName = prop.Name;
                    //lấy giá trị của property tương ứng đối tượng.
                    var propValue = prop.GetValue(customer);
                    //tạo ra mã khách hàng mới ngẫu nhiên
                    //if (propName == $"{className}Id" && prop.PropertyType == typeof(Guid))
                    //{
                    //    propValue = Guid.NewGuid();
                    //}
                    //cập nhập chuỗi lệnh thêm mới và add tham số tương ứng.
                    colums += $"{propName},";
                    columsParams += $"@{propName},";
                    parameters.Add($"@{propName}", propValue);
                }
                //thực hiện trừ đi kí tự (,) ở cuối cùng.
                colums = colums.Substring(0, colums.Length - 1);
                columsParams = columsParams.Substring(0, columsParams.Length - 1);
                var sql = $"UPDATE Customer set ({colums}) = ({columsParams})";
                // thực thi thêm
                var rowAffect = mySqlConnection.Execute(sql, param: parameters);
                return StatusCode(201, rowAffect);
                //var rowAffect = dbConnection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);// excute đọc hàm
                //if (rowAffect > 0)
                //{
                //    return Created("add successfull", customer);
                //}
            }
            catch (Exception ex)
            {
                var result = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xảy ra vui lòng liên hệ dev:ngọc lê để được hỗ trợ",
                    data = DBNull.Value,
                    moreInfo = ""
                };
                return StatusCode(500, result);
            }
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{customerId}")]
        public IActionResult Delete(Guid customerId)
        {
            try
            {
                // Khai báo thông tin kết nối:
                var connectionString = "" + "Host = 47.241.69.179;" +
                    "Port= 3306;" +
                    "User Id = dev;" +
                    "Password = manhmisa;" +
                    "Database = MISA.CukCuk_Demo_NVMANH_copy";
                // Khởi tạo kết nối với Database:
                MySqlConnection sqlConnection = new MySqlConnection(connectionString);
                // Thực thi xóa dữ liệu trong Database:
                var sql = $"DELETE FROM Customer WHERE CustomerId = @CustomerId";
                DynamicParameters parameters = new DynamicParameters(); // Để tránh lỗi sql injection
                parameters.Add("CustomerId", customerId);
                var rowAffect = sqlConnection.Execute(sql, param: parameters);
                sqlConnection.Close();
                return StatusCode(201, rowAffect);

            }
            catch (Exception ex)
            {
                var result = new
                {
                    devMsg = ex.Message,
                    userMsg = "Có lỗi xảy ra vui lòng liên hệ ngọc dz để được trợ giúp",
                    data = DBNull.Value,
                    moreInfo = ""
                };
                return StatusCode(500, result);
            }
        }
    }
}
