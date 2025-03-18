# README

[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/mops1k/steam-deck-tools?label=stable&style=flat-square)](https://github.com/mops1k/steam-deck-tools/releases/latest)
![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/mops1k/steam-deck-tools?color=red&include_prereleases&label=beta&style=flat-square)
![GitHub all releases](https://img.shields.io/github/downloads/mops1k/steam-deck-tools/total?style=flat-square)

**This is a fork with my own extensions for overlays and other settings**

This repository contains my own personal set of tools to help running Windows on Steam Deck.

**This software is provided on best-effort basis and can break your SteamDeck.**

<img src="../docs/images/overlay.png" height="400"/>

## Install

See all instructions here: [Install](#Install).

## Applications

This project provides the following applications:

- [Fan Control](fan-control.md) - control Fan on Windows
- [Performance Overlay](performance-overlay.md) - see FPS and other stats
- [Power Control](power-control.md) - change TDP or refresh rate
- [Steam Controller](steam-controller.md) - use Steam Deck with Game Pass
- Launcher - launch all tools in one place

## Install

Download and install latest `SteamDeckTools-<version>-setup.exe` from [Latest GitHub Releases](https://github.com/mops1k/steam-deck-tools/releases/latest).

This project requires those dependencies to be installed in order to function properly. Those dependencies needs to be manually installed if portable archive is to be used:

- [Microsoft Visual C++ Redistributable](https://aka.ms/vs/17/release/vc_redist.x64.exe)
- [Rivatuner Statistics Server](https://www.guru3d.com/files-details/rtss-rivatuner-statistics-server-download.html)
- [ViGEmBus](https://github.com/ViGEm/ViGEmBus/releases)

It is strongly advised that following software is uninstalled or disabled:

- [SWICD](https://github.com/mKenfenheuer/steam-deck-windows-usermode-driver)
- [GlosSI](https://github.com/Alia5/GlosSI)
- [HidHide](https://github.com/ViGEm/HidHide)

## Additional informations

- [Controller Shortcuts](shortcuts.md) - default shortcuts when using [Steam Controller](steam-controller.md).
- [Development](development.md) - how to compile this project.
- [Risks](risks.md) - this project uses kernel manipulation and might result in unstable system.
- [Privacy](privacy.md) - this project can connect to remote server to check for auto-updates or track errors
- [Troubleshooting](troubleshooting.md) - if you encounter any problems.
- The latest beta version can be found in [GitHub Releases](https://github.com/mops1k/steam-deck-tools/releases).

## Join Us

Join Us for help or chat. We are at [Steam Deck Windows | Legion GO](https://t.me/steamdeckwin) .

## Anti-Cheat and Antivirus software

Since this project uses direct manipulation of kernel memory via `inpoutx64.dll`
it might trigger Antivirus and Anti-Cheat software.

**READ IF PLAYING ONLINE GAMES AND/OR GAMES THAT HAVE ANTI-CHEAT ENABLED**

Since this project uses direct manipulation of kernel memory via `inpoutx64.dll`
it might trigger Antivirus and Anti-Cheat software. This could result in interference
or issues in games that use anti-cheat technology (including the possibility of a suspension or ban).

Application by default does not use any kernel-level features. If you request a change
that might trigger anti-cheat detection application does require to acknowledge this.

<img src="images/anti_cheat_protection.png" height="150"/>

### Safe settings

If you play online games application needs to disable kernel-features.
**Those settings should be considered safe**:

- **Fan Control**: Use **Default** FAN
- **Performance Overlay**: OSD Kernel Drivers are **DISABLED**

### Features missing without Kernel Drivers

By disabling usage of Kernel Drivers you are loosing the:

- **Fan Control**: Optimised `SteamOS` FAN curve
- **Performance Overlay**: See CPU % and MHz, GPU MHz, APU W usage
- **Power Control**: Ability to change TDP, CPU and GPU frequency - you can change this safely before you start the game
- **Steam Controller**: There's no impact

## Supported devices

The application tries it best to not harm device (just in case).
So, it validates bios version. Those are currently supported:

- SteamDeck LCD:

  - F7A0107 (PD ver: 0xB030)
  - F7A0110 (PD ver: 0xB030)
  - F7A0113 (PD ver: 0xB030)
  - F7A0115 (PD ver: 0xB030)
  - F7A0116 (PD ver: 0xB030)
  - F7A0119 (PD ver: 0xB030)

- SteamDeck OLED:

  - F7G0107 (PD ver: 0x1050)

## Author

Kamil Trzciński, 2022-2024
Aleksandr Kvintilyanov, 2025

Steam Deck Tools is not affiliated with Valve, Steam, or any of their partners.

## License

[Creative Commons Attribution-NonCommercial-ShareAlike (CC-BY-NC-SA)](http://creativecommons.org/licenses/by-nc-sa/4.0/).

Free for personal use. Contact me in other cases (`bednyj.mops@gmail.com`).
