using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public class OwnerAssignment
    {
        public int OwnerID { get; set; }
        public string EventLocationName { get; set; }

        public Owner Owner { get; set; }
        public EventLocation EventLocation { get; set; }
    }
}
