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
        public ResponseModel<ClientDTO> CreateAccount(string email)
        {
            ResponseModel<ClientDTO> response = new ResponseModel<ClientDTO>();

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            if(client.Accounts.Count >= 3)
            {
                response.StatusCode = 403;
                response.Message = "El cliente ya posee 3 cuentas asociadas";
                return response;
            }

            Account newAccount = new Account
            {
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = client.Id
            };

            _accountRepository.Save(newAccount);

            response.StatusCode = 201;
            response.Message = "Ok";

            return response;
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
            ResponseCollection<AccountClientDTO> response = new ResponseCollection<AccountClientDTO>();

            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            if (client.Accounts == null)
            {
                response.StatusCode = 404;
                response.Message = "No hay cuentas";
                return response;
            }

            List<AccountClientDTO> accountsDTO = new List<AccountClientDTO>();
            foreach (var a in client.Accounts)
            {
                AccountClientDTO accountDTO = new AccountClientDTO(a);
                accountsDTO.Add(accountDTO);
            }

            response = new ResponseCollection<AccountClientDTO>(200, "Ok", accountsDTO);

            return response;
        }

        public ResponseModel<ClientDTO> GetById(long id)
        {
            ResponseModel<ClientDTO> response = new ResponseModel<ClientDTO>();

            var client = _clientRepository.FindById(id);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            var clientDTO = new ClientDTO(client);
            response.StatusCode = 200;
            response.Message = "Ok";
            response.Model = clientDTO;

            return response;

        }

        public ResponseModel<ClientDTO> GetCurrent(string email)
        {
            ResponseModel<ClientDTO> response = new ResponseModel<ClientDTO>();

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            var clientDTO = new ClientDTO(client);
            response.StatusCode = 200;
            response.Message = "Ok";
            response.Model = clientDTO;

            return response;
        }

        public ResponseModel<ClientDTO> Post(ClientRegisterDTO client)
        {
            ResponseModel<ClientDTO> response = new ResponseModel<ClientDTO>();

            Client user = _clientRepository.FindByEmail(client.Email);

            if (user != null)
            {
                response.StatusCode = 403;
                response.Message = "El email ya se encuentar registrado";
                return response;
            }

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
            {
                response.StatusCode = 404;
                response.Message = "Usuario no encontrado";
                return response;
            }

            Account newAccount = new Account
            {
                CreationDate = DateTime.Now, Balance = 0, ClientId = dbClient.Id
            };

            _accountRepository.Save(newAccount);

            var clientDTO = new ClientDTO(dbClient);
            response.StatusCode = 201;
            response.Message = "Ok";
            response.Model = clientDTO;

            return response;
        }

        public Models.Response CreateCard(string email, NewCardDTO newCard)
        {
            Models.Response response = new Models.Response();

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            bool allow = true;

            foreach (var card in client.Cards)
            {
                if(card.Type == newCard.type.ToUpper() && card.Color == newCard.color.ToUpper())
                    allow = false;
            }

            if (allow)
            {

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

                response.StatusCode = 201;
                response.Message = "Ok";
                return response;

            }
            else
            {
                response.StatusCode = 403;
                response.Message = "Ya posee una tarjeta igual";
                return response;
            }

        }
        public ResponseCollection<CardDTO> GetCards(string email)
        {
            ResponseCollection<CardDTO> response = new ResponseCollection<CardDTO>();

            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                response.StatusCode = 404;
                response.Message = "El usuario no existe";
                return response;
            }

            if (client.Cards.IsNullOrEmpty())
            {
                response.StatusCode = 404;
                response.Message = "No hay tarjetas";
                return response;
            }

            List<CardDTO> cardsDTO = new List<CardDTO>();

            foreach (var card in client.Cards)
            {
                CardDTO cardDTO = new CardDTO(card);
                cardsDTO.Add(cardDTO);
            }

            response = new ResponseCollection<CardDTO>(200, "Ok", cardsDTO);

            return response;
        }
    }
}
