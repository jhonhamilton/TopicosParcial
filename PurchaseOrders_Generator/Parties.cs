using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PurchaseOrders_Generator
{
    public class Parties
    {
        Random r;
        private XmlDocument xml;
        private XmlNode sellerSupplierParty;
        private XmlNode buyerCustomerParty;
        private XmlNamespaceManager nsmgr;

        public Parties(FileStream ruta)
        {
            xml = new XmlDocument();            
            xml.Load(ruta);
            setNameSpaceManager();
            r = new Random();
        }

        public XmlDocument Xml
        {
            get { return xml; }
            set { xml = value; }
        }

        public XmlNode SellerSupplierParty
        {
            get { return sellerSupplierParty; }
            set { sellerSupplierParty = value; }
        }

        public XmlNode BuyerCustomerParty
        {
            get { return buyerCustomerParty; }
            set { buyerCustomerParty = value; }
        }

        private void setNameSpaceManager()
        {
            nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("", "urn:oasis:names:specification:ubl:schema:xsd:Order-2");
        }

        public void rotateSeller()
        {
            XmlNodeList nodes = xml.SelectNodes("/*/cac:Party", nsmgr);
            XmlNode node = nodes[r.Next(nodes.Count)];
            sellerSupplierParty = node;
        }

        public void rotateBuyer()
        {
            XmlNodeList nodes = xml.SelectNodes("/*/cac:Party", nsmgr);
            XmlNode node = nodes[r.Next(nodes.Count)];
            buyerCustomerParty = node;
        }
    }
}
