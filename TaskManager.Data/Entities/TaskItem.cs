using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManager.Data.Entities
{
    
        public class TaskItem
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public bool IsCompleted { get; set; } = false;
            public DateTime? DueDate { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public int UserId { get; set; }
            public User User { get; set; } = null!;

            public int CategoryId { get; set; }
            public Category Category { get; set; } = null!;
        
    }
}
