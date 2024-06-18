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
                 return response = new Response(403, "Faltan campos");
            }
            if(loan == null)
            {
                return response = new Response(403, "El préstamo no existe");
            }
            if(loanApplicationDTO.Amount > loan.MaxAmount)
            {
                 return response = new Response(403, "El monto solicitado excede el monto máximo");
            }
            if (!payments.Contains(loanApplicationDTO.Payments))
            {
                return response = new Response(403, "Las cuotas elegidas no están disponibles para este préstamo");
            }
            if(toAccount == null)
            {
                return response = new Response(403, "La cuenta de destino no existe");
            }
            if(toAccount.ClientId != currentId)
            {
                return response = new Response(403, "La cuenta no pertenece al cliente autenticado");
            }

            return response = new Response(201, "Ok");
        }

        public Response MakeTransfer(TransferDTO transferDTO, Account fromAccount, Account toAccount, long currentId)
        {
            Response response;

            if (transferDTO.Description.IsNullOrEmpty() || transferDTO.Amount <= 0 ||
                transferDTO.FromAccountNumber.IsNullOrEmpty() || transferDTO.ToAccountNumber.IsNullOrEmpty())
            {
                return response = new Response(403, "Faltan campos");
            }
            if (transferDTO.FromAccountNumber.Equals(transferDTO.ToAccountNumber))
            {
                return response = new Response(403, "La cuenta de origen es igual a la de destino");
            }
            if (fromAccount == null)
            {
                return response = new Response(403, "La cuenta de origen no existe");
            }
            if (fromAccount.ClientId != currentId)
            {
                return response = new Response(403, "La cuenta de origen no pertenece al cliente autenticado");
            }
            if (toAccount == null)
            {
                return response = new Response(403, "La cuenta de destino no existe");
            }
            if (fromAccount.Balance < transferDTO.Amount) 
            {
                return response = new Response(403, "Saldo insuficiente");
            }
            
            return response = new Response(201, "Ok");
        }
    }
}
