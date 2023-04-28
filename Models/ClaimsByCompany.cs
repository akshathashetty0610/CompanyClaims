using System;

namespace CompanyClaims.Models
{
    public class ClaimsByCompany
    {
       

        public string? UCR { get; set; }
        public int CompanyId { get; set; }
        public DateTime ClaimDate { get; set; }
        public DateTime LossDate { get; set; }
        public string AssuredName { get; set; }
        public decimal IncurredLoss { get; set; }

        public bool Closed { get; set; }





    }
}
