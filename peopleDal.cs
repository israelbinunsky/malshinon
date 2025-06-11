using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

public class peopleDal
{
    public string strCon = "server=localhost;user=root;passward=;database=malshinon;";
    public MySqlConnection conn;
    public string query;

    public peopleDal()
    {
        this.conn = new MySqlConnection(this.strCon);
    }

    public bool manIdentification(string[] names)
    {  
        string first = names[0];
        string last = names[1];
        bool result = false;
       try 
        { 
        this.conn.Open();
        this.query = "SELECT EXISTS (SELECT 1 FROM people WHERE first_name = @first AND last_name = @last);";
        MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
        cmd.Parameters.AddWithValue("@first", names[0]);
        cmd.Parameters.AddWithValue("@last", names[1]);
        result = Convert.ToBoolean(cmd.ExecuteScalar());
        this.conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return result;
    }


    public void SetNewMan(string[] names, string type)
    {
        string first = names[0];
        string last = names[1];
        string secret_code = local.ganarateCode(4);
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
            Console.WriteLine($"{first} {last} edded to table as a {type}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
    }


    public int getPersonId(string[] names)
    {
        int id = 0;
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

    public string getPersonName(int id)
    {
        this.query = "SELECT first_name, last_name FROM people WHERE id = @id;";
        string name = "";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string first = reader.GetString("first_name");
                string last = reader.GetString("last_name");
                name = $"{first} {last}";
            }
            reader.Close();
            this.conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return name;
    }


    public string getPersonType(int id)
    {
        this.query = "SELECT type FROM people WHERE id = @id;";
        string type = "";
        try
        {
            this.conn.Open();
            MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                type = reader.GetString("type");
            }
            reader.Close();
            this.conn.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e}");
        }
        return type;
    }
}