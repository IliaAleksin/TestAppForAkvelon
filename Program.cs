using System;
using System.IO;
using System.Data;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace TestAppForAkvelon
{
    class Program
    {
        static string[] requiredUsernames = new string[] { };
        static string urlAdress = "https://jsonmock.hackerrank.com/api/article_users?page=";
        static int startPage;
        static int pages;
        static string inputString;
        static List<string> InputUsernames = new List<string> { };
        static List<int> InputNumberOfPosts = new List<int> { };
        static void Main(string[] args)
        {
            //WriteToConsoleData("Я понятия не имею - почему коппилятор на HackerRank отказалася компилиповать этот код после раскрытия списка зависимостей, поэтому я его написал в Visual Studio");
            //WriteToConsoleData("");

            GetRequest(urlAdress);
            string[] arr = new string[10];
            arr = InsertSortingFunction(InputUsernames, InputNumberOfPosts);
            for (int i = 0; i < arr.Length; i++)
            {
                WriteToConsoleData(arr[i]);
            }
            //WriteToConsoleData("");
            //WriteToConsoleData("Это весь список топ-10 пользователей по количеству постов.");
            //Console.ReadLine();
        }
        private static void GetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes("username:password"));
            request.PreAuthenticate = true;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                while (!reader.EndOfStream)
                {
                    inputString = reader.ReadLine();
                }
                Rootobject? rootobject = JsonSerializer.Deserialize<Rootobject>(inputString);
                pages = rootobject.total_pages;
                startPage = rootobject.page;
                for (int j = 0; j < rootobject.data.Length; j++)
                    {
                    InputUsernames.Add(rootobject.data[j].username);
                    InputNumberOfPosts.Add(rootobject.data[j].submission_count);
                    }
            }

            for (int i = startPage + 1; i <= pages; i++)
            {
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url + i.ToString());
                request2.ContentType = "application/json; charset=utf-8";
                request2.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes("username:password"));
                request2.PreAuthenticate = true;
                HttpWebResponse response2 = request2.GetResponse() as HttpWebResponse;

                using (Stream responseStream = response2.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                    while (!reader.EndOfStream)
                    {
                        inputString = reader.ReadLine();
                    }
                    Rootobject? rootobject = JsonSerializer.Deserialize<Rootobject>(inputString);

                    for (int j = 0; j < rootobject.data.Length; j++)
                    {
                        InputUsernames.Add(rootobject.data[j].username);
                        InputNumberOfPosts.Add(rootobject.data[j].submission_count);
                    }
                }
            }
        }
        public static string[] InsertSortingFunction(List<string> namesList, List<int> submissionCountList)
        {
            string[] outputArray = new string[10];
            
                for (var i = 1; i < submissionCountList.Count; i++)
                {
                    var key1 = submissionCountList[i];
                    var key2 = namesList[i];
                var j = i;
                    while ((j > 1) && (submissionCountList[j - 1] > key1))
                    {
                        Swap(submissionCountList[j - 1], submissionCountList[j]);
                        Swap(namesList[j - 1], namesList[j]);
                    j--;
                    }
                    submissionCountList[j] = key1;
                    namesList[j] = key2;
                }
            for(int i=0;i<10;i++)
            {
                outputArray[i] = namesList[i];
            }

            return outputArray;
        }
        static void Swap(object e1, object e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
        private static void WriteToConsoleData(string data)
        {
            Console.WriteLine(data);
        }
            public class Rootobject
            {
                public int page { get; set; }
                public int per_page { get; set; }
                public int total { get; set; }
                public int total_pages { get; set; }
                public Datum[] data { get; set; }
            }            
            public class Datum
            {   
                public int id { get; set; }
                public string username { get; set; }
                public string about { get; set; }
                public int submitted { get; set; }
                public DateTime updated_at { get; set; }
                public int submission_count { get; set; }
                public int comment_count { get; set; }
                public int created_at { get; set; }
            }
    }
}