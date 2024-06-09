using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Services.Implementations
{
    public class ValidationsService : IValidationsService
    {
        public Response GrantLoan(LoanApplicationDTO loanApplicationDTO, Loan loan, List<string> payments, Account toAccount, long currentId)
        {
            Response response;

            if (loanApplicationDTO.LoanId == 0 || loanApplicationDTO.Amount == 0 ||
                loanApplicationDTO.Payments.Equals("0") || loanApplicationDTO.Payments.IsNullOrEmpty() || loanApplicationDTO.ToAccountNumber.IsNullOrEmpty())
            {
                response = new Response(403, "Faltan campos");
            }
            else if(loan == null)
            {
                response = new Response(403, "El préstamo no existe");
            }
            else if(loanApplicationDTO.Amount > loan.MaxAmount)
            {
                response = new Response(403, "El monto solicitado excede el monto máximo");
            }
            else if (!payments.Contains(loanApplicationDTO.Payments))
            {
                response = new Response(403, "Las cuotas elegidas no están disponibles para este préstamo");
            }
            else if(toAccount == null)
            {
                response = new Response(403, "La cuenta de destino no existe");
            }
            else if(toAccount.ClientId != currentId)
            {
                response = new Response(403, "La cuenta no pertenece al cliente autenticado");
            }
            else
            {
                response = new Response(201, "Ok");
            }

            return response;
        }

        public Response MakeTransfer(TransferDTO transferDTO, Account fromAccount, Account toAccount, long currentId)
        {
            Response response;

            if (transferDTO.Description.IsNullOrEmpty() || transferDTO.Amount <= 0 ||
                transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty())
            {
                response = new Response(403, "Faltan campos");
            }
            else if (transferDTO.FromAccountNumber.Equals(transferDTO.ToAccountNumber))
            {
                response = new Response(403, "La cuenta de origen es igual a la de destino");
            }
            else if (fromAccount == null)
            {
                response = new Response(403, "La cuenta de origen no existe");
            }
            else if (fromAccount.ClientId != currentId)
            {
                response = new Response(403, "La cuenta de origen no pertenece al cliente autenticado");
            }
            else if (toAccount == null)
            {
                response = new Response(403, "La cuenta de destino no existe");
            }
            else if (fromAccount.Balance < transferDTO.Amount) 
            {
                response = new Response(403, "Saldo insuficiente");
            }
            else
            {
                response = new Response(201, "Ok");
            }
            
            return response;
        }
    }
}
