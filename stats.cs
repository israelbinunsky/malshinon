using System.Xml.Linq;
using MySql.Data.MySqlClient;

public static class Stats
{
    static peopleDal dal;
    static Stats()
    {
        dal = new peopleDal();
    }

    public static void getAllThreats()
    {
        int cnt = 0;
        dal.query = "SELECT target_id, created_at FROM alerts";
        try
        {
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int target_id = reader.GetInt32("target_id");
                string name = dal.getPersonName(target_id);
                string created_at = reader.GetString("created_at");
                cnt++;
                Console.WriteLine($"name: {name}. created at: {created_at}");
            }
            dal.conn.Close();
            Console.WriteLine($"num of alerts: {cnt}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }

    public static void getAllAgents()
    {
        int cnt = 0;
        dal.query = "SELECT first_name, last_name FROM people WHERE type = potential_agent";
        try
        {
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string first_name = reader.GetString("first_name");
                string last_name = reader.GetString("last_name");
                cnt++;
                Console.WriteLine($"{first_name} {last_name}.");
            }
            dal.conn.Close();
            Console.WriteLine($"num of alerts: {cnt}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }

}