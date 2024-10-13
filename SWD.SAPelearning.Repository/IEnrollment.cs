using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.EnrollmentDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IEnrollment
    {
        Task<List<EnrollmentDTO>> GetAllEnrollmentsAsync(GetAllDTO getAllDTO);
        Task<EnrollmentDTO> CreateEnrollmentAsync(string userId, int courseId, double enrollmentPrice);
        Task<EnrollmentDTO> GetEnrollmentByIdAsync(int enrollmentId);
        Task<bool> ConfirmEnrollmentAsync(int enrollmentId);

        Task<bool> CancelEnrollmentAsync(int enrollmentId);
        Task<bool> MarkEnrollmentAsConfirmedAsync(int enrollmentId, string userId);
    }
}
