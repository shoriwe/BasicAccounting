using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAccounting
{
    public class RegistryRow
    {
        public int ID { get; set; }
        public bool Selected { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }

        public string Currency { get; set; }
        public string Type_ { get; set; }
        public string Description { get; set; }
    }
}
