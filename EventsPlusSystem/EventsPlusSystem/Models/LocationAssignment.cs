using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusSystem.Models
{
    public class LocationAssignment
    {
        public string LocationName { get; set; }
        public int EventID { get; set; }

        public EventLocation EventLocation { get; set; }
        public Event Event { get; set; }

    }
}
