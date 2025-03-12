[**READ IF PLAYING ONLINE GAMES AND/OR GAMES THAT HAVE ANTI-CHEAT ENABLED**](https://steam-deck-tools.ayufan.dev/#anti-cheat-and-antivirus-software)

## #{GIT_TAG_NAME}

- PerfomanceOverlay: add possibility to load custom overlay configs
- PerfomanceOverlay: fix battery time sensors

## 0.7.7

- PerfomanceOverlay: change battery time to show hours and minutes
- FpsLimit: add limit equals reshresh rate + 3fps
- FpsLimit: add half and quarter limit (if such limit exists, will be replaced in list from number to word)

## 0.7.6

- PerfomanceOverlay: add overlays with time

## 0.7.4

- FpsLimit: change limits to steps by 5

## 0.7.3

- SteamDeck LCD: Support BIOS F7A0131

## 0.7.2

- PowerControl: Add Charge Limit (70%, 80%, 90%, 100%)

## 0.7.1

- SteamDeck OLED: Support BIOS 107 with temperature readings
- SteamDeck OLED: Remove BIOS 105 support as it is buggy

## 0.7.0

- FanControl: Support for SteamDeck OLED
- PerformanceOverlay: Support the `AMD Custom GPU 0932` found in SteamDeck OLED
- PowerControl: Support `AMD Custom GPU 0932` with a SMU at `0x80600000-0x8067ffff` ver.: `0x063F0E00`

## 0.6.22

- SteamController: Fix broken scroll on left pad introduced by 0.6.21

## 0.6.21

- SteamController: Add support for circular deadzone on left/right sticks
- FanControl: Add Silent fan profile. Configure `Silent4000RPMTemp` threshold in `FanControl.dll.ini`
- SteamController: Added `Win+D` shortcut under `Steam+RightStickPress`

## 0.6.20

- PerformanceOverlay/PowerControl: Add support for `AMD Radeon RX 670 Graphics`
