using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System.IO;
using System.Globalization;
using CsvHelper;
// See https://aka.ms/new-console-template for more information
namespace Scraping
{
    class Program
    {
        static ScrapingBrowser _scrapingBrowser = new ScrapingBrowser();
        static void Main(string[] args)
        {
            var cactusPress= 
            
            GetLinks("https://cactusglobal.com/");
            GetPressDetails(cactusPress);
        }
        static List<string> GetLinks(string url)
        {
            var press = new List<string>();
            var html = GetHtml(url);
            var PressNews = html.CssSelect("a");
            foreach (var pressNew in PressNews)
            {
                if (pressNew.Attributes["href"].Value.Contains("press"))
                {
                    press.Add(pressNew.Attributes["href"].Value);
                }
            }
            return press;
        }



        static List<PressDetails> GetPressDetails(List<string> urls)
        {
            var lstPressDetails = new List<PressDetails>();
            foreach (var press in urls)
            {
                var htmlNode = GetHtml(press);
                var pressDetails = new PressDetails();
        
        
                pressDetails.Heading = htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("/html/body/div/main/main/section[1]/div/div/div[1]/div/h1").InnerText;
                pressDetails.Title= htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("/html/body/div/main/main/section[2]/div/div[1]/div[1]/h2").InnerText;
                pressDetails.Date= htmlNode.OwnerDocument.DocumentNode.SelectSingleNode("/html/body/div/main/main/section[5]/div/div/div/div/p").InnerText;
                lstPressDetails.Add(pressDetails);
            }
            
            
            using (var writer = new StreamWriter("PressInformation.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(lstPressDetails);
            }
            
            return lstPressDetails;
        }

        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _scrapingBrowser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
    public class PressDetails
    {
        public string? Heading { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }

    }
    
    public class PressInfo
    {
        public string? Heading { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }

    }

}



