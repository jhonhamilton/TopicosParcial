using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PurchaseOrders_Generator
{
    public class Items
    {
        public Items(FileStream ruta)
        {
            xml = new XmlDocument();
            xml.Load(ruta);
            setNameSpaceManager();
            r = new Random();
        }

        Random r;
        private XmlDocument xml;
        private XmlNamespaceManager nsmgr;
        private string description;
        private double priceAmount;
        private int quantity;
        private double lineExtensionAmount;


        public XmlDocument Xml
        {
            get { return xml; }
            set { xml = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double PriceAmount
        {
            get { return priceAmount; }
            set { priceAmount = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public double LineExtensionAmount
        {
            get { return PriceAmount * Quantity; }
            set { lineExtensionAmount = value; }
        }


        private void setNameSpaceManager()
        {
            nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"); nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("", "urn:oasis:names:specification:ubl:schema:xsd:Catalogue-2");
        }

        public void rotateItem()
        {
            XmlNodeList nodes = xml.SelectNodes("/*/cac:CatalogueLine", nsmgr);
            int rand = r.Next(nodes.Count);
            XmlNode node = nodes[rand].CloneNode(true);
            description = (node.SelectSingleNode("//cbc:Description", nsmgr).InnerText);
            priceAmount = double.Parse(node.SelectSingleNode("//cbc:PriceAmount", nsmgr).InnerText);
            Quantity = r.Next(100);
        }
    }
}
