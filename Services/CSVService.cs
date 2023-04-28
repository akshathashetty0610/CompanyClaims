using System.Collections.Immutable;
using System.Globalization;
using System.Security.Claims;
using CompanyClaims.Models;
using CsvHelper;
using Microsoft.AspNetCore.WebUtilities;

namespace CompanyClaims.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<T>();
            return records;
        }
        

        public ActiveCompany? GetCompanyById<T>(short id, Stream file)
        {
            var companies = ReadCSV<Company>(file);
           var company = companies.FirstOrDefault(c => c.Id == id);
           var activeCompany = new ActiveCompany()
           {
               Name = company.Name,
               IsActive = company.Active
           };
            return activeCompany;
        }

        public IList<ClaimsByCompany>? GetClaimsByCompany<T>(short companyId, Stream file)
        {
            var claims = ReadCSV<ClaimsByCompany>(file);
            var claimsByCompany = claims.Where(c => c.CompanyId == companyId).ToList();
            return claimsByCompany;
        }

       

        public ClaimAndExpiredDays GetClaimsWithExpiredDays<T>(string claimId, Stream file)
        {
            var claims = ReadCSV<ClaimsByCompany>(file);
            var claimsByCompany = claims.FirstOrDefault(c => c.UCR == claimId);

            var claimAndExpiredDays = new ClaimAndExpiredDays()
            {
                Claim = claimsByCompany,
                ExpiredDays = GetExpiredDays(claimsByCompany.ClaimDate)
            };
          
            return claimAndExpiredDays;
        }

      

        public string? UpdateClaim<T>( ClaimsByCompany claim, IFormFile file)
        {
            StreamReader reader = new StreamReader(file.OpenReadStream());
            List<String> lines = new List<String>();

            string returnValue; 

            try
            {
                if (file.Length > 0)
                {
                    using (reader)
                    {
                        String line;
                        bool isFirstLine = true;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (isFirstLine)
                            {
                                isFirstLine = false;
                                lines.Add(line);
                                continue;
                            }

                            if (line.Contains(","))
                            {
                                String[] split = line.Split(',');

                                if (split[0].Contains(claim.UCR))
                                {
                                    split[4] = claim.AssuredName;
                                    split[2] = claim.ClaimDate.ToString();
                                    split[5] = claim.IncurredLoss.ToString();
                                    split[1] = claim.CompanyId.ToString();
                                    split[3] = claim.LossDate.ToString();
                                    split[6] = claim.Closed.ToString();
                                    line = String.Join(",", split);
                                }
                            }

                            lines.Add(line);

                        }
                    }
                }

                WriteCsv(file, lines);

                reader.Close();
                returnValue = "Claim updated successfully";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return returnValue;
        }

        private static void WriteCsv(IFormFile file, List<string> lines)
        {
            var filesPath = Directory.GetCurrentDirectory() + "\\Domain\\SeedData";
            var fileName = Path.GetFileName(file.FileName);
            var filePath1 = Path.Combine(filesPath, fileName);
            File.WriteAllLines(filePath1, lines);
        }


        private int GetExpiredDays(DateTime date)
        {
            return (DateTime.Today - date).Days;
        }
    }

    public class ClaimAndExpiredDays
    {
        public ClaimsByCompany? Claim { get; set; }
        public int ExpiredDays { get; set; }
    }
}
