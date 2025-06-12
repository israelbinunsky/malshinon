using System.Runtime.Intrinsics.X86;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

public class Updates
{
    peopleDal dal;
    Alerts alerts;
    public Updates()
    {
        dal = new peopleDal();
        alerts = new Alerts();
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

    public void updateManType(int id, string type)
    {
        dal.query = "UPDATE people SET type = @type WHERE id = @id;";
        dal.conn.Open();
        MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
        cmd.Parameters.AddWithValue("@type", type);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        dal.conn.Close();
    }

    public void reporterToAgent(int id)
    {
        string type = dal.getPersonType(id);
        if (type != "potential_agent")
        {
            int num_reports = dal.getNumReports(id);
            if (num_reports >= 9)
            {
                Console.WriteLine("This is your 10th report.");
                int ReportsAverageLen = calculateAverageLen(id);
                if (ReportsAverageLen > 15)
                {
                    updateManType(id, "potential_agent");
                    string name = dal.getPersonName(id);
                    Console.WriteLine("The total average of your reports is abouve 10 letters.");
                    Console.WriteLine($"Congratulations {name}! your status updated to potential agent.");
                }
            }
        }
    }

    public void targetToThreat(int id)
    {
        string type = dal.getPersonType(id);
        if (type != "potential_threat")
        {
            int num_mentions = dal.getNumMentions(id);
            if (num_mentions >= 10)
            {
                updateManType(id, "potential_threat");
                string name = dal.getPersonName(id);
                Console.WriteLine($"10 reports about the target. {name} is a potential threat!");
                alerts.addAlert(id);
                return;
            }
            bool isIn15 = isIn15Min(id);
            if (isIn15 == true)
            {
                updateManType(id, "potential_threat");
                string name = dal.getPersonName(id);
                Console.WriteLine($"3 reports about the target in 15 minutes. {name} is a potential threat!");
                alerts.addAlert(id);
            }
        }
    }

    public List<int> getReportsLens(int id)
    {
        List<int> lens = new List<int>();
        try
        {
            dal.query = "SELECT text FROM intelreports WHERE reporter_id = @id";
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string txt = reader.GetString("text");
                int len = txt.Count(c => c != ' ');
                lens.Add(len);
            }
            dal.conn.Close();
        }

        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return lens;
    }

    public int calculateAverageLen(int id)
    {
        List<int> lens = getReportsLens(id);
        int result = 0;
        int cnt = lens.Count();
        int sum = 0;
        foreach (int n in lens)
        {
            sum += n;
        }
        result = sum / cnt;
        return result;
    }

    public List<DateTime> getDatetimes(int targetId)
    {
        List<DateTime> times = new List<DateTime>();
        try
        {
            dal.query = "SELECT datetime FROM intelreports WHERE target_id = @targetId";
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            cmd.Parameters.AddWithValue("@target_id", targetId);
            var reader = cmd.ExecuteReader();
            int cnt = 0;
            while (reader.Read())
            { 
                DateTime time = reader.GetDateTime("text");
                times.Add(time);
                cnt++;
                if (cnt >= 3)
                { return times; }
            }
            dal.conn.Close();
        }

        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return times;
    }

    public bool isIn15Min(int targetId)
    {
        List<DateTime> times = getDatetimes(targetId);
        DateTime min = times.Min();
        DateTime max = times.Max();

        double diff = (max - min).TotalMinutes;
        if (diff <= 15)
        {
            return true;
        }
        else { return false; }
    }
}
