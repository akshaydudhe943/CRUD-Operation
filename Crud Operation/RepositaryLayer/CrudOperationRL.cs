using Crud_Operation.ComonLayer.Model;
using Crud_Operation.ServiceLayer;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.SqlClient;

namespace Crud_Operation.RepositaryLayer
{
    public class CrudOperationRL : ICrudOperationRL
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _sqlConnection;
        public CrudOperationRL(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:DBSettingConnection"]);
        }

        public async Task<CreateRecordResponse> CreateRecord(CreateRecordRequest request)
        {
            CreateRecordResponse response = new CreateRecordResponse();
            try
            {
                response.IsSuccess = true;
                response.Message = "Successful!!";
                string SqlQuery = "Insert into CrudOperationTable(UserName, Age) values (@UserName,@Age)";
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@Age", request.Age);
                    _sqlConnection.Open();
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went Wrong!";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                _sqlConnection.Close();
            }
            return response;
            
        }

        public async Task<ReadRecordResponse> ReadRecord()
        {
            ReadRecordResponse response = new ReadRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful!!";
            string SqlQuery = "Select UserName, Age from CrudOperationTable";
            try
            {
                using (SqlCommand command = new SqlCommand(SqlQuery, _sqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 180;
                    _sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = await command.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.readRecordData = new List<ReadRecordData>();
                            while (await sqlDataReader.ReadAsync())
                            {
                                ReadRecordData dbData = new ReadRecordData();
                                dbData.UserName = sqlDataReader["UserName"] != DBNull.Value ? sqlDataReader["UserName"].ToString() : string.Empty;
                                dbData.Age = sqlDataReader["Age"] != DBNull.Value ?Convert.ToInt32(sqlDataReader["Age"]) : 0;
                                response.readRecordData.Add(dbData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            finally 
            {
                _sqlConnection.Close(); 
            }
            return response;
        }
    }
}
