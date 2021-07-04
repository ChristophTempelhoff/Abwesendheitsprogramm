using System;
using System.Windows;
using Abwesendheitsprogramm.CSharp_Klassen;

namespace Abwesendheitsprogramm.CSharp_Klassen
{
    // <summary>
    // User-Klasse für die Arbeit mit diesen
    // </summary>
    class User
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Passwort { get; set; }

        public string Abwesend { get; set; }
        public DateTime? AbwesendSeit { get; set; }

        public DateTime? AbwesendBis { get; set; }

        //Methoden zur Bearbeitung von Abmeldungen
        public void SetUserAbwesend(DateTime abwesendSeit, DateTime abwesendBis)
        {
            Abwesend = "Ja";
            AbwesendSeit = abwesendSeit;
            AbwesendBis = abwesendBis;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE user SET istAbwesend = " + Abwesend + ", abwesendSeit = " + AbwesendSeit + ", abwesendBis = " + AbwesendBis + " WHERE id = " + ID + ";");
        }

        public void SetAnwesend(int id, string abwesend)
        {
            if(abwesend == "Nein")
            {
                MessageBox.Show("Der User ist bereits anwesend.");
                return;
            }
            Abwesend = "Nein";
            AbwesendSeit = null;
            AbwesendBis = null;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE user SET istAbwesend = false, abwesendSeit = null, abwesendBis = null WHERE id = "+ id +";");
        }

        public void checkObWiederAnwesend(int id, string abwesend)
        {
            if (DateTime.Compare((DateTime)AbwesendBis, DateTime.Now) < 0)
            {
                SetAnwesend(id, abwesend);
            }
        }
    }
}
