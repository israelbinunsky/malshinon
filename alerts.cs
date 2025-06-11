using MySql.Data.MySqlClient;
using Mysqlx.Crud;

public class Alerts
{
    peopleDal dal;
    public Alerts()
    {
        dal = new peopleDal();
    }

    internal void addAlert(int targetId)
    {
        dal.query = "INSERT INTO alerts (target_id) VALUES (@targetId);";
        try
        {
            dal.conn.Open();
            MySqlCommand cmd = new MySqlCommand(dal.query, dal.conn);
            cmd.Parameters.AddWithValue("@target_id", targetId);
            cmd.ExecuteNonQuery();
            dal.conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }
}