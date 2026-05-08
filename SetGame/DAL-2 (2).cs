using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;


public class DAL
{
    public static string Path()
    {
        string s = Environment.CurrentDirectory;
        string[] ss = s.Split('\\');
        int x = ss.Length - 3;
        Array.Resize(ref ss, x + 1);
        string s1 = String.Join("\\", ss);
        return s1;
    }
    

    public static OleDbConnection GetConnection()
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path() + @"\DatabaseSet.accdb;Persist Security Info=True";
        return new OleDbConnection(connectionString);
    }

    public static OleDbCommand GetCommand(OleDbConnection con, string sqlStr)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.Connection = con;
        cmd.CommandText = sqlStr;
        return cmd;
    }

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
    public static DataView GetDataView(string sqlStr)
    {
        return GetDataTable(sqlStr).DefaultView;
    }

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

