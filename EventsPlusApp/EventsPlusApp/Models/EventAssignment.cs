using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class EventAssignment
    {
        public int ManagerID { get; set; }
        public int EventID { get; set; }

        public Manager Manager { get; set; }
        public Event Event { get; set; }
    }
}
