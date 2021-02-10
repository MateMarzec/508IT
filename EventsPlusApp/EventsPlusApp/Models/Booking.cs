using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.Models
{
    public class Booking
    {
        public int ID { get; set; }
        
        public int EventParticipantID { get; set; }
        
        public int EventID { get; set; }
        public EventParticipant EventParticipant { get; set; }
        public Event Event { get; set; }
    }
}
