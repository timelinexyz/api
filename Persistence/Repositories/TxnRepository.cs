using Domain.Entities;
using Domain.Interfaces;
using Domain.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;

internal sealed class TxnRepository : ITxnRepository
{
  public async Task Patch(TxnPatch txn)
  {
    throw new NotImplementedException();
  }

  public async Task<IPaginatedList<Txn>> Search(TxnFilter filter, TxnSort sort)
  {
    throw new NotImplementedException();
  }
}
