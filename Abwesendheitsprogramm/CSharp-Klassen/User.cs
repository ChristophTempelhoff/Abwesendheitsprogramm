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

        public bool Abwesend { get; set; }
        public string AbwesendSeit { get; set; }

        public string AbwesendBis { get; set; }

        //Methoden zur Bearbeitung von Abmeldungen

        public void SetAnwesend()
        {
            if(this.Abwesend == false)
            {
                MessageBox.Show("Der User ist bereits anwesend.");
                return;
            }
            Abwesend = false;
            AbwesendSeit = null;
            AbwesendBis = null;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE user SET abwesend = false, abwesendSeit = null, abwesendBis = null WHERE id = "+ this.ID +";");
        }

        public void checkObWiederAnwesend()
        {
            if (this.AbwesendBis == null) return;
            if (DateTime.Compare((DateTime)DateTime.Parse(AbwesendBis), DateTime.Now) < 0)
            {
                this.SetAnwesend();
            }
        }
    }
}
