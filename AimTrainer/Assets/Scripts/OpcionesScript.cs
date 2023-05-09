using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data;
using System.Data.SqlClient;
using PlayFab;
using PlayFab.ClientModels;
using System.Diagnostics;

public class OpcionesScript : MonoBehaviour
{

    public void Start()
    {
        /*try
            {
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.UseShellExecute = false;
                // You can start any process, HelloWorld is a do-nothing example.
                myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
                //myProcess.StartInfo.FileName = "C:\\Program Files\\Common Files\\Oracle\\Java\\javapath\\javac.exe";
                //myProcess.StartInfo.Arguments = "C:\\Users\\ivaan\\eclipse-workspace\\Miscelanious\\src\\com\\jg\\misc\\linkformatter\\LinkFormatter.java";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                // This code assumes the process you are starting will terminate itself.
                // Given that it is started without a window so you cannot terminate it
                // on the desktop, it must terminate itself or you can do it programmatically
                // from this application using the Kill method.
            }
        } catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }*/
        //Process.Start("C:\\Program Files\\Common Files\\Oracle\\Java\\javapath\\java.exe", "-jar \"C:\\Users\\ivaan\\test.jar\"");

        /*var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "C:\\Program Files\\Common Files\\Oracle\\Java\\javapath\\javac.exe",
                Arguments = "C:\\Users\\ivaan\\eclipse-workspace\\Miscelanious\\src\\com\\jg\\misc\\linkformatter\\LinkFormatter.java",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };*/

        /*if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "385B1";
        }
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);*/
    }

    private void OnLoginSuccess(LoginResult result)
    {
        UnityEngine.Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        UnityEngine.Debug.LogWarning("Something went wrong with your first API call.  :(");
        UnityEngine.Debug.LogError("Here's some UnityEngine.Debug information:");
        UnityEngine.Debug.LogError(error.GenerateErrorReport());
    }

    public void Save()
    {
        /*string connString = "server=localhost;uid=root;pwd=root;database=AimTrainerBD";
        SqlConnection con = new SqlConnection();
        con.ConnectionString = connString;
        con.Open();
        string sql = "select * from usuario";
        SqlCommand command = new SqlCommand(sql, con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            UnityEngine.Debug.Log(String.Format("Data: {0}, {1}, {2}, {3}, {4}, {5}", reader.GetInt32(0),
                                reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4),
                                reader.GetInt32(5)));
        }*/
        /*string connString = "server=localhost\\Juggernogking;user id=root;" +
            "password=root;initial catalog=AimTrainerBD";
        //"server=localhost;uid=root;pwd=root;database=AimTrainerBD";
        // Build connection string
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.ConnectionString = connString;
        builder.DataSource = "localhost";
        builder.UserID = "root";//"root";
        builder.Password = "root";//"123456789";
        builder.InitialCatalog = "AimTrainerBD";
        try
        {
            // connect to the databases
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                // if open then the connection is established
                connection.Open();
                UnityEngine.Debug.Log("connection established");
                // sql command
                string sql = "select * from usuario";
                // execute sql command
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // read
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // each line in the output
                        while (reader.Read())
                        {
                            // to avoid SqlNullValueException
                            if (!reader.IsDBNull(0))
                            {
                                UnityEngine.Debug.Log(String.Format("Data: {0}, {1}, {2}, {3}, {4}, {5}", reader.GetInt32(0), 
                                    reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), 
                                    reader.GetInt32(5)));
                            }
                        }
                        reader.Close();
                    }
                }
            }
        }
        catch (SqlException e)
        {
            UnityEngine.Debug.Log("Connection Failed");
            UnityEngine.Debug.Log(e.ToString());
        }*/
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
