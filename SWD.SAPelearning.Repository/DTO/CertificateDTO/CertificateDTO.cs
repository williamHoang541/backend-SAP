using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.SapModuleDTO;

public class CertificateDTO
{
    public int Id { get; set; }
    public string? CertificateName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Level { get; set; }
    public string? Environment { get; set; }
    public bool? Status { get; set; }
    public List<int> ModuleIds { get; set; }



}

