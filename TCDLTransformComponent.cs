using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tridion.Templating
{
    class CustomTagComponentModel
    {
        public string runat { get; set; }
        public string PageURI { get; set; }
        public string ComponentURI { get; set; }
        public string TemplateURI { get; set; }
        public bool AddAnchor { get; set; }
        public string LinkText { get; set; }
        public string LinkAttributes { get; set; }
        public bool TextOnFail { get; set; }        
    }
}
