using System.ComponentModel.DataAnnotations;

namespace AccessToAuth.Data.Entities.MainRoleCookie
{
    public class MainRole
    {

        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? LastValidCookie { get; set; }

    }
}