using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;


public class DAL
{
    // make the path work for every conputer
    public static string Path()
    {
        string s = Environment.CurrentDirectory;
        string[] ss = s.Split('\\');
        int x = ss.Length - 3;
        Array.Resize(ref ss, x + 1);
        string s1 = String.Join("\\", ss);
        return s1;
    }
    
    // create new connection for the database
    public static OleDbConnection GetConnection()
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path() + @"\DatabaseSet.accdb;Persist Security Info=True";
        return new OleDbConnection(connectionString);
    }

    // create a new cammand for a database connection
    public static OleDbCommand GetCommand(OleDbConnection con, string sqlStr)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.Connection = con;
        cmd.CommandText = sqlStr;
        return cmd;
    }

    // create new data table for a given sql command
    public static DataTable GetDataTable(string sqlStr)
    {
        OleDbConnection con = GetConnection();
        OleDbCommand cmd = GetCommand(con, sqlStr);

        OleDbDataAdapter adp = new OleDbDataAdapter();
        adp.SelectCommand = cmd;
        DataTable dt = new DataTable();
        adp.Fill(dt);

        return dt;
    }
    // create new data view for a given sql command
    public static DataView GetDataView(string sqlStr)
    {
        return GetDataTable(sqlStr).DefaultView;
    }

    // execute an sql command on the database
    public static int ExecuteNonQuery(string sqlStr)
    {
        OleDbConnection con = GetConnection();
        con.Open();

        OleDbCommand cmd = GetCommand(con, sqlStr);

        int rowAfferted = cmd.ExecuteNonQuery();
        con.Close();

        return rowAfferted;
    }

}

