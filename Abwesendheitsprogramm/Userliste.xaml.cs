using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Abwesendheitsprogramm.CSharp_Klassen;
using System.Threading.Tasks;

namespace Abwesendheitsprogramm
{
    /// <summary>
    /// Interaktionslogik für Userliste.xaml
    /// </summary>
    public partial class Userliste : Window
    {
        int amountRefreshed;
        DateTime start = DateTime.Now;
        bool setToAbwesend = false;

        public Userliste()
        {
            InitializeComponent();
            FillDataGridAsync("SELECT * FROM user");
        }

        //This is used to fill the Datagrid
        async void FillDataGridAsync(string SQLQuery)
        {
            DataGrid dg = CustomerGrid;
            Database db = new Database();
            try
            {
                List<User> data = await db.GetDataFromDatabase(SQLQuery);

                //checking if a user isn't absent anymore and storing him in a datagrid
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].checkObWiederAnwesend();
                    dg.Items.Add(new DataItem { id = data[i].ID.ToString(), name = data[i].Name, abwesend = data[i].Abwesend, abwesendSeit = data[i].AbwesendSeit, abwesendBis = data[i].AbwesendBis });
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
            if (id != null)
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
                FillDataGridAsync("SELECT * FROM user");
                return;
            }
            MessageBox.Show("Fehler, ID nicht eingetragen.");
        }

        //sort for absent
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!setToAbwesend)
            {
                RefreshData("SELECT * FROM user WHERE abwesend = 1");
                setToAbwesend = true;
                amountRefreshed++;
            }
        }

        //sort for everyone
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (setToAbwesend)
            {
                RefreshData("SELECT * FROM user");
                setToAbwesend = false;
                amountRefreshed++;
            }
        }

        //function to refresh the datagrid every x miliseconds
        private void RefreshData(string SQLQuery)
        {
            if ((int)DateTime.Now.Subtract(start).TotalSeconds != 0)
            {
                if (amountRefreshed / (int)DateTime.Now.Subtract(start).TotalSeconds < 1)
                {
                    ClearDataGrid();
                    FillDataGridAsync(SQLQuery);
                    return;
                }
                MessageBox.Show("Du versuchst es zu schnell, bitte sei etwas langsamer.");
            }
        }


        //Refreshbutton
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //This is basically just to prevent users from spamming the database. It's set to 3 request a minute but could be changed if necessary. 
            amountRefreshed++;
            if (setToAbwesend)
            {
                RefreshData("SELECT * FROM user WHERE abwesend = 1");
                return;
            }
            RefreshData("SELECT * FROM user");
        }
    }
}