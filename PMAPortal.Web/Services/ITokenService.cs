using System;
using System.Collections.Generic;
using System.Text;

namespace PMAPortal.Web.Services
{
    public interface ITokenService
    {
        string GenerateTokenFromData(string data);
        string ExtractDataFromToken(string token);
        bool IsTokenExpired(string token, int ExpiryPeriodInHours);
    }
}
