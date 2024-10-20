using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Txns.Commands.DeleteTxns;

public sealed class DeleteTxnsCommandValidator : AbstractValidator<DeleteTxnsCommand>
{
  public DeleteTxnsCommandValidator()
  {

  }
}
