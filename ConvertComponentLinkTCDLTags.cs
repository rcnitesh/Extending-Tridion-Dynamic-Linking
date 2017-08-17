/*
 * Date Created: 07.26.2017
 * Author: Nitesh <rc.nitesh@gmail.com>
 */
using System;
using System.Text.RegularExpressions;
using Tridion.ContentManager.Templating;
using Tridion.ContentManager.Templating.Assembly;
using System.Web;

namespace Emaar.Templating
{
    /// <summary>
    /// Converts TCDL Markup for Component Links to a custom ComponentLink control that removes trailing index.aspx/default.aspx from urls.
    /// The Template currently supports customization of TCDL Component type links only.
    /// <tcdl:link type="Component ... /> will be converted to <emaar:ComponentLink ... />
    /// </summary>
    [TcmTemplateTitle("ConvertTCDLComponentLinkTagsToCustomTags")]
    public class ConvertComponentLinkTCDLTags: TemplateBase
    {
        #region Private Variables

        private bool linksFound; 

        private enum TCDLLinkType
        {           
            Component,
            Binary,
            Page
        };

        #endregion

        #region TRANSFORM OVERRIDE
        public override void Transform(Engine engine, Package package)
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: Entered Transform");
            Initialize(engine, package);

            Item outputItem = GetOutputItem();
            string strippedOutputItem = "";
            
            if (outputItem == null)
            {
                Logger.Error("ConvertComponentLinkTCDLTags>>>: Package doesn't contain Output-item");
                return;
            }
            else
            {
                strippedOutputItem = outputItem.GetAsString();
                // Remove all \r\n from the string, if any present, as they interfere with Regex Matching
                strippedOutputItem = string.Join("", Regex.Split(strippedOutputItem, GenericConstants.FILE_SYSTEM_OFFENDING_REGEX));

                UpdateOutputItemIfLinksFound(outputItem, ReplaceOutputItemWithCustomComponentLinkTag(strippedOutputItem));
            }
        }

        #endregion

        #region OUTPUT ITEM PROCESSING
        private string ReplaceOutputItemWithCustomComponentLinkTag(string output)
        {
            string newOutput = output;
            newOutput = ReplaceLinksOfType(TCDLLinkType.Component, newOutput);            
            return newOutput;
        }

        /// <summary>
        /// Function to match the TCDL Links for type="Component" and convert such tags to a custom tag.
        /// </summary>
        /// <param name="type">Type of Link to Replace</param>
        /// <param name="output">The Output item in Package</param>
        /// <returns></returns>
        private string ReplaceLinksOfType(TCDLLinkType type, string output)
        {
            return Regex.Replace(output, GetRegExForType(type), new MatchEvaluator((match) =>
            {
                Logger.Info("MATCH_FOUND>>");
                linksFound = true;
                return ConvertToCustomComponentLinkTag(match);
            }));
        }       

