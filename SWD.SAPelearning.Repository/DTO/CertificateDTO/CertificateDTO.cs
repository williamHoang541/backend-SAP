public class CertificateDTO
{
    public string CertificateName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Level { get; set; }
    public string? Environment { get; set; }
    public bool Status { get; set; }
    public List<int> ModuleIds { get; set; } = new List<int>(); // New property for module IDs
}
