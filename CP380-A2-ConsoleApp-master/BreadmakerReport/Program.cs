using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading;
using System.ComponentModel;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService _service = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");            
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);

            var BMList = BreadmakerDb.Breadmakers                
                .Select(x => new { Reviews = x.Reviews.Count
                                    , Average = Math.Round(x.Reviews.Average(r => r.stars), 2)
                                    , Adjust = Math.Round(_service.Adjust(x.Reviews.Average(r => r.stars) , x.Reviews.Count), 2)
                                    , Description = x.title
                })          
                .AsEnumerable()
                .OrderByDescending(x => x.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output                
                Console.WriteLine($"[{j+1}] {i.Reviews,8} {i.Average, -2:F2} {i.Adjust, 8:F2}      {i.Description}");
            }
        }
    }
}
