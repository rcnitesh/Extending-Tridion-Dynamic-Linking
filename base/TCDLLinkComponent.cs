using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Templating
{
    class TCDLLinkComponent
    {
        public string type { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string templateURI { get; set; }
        public string linkAttributes { get; set; }
        public bool textOnFail { get; set; }
        public bool addAnchor { get; set; }
        public string variantId { get; set; }
        public string linkText { get; set; }
    }
}
