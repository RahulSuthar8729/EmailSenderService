using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EmailSenderService.Database
{
    public class EmailRepository
    {
        private readonly string _connectionString;

        public EmailRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Constr"); 
        }

        public async Task<List<EmailData>> GetPendingEmailsAsync()
        {
            var emails = new List<EmailData>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetPendingEmails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                emails.Add(new EmailData
                                {
                                    Code = reader.IsDBNull(0) ? 0 : Convert.ToInt64(reader.GetDecimal(0)), 
                                    MailTo = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    MailFrom = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Subject = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    MailCcTo = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    MailBccTo = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    MailHeader = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    MailFooter = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    MailMessage = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    DataFlag = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    FY_ID = reader.IsDBNull(10) ? (int?)null : Convert.ToInt32(reader.GetDecimal(10)), 
                                    DOE = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                                    MessageId = reader.IsDBNull(12) ? null : reader.GetString(12),
                                    DelStatus = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    DoneAt = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14),
                                    SentAt = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                                    StatusReason = reader.IsDBNull(16) ? null : reader.GetString(16),
                                    SingleBulk = reader.IsDBNull(17) ? (string?)null : reader.GetString(17),
                                    SmsSignature = reader.IsDBNull(18) ? null : reader.GetString(18),
                                    Msg = reader.IsDBNull(19) ? null : reader.GetString(19),
                                    IsMailSent = reader.IsDBNull(20) ? (bool?)null : reader.GetBoolean(20),
                                    IsSmsSent = reader.IsDBNull(21) ? (bool?)null : reader.GetBoolean(21),
                                    MobileNo = reader.IsDBNull(22) ? null : reader.GetString(22),
                                    ReceiveIdCode = reader.IsDBNull(23) ? null : reader.GetString(23),
                                    FileHtml = reader.IsDBNull(24) ? null : reader.GetString(24),
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }

            return emails;
        }

        public async Task MarkEmailAsSentAsync(long emailCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UPDATE Mail_Master SET IsMailSent = 1, Sent_At = @SentAt WHERE Code = @Code", connection))
                {
                    command.Parameters.AddWithValue("@Code", emailCode);
                    command.Parameters.AddWithValue("@SentAt", DateTime.UtcNow); 
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}