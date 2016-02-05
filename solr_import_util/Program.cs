using Bookmarks.Common;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using BookmarkProcessor;


namespace ImportUtil
{
    /// <summary>
    /// this code will retire soon
    /// to run card sort method use unit test project
    /// </summary>
    [Obsolete]
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new DeliciousParser.Parser();
            var bookmarks = parser.ParseBookmarks(args[0]);

            //var bookmarks = parser.ParseBookmarks("../../storage/delicious-2015-12-19.html");
            Console.WriteLine("Bookmarks Count: " + bookmarks.Count);

            var processedTags = Processor.CalculateTermCounts(bookmarks);
            //var counts = Processor.GetTermCounts(processedTags.Item2);

            //var freqTerms = Processor.GetMostFrequentTags(processedTags.Item2, GetExludeList(), 50);
            string output = null;
            if (args.Length > 1)
                output = args[1];

            //PrintTerms(freqTerms, output);

            var associatedTerms = Processor.GetAssociatedTerms(bookmarks, GetMSTechTopTags(), GetExludeList());

            PrintTerms(associatedTerms, output);
        }

        private static List<string> GetMSTechTopTags()
        {
            return new List<string>()
            {
                "c#","sharepoint",".net","csharp","windows","microsoft","codeplex","winforms","jquery","sqlserver"
                ,"WPF","win32","asp.net","wpf","wcf","webcontrols","visualstudio","asp.net-mvc","silverlight","powershell"
                ,"linq","angularjs","azure","fsharp","f#","ssis","windbg","sql2008","vs2010","xbox","msdn","WF","active-directory"
                ,"ide","dotnet","LINQ","ASP.NET","entityframework","C#","WCF","nhibernate","win32-calls","ie-helper","mono","fiddler"
                ,"kinect",".net4","sharepoint2010", "winapi","windows7","linq-provider","nuget","MSDN", "signalr","wf4","ado.net"
                ,"sysinternals",".net4.5","ninject","tsql","windowsmobile","ms-office","asp.net mvc", "aspnetmvc", "sqlserver2005"
                , "sqlserver2012", "excel", "sharepoint2003", "moss", "CardSpace", "razor", "vs-addons", "lucene.net", "ravendb"
                , "sharepoint2007", ".net4.0", "caml", "stsadm", "web.config", "tfs", "infopath", ".net5", "windows10", "winbugs"
                , "win8", "async-wcf", "mvvm", "documentdb", ".net-core", "charles-petzold", "azuredev", "Microsoft SQL Server"
                , "mssql", "russinovich", "windows_OS", "vs2013", "c#libraries", "awsome.net", "c#frameworks", "visual-studio"
                , "visual_studio", "c-sharp", "skype-c#", "platform-invoke", "IIS", "ATL", "Dotnet-Strings", "c#_types", "AutoResetEvent"
                , ".net-threading", "rhino-etl", "sdr#", "vs2012", "automapper", ".net utility library craigs", "Excel", "svchost", "win"
                , "IIS7", "xbox360", "Entity_Framework", "wcf-data-services", ".NET"
            };
        }

        private static List<string> GetExludeList()
        {
            var result = new List<string>() { "opensource", "security", "tools", "android", "books", "linux" 
            ,"python","!github", "cryptography", "education", "java", "free", "machine-learning", "!toronto-public-library"
            , "computer-networks", "statistics", "video", "coursera", "c++", "webdesign", "programming", "trading"
            , "mathematics", "tutorial", "r-project", "c-code", "voip", "tutorials", "mobile", "papers", "!filetype:pdf"
            , "javascript", "embedded", "testing", "business-tips", "search", "cpp", "business", "robotics", "awesome"
            , "debugging", "samples", "network-analysis", "google", "cryptocurrency", "howto", "pending-task", "douglas-schmidt"
            , "algorithms", "asterisk", "canada", "ai", "text-mining", "image-processing", "hadoop", "threading","arduino","blogs"
            , "online", "diy", "bitcoins", "hardware", "search-engine", "stanford", "public-web-services", "privacy", "concurrency"
            , "Tools", "virtualization", "designpatterns", "exploits", "development", "russian", "3d-printing", "hosting", "reference"
            , "api", "spring", "big-data", "performance", "design", "toronto", "tcp-ip", "ebooks", "templates", "datamining", "reversing"
            , "natural-language-parser", "ethereum", "dotnetnuke", "kazakhstan", "machinelearning", "software", "pentesting", "videos"
            , "semantic", "malware-analysis", "docs", "proxy", "math", "job", "library", "documentation", "ubuntu", "analysis", "certification"
            , "realestate", "nlp", "signal-processing", "raspberry-pi", "marketing", "ecommerce","research","reverse-engineering","encryption"
            ,"tips", "english", "apps", "google-api", "vs2008", "openhardware", "cloud", "code", "web" , "mvc", "computer-vision","data-mining"
            , "framework", "pdf", "flash", "bitcoin", "themes", "ssl", "cheap", "cms", "graph-processing", "edx", "virtual-box", "radio"
            , "gsm", "forensics", "ruby", "sip","css","audio","wifi","pentest","sms","matlab","food","youtube","ebook","ui"
            ,"search-engine-optimization","magazine","graphics","coding","owasp","eclipse","scanner","data","maps","science","facebook"
            ,"bigdata","wishlist","!googlecode","malware","semanticweb","information-retrieval","fitness","!wikipedia","social-networks"
            ,"car","tor","gps","nutrition-facts-today","operatingsystems","hacking","database","springmvc","skins","github","neuralnetworks"
            ,"camera","solr","php","appengine","startup","ocr","usb","network","finance","westpalmbeach","taxonomy-tools"
            ,"cloud-hosting","parser","visualization","mooc","password","wiki","assembly","ajax","edmonton","analytics","anonymity"
            ,"sdr","download","unittesting","xss","Opensource","news","internet-of-things","crypto","functional-programming","linguistics"
            ,"blog","electronics","3d","games","remote-shell","screencast","learning","semantic-web"
            ,"google-apps","satellite","satellite","ibm","language","xmpp","resources","orm","memory","Statistical Inference"
            ,"e-commerce","music","optimization","money", "golang", "computerscience", "guide", "html5", "intel", "metasploit"
            , "Penetration", "bayesian", "sniffer", "chromeextension", "wikipedia", "udacity","freeware", "deep-learning", "ios", "starter-kits"
            , "xml", "psychology-practices", "router", "exploit", "disassembler", "articles", "deployment", "graph", "oracle", "sensors", "agile"
            , "guitar", "garbage-collection", "sip-proxy", "addins", "vmware", "internet-tv", "ssl-certificate", "node.js", "elearning", "job-search"
            , "mining", "vendor", "streaming", "bruteforce", "octave", "open-router", "images", "bash", "downloads", "statistical inference"
            , "affiliate", "2008", "OLAP", "softphone", "movies", "biotech", "codeproject", "sourceforge", "shell", "course", "MIT", "moocs"
            , "crawler", "gis", "twitter", "machine_learning", "crack", "hack-examples", "examples", "nat", "plugin", "journal", "3d-printer"
            , "algorithm", "conference", "skype", "gc", "tool", "taxes", "energy", "compiler", "health", "community", "kernel", "wallet"
            , "hash", "amazon", "codecs", "magazines", "obfuscation", "networking", "nutrition", "server", "psychology", "sql", "paypal"
            , "injection", "restful", "C++", "ontology", "trading-robots", "wordnet", "designer","template", "spark","mysql", "toolkit"
            ,"bootstrap", "asynchronous","compression", "regex", "video-processing", "lectures", "space", "vpn", "md5", "passwords", "torrent"
            , "kali", "webcast", "SIM-card", "Android", "training", "monitoring", "arm", "unix", "volatility", "mapping", "bayes", "online courses"
            , "seo", "technology", "image", "stocks", "probability", "taxonomy", "Flash"
            , "mitm","archive","!google-play","space-program","raspberrypi","drones","rest","realtime","service"
            , "openwrt", "mimikatz", "ssh", "gnuradio", "ROP", "shopping","webrtc","extensions","parsing","classification","parallel"
            , "devices","jazz","full-text-search","input-output-port-c-programs"
            , "nodejs","infographics","ux","rss","plugins","backdoor","bioinformatics","maven","!stackoverflow","number-theory","scalability"
            , "Wi-Fi","dapps","patterns","webservices","3dprinting","subtitles","standards","ndk","p2p","cad","sandbox","fuzzing","shellcode"
            ,"nmap","recon","dns","architecture","vulnerability","opendata","uml","usbrubberducky","rubberducky","litecoins","anti-virus"
            ,"tdd","weightloss","async","mock","hack","apache","iphone","app-store","garbagecollection","filetype:pdf","engine","graphs","R"
            ,"USA","crime-rate","DID","yahoo","common-tasks"
            //, "", "", "", "", ""
            //, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
            };
            return result;
        }

        private static void PrintTerms(IEnumerable<KeyValuePair<string, int>> termCounts, string output)
        {
            StreamWriter writer = null;
            
            if (!string.IsNullOrEmpty(output)) 
            {
                writer = new StreamWriter(output);
                Console.SetOut(writer);
            }

            try
            {
                foreach (var termPair in termCounts)
                {
                    Console.WriteLine(string.Format("{0} -- {1}", termPair.Key, termPair.Value));
                }
            }
            finally 
            {
                writer.Close();
            }
        }

        private static List<string> LoadTagBundle(string path)
        {
            var result = new List<string>();
            using (var reader = File.OpenText(path))
            {
                var txt = reader.ReadToEnd();
                var split = txt.Split(new char[] { '\r', '\n', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var str in split)
                {
                    string tmp = str.Replace("\"", "").Trim();
                    //add only non-empty strings
                    if (!tmp.Equals(string.Empty))
                        result.Add(tmp);
                }
            }

            return result;
        }
    }
}
