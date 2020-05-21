using System;
using WebCrawler;

namespace WordFrequencyCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler("http://www.chinadaily.com.cn", 1, 25, "chinadaily.txt");
            crawler.Start();
        }
    }
}
