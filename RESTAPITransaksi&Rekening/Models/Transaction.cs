namespace RESTAPITransaksi_Rekening.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();
        public string AccountNameFrom { get; set; }
        public string AccountNumberFrom { get; set; }
        public string AccountNameTo { get; set; }
        public string AccountNumberTo { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
