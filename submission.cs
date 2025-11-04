using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static string xmlURL = "https://styamamo.github.io/CSE445_A4/Hotels.xml";
        public static string xmlErrorURL = "https://styamamo.github.io/CSE445_A4/HotelsErrors.xml";
        public static string xsdURL = "https://styamamo.github.io/CSE445_A4/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try{
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdUrl);
                settings.ValidationType = ValidationType.Schema;

                string errMsg = "";

                // collect violations
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errMsg += e.Message + Environment.NewLine;
                };

                //reader validate
                using(XmlReader reader = XmlReader.Create(xmlUrl, settings)){
                    //read document
                    while(reader.Read()){}
                }

                // if no messages then the xml matches schema
                if(errMsg.Length == 0){
                    return "No errors are found";
                } else {
                    return errMsg;
                }
            }
            catch (Exception ex){
                return ex.Message;
            }
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {
            try{
                // download xml from url
                WebClient client = new WebClient();
                string xmlString = client.DownloadString(xmlUrl);

                // load xml into dom
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                // convert xml dom to json 
                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

                //return json
                return jsonText;
            }
            catch(Exception ex){
                return ex.Message;
            }

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
        }
    }

}
