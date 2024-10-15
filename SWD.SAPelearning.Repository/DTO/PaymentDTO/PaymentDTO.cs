

namespace SWD.SAPelearning.Repository.DTO.PaymentDTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }  // Primary Key
        public int? EnrollmentId { get; set; }  // Foreign Key to Enrollment
        public double? Amount { get; set; }  // Amount of the payment
        public DateTime? PaymentDate { get; set; }  // Date and time of payment
        public int? TransactionId { get; set; }  // Numeric identifier for the transaction
        public string? Status { get; set; }
    }
}
