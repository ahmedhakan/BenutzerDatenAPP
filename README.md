Projekt: Beispielapplikation für Benutzer Daten speicher

![Screenshot 2024-11-25 155042](https://github.com/user-attachments/assets/8bf4be86-f557-499b-a01c-6a5cf375d759)


Dieses Projekt ermöglicht die Verwaltung von Benutzerdaten (wie Name, Nachname und Adresse) über eine SQL-Datenbank. Es enthält Funktionen, um Benutzerdaten hinzuzufügen, zu ändern, zu löschen sowie eine benutzerfreundliche Oberfläche zum Suchen und Filtern von Benutzerdaten.


Funktionen:
Datenbankanbindung: Eine SQL-Datenbank zur Speicherung von Benutzerdaten.
Benutzerverwaltung: Benutzer können hinzugefügt, geändert oder gelöscht werden.
Suche und Filter: Eine Tabelle, die es ermöglicht, Benutzerdaten zu durchsuchen und nach bestimmten Kriterien zu filtern.
Einfache Bedienoberfläche: Eine übersichtliche Benutzeroberfläche zur Verwaltung und Anzeige der Benutzerdaten.


Wichtigsten Code Datein sind:

BenutzerDatenAPP/Page1.xaml.cs

BenutzerDatenAPP/Page2.xaml.cs


Voraussetzungen:

Visual Studio 2022

SQL Server Management Studio 



Installation:

Klone das Repository oder lade die Projektdateien herunter.
Öffne das Projekt in Visual Studio.
Stelle sicher, dass die Verbindung mit SQL-Datenbank ordnungsgemäß ist. (Wenn es keine Verbindung macht, klicken sie auf gerät vertrauen (Erster fenster wenn sie SSMS öffnen) und versuchen sie es erneut.)
Achten sie darauf das in Page2.xaml.cs 23 Zeile und in Page1.xaml.cs 136 Zeile geändert muss, weil ihr gerät name anders ist:

private readonly string connectionString = "Data Source=LAPTOP-89SP8JB6\\SQLEXPRESS;Initial Catalog=BenutzerDaten;Integrated Security=True;TrustServerCertificate=True";

using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-89SP8JB6\\SQLEXPRESS;Initial Catalog=BenutzerDaten;Integrated Security=True;TrustServerCertificate=True"))

Für Data Source= "", müssen sie ihr gerät name hinzufügen.
        


Verwendete Technologien:

C#: Für die Logik und die Verbindung zur SQL-Datenbank.
SQL Server: Zum Speichern von Benutzerdaten.
Windows Forms oder WPF: Für die Benutzeroberfläche (abhängig von der Art des Projekts).



Funktionen im Detail:

Daten Hinzufügen: Neue Benutzer können mit einem Formular hinzugefügt werden, indem der Name, Nachname und die Adresse, Telefonnummer und Bild eingegeben werden.
Daten Ändern: Bestehende Benutzerdaten können bearbeitet und aktualisiert werden.
Daten Löschen: Benutzer können aus der Datenbank entfernt werden.
Daten Suchen: Eine Tabelle zeigt alle Benutzerdaten, und Benutzer können nach Name, Nachname oder Adresse suchen.

Im Anhang finden Sie eine detaillierte Dokumentation des Projekts, einschließlich anschaulicher Abbildungen zur Veranschaulichung.

[BeispielapplikationReport.pdf](https://github.com/user-attachments/files/17904953/BeispielapplikationReport.pdf)
