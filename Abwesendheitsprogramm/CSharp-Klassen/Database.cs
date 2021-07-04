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
        public void InsertIntoDatabase(string query)
        {
            string verbindungsdaten;
            StreamReader streamReader = new StreamReader("connectionData.txt");
            verbindungsdaten = streamReader.ReadLine();
            //This function is not suited for collecting data from the database but can be used for all other operations as long you don't care about the result.
            using (MySqlConnection verbindung = new MySqlConnection(verbindungsdaten))
            {
                MySqlCommand commandObject = new MySqlCommand(query);
                commandObject.Connection = verbindung;
                verbindung.Open();
                commandObject.ExecuteNonQuery();
                verbindung.Close();
            }
        }
        public Task<List<User>> GetDataFromDatabase(string query)
        {
            string verbindungsdaten;
            StreamReader streamReader = new StreamReader("connectionData.txt");
            verbindungsdaten = streamReader.ReadLine();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(verbindungsdaten))
                {
                    return Task.Factory.StartNew( () =>
                    {
                        MySqlCommand command = new MySqlCommand(query);
                        command.Connection = connection;
                        MySqlDataReader dataReader;
                        connection.Open();
                        dataReader = command.ExecuteReader();
                        int i = 0;
                        List<User> user = new List<User>();
                        while (dataReader.Read())
                        {
                            int ID = 0;
                            string Name = "";
                            string Passwort = "";
                            string abwesend = "";
                            DateTime? abwesendSeit = null;
                            DateTime? abwesendBis = null;
                            //I know that this is technically quite slow (O(n²)) but I don't know how else I should do it.
                            for (int j = 0; j < dataReader.FieldCount; j++)
                            {
                                if (j == 0)
                                {
                                    ID = dataReader.GetInt32(j);
                                }
                                if (j == 1)
                                {
                                    Name = dataReader.GetString(j);
                                }
                                if (j == 2)
                                {
                                    Passwort = dataReader.GetString(j);
                                }
                                if (j == 3)
                                {
                                    if (dataReader.GetString(j) == "0")
                                    {
                                        abwesend = "Nein";
                                    }
                                    else
                                    {
                                        abwesend = "Ja";
                                    }
                                }
                                if (j == 4)
                                {
                                    if (!dataReader.IsDBNull(j))
                                    {
                                        abwesendSeit = (DateTime)DateTime.Parse(dataReader.GetDateTime(j).ToString("dd.MM.yyyy"));
                                    }
                                    else
                                    {
                                        abwesendSeit = null;
                                    }
                                }
                                if (j == 5)
                                {
                                    if (!dataReader.IsDBNull(j))
                                    {
                                        abwesendBis = (DateTime)DateTime.Parse(dataReader.GetDateTime(j).ToString("dd.MM.yyyy"));
                                    }
                                    else
                                    {
                                        abwesendBis = null;
                                    }
                                }
                            }
                            user.Add(new User { ID = ID, Name = Name, Passwort = Passwort, Abwesend = abwesend, AbwesendSeit = abwesendSeit, AbwesendBis = abwesendBis });
                            i++;
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
    class Databaseconnection
    {
        public string Server { set; get; }
        public string Datebase { set; get; }
        public string UID { set; get; }
        public string Password { set; get; }
    }
}
