using MySql.Data.MySqlClient;

public class report
{
    Dal dal;
    public report()
    {
        dal = new Dal();
    }

    public void addReport()
    {
        string[] reporterNames = dal.reporterIdentification();
        Console.WriteLine("enter your report. the target name should be Capitalized:");
        string report = Console.ReadLine();
        int reporterId = dal.getPersonId(reporterNames);
        string[] targetNames = dal.getTargetName(report);
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
            addCount("reporter", reporterId);
            addCount("target", targetId);
            Console.WriteLine("edded report.");
            reporterToAgent(reporterId);
            targetToThreat(targetId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }

    }

    public void addCount(string type, int id)
    {
        switch (type)
        {
            case "reporter":
                dal.query = "UPDATE people SET num_reports = num_reports + 1 WHERE id = @id;";
                break;
            case "target":
                dal.query = "UPDATE people SET num_mentions = num_mentions + 1 WHERE id = @id;";
                break;
            default:
                Console.WriteLine("invalid type");
                break;
        }
        dal.conn.Open();
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        dal.conn.Close();
    }

    private void updateManType(int id, string type)
    {
        dal.query = "UPDATE people SET type = @type WHERE id = @id;";
        dal.conn.Open();
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@type", type);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        dal.conn.Close();
    }

    private void reporterToAgent(int id)
    {
        dal.query = "SELECT num_reports FROM people WHERE id = @id";
        dal.conn.Open();
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@id", id);
        int num_reports = Convert.ToInt32(cmd.ExecuteScalar());
        dal.conn.Close();
        if (num_reports >= 10)
        {
            updateManType(id, "potential_agent");
            string name = dal.getPersonName(id);
            Console.WriteLine($"{name} changed to potential agent.");
        }
    }

    private void targetToThreat(int id)
    {
        dal.query = "SELECT num_mentions FROM people WHERE id = @id";
        dal.conn.Open();
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@id", id);
        int num_mentions = Convert.ToInt32(cmd.ExecuteScalar());
        dal.conn.Close();
        if (num_mentions >= 10)
        {
            updateManType(id, "potential_threat");
            string name = dal.getPersonName(id);
            Console.WriteLine($"{name} is a potential threat!");
        }
    }

}