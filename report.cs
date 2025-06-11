using MySql.Data.MySqlClient;

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
        Console.WriteLine("enter your name:");
        string name = Console.ReadLine();
        string[] names = name.Split(' ');
        dal.manIdentification(names, "reporter");
        return names;
    }

    public void addReport()
    {
        string[] reporterNames = this.reporterIdentification();
        Console.WriteLine("enter your report. the target name should be Capitalized:");
        string report = Console.ReadLine();
        int reporterId = dal.getPersonId(reporterNames);
        string[] targetNames = local.getTargetName(report);
        dal.manIdentification(targetNames, "target");
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
            Console.WriteLine("edded report.");
            updates.reporterToAgent(reporterId);
            updates.targetToThreat(targetId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }

    }

}