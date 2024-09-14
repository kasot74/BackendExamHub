using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BackendExamHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbTestController : Controller
    {
        private readonly string _connectionString;

        public DbTestController(IConfiguration configuration)
        {            
            // 從 appsettings.json 讀取連線字串
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                // 使用原生的 SqlConnection 測試連線
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();  // 嘗試打開連線
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        return Ok("連線成功！");
                    }
                    else
                    {
                        return StatusCode(500, "無法開啟資料庫連線。");
                    }
                }
            }
            catch (SqlException ex)
            {
                // 捕捉連線錯誤
                return StatusCode(500, $"連線失敗: {ex.Message}");
            }
        }
    }
}
