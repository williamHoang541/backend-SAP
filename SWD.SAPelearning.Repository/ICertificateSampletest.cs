using SWD.SAPelearning.Repository.DTO.CertifficateTestAttempt;
using SWD.SAPelearning.Repository.DTO.SampleTest;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICertificateSampletest
    {
        Task<List<CertificateSampleTest>> GetAllCertificateSampletest();
        Task<CertificateSampleTest> CreateSampleTest(CreateSampleTestDTO request);
        Task<bool> DeleteAttempt(RemoveSDTO sampleId);
    }
}
