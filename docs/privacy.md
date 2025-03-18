# Privacy

## Error Tracking

You can see exact exceptions being sent in `Documents/SteamDeckTools/Logs`
- if it is empty it means nothing was sent.

## Auto-update

Application for auto-update purposes checks for latest release on a start,
or every 24 hours. As part of auto-update it sends information about installation
time of the application, application version, which SteamDeckTools apps are used.

## Disable it

Create `DisableCheckForUpdates.txt` file. To validate that this is working,
when clicking `Check for Updates` or running `Updater.exe` the application
will show `This application has explicitly disabled auto-updates`.
