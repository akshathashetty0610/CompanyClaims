using CompanyClaims.Models;

namespace CompanyClaims.Services
{
    public interface ICSVService
    {
        //public IEnumerable<T> ReadCSV<T>(Stream file);
        ActiveCompany? GetCompanyById<T>(short id, Stream file);
        IList<ClaimsByCompany>? GetClaimsByCompany<T>(short companyId, Stream file);

        public ClaimAndExpiredDays  GetClaimsWithExpiredDays<T>(string claimId, Stream file);
        

        public string? UpdateClaim<T>( ClaimsByCompany claim, IFormFile file);
    }
}
