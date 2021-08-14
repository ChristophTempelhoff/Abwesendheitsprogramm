using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

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

        //Gets the absent-state from the table in the db
        public Task<List<Abwesendheiten>> GetAbsentsFromDatabase(string Query)
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
                        MySqlCommand command = new MySqlCommand(Query);
                        command.Connection = connection;
                        MySqlDataReader dataReader;
                        connection.Open();
                        dataReader = command.ExecuteReader();

                        //stuff for storing
                        int i = 0;
                        int j = 0;
                        int ID = 0;
                        int userID = 0;
                        bool abwesend = true;
                        string abwesendSeit = "";
                        string abwesendBis = "";
                        List<Abwesendheiten> abwesendheiten = new List<Abwesendheiten>();

                        while (dataReader.Read())
                        {
                            //Gets ID
                            if (j == 0)
                            {
                                ID = dataReader.GetInt32(j);
                                j++;
                            }

                            //Gets userID
                            if (j == 1)
                            {
                                userID = dataReader.GetInt32(j);
                                j++;
                            }

                            //Gets absent
                            if (j == 2)
                            {
                                if (dataReader.GetString(j) == "0")
                                {
                                    abwesend = false;
                                }
                                j++;
                            }

                            //gets 'absent since'
                            if (j == 3)
                            {
                                if (dataReader.GetString(j).Equals(""))
                                {
                                    abwesendSeit = "";
                                }
                                else
                                {
                                    abwesendSeit = dataReader.GetDateTime(j).ToString("dd.MM.yyyy");
                                }
                                j++;
                            }

                            //gets 'absent untill'
                            if (j == 4)
                            {
                                if (dataReader.GetString(j).Equals(""))
                                {
                                    abwesendBis = "";
                                }
                                else
                                {
                                    abwesendBis = dataReader.GetDateTime(j).ToString("dd.MM.yyyy");
                                }
                                j++;
                            }

                            //Add user to a list and than set the variables to their default value and increases the line count
                            if (j == dataReader.FieldCount)
                            {
                                abwesendheiten.Add(new Abwesendheiten { ID = ID, userID = userID, abwesend = abwesend, abwesendVon = abwesendSeit, abwesendBis = abwesendBis });
                                i++;
                                ID = 0;
                                abwesend = true;
                                abwesendSeit = null;
                                abwesendBis = null;
                                j = 0;
                            }
                        }
                        int line = i;
                        int column = dataReader.FieldCount;
                        connection.Close();
                        return abwesendheiten;

                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        //gets the user from the table in the db
        public Task<List<User>> GetUserFromDatabase(string Query)
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
                        MySqlCommand command = new MySqlCommand(Query);
                        command.Connection = connection;
                        MySqlDataReader dataReader;
                        connection.Open();
                        dataReader = command.ExecuteReader();

                        //stuff for storing
                        int i = 0;
                        int j = 0;
                        int ID = 0;
                        string name = "";
                        string passwort = "";
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
                                name = dataReader.GetString(j);
                                j++;
                            }

                            //gets password
                            if (j == 2)
                            {
                                passwort = dataReader.GetString(j);
                                j++;
                            }

                            //Add user to a list and set the variables to their default value and increases the line count
                            if (j == dataReader.FieldCount)
                            {
                                user.Add(new User { ID = ID, Name = name, Passwort = passwort });
                                i++;
                                ID = 0;
                                name = "";
                                passwort = "";
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