using EventsPlusApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPlusApp.ViewModels
{
    public class Booking
    {
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Participant> Participants { get; set; }

    }
}
