using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Abwesendheitsprogramm.CSharp_Klassen;

namespace Abwesendheitsprogramm
{
    /// <summary>
    /// Interaktionslogik für Userliste.xaml
    /// </summary>
    public partial class Userliste : Window
    {
        public Userliste()
        {
            InitializeComponent();
            FillDataGridAsync();
        }

        //This is used to fill the Datagrid
        async void FillDataGridAsync()
        {
            DataGrid dg = CustomerGrid;
            Database db = new Database();
            try
            {
                List<User> data = await db.GetDataFromDatabase("SELECT * FROM user");

                //checking if a user isn't absent anymore and storing him in a datagrid
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].checkObWiederAnwesend();
                    dg.Items.Add(new DataItem { id = data[i].ID.ToString(), name = data[i].Name, abwesend = data[i].Abwesend, abwesendSeit = data[i].AbwesendSeit, abwesendBis = data[i].AbwesendBis});
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //This is used to clear the datagrid
        void ClearDataGrid()
        {
            DataGrid dg = CustomerGrid;
            dg.Items.Clear();
        }

        //Class to specify the actual structure of the datagrid
        class DataItem
        {
            public string id { set; get; }
            public string name { set; get; }
            public bool abwesend { set; get; }
            public string abwesendSeit { set; get; }
            public string abwesendBis { set; get; }
        }

        //Button to store changes
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Database database = new Database();
            string id = ID.Text.ToString();
            bool abwesend = (bool)IstAbwesend.IsChecked;
            string abwesendSeit = "";
            string abwesendBis = "";

            //checking if there are values
            if (AbwesendSeit.SelectedDate.HasValue)
            {
                abwesendSeit = AbwesendSeit.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (AbwesendBis.SelectedDate.HasValue)
            {
                abwesendBis = AbwesendBis.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if(id != null)
            {
                //Handling if the user has set 'absent' to true but didn't set dates or set dates and didn't set 'absent' to true
                if ((abwesend && (abwesendSeit == "" || abwesendBis == "")) || (!abwesend && (abwesendSeit != "" || abwesendBis != "")))
                {
                    MessageBox.Show("Fehler! Entweder User nicht auf abwesend gesetzt und Zeiten angegeben oder umgekehrt");
                    return;
                }
                
                //Storing the data in the database
                database.InsertIntoDatabase("UPDATE user SET abwesend = " + abwesend.ToString() + ", abwesendSeit = '" + abwesendSeit + "', abwesendBis = '" + abwesendBis + "' WHERE id = " + id + ";");
                
                //giving the user feedback
                MessageBox.Show("Erfolgreich durchgeführt");
                
                //clear and repopulate the datagrid
                ClearDataGrid();
                FillDataGridAsync();
                return;
            }
            MessageBox.Show("Fehler, ID nicht eingetragen.");
        }
    }
}