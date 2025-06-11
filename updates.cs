using MySql.Data.MySqlClient;

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

    public void reporterToAgent(int id)
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

    public void targetToThreat(int id)
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
            alerts.addAlert(id);
        }
    }
}