using System;
using WebCrawler;

namespace WordFrequencyCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler("http://www.chinadaily.com.cn", 2, 25, "test.txt");
            crawler.Start();
        }
    }
}
