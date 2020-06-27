using System;
using WebCrawler;

namespace WordFrequencyCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //爬取目标为ChinaDaily新闻网站
            Crawler crawler = new Crawler("http://www.chinadaily.com.cn", 10, 25, 3600, "chinadaily.txt");
            //crawler.Start();
            crawler.CarryOn();
        }
    }
}
