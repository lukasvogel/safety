# safety

## Aufgabe 1

## Aufgabe 3
Das zugehörige VisualStudio-Projekt ist in uebung02/Bahnübergang.
Wir haben zusätzlich noch folgende Störungen ausgewählt:

| Störung | Beschreibung |
| ------- | ------------ |
| FailureSMotor | Der Motor, der die Schranke senkt und hebt, fällt aus |
| FaultHodometerPos | Das Hodometer behauptet, der Zug sei noch weiter vom Übergang entfernt, als er eigentlich ist |
| FaultHodometerSpeed | Das Hodometer zeigt eine Geschwindigkeit von 1 an, egal wie schnell der Zug tatsächlich fährt |
| FaultTimer | Der Timer triggert, bevor die Zeit eigentlich abgelaufen wäre |

## Aufgabe 4
Die Plausibilitätschecks sind im Projekt Bahnübergang in der Datei NuSMV1.tt im Abschnitt "Sollverhalten" aufgeführt.
Folgende Checks haben wir durchgeführt:

  * Ausschluss des Hazards
    - Der Zug ist niemals auf einem ungesicherten Bahnübergang
  * Zug
    - Der Zug überfährt immer den Überfahrtsensor und ist davor nie stehen geblieben
    - Der Zug ist irgendwann immer hinter dem Gefahrenpunkt
    - Wenn der Zug bremst, kommt er irgendwann auch zum Stehen
  * Übergang
    - Wenn der Übergang die Sicherungsnachricht erhält, ist er irgendwann auch gesichert
    - Wenn der Bahnübergang meldet, er sei gesichert, dann ist auch die Schranke unten
    - Wenn die Schranke unten ist, ist der Übergang im nächsten(!) Schritt gesichert
    - Der Zug erhält die Sicherungsbestätigung, wenn er sie denn erhält, zumindest einmal vor dem Gefahrenpunkt
    - Am Ende öffnet sich der Übergang immer
    - Wenn sich die Schranke öffnet, ist der Zug hinter dem Gefahrenpunkt oder der Timer ist abgelaufen
    
## Aufgabe 6
Die Plausibilitätschecks für die Störungen sind ebenfalls in NuSMV1.tt definiert. Folgende Checks haben wir durchgeführt:

| Störung | Plausibilitätscheck |
| ------- | --------------------|
| FaultHodometerPos | Wenn die Positionsbestimmung des Hodometers fehlschlägt, ist danach die tatsächliche Position größer als die gemeldete |
| FaultHodometerSpeed | Wenn die Geschwindigkeitsanzeige falsche Werte liefert, ist die gemessene Geschwindigkeit im nächsten Schritt immer 1 |
| FailureSMotor | Wenn der Schrankenmotor versagt, während die Schranke offen ist, geht sie nie zu |
| FailureBrakes | Wenn die Bremsen irgendwann versagen während der Zug fährt, hält der Zug niemals an |
| FaultSensor | Obwohl der Zug noch vor dem Sensorpunkt ist, kann der Sensor eine Überfahrt melden |
| FaultTimer | Der Timer triggered in manchen Pfaden bevor der Zug überhaupt am Gefahrenpunkt ist, obwohl er nicht abgebremst hat |
| FaultSecured | Es gibt mindestens einen Pfad, indem der Übergang meldet, er sei gesichert, obwohl die Schranke nicht unten ist |

## Aufgabe 7
Die DCCA wurde in File DCCA.tt durchgeführt. Die minimal kritischen Fehlermengen sind dort bereits definiert.
Die LTL-Formeln darunter belegen, dass diese Mengen jeweils bereits kritisch sind.
Folgende Mengen minimal kritische Mengen ergeben sich:

* \{ FaultHodometerPos \}
* \{ FaultHodometerSpeed \}
* \{ FaultSensor \}
* \{ FaultTimer \}
* \{ FailureBrakes, FailureSMotor \}
* \{ FaultSecured, FailureSMotor \}

## Aufgabe 8
TODO

## Aufgabe 9
Siehe uebung05/Drucktank.smv

Durch 
`DEFINE faults := v.fs1 = No &
				 v.fk1 = No &
				 v.fk2 = No &
				 v.ft = No &
				 v.fs = No`
kann man anpassen, welche Fehler auftreten dürfen.

Die Formel `LTLSPEC !(faults U hazard)` zeigt dann an, ob die Fehlermenge kritisch ist.

Es ergeben sich folgende minimal kritische Fehlermengen:
  - \{ fk2 \}
  - \{ fs, ft \}
  - \{ fs, fk1 \}
  - \{ fs, fs1 \}
  
## Aufgabe 10
P(H) <= P(fk2) + P(fs) * P(ft) + P(fs) * P(fk1) + P(fs) * P(fs1)  
      = P(fk2) + P(fs) * (P(ft) + P(fk1) + P(fs1)  
      = 3*10^-5 + 10^-4 * (10^-4 + 3*10^-5 + 3*10^-5)  
      = 0.00003  
      = 0.003%

## Aufgabe 11
Siehe uebung05/drucktank.prism

## Aufgabe 12
TODO

## Aufgabe 13
In Projekt SSharpÜbergang in Solution Bahnübergang in uebung02/Bahnübergang
