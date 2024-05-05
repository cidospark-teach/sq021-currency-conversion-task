namespace WebApplication3.Models.DTOs
{
    public class UserToReturn
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }

        public string UserRole { get; set; }

        public List<WalletTOReturn> Wallets { get; set; } = new List<WalletTOReturn>();
    }
}
