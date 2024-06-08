using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Services.Implementations
{
    public class ValidationService : IValidationsService
    {
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
