using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //UTC, not local time, is the professional standard,
        //since your server and your users could be in different time zones.
        public ICollection<TaskItem>? Tasks{ get; set; }=new List<TaskItem>();
        //this is the "many" side of the relationship from the diagram.
        //It tells EF Core "one Category can have many Tasks
        //." You don't write any foreign key here, that lives on the TaskItem side,
        //which we'll build next.
    }
}
