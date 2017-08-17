using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emaar.Templating
{
    public static class TridionConstants
    {
        public const string TCDL_COMPONENT_LINK_TAG_SKELETON = "<tcdl:Link type=\"Component\" origin=\"{1}\" destination=\"{0}\" templateURI=\"{2}\" linkAttributes=\"{6}\" textOnFail=\"{4}\" addAnchor=                                                       \"{5}\" variantId=\"{7}\">{3}</tcdl:Link>";

        public const string TCDL_COMPONENT_LINK_TAG_REGEX = @"<tcdl:Link\s*type\s*=\s*""\s*Component\s*""\s*origin\s*=\s*""(?<origin>\s*tcm\s*:\s*\d+\s*-\s*\d+\s*-\s*\d+\s*){1}""\s*destination\s*=\s*""(?<destination>\s*tcm\s*:\s*\d+\s*-\s*\d+\s*){1}""\s*templateURI\s*=\s*""(?<templateuri>\s*tcm\s*:\s*\d+\s*-\s*\d+\s*-\s*\d+\s*){1}""\s*linkAttributes\s*=\s*""(?<linkattributes>\s*.*?(?=""))""\s*textOnFail\s*=\s*""(?<textonfail>\s*[a-z]*)""\s*addAnchor\s*=\s*""(?<addanchor>\s*[a-z]*)""\s*variantId\s*=\s*""(?<variantid>\s*[a-zA-Z]*)"">(?<linktext>.*?(?=</tcdl:Link>))</tcdl:Link>\s*";

        public const string TCDL_CUSTOM_COMPONENT_LINK_TAG_SKELETON = @"<{0}:ComponentLink runat=""server"" PageURI=""{1}"" ComponentURI=""{2}"" templateURI=""{3}"" addAnchor=""{4}"" LinkText=""{5}"" linkAttributes=""{6}"" textOnFail=""{7}""></{0}:ComponentLink>";        

        public const string TCDL_CUSTOM_TAG_PREFIX = "emaar";
        public const string TCDL_TAG_ATTRIBUTE_ORIGIN = "origin";
        public const string TCDL_TAG_ATTRIBUTE_DESTINATION = "destination";
        public const string TCDL_TAG_ATTRIBUTE_LINKATTRIBUTES = "linkattributes";
        public const string TCDL_TAG_ATTRIBUTE_TEMPLATEURI = "templateuri";
        public const string TCDL_TAG_ATTRIBUTE_TEXTONFAIL = "textonfail";
        public const string TCDL_TAG_ATTRIBUTE_TYPE = "type";
        public const string TCDL_TAG_ATTRIBUTE_VARIANT = "variant";
        public const string TCDL_TAG_ATTRIBUTE_ADDANCHOR = "addanchor";
        public const string TCDL_TAG_ATTRIBUTE_LINKTEXT = "linktext";

        public const string TCDL_TYPE_COMPONENT = "Component";
    }

    public static class GenericConstants
    {
        public const string RUN_AT_SERVER = "server";
        public const string FILE_SYSTEM_OFFENDING_REGEX = @"(?:\r\n|\n|\r)";
    }
}
