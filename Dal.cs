using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
    public void reporterIdentification()
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
            else
            {
                Console.WriteLine("already exist.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
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
        this.query = $"INSERT INTO people (first_name, last_name, secret_code, type) VALUES (@first_name, @last_name, @secret_code, @type);";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, this.conn);  
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

    }
}