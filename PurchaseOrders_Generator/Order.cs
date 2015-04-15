using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PurchaseOrders_Generator
{
    public class Order
    {
        private XmlDocument xml;
        private string id;
        private string guuid;
        private DateTime issueDate;
        private XmlNamespaceManager nsmgr;

        public Order()
        {
            FileStream fs = new FileStream("Order-template.xml", FileMode.Open, FileAccess.Read);
            xml = new XmlDocument();
            xml.Load(fs);
            setNameSpaceManager();
        }

        public XmlDocument Xml
        {
            get { return xml; }
            set { xml = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string GUUID
        {
            get { return guuid; }
            set { guuid = value; }
        }

        public DateTime IssueDate
        {
            get { return DateTime.Now; }
            set { issueDate = value; }
        }

        public XmlNamespaceManager Nsmgr
        {
            get { return nsmgr; }
            set { nsmgr = value; }
        }

        public void generarID()
        {
            Random r = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = posibles.Length;
            char letra;
            int longitudNuevaCadena = 10;
            string nuevaCadena = "";
            for (int i = 0; i < longitudNuevaCadena; i++)
            {
                letra = posibles[r.Next(longitud)];
                nuevaCadena += letra.ToString();
            }
            id = nuevaCadena;
            setSingleNodo("/*/cbc:ID", nuevaCadena);
        }

        public void generarGUIID()
        {
            guuid = Guid.NewGuid().ToString();
            setSingleNodo("/*/cbc:UUID", guuid);
        }

        public void setIssueDate()
        {
            setSingleNodo("/*/cbc:IssueDate", IssueDate.ToString("yyyy/MM/dd hh:mm:ss"));
        }

        public void setSingleNodo(string xpath, string value)
        {
            XmlNode node = xml.SelectSingleNode(xpath, Nsmgr);
            node.InnerText = value;
        }

        public void reemplazarNodo(string xpath, XmlNode nuevoNodo)
        {
            XmlNode hijo = xml.SelectSingleNode(xpath, Nsmgr);
            XmlNode padre = hijo.ParentNode;
            padre.AppendChild(hijo.OwnerDocument.ImportNode(nuevoNodo, true));
            padre.RemoveChild(hijo);
        }

        public void agregarOrderLine(XmlNode orderLine)
        {
            XmlNode hijo = Xml.SelectSingleNode("/*/cac:OrderLine", nsmgr);
            hijo.ParentNode.AppendChild(hijo.OwnerDocument.ImportNode(orderLine, true));
        }

        private void setNameSpaceManager()
        {
            Nsmgr = new XmlNamespaceManager(xml.NameTable);
            Nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"); Nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            Nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            Nsmgr.AddNamespace("", "urn:oasis:names:specification:ubl:schema:xsd:Order-2");
        }

        public XmlNode getOrderLine()
        {
            return xml.SelectSingleNode("/*/cac:OrderLine", Nsmgr);
        }
    }
}
