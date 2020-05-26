using System;
using WebCrawler;

namespace WordFrequencyCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler("http://www.chinadaily.com.cn", 10, 25, 3600, "chinadaily.txt");
            //crawler.Start();
            crawler.CarryOn();
        }
    }
}
