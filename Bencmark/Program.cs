using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BreakAwayConsole
{
    class Program
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main(string[] args)
        {          
            //InsertDestination();

            var summary = BenchmarkRunner.Run<Test>();

            Console.ReadLine();
        }

        private static void InsertDestination()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
                      
            TimeSpan delta;

            var context = new TestContext();

            var _dest = new List<Destination>();

            for (var i = 0; i < 1000; i++)
            {
                _dest.Add(new Destination
                {
                    Country = RandomString(6),
                    Description = RandomString(6),
                    Name = RandomString(6)
                });
            }

            var time1 = DateTime.Now;

            context.Destinations.AddRange(_dest);

            context.SaveChanges();

            var time2 = DateTime.Now;

             delta = time2 - time1;

            context.Dispose();

            Console.WriteLine(delta.TotalSeconds + "s");
            Console.WriteLine("Close it");
            Console.ReadLine();
        }
    }

    //[SimpleJob(RunStrategy.ColdStart, launchCount: 2)]
    [SimpleJob(launchCount: 1, warmupCount: 0, targetCount: 1)]
    public class Test
    {
        private TestContext _ctx;
        private List<Destination> _dest;
        private readonly int _count = 1000;
        private static Random random = new Random();
        private int setupCounter;
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Console.WriteLine("// " + "My GlobalSetup");
            _ctx = new TestContext();
            _dest = new List<Destination>();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Console.WriteLine("// " + " My GlobalCleanup");
            _ctx.Dispose();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            Console.WriteLine($"// My IterationSetup ({++setupCounter})");
            Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
            _dest.Clear();

            for (var i = 0; i < _count; i++)
            {
                _dest.Add(new Destination
                {
                    Country = RandomString(6),
                    Description = RandomString(6),
                    Name = RandomString(6)
                });
            }
        }       

        [Benchmark]
        public void InsertDestinations()
        {
                _ctx.Destinations.AddRange(_dest);

                _ctx.SaveChanges();        
        }
    }

}
