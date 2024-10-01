using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CertificateDTO;
using SWD.SAPelearning.Repository.Models;


namespace SWD.SAPelearning.Services
{
    public class SCertificate : ICertificate
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCertificate(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }

        public async Task<Certificate> CreateCertificateAsync(CreateCertificateDTO request)
        {
            try
            {
                // Kiểm tra nếu DTO không null
                if (request != null)
                {
                    // Kiểm tra xem tên chứng chỉ đã tồn tại hay chưa
                    foreach (var x in this.context.Certificates)
                    {
                        if (request.CertificateName.Equals(x.CertificateName))
                        {
                            throw new Exception("Certificate name already exists!");
                        }
                    }

                    // Tạo chứng chỉ mới
                    var newCertificate = new Certificate
                    {
                        CertificateId = "CE" + Guid.NewGuid().ToString().Substring(0, 5),  // Tạo ID với tiền tố "C"
                        CertificateName = request.CertificateName,
                        Description = request.Description,
                        Level = request.Level,
                        Environment = request.Environment,
                        
                    };

                    // Thêm chứng chỉ vào cơ sở dữ liệu
                    await this.context.Certificates.AddAsync(newCertificate);
                    await this.context.SaveChangesAsync();

                    return newCertificate;  // Trả về chứng chỉ vừa tạo
                }

                return null;  // Trả về null nếu yêu cầu không hợp lệ
            }
            catch (Exception ex)
            {
                // Bắt lỗi và trả về thông báo lỗi chi tiết
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<Certificate>> GetAllCertificate()
        {
            try
            {
                var a = await this.context.Certificates.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }

        public async Task<Certificate> GetCertificateById(string id)
        {
            try
            {
                var certificate = await context.Certificates.FindAsync(id);
                if (certificate == null)
                {
                    throw new Exception("Certificate not found");
                }
                return certificate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
