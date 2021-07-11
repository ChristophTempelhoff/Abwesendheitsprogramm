using System.Collections.Generic;
using System.Text;
using System.Windows;
using Abwesendheitsprogramm.CSharp_Klassen;

namespace Abwesendheitsprogramm
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Log In Button
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            List<User> data = await db.GetDataFromDatabase("SELECT * FROM user WHERE name = '" + Benutzername.Text + "'");
            for (int i = 0; i < data.Count; i++)
            {
                if (hashPasswords.CreateMD5(Password.Password.ToString()) == data[i].Passwort)
                {
                    Userliste userliste = new Userliste();
                    userliste.Show();
                    this.Close();

                    return;
                }
            }
            MessageBox.Show("Benutzername oder Passwort falsch.");
        }
    }

    public class hashPasswords
    {
        /// <summary>
        /// Gibt einen MD5 Hash als String zurück
        /// </summary>
        /// <returns>Hash als string.</returns>
        /// found on https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
