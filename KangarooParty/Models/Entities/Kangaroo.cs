using System;
using Microsoft.EntityFrameworkCore;

namespace KangarooParty.Models
{
	public class Kangaroo
	{
		public int Id { get; set; }
        public string Name { get; set; } = "";
        //kangaroo profile pic alternates 
        private static int Count = 0;
        public int Pic { get; set; } = Count++ % 3;

        //for every party that exists, one kangaroo will be host
        public Party HostingParty { get; set; }

        //every kangaroo can go to one party, or none
        public int? AttendingPartyId { get; set; }
		public Party? AttendingParty { get; set; }

    }
}

