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

        async void FillDataGridAsync()
        {
            DataGrid dg = CustomerGrid;
            Database db = new Database();
            try
            {
                List<User> data = await db.GetDataFromDatabase("SELECT * FROM user");
                User users = new User();
                for (int i = 0; i < data.Count; i++)
                {
                    dg.Items.Add(new DataItem { id = data[i].ID.ToString(), name = data[i].Name, istAbwesend = data[i].Abwesend, abwesendSeit = data[i].AbwesendSeit, abwesendBis = data[i].AbwesendBis});
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        class DataItem
        {
            public string id { set; get; }
            public string name { set; get; }
            public string istAbwesend { set; get; }
            public string abwesendSeit { set; get; }
            public string abwesendBis { set; get; }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
