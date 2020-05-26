using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

namespace WebCrawler
{
	[Serializable]
	public class Url
	{
		public string url;
		public int depth;

		public Url(string url, int depth)
		{
			this.url = url;
			this.depth = depth;
		}
	}

	public class DownloadedEventArgs : EventArgs
	{
		public string html;

		public DownloadedEventArgs(string html)
		{
			this.html = html;
		}
	}

	public class TaskTerminatedEvent : EventArgs
	{

	}

    #region delegates
    public delegate void DownloadedPageEventHandler(object sender, DownloadedEventArgs e);
	public delegate void TaskTerminatedEventHandler(object sender, TaskTerminatedEvent e);
    #endregion

    public class Crawler
	{
        #region const fields
        private const int waitTime = 1500;				//单位: ms
		private const int maxCheckNullCount = 3;        //线程会检查url队列是否为空，为空时等待一段时间再检查，若达到最大检查次数，则结束
		private const int CheckStateTime = 120;			//定时保存的时间，单位: s
		private const string wordFreqTempFilePath = "wf.temp";
		private const string urlsUndisposedTempFilePath = "urlUnd.temp";
		private const string urlsDisposedTempFilePath = "urlDis.temp";
		#endregion

		#region private fields
		private int maxDepth;
		private int threadCount;
		private int maxThreadCount;
		private int maxRunTime;							//最大运行时间，单位: s
		private string wordFrequencyFilePath;

		private DateTime startTime;
		private DateTime endTime;
		private string mainDomainName;

		private Queue<Url> urlsUndisposed;
		private Queue<Url> urlsDisposed;
		private Dictionary<string, int> wordFrequency;
		private Thread[] downloadThreads;

		private static Crawler instance;                //自身的静态引用，用于定时器中调用Save函数等
		private System.Timers.Timer checkStateTimer;
		private System.Timers.Timer shutDownTimer;
		private bool shutDownFlag;
		#endregion

		#region events
		public event DownloadedPageEventHandler Downloaded;
		public event TaskTerminatedEventHandler TaskTerminated;
        #endregion

        #region public methods
        public Crawler(string baseUrl, int maxDepth, int maxThreadCount, int maxRunTime, string wordFrequencyFilePath)
		{
			this.urlsDisposed = new Queue<Url>();
			this.urlsUndisposed = new Queue<Url>();
			this.wordFrequency = new Dictionary<string, int>();

			this.urlsUndisposed.Enqueue(new Url(baseUrl, 0));
			this.mainDomainName = GetMainDomain(baseUrl);
			this.maxDepth = maxDepth;
			this.maxThreadCount = maxThreadCount;
			this.maxRunTime = maxRunTime;
			this.wordFrequencyFilePath = wordFrequencyFilePath;

			checkStateTimer = new System.Timers.Timer();						//定时保存的定时器（用于避免内存爆掉）
			checkStateTimer.Interval = CheckStateTime * 1000;
			checkStateTimer.Elapsed += new ElapsedEventHandler(CheckStateTimerEvent);
			checkStateTimer.Enabled = true;

			shutDownTimer = new System.Timers.Timer();					//定时关闭的定时器（用于避免运行时间过长）
			shutDownTimer.Interval = maxRunTime * 1000;
			shutDownTimer.Elapsed += new ElapsedEventHandler(ShutDownTimerEvent);
			shutDownTimer.Enabled = true;
			shutDownFlag = false;

			instance = this;
		}

		public Crawler(List<string> baseUrls, int maxDepth, int maxThreadCount, int maxRunTime, string filePath):
			this(baseUrls[0], maxDepth, maxThreadCount, maxRunTime, filePath)
		{
			for(int i = 1; i < baseUrls.Count; i++)
			{
				this.urlsUndisposed.Enqueue(new Url(baseUrls[i], 0));
			}
		}

		public void Start()
		{
			startTime = DateTime.Now;
			downloadThreads = new Thread[maxThreadCount];

			for (int i = 0; i < maxThreadCount; i++)
			{
				ThreadStart ts = new ThreadStart(this.Process);
				downloadThreads[i] = new Thread(ts);
				downloadThreads[i].Start();
				threadCount = maxThreadCount;
				Console.WriteLine("线程{0}开始执行", i);
			}
		}

