using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NW_GraphicPrograming;
using NW_GraphicPrograming.XML;

namespace NW_GraphicPrograming.UnitTest
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

            //Tools.readXML(@"C:\Users\pdere\Desktop\python\RWLV-Search Sets-General Model.xml");
            //var set = new JsonSelectionSets();
            ////Task.Factory.StartNew(() =>
            ////{
            //Thread t = new Thread(new ParameterizedThreadStart(Tools.convertXMLtoConfiguration),10);
           
            //t.Start(Tools.exchangeFile);


            
            JsonSelectionSets set = Tools.jsonSelectionSetsFile;
            
            Assert.IsNotNull(set);

            
        }
        

    }
}
