using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace KangarooParty.Models
{
	public class Party
	{
        public int Id { get; set; }
        //one-to-one rel b/w kangaroo host and party
        public int HostId { get; set; }
        public Kangaroo Host { get; set; }
        //one-to-many rel b/w party and kangaroos
        public ICollection<Kangaroo> Attendees { get; } = new List<Kangaroo>();
        public int Prestige { get; set; } = 0;
    }
}

