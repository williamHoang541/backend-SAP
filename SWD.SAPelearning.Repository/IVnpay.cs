using Microsoft.AspNetCore.Http;
using SWD.SAPelearning.Repository.DTO.VNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository
{
    public interface IVnPay
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
