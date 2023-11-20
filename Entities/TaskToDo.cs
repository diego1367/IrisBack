namespace Entities
{
    using System.ComponentModel.DataAnnotations;

    public class TaskToDo
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}