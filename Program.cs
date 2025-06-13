using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menue();
        }

        static void menue()
        {
            report report = new report();
            Console.WriteLine("Wellcome to Malshinon!");
            string[] reporterNames = report.reporterIdentification();
            if (reporterNames.Length != 2)
            { Console.WriteLine("invalid name.");
                return;
            }
            Console.WriteLine("To add a new report enter 1");
            Console.WriteLine("To get all alerts enter 2");
            Console.WriteLine("To get all potential agents enter 3");
            Console.WriteLine("To exit enter 4:");
            string c = Console.ReadLine();
            int choise = int.Parse(c);
            switch (choise)
            {
                case 1:
                    report.addReport(reporterNames);
                    break;
                case 2:
                    Stats.getAllAlerts();
                    menue();
                    break;
                case 3:
                    Stats.getAllAgents();
                    menue();
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("invalid choise.");
                    menue();
                    break;
            }
        }
    }
}