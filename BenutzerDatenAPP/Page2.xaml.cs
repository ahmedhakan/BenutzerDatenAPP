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
using System.Data;

namespace BenutzerDatenAPP
{
    public partial class Page2 : Page
    {
        // Verbindung zur Datenbank (aus Page1 übernommen)
        private readonly string connectionString = "Data Source=LAPTOP-89SP8JB6\\SQLEXPRESS;Initial Catalog=BenutzerDaten;Integrated Security=True;TrustServerCertificate=True";

        public Page2()
        {
            InitializeComponent();
            LadeDaten(); // Daten laden beim Start
        }

        // Methode zum Laden der Daten
        private void LadeDaten(string filter = "")
{
    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT [Name], [Nachname], [Straße], [PLZ], [Ort], [Telefonnummer],[Benutzerbild] FROM [dbo].[Benutzer]";

            // Filter hinzufügen, wenn ein Suchbegriff angegeben wurde
            if (!string.IsNullOrEmpty(filter))
            {
                query += " WHERE [Name] LIKE @Filter OR [Nachname] LIKE @Filter OR [Straße] LIKE @Filter OR [PLZ] LIKE @Filter OR [Ort] LIKE @Filter OR [Telefonnummer] LIKE @Filter  OR [Benutzerbild] LIKE @Filter";
            }

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    command.Parameters.AddWithValue("@Filter", $"%{filter}%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Daten an DataGrid binden
                BenutzerDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}


        // Event-Handler für den "Aktualisieren"-Button
        private void Aktualisieren_Click(object sender, RoutedEventArgs e)
        {
            if (BenutzerDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Datensatz zum Bearbeiten aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Textfelder sichtbar machen
                EditStackPanel.Visibility = Visibility.Visible;

                DataRowView selectedRow = BenutzerDataGrid.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    // Werte aus der ausgewählten Zeile holen
                    string name = selectedRow["Name"].ToString();
                    string nachname = selectedRow["Nachname"].ToString();
                    string strasse = selectedRow["Straße"].ToString();
                    string plz = selectedRow["PLZ"].ToString();
                    string ort = selectedRow["Ort"].ToString();
                    string telefonnummer = selectedRow["Telefonnummer"].ToString();
                    string bild = selectedRow["Benutzerbild"].ToString();

                    // Daten für die Aktualisierung an Textfelder übergeben
                    NameTextBox.Text = name;
                    NachnameTextBox.Text = nachname;
                    StrasseTextBox.Text = strasse;
                    PLZTextBox.Text = plz;
                    OrtTextBox.Text = ort;
                    TelefonnummerTextBox.Text = telefonnummer;
                    BenutzerbildTextBox.Text = bild;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Datensatzes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event-Handler für den "Daten Speichern"-Button
        private void SaveUpdatedData_Click(object sender, RoutedEventArgs e)
        {
            if (BenutzerDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Datensatz zum Speichern aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Die Werte aus den Textfeldern holen
                string name = NameTextBox.Text;
                string nachname = NachnameTextBox.Text;
                string strasse = StrasseTextBox.Text;
                string plz = PLZTextBox.Text;
                string ort = OrtTextBox.Text;
                string telefonnummer = TelefonnummerTextBox.Text;
                string bild= BenutzerbildTextBox.Text;

                DataRowView selectedRow = BenutzerDataGrid.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    // Benutzer-ID für die Aktualisierung (hier nur Name und Nachname als Beispiel)
                    string originalName = selectedRow["Name"].ToString();
                    string originalNachname = selectedRow["Nachname"].ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE [dbo].[Benutzer] SET [Name] = @Name, [Nachname] = @Nachname, [Straße] = @Strasse, [PLZ] = @PLZ, [Ort] = @Ort, [Telefonnummer] = @Telefonnummer, [Benutzerbild] = @Benutzerbild WHERE [Name] = @OriginalName AND [Nachname] = @OriginalNachname";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Nachname", nachname);
                            command.Parameters.AddWithValue("@Strasse", strasse);
                            command.Parameters.AddWithValue("@PLZ", plz);
                            command.Parameters.AddWithValue("@Ort", ort);
                            command.Parameters.AddWithValue("@Telefonnummer", telefonnummer);
                            command.Parameters.AddWithValue("@Benutzerbild", bild);

                            if (bild != null)
                            {
                                command.Parameters.AddWithValue("@Benutzerbild", bild); // `bild` sollte ein Byte-Array sein
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@Benutzerbild", DBNull.Value); // Wenn kein Bild hochgeladen wurde
                            }

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Datensatz erfolgreich aktualisiert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                                LadeDaten(); // Aktualisiere die Daten
                                EditStackPanel.Visibility = Visibility.Collapsed; // Textfelder wieder ausblenden
                            }
                            else
                            {
                                MessageBox.Show("Fehler beim Aktualisieren des Datensatzes.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern der Daten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event-Handler für den "Löschen"-Button
        private void Loeschen_Click(object sender, RoutedEventArgs e)
        {
            if (BenutzerDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Datensatz zum Löschen aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedRow = BenutzerDataGrid.SelectedItem as DataRowView;
                string name = selectedRow["Name"].ToString();
                string nachname = selectedRow["Nachname"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM [dbo].[Benutzer] WHERE [Name] = @Name AND [Nachname] = @Nachname";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Nachname", nachname);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datensatz erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                            LadeDaten(); // Daten nach dem Löschen neu laden
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Löschen des Datensatzes.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen des Datensatzes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();
            LadeDaten(searchText); // Daten mit Suchbegriff laden
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Name, Nachname...")
            {
                SearchTextBox.Text = string.Empty;
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Black); // Textfarbe ändern
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Name, Nachname...";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray); // Textfarbe zurücksetzen
            }

            // Optional: Weitere Aktionen, wenn der Fokus verloren wird
        }


    }
}
