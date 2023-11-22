# Modul295 SkiService

Dieses Projekt ist eine Web-API für einen Skiservice. Es verwendet .NET für die Backend-Entwicklung.

## Voraussetzungen

- XAMPP (oder eine ähnliche Software) für MySQL und Apache
- Visual Studio
- .NET SDK

## Lokale Einrichtung

### Schritt 1: Repository klonen

Klonen Sie das Repository auf Ihren lokalen Rechner:

```git clone https://github.com/RobinRuf/modul295_skiservice.git```


### Schritt 2: XAMPP Konfiguration

1. Starten Sie XAMPP und stellen Sie sicher, dass der MySQL-Service auf Port `3306` und der Apache-Service auf Port `8080` läuft.
2. Öffnen Sie phpMyAdmin (http://localhost:8080/phpmyadmin) in Ihrem Browser, um die Datenbank zu verwalten.

### Schritt 3: Datenbank einrichten

1. Importieren Sie die `database_setup.sql` Datei in Ihre MySQL-Datenbank. Diese Datei finden Sie im Repository und sie enthält die notwendigen SQL-Befehle, um die Datenbank und die erforderlichen Tabellen zu erstellen.
   - [database_setup.sql](https://github.com/RobinRuf/modul295_skiservice/blob/main/database_setup.sql)

### Schritt 4: Web-API mit Visual Studio starten

1. Öffnen Sie das Projekt in Visual Studio.
2. Stellen Sie sicher, dass die Web-API auf Port `7214` läuft. Sie können dies in den Einstellungen des Projekts unter `Properties -> launchSettings.json` überprüfen.
   Das ist wichtig, da die API Fetch-Calls im Frontend auf `localhost:7214` konfiguriert sind. Sollte jedoch der Standard-Port sein. Projekt starten, dann sehen Sie es in der URL.
   - [launchSettings.json](https://github.com/RobinRuf/modul295_skiservice/blob/main/SkiService/Properties/launchSettings.json)
4. Starten Sie das Projekt **mit HTTPS**.

### Schritt 5: Zugriff auf die Anwendung

Nachdem die Web-API läuft, können Sie auf die API-Endpunkte über `https://localhost:7214/swagger/index.html` zugreifen.

## Hinweis

Bitte beachten Sie, dass die Docker-Unterstützung für dieses Projekt noch in Arbeit ist und in einer zukünftigen Version hinzugefügt wird.

*****************************

## Postman Collection verwenden

Diese Anleitung beschreibt, wie Sie die bereitgestellte Postman Collection importieren und verwenden können, um die API-Endpunkte zu testen.

### Schritt 1: Postman installieren

Falls Sie Postman noch nicht installiert haben, laden Sie es von der [offiziellen Website](https://www.postman.com/downloads/) herunter und installieren Sie es.

### Schritt 2: Postman Collection importieren

1. Öffnen Sie Postman.
2. Klicken Sie auf "Import" in der oberen linken Ecke.
3. Wählen Sie die Option "File" und klicken Sie auf "Upload Files".
4. Navigieren Sie zu dem Verzeichnis, in dem Sie das Repository geklont haben, und wählen Sie die `SkiService_Backend.postman_collection.json` Datei aus.
   - [SkiService_Backend.postman_collection.json](https://github.com/RobinRuf/modul295_skiservice/blob/main/SkiService_Backend.postman_collection.json)
5. Klicken Sie auf "Import", um die Collection in Postman zu laden.

### Schritt 3: API-Endpunkte testen

Nachdem Sie die Collection importiert haben, können Sie die verschiedenen Anfragen in der Collection ausführen, um die Funktionalität der API zu testen. Stellen Sie sicher, dass Ihre lokale API-Instanz läuft, bevor Sie die Anfragen in Postman ausführen.

1. Wählen Sie eine Anfrage aus der Collection aus.
2. Passen Sie bei Bedarf die URL (oft id/priority am Ende) oder den Body an.
3. Klicken Sie auf "Send", um die Anfrage auszuführen und die Antwort zu erhalten.

### Hinweise

- Stellen Sie sicher, dass die URL und die Ports in den Postman-Anfragen mit Ihrer lokalen Konfiguration übereinstimmen.
- Für einige Anfragen benötigen Sie möglicherweise Authentifizierungstoken oder spezielle Header. Stellen Sie sicher, dass diese korrekt konfiguriert sind.

Mit dieser Postman Collection können Sie die Funktionalität und das Verhalten der API schnell und effizient testen.


## Weitere Informationen

Für weitere Informationen über das Projekt, wie z.B. API-Endpunkte und Nutzung, siehe die Dokumentation im Repository.
