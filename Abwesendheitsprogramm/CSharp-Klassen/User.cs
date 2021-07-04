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
        public string AbwesendSeit { get; set; }

        public string AbwesendBis { get; set; }

        //Methoden zur Bearbeitung von Abmeldungen
        public void SetUserAbwesend(string abwesendSeit, string abwesendBis)
        {
            Abwesend = "Ja";
            AbwesendSeit = abwesendSeit;
            AbwesendBis = abwesendBis;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE user SET istAbwesend = " + this.Abwesend + ", abwesendSeit = " + this.AbwesendSeit + ", abwesendBis = " + this.AbwesendBis + " WHERE id = " + this.ID + ";");
        }

        public void SetAnwesend()
        {
            if(this.Abwesend == "Nein")
            {
                MessageBox.Show("Der User ist bereits anwesend.");
                return;
            }
            Abwesend = "Nein";
            AbwesendSeit = null;
            AbwesendBis = null;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE user SET istAbwesend = false, abwesendSeit = null, abwesendBis = null WHERE id = "+ this.ID +";");
        }

        public void checkObWiederAnwesend()
        {
            if (DateTime.Compare((DateTime)DateTime.Parse(AbwesendBis), DateTime.Now) < 0)
            {
                this.SetAnwesend();
            }
        }
    }
}
