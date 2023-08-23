using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database
{
    [Table("to_do_lists", Schema = "public")]
    public class ToDoList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Column("creation_date_time")]
        public DateTime CreationDateTime { get; set; }

        [Column("completion_date_time")]
        public DateTime? CompletionDateTime { get; set; }

        public ICollection<ToDoTask> ToDoTasks { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}