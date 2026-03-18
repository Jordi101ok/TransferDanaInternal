namespace TransferDanaInternal.Models
{
    public class TransferForm
    {
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
