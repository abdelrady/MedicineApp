using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDbTest.Models
{
    public class Problem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string Image { get; set; }
        public int UserId { get; set; }
        public long Rating { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Category { get; set; }
        public bool? IsActive { get; set; }
    }
}
