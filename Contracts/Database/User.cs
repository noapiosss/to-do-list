using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database
{
    [Table("users", Schema = "public")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("username")]
        public string Username { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("password")]
        public string Password { get; set; }
        
        public ICollection<ToDoList> ToDoLists { get; set; }
    }
}