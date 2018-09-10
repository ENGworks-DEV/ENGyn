using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENGyne;
using ENGyne.XML;

namespace ENGyne.UnitTest
{
    [TestClass]
    public class ImportXMLConfig
    {
        [TestMethod]
        public void readXML_exchangeFileIsValid()
        {
            
            Tools.readXML(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.xml");

            Assert.IsNotNull(Tools.exchangeFile);

        }
        [TestMethod]
        public void readXML_exchangeContainsSearchSets()
        {

            Tools.readXML(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.xml");

            Assert.IsNotNull(Tools.exchangeFile.Selectionsets);

        }

        [TestMethod]
        public void XconvertXMLtoConfiguration_convertsToJson()
        {

            Tools.readXML(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.xml");
            //var set = new JsonSelectionSets();
            ////Task.Factory.StartNew(() =>
            ////{
            //Thread t = new Thread(new ParameterizedThreadStart(Tools.convertXMLtoConfiguration),10);

            //t.Start(Tools.exchangeFile);

            Tools.convertXMLtoConfiguration(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.json");

           
            
            Assert.IsTrue(File.Exists(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.json"));

            
        }
        [TestMethod]
        public void XconvertXMLtoConfiguration_convertsClass()
        {

            Tools.readXML(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.xml");
            //var set = new JsonSelectionSets();
            ////Task.Factory.StartNew(() =>
            ////{
            //Thread t = new Thread(new ParameterizedThreadStart(Tools.convertXMLtoConfiguration),10);

            //t.Start(Tools.exchangeFile);

            Tools.convertXMLtoConfiguration(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.json");



            Assert.IsNotNull(Tools.jsonSelectionSetsFile);


        }


    }
}
