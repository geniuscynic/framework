using System;
using DoCare.Extension.DataBase;
using DoCare.Extension.DataBase.Utility;

namespace Framework.test
{
    class Program
    {
        class Blog
        {
            public DateTime author { get; set; }
            public string title { get; set; }
            public string content { get; set; }
        }

        class User
        {
            public DateTime author { get; set; }
            public string title { get; set; }
            public string content { get; set; }
        }

        static void Main(string[] args)
        {
            var a = "1";
            var b = new
            {
                b = DateTime.Now.ToShortDateString()
            };
            var c = new
            {
                d = new
                {
                    e = DateTime.Now
                }
            };

            var connectStr = "Server=localhost;Database=blog;Trusted_Connection=True;MultipleActiveResultSets=true";
            var provider = "MsSql";

           // var generateModel = new GenerateModel(connectStr, provider);
           // var result = generateModel.GetTableDefine().Result;
            var dbclient = new Dbclient("Server=localhost;Database=blog;Trusted_Connection=True;MultipleActiveResultSets=true", "MsSql");

            dbclient.ComplexQueryable<Blog>().Where(t => t.title == a)
                .Where(t=>t.content == "dddd")
                .OrderBy(t => t.title)
                .OrderBy(t => new { t.author, t.content })
                .OrderByDesc(t => t.content)
                .ExecuteFirst();


            //dbclient.Queryable<Blog>().Where(t =>  t.title == a)
            //    .OrderBy(t=>t.title)
            //    .OrderBy(t=> new {t.author,t.content})
            //    .OrderByDesc(t=>t.content).ExecuteFirst();
            //dbclient.Saveable<Blog>(new Blog()).UpdateColumns(t => t.title);
            //dbclient.Saveable<Blog>(new Blog()).UpdateColumns(t => new {t.title, t.author});
            //Console.WriteLine("Hello World!");




            Console.Read();
        }
    }
}
