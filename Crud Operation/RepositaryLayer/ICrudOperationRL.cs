using Crud_Operation.ComonLayer.Model;

namespace Crud_Operation.RepositaryLayer
{
    public interface ICrudOperationRL
    {
        public Task<CreateRecordResponse> CreateRecord(CreateRecordRequest request);
        public Task<ReadRecordResponse> ReadRecord();
    }
}
