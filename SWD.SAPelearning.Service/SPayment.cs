using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SPayment : IPayment
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SPayment(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;

        }


        public async Task<Payment> createPayment(string EnrollmentId)
        {
            try
            {
                var order = await this.context.Enrollments
                                .Where(x => x.Id.Equals(EnrollmentId))
                                .FirstOrDefaultAsync();
                if (order != null)
                {
                    var payment = new Payment
                    {
                        EnrollmentId = order.Id,
                        Amount = order.Price, // Assuming 'Total' is a property in 'Enrollment'
                        PaymentDate = DateTime.Now,
                        Status = "Pending"  // Sử dụng trạng thái dưới dạng chuỗi
                    };

                    await this.context.Payments.AddAsync(payment);
                    await this.context.SaveChangesAsync();
                    return payment;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Payment> DeletePayment(string paymentID)
        {
            try
            {
                if (paymentID != null)
                {
                    var obj = await this.context.Payments
                                    .Where(x => x.Id.Equals(paymentID))
                                    .FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        obj.Status = "Cancelled"; // Cập nhật trạng thái thành hủy
                        this.context.Payments.Update(obj);
                        await this.context.SaveChangesAsync();
                        return obj;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> DeletePaymentComplete(string paymentID)
        {
            try
            {
                if (paymentID != null)
                {
                    var obj = await this.context.Payments
                                    .Where(x => x.Id.Equals(paymentID))
                                    .FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        this.context.Payments.Remove(obj);
                        await this.context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Payment> GetPayment(string EnrollmentId)
        {
            try
            {
                var payment = await this.context.Payments
                                    .Where(x => x.Enrollment.Id.Equals(EnrollmentId))
                                    .FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Payment> GetPaymentFail(string EnrollmentId)
        {
            try
            {
                var payment = await this.context.Payments
                                    .Where(x => x.Enrollment.Id.Equals(EnrollmentId) && x.Status == "Failed")
                                    .FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Payment> GetPaymentSuccess(string EnrollmentId)
        {
            try
            {
                var payment = await this.context.Payments
                                    .Where(x => x.Enrollment.Id.Equals(EnrollmentId) && x.Status == "Completed")
                                    .FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Payment> UpdatePayment(string id)
        {
            try
            {
                var payment = await context.Payments.FindAsync(id);

                if (payment == null)
                {
                    throw new Exception($"Payment with ID {id} not found.");
                }

                // Cập nhật thuộc tính
                payment.Status = "Completed"; // Giả sử trạng thái thành công là "Completed"

                // Lưu thay đổi vào cơ sở dữ liệu
                context.Payments.Update(payment);
                await context.SaveChangesAsync();

                return payment;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while updating the payment.", e);
            }
        }
    }
}
