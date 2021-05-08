$steam_path = Get-ItemPropertyValue -Path 'HKCU:\Software\Valve\Steam' -Name 'SteamPath'
$game_path = Join-Path $steam_path 'steamapps\common\They Are Billions'
Copy-Item .\Ionic.Zip.dll $game_path -Force
Copy-Item .\Mods $game_path -Recurse -Force