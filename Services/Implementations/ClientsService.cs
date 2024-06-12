using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOS;
using HomeBankingMindHub.Models.Enums;
using HomeBankingMindHub.Repositories.Interfaces;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Services.Implementations
{
    public class ClientsService : IClientsService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IEncryptsService _encryptsService;
        
        public ClientsService(IClientRepository clientRepository, IAccountRepository accountRepository, ICardRepository cardRepository, IEncryptsService encryptsService)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _cardRepository = cardRepository;
            _encryptsService = encryptsService;
        }
        public Response CreateAccount(string email)
        {
            Response response;

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
                return new Response(404, "El usuario no existe");

            if(client.Accounts.Count >= 3)
                return new Response(403, "El cliente ya posee 3 cuentas asociadas");

            Account newAccount = new Account
            {
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = client.Id
            };

            _accountRepository.Save(newAccount);

            return new Response(201, "Ok");
        }

        public ResponseCollection<ClientDTO> Get()
        {
 
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();
            clientsDTO = clients.Select(c => new ClientDTO(c)).ToList();

            ResponseCollection<ClientDTO> response = new ResponseCollection<ClientDTO>(200, "Ok", clientsDTO);

            return response;
        }

        public ResponseCollection<AccountClientDTO> GetAccounts(string email)
        {
            ResponseCollection<AccountClientDTO> response;

            var client = _clientRepository.FindByEmail(email);

            if (client == null)
                return new ResponseCollection<AccountClientDTO>(404, "El usuario no existe", null);

            if (client.Accounts == null)
                return new ResponseCollection<AccountClientDTO>(404, "No hay cuentas", null);

            List<AccountClientDTO> accountsDTO = new List<AccountClientDTO>();
            foreach (var a in client.Accounts)
            {
                AccountClientDTO accountDTO = new AccountClientDTO(a);
                accountsDTO.Add(accountDTO);
            }

            return new ResponseCollection<AccountClientDTO>(200, "Ok", accountsDTO);
        }

        public ResponseModel<ClientDTO> GetById(long id)
        {
            ResponseModel<ClientDTO> response;

            var client = _clientRepository.FindById(id);

            if (client == null)
                return new ResponseModel<ClientDTO>(404, "El usuario no existe", null);

            var clientDTO = new ClientDTO(client);

            return new ResponseModel<ClientDTO>(200, "Ok", clientDTO);
        }

        public ResponseModel<ClientDTO> GetCurrent(string email)
        {
            ResponseModel<ClientDTO> response;

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
                return new ResponseModel<ClientDTO>(404, "El usuario no existe", null);

            var clientDTO = new ClientDTO(client);

            return new ResponseModel<ClientDTO>(200, "Ok", clientDTO);
        }

        public ResponseModel<ClientDTO> Post(ClientRegisterDTO client)
        {
            ResponseModel<ClientDTO> response;

            if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || 
                String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                return new ResponseModel<ClientDTO>(400, "Datos inválidos", null);


            Client user = _clientRepository.FindByEmail(client.Email);

            if (user != null)
                return new ResponseModel<ClientDTO>(403, "El email ya se encuentar registrado", null);

            Client newClient = new Client()
            {
                Email = client.Email,
                Password = client.Password,
                FirstName = client.FirstName,
                LastName = client.LastName
            };

            _encryptsService.EncryptPassword(client.Password, out byte[] salt, out string password);

            newClient.Salt = salt;
            newClient.Password = password;

            _clientRepository.Save(newClient);

            var dbClient = _clientRepository.FindByEmail(newClient.Email);

            if (dbClient == null)
                return new ResponseModel<ClientDTO>(404, "Usuario no encontrado", null);

            Account newAccount = new Account
            {
                CreationDate = DateTime.Now, Balance = 0, ClientId = dbClient.Id
            };

            _accountRepository.Save(newAccount);

            var clientDTO = new ClientDTO(dbClient);

            return new ResponseModel<ClientDTO>(201, "Ok", clientDTO);
        }

        public Response CreateCard(string email, NewCardDTO newCard)
        {
            Response response;

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
                return new Response(404, "El usuario no existe");

            foreach (var card in client.Cards)
            {
                if (card.Type == newCard.type.ToUpper() && card.Color == newCard.color.ToUpper())
                    return new Response(403, "Ya posee una tarjeta igual");
            }

            CardType cardType = (CardType)Enum.Parse(typeof(CardType), newCard.type.ToUpper());
            CardColor cardColor = (CardColor)Enum.Parse(typeof(CardColor), newCard.color.ToUpper());

            Card newCardToSave = new Card
            {
                CardHolder = $"{client.FirstName}, {client.LastName}",
                Type = cardType.ToString(),
                Color = cardColor.ToString(),
                FromDate = DateTime.Now,
                ThruDate = DateTime.Now.AddYears(5),
                ClientId = client.Id
            };

            _cardRepository.Save(newCardToSave);

            return new Response(201, "Ok");
        }
        public ResponseCollection<CardDTO> GetCards(string email)
        {
            ResponseCollection<CardDTO> response;

            var client = _clientRepository.FindByEmail(email);

            if (client == null)
                return new ResponseCollection<CardDTO>(404, "El usuario no existe", null);

            if (client.Cards.IsNullOrEmpty())
                return new ResponseCollection<CardDTO>(404, "No hay tarjetas", null);

            List<CardDTO> cardsDTO = new List<CardDTO>();

            foreach (var card in client.Cards)
            {
                CardDTO cardDTO = new CardDTO(card);
                cardsDTO.Add(cardDTO);
            }

            return new ResponseCollection<CardDTO>(200, "Ok", cardsDTO);
        }
    }
}
