using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Array.Test.Data.Entity 
{
    [Table("Account", Schema="app")]
    public class Account 
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}