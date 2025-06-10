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
        manIdentification(names, "reporter");
        return names;
    }

    public string[] manIdentification(string[] names, string type)
    {
        
        string first = names[0];
        string last = names[1];
       try 
        { 
        this.conn.Open();
        this.query = "SELECT EXISTS (SELECT 1 FROM people WHERE first_name = @first AND last_name = @last);";
        MySqlCommand cmd = new MySqlCommand(this.query, this.conn);
        cmd.Parameters.AddWithValue("@first", names[0]);
        cmd.Parameters.AddWithValue("@last", names[1]);
        bool reader = Convert.ToBoolean(cmd.ExecuteScalar());
        this.conn.Close();
            if (reader == false)
            {
                this.SetNewMan(first, last, type);
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

    private void SetNewMan(string first, string last, string type)
    {
        string secret_code = ganarateCode(4);
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
            Console.WriteLine("edded man.");
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

    public string[] getTargetName(string txt)
    {
        string[] names = new string[2];
        string[] reportWards = txt.Split(' ');
        for (int i = 0; i < reportWards.Length -1; i++)
        {
            if (char.IsUpper(reportWards[i][0]) && char.IsUpper(reportWards[i + 1][0]))
            {
                names[0] = reportWards[i];
                names[1] = reportWards[i + 1];
            }
        }
        return names;
    }

}