using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PurchaseOrders_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Console.WriteLine("Ingrese la cantidad de ordenes que desea realizar");
            string cantidadOrdenes = Console.ReadLine();
            while(!EsNumerico(cantidadOrdenes)){
                Console.WriteLine("Lo ingresado no es numerico, intentelo de nuevo");
                cantidadOrdenes = Console.ReadLine();
            }

            Stopwatch tiempo = new Stopwatch();

            for (int i = 0; i < Convert.ToInt32(cantidadOrdenes); i++)
            {
                FileStream partie = new FileStream("parties.xml", FileMode.Open, FileAccess.Read);
                FileStream items = new FileStream("items.xml", FileMode.Open, FileAccess.Read);
                
                Order ordenCompra = new Order();
                ordenCompra.generarID();
                ordenCompra.generarGUIID();
                ordenCompra.setIssueDate();

                Parties parties = new Parties(partie);
                parties.rotateSeller();
                parties.rotateBuyer();

                ordenCompra.reemplazarNodo("/*/cac:SellerSupplierParty/cac:Party", parties.SellerSupplierParty);
                ordenCompra.reemplazarNodo("/*/cac:BuyerCustomerParty/cac:Party", parties.BuyerCustomerParty);

                Items item = new Items(items);

                int cantidadOrderLines = r.Next(10);

                for (int j = 0; j < cantidadOrderLines; j++)
                {
                    item.rotateItem();

                    XmlNode orderLine = ordenCompra.getOrderLine();
                    orderLine.SelectSingleNode("//cac:Item/cbc:Description", ordenCompra.Nsmgr).InnerText = item.Description.ToString();
                    orderLine.SelectSingleNode("//cbc:Quantity", ordenCompra.Nsmgr).InnerText = item.Quantity.ToString();
                    orderLine.SelectSingleNode("//cbc:PriceAmount", ordenCompra.Nsmgr).InnerText = item.PriceAmount.ToString();
                    orderLine.SelectSingleNode("//cac:LineItem/cbc:LineExtensionAmount", ordenCompra.Nsmgr).InnerText = item.LineExtensionAmount.ToString();
                    
                    ordenCompra.agregarOrderLine(orderLine);
                }

                string path = ordenCompra.GUUID + ".xml";
                FileStream guardar = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                ordenCompra.Xml.Save(guardar);

                partie.Close();
                items.Close();
            }
            Console.WriteLine("Tiempo total: " + tiempo.Elapsed + " milésimas de segundos");
            Console.ReadKey();
        }

        private static bool EsNumerico(string dato)
        {
            for (int i = 0; i < dato.Length; i++)
            {
                if (!Char.IsDigit(dato[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
