using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class Booking
    {   
        public int ParticipantID { get; set; }
        public int EventID { get; set; }
        public Participant Participant { get; set; }
        public Event Event { get; set; }
    }
}
