using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

public class report
{
    peopleDal dal;
    Updates updates;
    public report()
    {
        dal = new peopleDal();
        updates = new Updates();

    }

    public string[] reporterIdentification()
    {
        Console.WriteLine("please enter your full name. separate with space:");
        string name = Console.ReadLine();
        string[] names = name.Split(' ');
        bool isReporterExist = dal.manIdentification(names);
        if (isReporterExist == false)
        {
            dal.SetNewMan(names, "reporter");
        }
        return names;
    }

    public void addReport(string[] reporterNames)
    {
        Console.WriteLine("enter your report. the target name should be Capitalized:");
        string report = Console.ReadLine();
        int reporterId = dal.getPersonId(reporterNames);
        string[] targetNames = local.getTargetName(report);
        bool isTargetExist = dal.manIdentification(targetNames);
        if (isTargetExist == false)
        {
            dal.SetNewMan(targetNames, "target");
        }
        int targetId = dal.getPersonId(targetNames);
        
        dal.query = "INSERT INTO intelreports (reporter_id, text, target_id) VALUES (@reporter_id, @text, @target_id);";
        try
        {
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            cmd.Parameters.AddWithValue("@reporter_id", reporterId);
            cmd.Parameters.AddWithValue("@text", report);
            cmd.Parameters.AddWithValue("@target_id", targetId);
            cmd.ExecuteNonQuery();
            dal.conn.Close();
            updates.addCount("reporter", reporterId);
            updates.addCount("target", targetId);
            Console.WriteLine($"Thenk you {reporterNames[0]}, your report has been edded.");
            updates.reporterToAgent(reporterId);
            updates.targetToThreat(targetId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }
}