# Environment-Setup
## Visual Studio 2013 Ultimate
1.	http://tinyurl.com/vs2013hgb öffnen und im Software-Katalog unter „Entwicklertools“ „Microsoft Visual Studio 2013“ auswählen
![image](https://gitlab.com/uploads/OperationPhrike/phrike/0a42ddb03a/image.png)
2.	Im Inhaltsverzeichnis „Microsoft Visual Studio Ultimate 2013“ auswählen und dann Bei „Microsoft Visual Studio Ultimate 2013 with Update 2 32-Bit (German) - DreamSpark“ auf „In den Warenkorb“ klicken.  
![image](https://gitlab.com/uploads/OperationPhrike/phrike/99af93490d/image.png)
3.	Sie werden nun auf eine Login-Seite der FH-OÖ weitergeleitet, hier einfach mit Ihrer Matrikelnr. und FH-internem Passwort anmelden. 
4.	Danach müssen sie nur noch „Download starten“ drücken, dadurch öffnet sich eine neue Seite die Sie anweist zuerst den Secure Download Manager herunterzuladen.
5.	Folgen Sie diesen Anweisungen und laden Sie wie angegeben nach abgeschlossener Installation die SDX-File herunter und führen Sie diese aus.
6.	Es startet nun der Secure Download Manager, wo sie 2 auswahlmöglichkeiten haben. Diese beschreiben die 2 verschiedenen Arten der installer die Sie für VS2013 verwenden können. Empfehlenswert wäre hier die 1. Version herunterzuladen, da die komplette iso dann auch später wenn nötig offline installiert werden kann. 
7.	Ist der Download abgeschlossen muss die heruntergeladene iso gemounted werden und die „vs_ultimate.exe“ ausgeführt werden.
8.	Installieren Sie nun VS2013 Ultimate in den gewünschten Ordner. 
9.	Nach Abschluss der Installation beim ersten Starten die Anmeldung überspringen und für die Voreinstellungen "C#" auswählen.
10.	Unter Hilfe -> Produkt Registrieren den vorhin von Dreamspark zur verfügung gestellten Key eingeben. 
11.	Fleißig Coden!

## Resharper/StyleCop
1. https://stylecop.codeplex.com/ öffnen und StyleCop herunterladen.
2. StyleCop mit Standardeinstellungen installieren.
3. Die neueste Version (dzt. 9.1) von https://www.jetbrains.com/resharper/download/ herunterlaen.
4. Resharper mit den Standardeinstellungen installieren.
5. Resharper aktivieren mit folgenden Daten:
    * Serial:IfTTh+StEAgrJHllgNuozA7jg9uPCvjS
    * Name:Hugo
6. In Visual Studio das Resharper-Menü anklicken und auf "Extension Manager" klicken. 
7. Im Extension Manager nach "StyleCop" suchen und von "Skip" auf "Install" wechseln. 
8. Im unteren Abschnitt des Resharpers auf "Install" klicken und auf Abschluss der Installation warten.
9. Visual Studio neu starten.
10. Es gibt keinen Schritt 10! Die StyleCop-Rules sind im
    git-Repository enthalten und werden von StyleCop für die
    Phrike-Solution automatisch übernommen.

## Zusätzliche Bibliotheken
Derzeit werden [OxyPlot](http://oxyplot.org/) und Boost (http://www.boost.org/) verwendet.

### OxyPlot

OxyPlot sollte von Visual Studio automatisch installiert werden, da es als NuGet-Package eingebunden
ist. Sollte es dennoch Build-Fehler die sich auf OxyPlot beziehen geben, kann dies üblicherweise mit
folgenden Schritten behoben werden:

1. Sicherstellen dass die Phrike-Solution in Visual Studio geöffnet ist.
2. VIEW -> Other Windows -> Package Manager Console aufrufen.
3. Im erscheinenden Konsolenfenster `Update-Package -reinstall` eingeben.

### Boost

Zur Installation der C++-Bibliothek Boost sind folgende Schritte notwendig:

1. Downloaden von Boost 1.58 als [ZIP](http://sourceforge.net/projects/boost/files/boost/1.58.0/boost_1_58_0.zip/download)
   oder als nur halb so große [7z-Datei](http://sourceforge.net/projects/boost/files/boost/1.58.0/boost_1_58_0.7z/download)
   (falls 7-Zip, WinRAR oder anderes kompatibles Programm installiert ist).
2. Enpacken des heruntergeladenen Archivs in beliebigen Ordner (aber nicht in den Profilordner auf FH-Rechnern und nicht
   ins git-Repository!)
3. Die Umgebungsvariable `BOOST_ROOT` auf den Pfad des Ordners setzen in den Boost enpackt wurde (der in dem z.B. die
   Datei `bootstrap.bat` liegt).
   1. Ein CMD-Fenster als Administrator öffnen.
   2. Den Befehl `setx /M BOOST_ROOT X:\Pfad\zu\Boost` eingeben (`X:\Pfad\zu\Boost` durch den richtigen Pfad ersetzen).
4. Visual Studio neu starten. Kann das Projekt Sensors/GMobiLabXferHelpers danach
   noch nicht erfolgreich kompiliert werden ist eventuell ein Ab- und erneutes Anmelden nötig
   damit die Umgebungsvariable übernommen wird.

## "Uninteressanter Rest"
*	Speicherort des Templates für "New Class":
C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\ItemTemplates\CSharp\Code\1031\Class\Class.cs
*	Key für das Merge/Diff-Tool "Beyond Compare" (http://www.scootersoftware.com/):

> H1bJTd2SauPv5Garuaq0Ig43uqq5NJOEw94wxdZTpU-pFB9GmyPk677gJ
> vC1Ro6sbAvKR4pVwtxdCfuoZDb6hJ5bVQKqlfihJfSYZt-xVrVU27+0Ja
> hFbqTmYskatMTgPyjvv99CF2Te8ec+Ys2SPxyZAF0YwOCNOWmsyqN5y9t
> q2Kw2pjoiDs5gIH-uw5U49JzOB6otS7kThBJE-H9A76u4uUvR8DKb+VcB
> rWu5qSJGEnbsXNfJdq5L2D8QgRdV-sXHp2A-7j1X2n4WIISvU1V9koIyS
> NisHFBTcWJS0sC5BTFwrtfLEE9lEwz2bxHQpWJiu12ZeKpi+7oUSqebX+