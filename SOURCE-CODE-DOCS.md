# TimeyCells Source Code Documentation

## Overview
TimeyCells is made up of around 60 scripts, with varying complexity — some are simple and help with animations or UI interactions, while others are full systems responsible for editing the timetable, saving data, copy-pasting, and more. The application was made with the Unity game engine, one of my favorite tools.
- **Most of the code** lives in `Assets/Scripts`.
- **Text validation scripts** are in `Assets/TextMesh Pro/Validators` (3 scripts total). These are used with TMP_InputFields and are meant to prevent the user from typing illegal characters.
- **Extra assets** are in `Assets/StreamingAssets`:
  - `ExtraProperties.json` – responsible for portable mode, which is explained [here](https://github.com/Dimitris-Klis/TimeyCells?tab=readme-ov-file#portable-mode-pc-only)
  - `CopyToClipboard.ps1` – a PowerShell script used to copy timetables as images (PC only)
- **Android-specific asset**:
  - `Assets/Plugins/Android/FileProviderLib-release.aar` – enables sharing timetables as images on mobile.

## Core Concepts

### Events & Event Types
All related scripts are located in `Assets/Scripts/Events - EventTypes`.
Since the term 'Event' is reserved by the `UnityEngine` library, the class representing any event during runtime is named `EventItem.cs`.
Similarly, the class that holds data for any event type is called `EventTypeItem.cs`.

Both events and event types are managed by `EventManager.cs`, which is responsible for:
- Initializing default events and event types
- Creating and deleting events/event types
- Updating the UI graphics associated with each event and event type

### Timetable
All related scripts are located in `Assets/Scripts/Timetable` and `Assets/Scripts/Time Management`

- `Timetable/TimetableGrid.cs` 
  Responsible for aligning cells correctly, adding/deleting columns and rows, and adding multi-row columns (cells that span across all rows).

- `Timetable/TimetableCell.cs`
  Holds references to the UI elements that make up a timetable cell.

- `Timetable/InfoCell.cs`
  Stores the detailed data for each timetable cell (e.g. Selected Event, Selected EventType, Event Length, etc.).

- `Time Management/DayTimeManager.cs`
Highlights the current timetable cell, shows how much time is left until the next event, and handles creation/deletion/swapping of weekdays and labels.  
  Also responsible for:
  - Parsing time
  - Retrieving a cell’s start time and length
  - Accessing current cell info
  - Automatically setting up `TimeIndexObject`s for each column

- `Time Management/WeekDay.cs`
  Holds info about which days this weekday applies to, the weekday's name, its common length, start time, and any temporary overrides.

- `Time Management/WeekDayObject.cs`
  Visualizes `WeekDay` data in the UI.

- `Time Management/LabelIndex.cs`
  Stores customization options for `TimeIndexObject`s.

- `Time Management/TimeIndexObject.cs`
  Displays a column’s time and index. Can optionally show a custom label instead of an index.

### Timetable Editing
All related scripts are located in `Assets/Scripts/Editors`, `Assets/Scripts/Timetable` and `Assets/Scripts/Drag N Swap`

- `Editors/TimetableEditor.cs`
  Responsible for setting up UI editing elements, preparing column/row addition or deletion, and preparing column/row swapping. Also prepares quick assignment of events to cells.

- `Timetable/TimetableGrid.cs`
  (Also mentioned in **Timetable** section)  
  Used during editing to create buttons for row/column adding, deleting, and swapping.

- `Timetable/TimetableCell.cs`
  (Also mentioned in **Timetable** section)  
  Used during editing to access and update the UI.

- `Timetable/InfoCell.cs`
  (Also mentioned in **Timetable** section)  
  Used during editing for updating the data stored in each cell.

- `Drag N Swap/DragHandleManager.cs`
  Handles user interaction when swapping rows or columns. Determines when drag handles should swap.

- `Drag N Swap/DragHandle.cs`
  Works with `DragHandleManager.cs` to manage user interaction for swapping.

- `Drag N Swap/TimeIndexDrag.cs`
  Inherits from `DragHandle.cs`. Handles swapping of timetable columns when appropriate.

- `Drag N Swap/WeekDayDrag.cs` 
  Also inherits from `DragHandle.cs`. Handles swapping of timetable rows when appropriate.

### Manual Editing
All related scripts are located in `Assets/Scripts/Editors`.

- `WeekdayEditor.cs`
  Allows users to edit weekdays. They can modify the weekday’s name, associated days, common length, and start time. Temporary overrides are also supported.

- `CellInfoEditor.cs`
  Allows users to edit individual cells. Users can change the assigned event and override properties such as event name, Info1, Info2, favourite status, and event type. Temporary overrides are supported here as well.

- `LabelEditor.cs`
  Allows users to turn a column’s index into a custom label (e.g. replacing `5` with `"TEXT"`).  
  Users can also choose whether the label should count as an index, which affects how later numbers are displayed:  
  - **Count as index: true** → `1, 2, 3, 4, TEXT, 6, 7, 8, 9`  
  - **Count as index: false** → `1, 2, 3, 4, TEXT, 5, 6, 7, 8`

### Saving, Loading & Data Management
All related scripts are located in `Assets/Scripts/Saving`.

- `SaveManager.cs`
  Handles saving and loading timetables and user settings.  
  - Creates new timetables.  
  - Manages copy/paste sharing (lets users copy timetable data as text or paste a timetable into the app).  
  - Converts between readable data and the internal format the app uses.

- `TimetableData.cs`
  Timetables are saved and loaded in this format.  
  Used when turning the current timetable into plain data (for saving or copying), and when rebuilding it (for loading or pasting).

- `S_ProgramData.cs`
  Format used by the older _School Timetable_ app.  
  When pasted text is detected in this format, `SaveManager.cs` automatically converts it to the new format.

- `SettingsData.cs`
  User settings are saved and loaded in this format.
  
- `TimetableButton.cs`
  Represents each timetable in the selection screen.  
  Lets the user select, rename, or delete saved timetables.

### Sharing Features
All related scripts are located in `Assets/Scripts` and `Assets/StreamingAssets`.

- `Scripts/PhotoManager.cs`
  Handles sharing timetable photos for Windows and Android.

- `StreamingAssets/CopyToClipboard.ps1`
  A PowerShell script that uses Windows Forms to quickly copy images prepared by `PhotoManager.cs`.
  Unfortunately, Unity doesn't have built-in support for copying images, so this is the most efficient way I've found to copy images on a Windows build.


### UI & Input Validation
All related scripts are located in `Assets/Scripts`, `Assets/Scripts/Layout Groups` and `Assets/Scripts/Stylizing`.

UI interaction is handled by Unity. I configured which functions are called and what UI appears when you interact with elements, using Unity’s `Unity Events`. Some interactions are set up manually in the editor, while others are configured through code during runtime.

- `Layout Groups/CustomLayoutGroup.cs`
  Simply put, a custom layout group that updates only when you tell it to.

- `Layout Groups/CenterAndFit.cs`
  A custom layout group that fits and centers its content. Works similarly to `CustomLayoutGroup.cs`.

- `Layout Groups/CustomGridLayoutGroup.cs`
  A special custom layout group used by `PhotoManager.cs` to generate an image of the timetable.

- `Scripts/ConfirmationManager.cs`
  Shows confirmation screens (e.g., "Are you sure you want to delete X?"). Useful for confirming deletions or unsaved changes on the fly.

- `Stylizing/ColorStylizer.cs`
  Changes UI colors. In the settings, the user can change the colors of the application to suit their needs.

- `Scripts/QuitButton.cs`
  Contains a function to exit the app.

## Script Breakdown & Unity Integration
### Script Breakdown:
Break down the key scripts. Keep descriptions short and focus on what each script is responsible for.

### Unity Integration:
Explain how the scripts connect to GameObjects, prefabs, scenes, and components in Unity.