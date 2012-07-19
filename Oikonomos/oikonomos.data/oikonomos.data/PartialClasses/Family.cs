using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oikonomos.data
{
    public partial class Family
    {
        public int ChurchId { get; set; }

        public Church Church { get; set; }
    }
}
