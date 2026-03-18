namespace TransferDanaInternal.Models
{
    public class TransferViewModel
    {
        public TransferForm Form { get; set; }
        public List<Account> SourceAccounts { get; set; }
        public List<Account> DestinationAccounts { get; set; }
    }
}
