using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto;
using static System.Collections.Specialized.BitVector32;

public class Dal
{
    public string strCon = "server=localhost;user=root;passward=;database=malshinon;";
    public MySqlConnection conn;
    public string query;

    public Dal()
    {
        this.conn = new MySqlConnection(this.strCon);
    }
    public string[] reporterIdentification()
    {
        Console.WriteLine("enter your name:");
        string name = Console.ReadLine();
        string[] names = name.Split(' ');
        string first = names[0];
        string last = names[1];
       try 
        { 
        this.conn.Open();
        this.query = "SELECT EXISTS (SELECT 1 FROM people WHERE first_name = @first AND last_name = @last);";
        MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
        cmd.Parameters.AddWithValue("@first", first);
        cmd.Parameters.AddWithValue("@last", last);
        bool reader = Convert.ToBoolean(cmd.ExecuteScalar());
        this.conn.Close();
            if (reader == false)
            {
                this.SetNewReporter(first, last);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return names;
    }

    private string ganarateCode(int len)
    {
        string code = "";
        Random random = new Random();
        for (int i = 0; i < len; i++)
        {
            code += random.Next(9);
        }
        return code;
    }

    private void SetNewReporter(string first, string last)
    {
        string secret_code = ganarateCode(4);
        string type = "reporter";
        this.query = "INSERT INTO people (first_name, last_name, secret_code, type) VALUES (@first_name, @last_name, @secret_code, @type);";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(this.query, this.conn);  
            cmd.Parameters.AddWithValue("@first_name", first);
            cmd.Parameters.AddWithValue("@last_name", last);
            cmd.Parameters.AddWithValue("@secret_code", secret_code);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.ExecuteNonQuery();
            this.conn.Close();
            Console.WriteLine("edded reporter.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }

    public void addReport()
    {
        string[] names = reporterIdentification();
        Console.WriteLine("enter report:");
        string report = Console.ReadLine();
        int id = getPersonId(names);
        this.query = "INSERT INTO intelreports (reporter_id, text) VALUES (@reporter_id, @text);";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
            cmd.Parameters.AddWithValue("@reporter_id", id);
            cmd.Parameters.AddWithValue("@text", report);
            cmd.ExecuteNonQuery();
            this.conn.Close();
            Console.WriteLine("edded report.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }

    }

    private int getPersonId(string[] names)
    {
        int id = 1000;
        this.query = "SELECT id FROM people WHERE first_name = @first_name AND last_name = @last_name;";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
            cmd.Parameters.AddWithValue("@first_name", names[0]);
            cmd.Parameters.AddWithValue("@last_name", names[1]);
            id = Convert.ToInt32(cmd.ExecuteScalar());
            this.conn.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return id;
    }
}