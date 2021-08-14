using System;
using System.Collections.Generic;
using System.Windows;

namespace Abwesendheitsprogramm.CSharp_Klassen
{
    // <summary>
    // User-Klasse für die Arbeit mit diesen
    // </summary>

    class Abwesendheiten
    {
        public int ID { set; get; }
        public int userID { set; get; }
        public bool abwesend { set; get; }
        public string abwesendVon { set; get; }
        public string abwesendBis { set; get; }
    }

    class User
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Passwort { set; get; }
    }
    class WholeAbsenty
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Abwesend { get; set; }

        public string AbwesendSeit { get; set; }

        public string AbwesendBis { get; set; }

        //Methoden zur Bearbeitung von Abmeldungen

        public void SetAnwesend()
        {
            if (this.Abwesend == false)
            {
                MessageBox.Show("Der User ist bereits anwesend.");
                return;
            }
            Abwesend = false;
            AbwesendSeit = null;
            AbwesendBis = null;
            Database database = new Database();
            database.InsertIntoDatabase("UPDATE users SET abwesend = false, abwesendSeit = '', abwesendBis = '' WHERE id = " + this.ID + ";");
        }

        public void checkObWiederAnwesend()
        {
            if (this.AbwesendBis == "") return;
            if (DateTime.Compare((DateTime)DateTime.Parse(AbwesendBis), DateTime.Now) < 0)
            {
                this.SetAnwesend();
            }
        }

        //Methode zum zusammenfuegen der Listen 'User' und 'Abwesendheiten'
        public List<WholeAbsenty> addTogether(List<User> users, List<Abwesendheiten> abwesendheit)
        {
            if (abwesendheit.Count != 0)
            {
                List<WholeAbsenty> wholeAbsenties = new List<WholeAbsenty>();
                for (int i = 0; i < users.Count; i++)
                {
                    for (int j = 0; j < abwesendheit.Count; j++)
                    {
                        if (users[i].ID == abwesendheit[i].userID)
                        {
                            wholeAbsenties.Add(new WholeAbsenty { ID = users[i].ID, Name = users[i].Name, Abwesend = abwesendheit[i].abwesend, AbwesendSeit = abwesendheit[i].abwesendVon, AbwesendBis = abwesendheit[i].abwesendBis });
                            break;
                        }
                    }
                }
                return wholeAbsenties;
            }
            else
            {
                List<WholeAbsenty> wholeAbsenties = new List<WholeAbsenty>();
                return wholeAbsenties;
            }
        }
    }
}
