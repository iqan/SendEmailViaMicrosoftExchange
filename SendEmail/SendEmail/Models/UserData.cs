using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SendEmail.Models
{
    public class UserData
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string DomainName { get; set; }
        public Uri Uri { get; set; }
        [Required]
        [EmailAddress]
        public string ToName { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}