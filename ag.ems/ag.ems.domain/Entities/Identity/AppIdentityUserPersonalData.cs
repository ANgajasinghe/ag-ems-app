using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ag.ems.domain.Entities.Identity
{
    public class AppIdentityUserPersonalData
    {
        public AppIdentityUserPersonalData()
        {

        }
        public AppIdentityUserPersonalData(string id, string email)
        {
            Id = id;
            Email = email;
        }
        public AppIdentityUserPersonalData(string email)
        {
            Email = email;
        }

        [Key]
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? MobileCountryCode { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmergencyCountryCode { get; set; }
        public string? EmergencyNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Nic { get; set; }
        public string? PassportNumber { get; set; }
        public string? Country { get; set; }
        public string? ColorCode { get; set; }

        public bool? CanTranslateJapanese { get; set; }

    }
}
