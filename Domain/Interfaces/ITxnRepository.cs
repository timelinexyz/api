using Domain.Entities;
using Domain.Payloads;

namespace Domain.Interfaces;

public interface ITxnRepository
{
  Task<IPaginatedList<Txn>> Search(TxnFilter filter, TxnSort sort);
  Task Patch(TxnPatch txn);
  Task Delete(IEnumerable<string> ids);
}
