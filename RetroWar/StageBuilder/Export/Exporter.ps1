$newName = $Args[0] + ".json";

Move-Item -Path "STAGE_OUTPUT.json" -Destination ("C:\Users\Adrian\source\repos\RetroWar\RetroWar\RetroWar\Content\LoadingScripts\Stages\" + $newName) -force