        private string ConvertToCustomComponentLinkTag(Match match)
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: ORIGINAL_TCDL_TAG:= " + match.Value);
            return CreateCustomComponentLinkTag(GetTCDLComponentLinkModel(match));
        }      

        /// <summary>
        /// Function to return the customized tag for TCDL Component Links
        /// </summary>
        /// <param name="lComponentModel">A Class model representing the TCDL Component Link</param>        
        /// <returns></returns>
        private string CreateCustomComponentLinkTag(TCDLLinkComponent lComponentModel)
        {
            CustomTagComponentModel tComponentModel = GetTCDLTransformedCustomComponentModel(lComponentModel);

            Logger.Info("ConvertComponentLinkTCDLTags>>>: FINAL_CUSTOM_LINK_TAG:= " + string.Format(TridionConstants.TCDL_CUSTOM_COMPONENT_LINK_TAG_SKELETON, TridionConstants.TCDL_CUSTOM_TAG_PREFIX, tComponentModel.PageURI, tComponentModel.ComponentURI, tComponentModel.TemplateURI, tComponentModel.AddAnchor, tComponentModel.LinkText, tComponentModel.LinkAttributes, tComponentModel.TextOnFail));

            return string.Format(TridionConstants.TCDL_CUSTOM_COMPONENT_LINK_TAG_SKELETON, TridionConstants.TCDL_CUSTOM_TAG_PREFIX, tComponentModel.PageURI, tComponentModel.ComponentURI, tComponentModel.TemplateURI, tComponentModel.AddAnchor, tComponentModel.LinkText, tComponentModel.LinkAttributes, tComponentModel.TextOnFail);
        }

        /// <summary>
        /// Convert the TCDL Component Link type to an Object/Model representing the Custom Tag
        /// </summary>
        /// <param name="lComponent"></param>
        /// <returns></returns>
        private CustomTagComponentModel GetTCDLTransformedCustomComponentModel(TCDLLinkComponent lComponent)
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: Generating Custom Model");

            CustomTagComponentModel temp = new CustomTagComponentModel();

            temp.PageURI = lComponent.origin;
            temp.ComponentURI = lComponent.destination;
            temp.TemplateURI = lComponent.templateURI;
            temp.AddAnchor = lComponent.addAnchor;
            temp.LinkText = lComponent.linkText;
            temp.LinkAttributes = lComponent.linkAttributes;
            temp.TextOnFail = lComponent.textOnFail;
            temp.runat = GenericConstants.RUN_AT_SERVER;

            Logger.Info("ConvertComponentLinkTCDLTags>>>: Generating Custom Model Success>>");

            return temp;
        }

        /// <summary>
        /// Get an Object/Model representing the TCDL Component Link
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private TCDLLinkComponent GetTCDLComponentLinkModel(Match match)
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: Generating TCDL Model For:>> " + match.ToString());

            TCDLLinkComponent tempC = new TCDLLinkComponent();
            tempC.origin = match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_ORIGIN].Value;
            tempC.destination = match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_DESTINATION].Value;
            tempC.linkAttributes = match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_LINKATTRIBUTES].Value;
            tempC.templateURI = match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_TEMPLATEURI].Value;
            tempC.textOnFail = bool.Parse(match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_TEXTONFAIL].Value); // TextOnFail is a boolean
            tempC.type = TridionConstants.TCDL_TYPE_COMPONENT;
            tempC.variantId = match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_VARIANT].Value;
            tempC.addAnchor = bool.Parse(match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_ADDANCHOR].Value); // AddAnchor is a boolean
            tempC.linkText = System.Net.WebUtility.HtmlEncode(match.Groups[TridionConstants.TCDL_TAG_ATTRIBUTE_LINKTEXT].Value); // LinkText can have <img ../> tags inside. Always Encode!

            Logger.Info("ConvertComponentLinkTCDLTags>>>: Generating TCDL Model Success>>");

            return tempC;
        }
        
        private string GetRegExForType(TCDLLinkType type)
        {
            switch (type)
            {
                case TCDLLinkType.Component:
                    return TridionConstants.TCDL_COMPONENT_LINK_TAG_REGEX;                
                default:
                    return string.Empty; // return string.Empty for NO replacement of tags
            }
        }

        #endregion 

        #region GET & UPDATE OUTPUT ITEM

        /// <summary>
        /// Gets the Output item from Package
        /// </summary>
        /// <returns></returns>
        private Item GetOutputItem()
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: GETTING_OUTPUT_ITEM");
            return _package.GetByName(Package.OutputName);
        }

        private void UpdateOutputItemIfLinksFound(Item outputItem, string content)
        {
            if (linksFound)
            {                
                UpdateItem(outputItem, content);
            }
        }

        /// <summary>
        /// Updates the Output item in Package
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatedContent"></param>
        private void UpdateItem(Item item, string updatedContent)
        {
            Logger.Info("ConvertComponentLinkTCDLTags>>>: UPDATING_OUTPUT_ITEM>>");

            _package.Remove(item);
            item.SetAsString(updatedContent);
            _package.PushItem(Package.OutputName, item);

            Logger.Info("ConvertComponentLinkTCDLTags>>>: UPDATING_OUTPUT_ITEM_SUCCESS>>");
        }

        #endregion 
    }
}
