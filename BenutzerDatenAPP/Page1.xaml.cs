using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using System.IO;
using System.Data;


namespace BenutzerDatenAPP
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        // Liste zur Speicherung aller Telefonnummern
        private List<string> telefonnummernListe = new List<string>();

        private void AddTelefonnummer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TelefonnummerTextBox.Text))
            {
                // Telefonnummer zur Liste hinzufügen
                telefonnummernListe.Add(TelefonnummerTextBox.Text);

                // Erstelle ein StackPanel für die neue Telefonnummer
                var telefonPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 0) };

                // Füge das Label "Telefonnummer:" hinzu
                var label = new Label { Content = "Telefonnummer:", VerticalAlignment = VerticalAlignment.Center, Width = 100 };
                telefonPanel.Children.Add(label);

                // Füge TextBox mit der Telefonnummer hinzu
                var telefonnummerTextBox = new TextBox { Width = 200, Text = TelefonnummerTextBox.Text, IsReadOnly = true };

                // Erstelle einen Button zum Löschen der Telefonnummer
                var deleteButton = new Button { Content = "-", Width = 30, Height = 30, Margin = new Thickness(5, 0, 0, 0) };
                deleteButton.Click += (s, ev) => DeleteTelefonnummer_Click(s, ev, telefonnummerTextBox.Text);

                telefonPanel.Children.Add(telefonnummerTextBox);
                telefonPanel.Children.Add(deleteButton);

                TelefonnummernContainer.Children.Add(telefonPanel);

                TelefonnummerTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine Telefonnummer ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTelefonnummer_Click(object sender, RoutedEventArgs e, string telefonnummer)
        {
            // Entferne Telefonnummer aus der Liste
            telefonnummernListe.Remove(telefonnummer);

            // Lösche das StackPanel
            if (sender is Button button && button.Parent is StackPanel stackPanel)
            {
                TelefonnummernContainer.Children.Remove(stackPanel);
            }
        }



        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            bool isAnyEmpty = false;
            string missingFields = "";

            // Überprüfe alle Eingabefelder, ob sie leer sind
            foreach (var control in FindVisualChildren<TextBox>(this))
            {
                if (string.IsNullOrEmpty(control.Text))
                {
                    isAnyEmpty = true;
                    missingFields += $"{control.Name.Replace("TextBox", "")}, ";
                }
            }

            if (isAnyEmpty)
            {
                MessageBox.Show($"Bitte geben Sie die fehlenden Felder ein: {missingFields.TrimEnd(',', ' ')}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string benutzerBildPfad = null;

            // Überprüfe, ob ein Bild ausgewählt wurde
            if (!string.IsNullOrEmpty(BenutzerbildTextBox.Text) && BenutzerbildTextBox.Text != "Kein Bild ausgewählt.")
            {
                try
                {
                    string imagePath = BenutzerbildTextBox.Text.Replace("Ausgewähltes Bild: ", "").Trim();

                    if (File.Exists(imagePath))
                    {
                        benutzerBildPfad = imagePath;
                    }
                    else
                    {
                        MessageBox.Show("Das ausgewählte Bild existiert nicht oder der Pfad ist ungültig.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Verarbeiten des Bildes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Daten in die Datenbank einfügen
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-89SP8JB6\\SQLEXPRESS;Initial Catalog=BenutzerDaten;Integrated Security=True;TrustServerCertificate=True"))
                {
                    con.Open();
                    string insertQuery = "INSERT INTO Benutzer (Name, Nachname, Straße, PLZ, Ort, Telefonnummer, Benutzerbild) " +
                                         "VALUES (@Name, @Nachname, @Straße, @PLZ, @Ort, @Telefonnummer, @Benutzerbild)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", NameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Nachname", NachnameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Straße", StraßeTextBox.Text);
                        cmd.Parameters.AddWithValue("@PLZ", PLZTextBox.Text);
                        cmd.Parameters.AddWithValue("@Ort", OrtTextBox.Text);
                        cmd.Parameters.AddWithValue("@Telefonnummer", TelefonnummerTextBox.Text);
                        cmd.Parameters.AddWithValue("@Benutzerbild", (object)benutzerBildPfad ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Benutzer hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Benutzers: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private void CaptureImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // OpenFileDialog zum Auswählen eines Bildes
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Bilddateien (*.jpg;*.png)|*.jpg;*.png"; // Filter für Bilddateien

                // Wenn der Benutzer eine Datei auswählt
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;  // Vollständiger Pfad des ausgewählten Bildes

                    // Den vollständigen Pfad im Textfeld speichern (anstatt nur den Dateinamen)
                    BenutzerbildTextBox.Text = "Ausgewähltes Bild: " + filePath;
                }
                else
                {
                    MessageBox.Show("Kein Bild ausgewählt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aufnehmen des Bildes: {ex.Message}");
            }
        }



    }
}
