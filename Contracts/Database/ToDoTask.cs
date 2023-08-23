using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database
{
    [Table("to_do_tasks", Schema = "public")]
    public class ToDoTask
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

        [Column("to_do_list_id")]
        public int ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
    }
}