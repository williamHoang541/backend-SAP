namespace SWD.SAPelearning.Repository.DTO.CertificateDTO
{
    public class CreateCertificateDTO
    {
        public string CertificateName { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Environment { get; set; }
        
    }

    public class UpdateCertificateDTO
    {
        public string CertificateId { get; set; }
        public string CertificateName { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Environment { get; set; }
        public bool Status { get; set; }
    }
}
