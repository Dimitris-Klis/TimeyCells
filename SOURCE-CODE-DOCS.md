# Source Code Documentation

## Overview
TimeyCells is made up of around 60 scripts, with varying complexity — some are simple and help with animations or UI interactions, while others are full systems responsible for editing the timetable, saving data, copy-pasting, and more.
- **Most of the code** lives in `Assets/Scripts`.
- **Text validation scripts** are in `Assets/TextMesh Pro/Validators` (3 scripts total). These are used with TMP_InputFields and are meant to prevent the user from typing illegal characters.
- **Extra assets** are in `Assets/StreamingAssets`:
  - `ExtraProperties.json` – explained [here](https://github.com/Dimitris-Klis/TimeyCells?tab=readme-ov-file#portable-mode-pc-only)
  - `CopyToClipboard.ps1` – a PowerShell script used to copy timetables as images (PC only)
- **Android-specific asset**:
  - `Assets/Plugins/Android/FileProviderLib-release.aar` – enables sharing timetables as images on mobile.

## Core Concepts
...

### Events & Event Types
...

### Timetable Editing
...

### Manual Editing
...

### Saving, Loading & Data Management
...

### Sharing Features
...

### UI & Input Validation
...
