using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using BackendExamHub.Model;
using Microsoft.Extensions.Configuration;

namespace BackendExamHub.Services
{
    public class MyofficeAcpdService
    {
        private readonly string _connectionString;

        public MyofficeAcpdService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// 取 Myoffice_ACPD table by SID.
        /// </summary>        
        public async Task<MyofficeAcpd> GetByIdAsync(int id)
        {
            MyofficeAcpd item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT * FROM Myoffice_ACPD WHERE acpd_sid = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new MyofficeAcpd
                            {
                                AcpdSid = reader.GetInt32(0),
                                AcpdCname = reader.GetString(1),
                                AcpdEname = reader.IsDBNull(2) ? null : reader.GetString(2),
                                AcpdSname = reader.IsDBNull(3) ? null : reader.GetString(3),
                                AcpdEmail = reader.IsDBNull(4) ? null : reader.GetString(4),
                                AcpdStatus = reader.GetByte(5),
                                AcpdStop = reader.GetBoolean(6),
                                AcpdStopMemo = reader.IsDBNull(7) ? null : reader.GetString(7),
                                AcpdLoginId = reader.GetString(8),
                                AcpdLoginPw = reader.GetString(9),
                                AcpdMemo = reader.IsDBNull(10) ? null : reader.GetString(10),
                                AcpdNowdatetime = reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                                AppdNowid = reader.IsDBNull(12) ? null : reader.GetString(12),
                                AcpdUpddatetime = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                                AcpdUpdid = reader.IsDBNull(14) ? null : reader.GetString(14)
                            };
                        }
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// 建立一筆 Myoffice_ACPD table 資料
        /// </summary>        
        public async Task CreateAsync(MyofficeAcpd item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(
                    "INSERT INTO Myoffice_ACPD (acpd_cname, acpd_ename, acpd_sname, acpd_email, acpd_status, acpd_stop, acpd_stopMemo, acpd_LoginID, acpd_LoginPW, acpd_memo, acpd_nowdatetime, appd_nowid, acpd_upddatetime, acpd_updid) " +
                    "VALUES (@Cname, @Ename, @Sname, @Email, @Status, @Stop, @StopMemo, @LoginID, @LoginPW, @Memo, @Nowdatetime, @Nowid, @Upddatetime, @Updid)", connection))
                {
                    command.Parameters.AddWithValue("@Cname", item.AcpdCname);
                    command.Parameters.AddWithValue("@Ename", (object)item.AcpdEname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Sname", (object)item.AcpdSname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)item.AcpdEmail ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", item.AcpdStatus);
                    command.Parameters.AddWithValue("@Stop", item.AcpdStop);
                    command.Parameters.AddWithValue("@StopMemo", (object)item.AcpdStopMemo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LoginID", item.AcpdLoginId);
                    command.Parameters.AddWithValue("@LoginPW", item.AcpdLoginPw);
                    command.Parameters.AddWithValue("@Memo", (object)item.AcpdMemo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Nowdatetime", (object)item.AcpdNowdatetime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Nowid", (object)item.AppdNowid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Upddatetime", (object)item.AcpdUpddatetime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Updid", (object)item.AcpdUpdid ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// 修改一筆 Myoffice_ACPD table 資料
        /// </summary>     
        public async Task UpdateAsync(MyofficeAcpd item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(
                    "UPDATE Myoffice_ACPD SET acpd_cname = @Cname, acpd_ename = @Ename, acpd_sname = @Sname, acpd_email = @Email, acpd_status = @Status, acpd_stop = @Stop, acpd_stopMemo = @StopMemo, acpd_LoginID = @LoginID, acpd_LoginPW = @LoginPW, acpd_memo = @Memo, acpd_nowdatetime = @Nowdatetime, appd_nowid = @Nowid, acpd_upddatetime = @Upddatetime, acpd_updid = @Updid " +
                    "WHERE acpd_sid = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", item.AcpdSid);
                    command.Parameters.AddWithValue("@Cname", item.AcpdCname);
                    command.Parameters.AddWithValue("@Ename", (object)item.AcpdEname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Sname", (object)item.AcpdSname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)item.AcpdEmail ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", item.AcpdStatus);
                    command.Parameters.AddWithValue("@Stop", item.AcpdStop);
                    command.Parameters.AddWithValue("@StopMemo", (object)item.AcpdStopMemo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LoginID", item.AcpdLoginId);
                    command.Parameters.AddWithValue("@LoginPW", item.AcpdLoginPw);
                    command.Parameters.AddWithValue("@Memo", (object)item.AcpdMemo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Nowdatetime", (object)item.AcpdNowdatetime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Nowid", (object)item.AppdNowid ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Upddatetime", (object)item.AcpdUpddatetime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Updid", (object)item.AcpdUpdid ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// 刪除 Myoffice_ACPD table 資料 acpd_sid
        /// </summary>     
        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("DELETE FROM Myoffice_ACPD WHERE acpd_sid = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
