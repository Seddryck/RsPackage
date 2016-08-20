using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RsPackage.Transform
{
    public class DataSourceTransformer
    {
        public XmlDocument ToReportingService(XmlDocument vsDocument)
        {
            if (!IsVisualStudio(vsDocument))
                throw new ArgumentException();

            var rsDocument = new XmlDocument();
            var rsDeclaration = rsDocument.CreateXmlDeclaration("1.0", "utf-8", "yes");
            rsDocument.AppendChild(rsDeclaration);

            var parentRsNode = rsDocument.CreateNode(XmlNodeType.Element, "DataSourceDefinition", ""); // See handling of namespace at the bottom
            rsDocument.AppendChild(parentRsNode);

            var parentVsNode = vsDocument.SelectSingleNode("//ConnectionProperties");
            var extensionVsNode = parentVsNode.SelectSingleNode("Extension");
            if (extensionVsNode!=null)
            {
                var extensionRsNode = rsDocument.ImportNode(extensionVsNode, true);
                parentRsNode.AppendChild(extensionRsNode);
            }

            var connectStringVsNode = parentVsNode.SelectSingleNode("ConnectString");
            if (connectStringVsNode != null)
            {
                var connectStringRsNode = rsDocument.ImportNode(connectStringVsNode, true);
                parentRsNode.AppendChild(connectStringRsNode);
            }

            var integratedSecurityVsNode = parentVsNode.SelectSingleNode("IntegratedSecurity");
            if (integratedSecurityVsNode != null && integratedSecurityVsNode.InnerText== "true")
            {
                var credentialRetrievalRsNode = rsDocument.CreateElement("CredentialRetrieval");
                credentialRetrievalRsNode.InnerText = "Integrated";
                parentRsNode.AppendChild(credentialRetrievalRsNode);
            }

            var enabledRsNode = rsDocument.CreateElement("Enabled");
            enabledRsNode.InnerText = "True";
            parentRsNode.AppendChild(enabledRsNode);

            //Heyyy we need to manually add the namespace to avoid xmlns="" for each element (Yes: it's an hack)
            var namespaceAttribute = rsDocument.CreateAttribute("xmlns");
            namespaceAttribute.Value= "http://schemas.microsoft.com/sqlserver/reporting/2006/03/reportdatasource";
            parentRsNode.Attributes.Append(namespaceAttribute);

            return rsDocument;
        }

        public virtual bool IsVisualStudio(XmlDocument vsDocument)
        {
            return vsDocument.DocumentElement.Name == "RptDataSource";
        }

        public virtual bool IsReportingService(XmlDocument vsDocument)
        {
            return vsDocument.DocumentElement.Name == "DataSourceDefinition";
        }
    }
}