		public void CarryOn()
		{
			if(File.Exists(wordFreqTempFilePath) && File.Exists(urlsUndisposedTempFilePath) && File.Exists(urlsDisposedTempFilePath))
			{
				this.urlsUndisposed.Clear();

				BinaryFormatter bf = new BinaryFormatter();
				FileStream wordFreqfs = new FileStream(wordFreqTempFilePath, FileMode.Open, FileAccess.Read);
				FileStream urlsUndisposedfs = new FileStream(urlsUndisposedTempFilePath, FileMode.Open, FileAccess.Read);
				FileStream urlsDisposedfs = new FileStream(urlsDisposedTempFilePath, FileMode.Open, FileAccess.Read);
				this.wordFrequency = bf.Deserialize(wordFreqfs) as Dictionary<string, int>;
				this.urlsUndisposed = bf.Deserialize(urlsUndisposedfs) as Queue<Url>;
				this.urlsDisposed = bf.Deserialize(urlsDisposedfs) as Queue<Url>;
				Start();
			}
			else
			{
				Console.WriteLine("Temp文件不存在，无法从上次的进度中继续执行");
			}
		}

		public void Save()					//Save函数兼具保存WordFrequency和临时状态的功能，方便发生意外时从状态中恢复
		{
			FileStream fout = new FileStream(wordFrequencyFilePath, FileMode.Create, FileAccess.Write);
			StreamWriter sout = new StreamWriter(fout, System.Text.Encoding.UTF8);

			foreach (var wf in wordFrequency)
			{
				sout.WriteLine(wf.Key + "," + wf.Value.ToString());
			}

			sout.Close();
			fout.Close();
			endTime = DateTime.Now;
			Console.WriteLine("词频已写入文件");

			if(urlsUndisposed.Count != 0)
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream wordFreqfs = new FileStream(wordFreqTempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				FileStream urlsUndisposedfs = new FileStream(urlsUndisposedTempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				FileStream urlsDisposedfs = new FileStream(urlsDisposedTempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				bf.Serialize(wordFreqfs, wordFrequency);
				bf.Serialize(urlsUndisposedfs, urlsUndisposed);
				bf.Serialize(urlsDisposedfs, urlsDisposed);

				wordFreqfs.Close();
				urlsUndisposedfs.Close();
				urlsDisposedfs.Close();
				Console.WriteLine("已保存临时状态");
			}

			TimeSpan ts = endTime.Subtract(startTime);
			Console.WriteLine("统计了{0}个URL, 耗费时长: {1}ms", urlsDisposed.Count, ts.TotalMilliseconds);
		}
		#endregion

		#region private methods
		private string GetMainDomain(string url)
		{
			string domainNameStr = ".com|.co|.info|.net|.org|.me|.mobi|.us|.biz|.xxx|.ca|.co.jp|.com.cn|" +
				".net.cn|.org.cn|.mx|.tv|.ws|.ag|.com.ag|.net.ag|.org.ag|.am|.asia|.at|.be|.com.br|.net.br|.bz|" +
				".com.bz|.net.bz|.cc|.com.co|.net.co|.nom.co|.de|.es|.com.es|.nom.es|.org.es|.eu|.fm|.fr|.gs|.in|" +
				".co.in|.firm.in|.gen.in|.ind.in|.net.in|.org.in|.it|.jobs|.jp|.ms|.com.mx|.nl|.nu|.co.nz|.net.nz|" +
				".org.nz|.se|.tc|.tk|.tw|.com.tw|.idv.tw|.org.tw|.hk|.co.uk|.me.uk|.org.uk|.vg";
			string[] domainName = domainNameStr.Split('|');

			foreach (var dn in domainName)
			{
				if(url.Contains(dn))
				{
					int startIndex = url.IndexOf("www.") + 4;
					int length = url.IndexOf(dn) - startIndex;
					return url.Substring(startIndex, length);
				}
			}
			return "";
		}

        private bool ExistUrl(string url)
		{
			//这里由于队列中存放的是Url结构体，不能直接使用.contains方法，否则两个url相同但不同深度的Url会被误判
			foreach (var urlUndisposed in urlsUndisposed)
			{
				if (urlUndisposed.url == url)
				{
					return true;
				}
			}

			foreach (var urlDisposed in urlsDisposed)
			{
				if (urlDisposed.url == url)
				{
					return true;
				}
			}
			return false;
		}

		private List<string> GetLinks(string html)
		{
			List<string> newUrls = new List<string>();
			string hrefRef = @"(href|HREF)[]*=[]*[""'][^""'#>]+[""']";
			MatchCollection hrefMatches = new Regex(hrefRef).Matches(html);
			foreach (Match hmatch in hrefMatches)
			{
				var temp = hmatch.Value.IndexOf('"');
				var url = hmatch.Value.Substring(temp + 1, hmatch.Value.Length - temp - 2);

				if(!url.Contains(mainDomainName))		//不包含主域名的网站一概不处理
				{
					continue;
				}

				if (url.Contains(".jpg") || url.Contains(".gif") || url.Contains(".png")            //筛去资源链接
					|| url.Contains(".css") || url.Contains(".js") || url.Length < 5)
				{
					continue;
				}

				if (url.Substring(0, 4) == "http" && url[4] != 's')				//筛去javascript:void(0)情况，且补全间接链接
				{
					newUrls.Add(url);
				}
				else if (url.Substring(0, 5) == "//www")
				{
					newUrls.Add("http:" + url);
				}
			}
			return newUrls;
		}

		private Dictionary<string, int> GetWordFrequency(string html)
		{
			string strRef = @"<p>[^<>]*</p>";			//段落的正则表达式，且除去了内部含有html标签的段落（通常是有渲染的标题）
			Dictionary<string, int> wordFrequency = new Dictionary<string, int>();
			MatchCollection paraMatches = new Regex(strRef).Matches(html);
			foreach (Match pm in paraMatches)
			{
				try
				{
					int startIndex = pm.Value.IndexOf('>') + 1;
					int endIndex = pm.Value.LastIndexOf('<');
					int length = endIndex - startIndex;
					string para = pm.Value.Substring(startIndex, length).ToUpper();		//截取引号内的文本，且全部转换为大写
					string[] words = para.Split(new char[] { ' ', '"', ',', '.', ':', ';', '-', '–',
						'(', ')', '<', '>', '|', '&', '#', '$', '￥', '‘', '’' });
					
					string wordRef = @"^[a-zA-Z]+$";
					Regex regex = new Regex(wordRef);
					foreach (var word in words)
					{
						if (regex.IsMatch(word))
						{
							if (wordFrequency.ContainsKey(word))
							{
								wordFrequency[word] += 1;
							}
							else
							{
								wordFrequency.Add(word, 1);
							}
						}
					}
				}
				catch(Exception e)
				{
					Console.WriteLine("词频处理失败: " + e.Message);
				}
			}
			return wordFrequency;
		}

		private void Process()
		{
			int checkNullCount = 0;			//检查发现url队列为空的次数，达到一定次数时结束线程

			while(!shutDownFlag)			//增加一个强制关闭Flag
			{
				if (urlsUndisposed.Count != 0)
				{
					Url task;
					lock (this)														//从队列中取出任务
					{
						task = urlsUndisposed.Dequeue();
						urlsDisposed.Enqueue(task);
						Console.WriteLine("处理URL: " + task.url + "\t深度: " + task.depth);
					}
					try
					{
						WebClient webClient = new WebClient();						//获取网页
						webClient.Encoding = Encoding.UTF8;
						string html = webClient.DownloadString(task.url);

						Downloaded?.Invoke(this, new DownloadedEventArgs(html));	//调用下载完成的事件

						List<string> newUrls = GetLinks(html);						//获取网页中的链接
						if (task.depth < maxDepth)									//查重后放入队列
						{
							lock (this)
							{
								foreach (var url in newUrls)
								{
									if (!ExistUrl(url))
									{
										urlsUndisposed.Enqueue(new Url(url, task.depth + 1));
									}
								}
							}
						}

						Dictionary<string, int> newWordFrequency = GetWordFrequency(html);		//获取单词
						lock (this)																//计算词频
						{
							foreach (var wf in newWordFrequency)
							{
								if (wordFrequency.ContainsKey(wf.Key))
								{
									wordFrequency[wf.Key] += wf.Value;
								}
								else
								{
									wordFrequency[wf.Key] = wf.Value;
								}
							}
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("URL处理失败: " + ex.Message);
					}
					checkNullCount = 0;
				}
				else
				{
					checkNullCount += 1;
					if (checkNullCount >= maxCheckNullCount)
					{
						break;
					}
					else
					{
						Thread.Sleep(waitTime);
					}
				}
			}

			threadCount -= 1;
			if(threadCount == 0)
			{
				Console.WriteLine("所有线程均已结束");
				Save();
				TaskTerminated?.Invoke(this, new TaskTerminatedEvent());	//当所有进程都结束时，调用任务结束事件
			}
		}

		private static void CheckStateTimerEvent(object o, ElapsedEventArgs e)
		{
			Console.WriteLine("定时写入中...");
			instance.Save();
		}

		private static void ShutDownTimerEvent(object o, ElapsedEventArgs e)
		{
			Console.WriteLine("达到最大运行时间，强制关闭中...");
			instance.shutDownTimer.Stop();
			instance.shutDownFlag = true;
		}
        #endregion
    }

}