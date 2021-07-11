using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.IO;

namespace Abwesendheitsprogramm.CSharp_Klassen
{
    class Database
    {
        //This function is not suited for collecting data from the database but can be used for all other operations as long you don't care about the result.
        public void InsertIntoDatabase(string query)
        {
            string verbindungsdaten;
            //This is just to not store the connection data in the actual code due to the code being on GitHub
            StreamReader streamReader = new StreamReader("connectionData.txt");
            verbindungsdaten = streamReader.ReadLine();
            using (MySqlConnection verbindung = new MySqlConnection(verbindungsdaten))
            {
                MySqlCommand commandObject = new MySqlCommand(query);
                commandObject.Connection = verbindung;
                verbindung.Open();
                commandObject.ExecuteNonQuery();
                verbindung.Close();
            }
        }

        //This is, how the name of the function says, to collect data from the database
        public Task<List<User>> GetDataFromDatabase(string query)
        {
            string verbindungsdaten;
            StreamReader streamReader = new StreamReader("connectionData.txt");
            verbindungsdaten = streamReader.ReadLine();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(verbindungsdaten))
                {
                    //This is to use it async-ish
                    return Task.Factory.StartNew(() =>
                    {
                        MySqlCommand command = new MySqlCommand(query);
                        command.Connection = connection;
                        MySqlDataReader dataReader;
                        connection.Open();
                        dataReader = command.ExecuteReader();

                        //stuff for storing
                        int i = 0;
                        int j = 0;
                        int ID = 0;
                        string Name = "";
                        string Passwort = "";
                        bool abwesend = true;
                        string abwesendSeit = null;
                        string abwesendBis = null;
                        List<User> user = new List<User>();

                        while (dataReader.Read())
                        {
                            //Gets ID
                            if (j == 0)
                            {
                                ID = dataReader.GetInt32(j);
                                j++;
                            }

                            //Gets name
                            if (j == 1)
                            {
                                Name = dataReader.GetString(j);
                                j++;
                            }

                            //gets password
                            if (j == 2)
                            {
                                Passwort = dataReader.GetString(j);
                                j++;
                            }

                            //Gets absent
                            if (j == 3)
                            {
                                if (dataReader.GetString(j) == "0")
                                {
                                    abwesend = false;
                                }
                                j++;
                            }

                            //gets 'absent since'
                            if (j == 4)
                            {
                                if (dataReader.GetString(j) != "")
                                {
                                    abwesendSeit = dataReader.GetDateTime(j).ToString("dd.MM.yyyy");
                                }
                                j++;
                            }

                            //gets 'absent untill'
                            if (j == 5)
                            {
                                if (dataReader.GetString(j) != "")
                                {
                                    abwesendBis = dataReader.GetDateTime(j).ToString("dd.MM.yyyy");
                                }
                                j++;
                            }

                            //Add user to a list and set the variables to their default value and increases the line count
                            if (j == dataReader.FieldCount)
                            {
                                user.Add(new User { ID = ID, Name = Name, Passwort = Passwort, Abwesend = abwesend, AbwesendSeit = abwesendSeit, AbwesendBis = abwesendBis });
                                i++;
                                ID = 0;
                                Name = "";
                                Passwort = "";
                                abwesend = true;
                                abwesendSeit = null;
                                abwesendBis = null;
                                j = 0;
                            }
                        }
                        int line = i;
                        int column = dataReader.FieldCount;
                        connection.Close();
                        return user;

                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}