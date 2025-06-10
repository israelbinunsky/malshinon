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
        Console.WriteLine("enter report your. the target name should be Capitalized:");
        string report = Console.ReadLine();
        int reporterId = dal.getPersonId(reporterNames);
        string[] targetNames = dal.getTargetName(report);
        dal.manIdentification(targetNames, "target");
        int targetId = dal.getPersonId(targetNames);
        addCount("reporter", reporterId);
        addCount("target", targetId);
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
            Console.WriteLine("edded report.");
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
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        dal.conn.Close();
    }

    private void setAgentType(int id)
    {
        dal.query = "SELECT num_reports FROM people WHERE first_name = @first_name AND last_name = @last_name;";
    }
}