# TimeyCells Source Code Documentation

## Table of Contents
- 01: [Overview](#overview)

- 02: [Core Concepts](#core-concepts)
  - [Events & Event Types](#events-event-types)
  - [Timetable](#timetable)
  - [Timetable Editing](#timetable-editing)
  - [Manual Editing](#manual-editing)
  - [Saving, Loading & Data Management](#saving-loading-data-management)
  - [Sharing Features](#sharing-features)
  - [UI & Input Validation](#ui-input-validation)

- 03: [Script Breakdown & Unity Integration](#-script-breakdown-unity-intergration)
  - [Common Patterns Across Scripts](#common-patterns-across-scripts)
    - [Singletons](#singletons)
    - [Unity's Attributes](#unity-s-attributes)

  - [`Scripts/Events - Event Types`](#scripts-events-event-types)
    - [EventTypeItem.cs](#eventtypeitem-cs)
    - [EventItem.cs](#eventitem-cs)
    - [EventItemOverride.cs](#eventitemoverride-cs)
    - [EventManager.cs](#eventmanager-cs) -> DOCUMENT PROPERTIES!!!
    - [EventCreator.cs](#eventcreator-cs) -> DOCUMENT PROPERTIES!!!
    - [EventTypeCreator.cs](#eventtypecreator-cs) -> DOCUMENT PROPERTIES!!!

  - [`Scripts/Timetable`](#scripts-timetable)
    - [TimetableCell.cs](#timetablecell-cs)
    - [CellInfo.cs](#cellinfo-cs)
    - [TimetableGrid.cs](#timetablegrid-cs)
    - [ScrollZoom.cs](#scrollzoom-cs)
    - [FreezeGrid.cs](#freezegrid-cs)

  - [`Scripts/Time Management`](#scripts-time-management)
    - [WeekDay.cs](#weekday-cs)
    - [WeekDayObject.cs](#weekdayobject-cs)
    - [TimeIndex.cs](#timeindex-cs)
    - [TimeIndexObject.cs](#timeindexobject-cs)
    - [DayTimeManager.cs](#daytimemanager-cs)

  - [`Scripts/Drag N Swap`](#scripts-drag-n-swap)
    - [DragHandle.cs](#draghandle-cs)
    - [TimeIndexDrag.cs](#timeindexdrag-cs)
    - [WeekDayDrag.cs](#weekdaydrag-cs)
    - [DragHandleManager.cs](#draghandlemanager-cs)

  - [`Scripts/Help Tab`](#scripts-help-tab)
    - [GIFObject.cs](#gifobject-cs)
    - [GIFHandler.cs](#gifhandler-cs)
    - [ContentButton.cs](#contentbutton-cs)
    - [HelpLayoutGroup.cs](#helplayoutgroup-cs)
    - [TableOfContents.cs](#tableofcontents-cs)
    - [HelpSection.cs](#helpsection-cs)

  -[`Scripts/Stylizing`](#scripts-stylizing)
    - [ColorStylePreset.cs](#colorstylepreset-cs)
    - [ColorStylizer.cs](#colorstylizer-cs)
    - [PaletteObject.cs](#paletteobject-cs)
    - [PaletteDropdown.cs](#palettedropdown-cs)
    - [CustomPaletteLister.cs](#custompalettelister-cs)
    - [PaletteCreator.cs](#palettecreator-cs)

  -[`Scripts/Editors`](#scripts-editors)
    - [TimetableEditor.cs](#timetableeditor-cs)
    - [CellInfoEditor.cs](#cellinfoeditor-cs)
    - [WeekdayEditor.cs](#weekdayeditor-cs)
    - [LabelEditor.cs](#labeleditor-cs)
    - [ColorEditor.cs](#coloreditor-cs)

  -[`Scripts/Localization`](#scripts-localization)
    - [TMPLocalizer.cs](#tmplocalizer-cs)
    - [TMPDropdownLocalizer.cs](#tmpdropdownlocalizer-cs)
    - [LocalizationSystem.cs](#localizationsystem-cs)

  -[`Scripts/Layout Groups`](#scripts-layout-groups)
    - [CustomLayoutGroup.cs](#customlayoutgroup-cs)
    - [CenterAndFit.cs](#centerandfit-cs)
    - [CustomGridLayoutGroup.cs](#customgridlayoutgroup-cs)

  - [`Scripts/Saving`](#scripts-saving)
    - [TimetableData.cs](#timetabledata-cs)
    - [S_ProgramData.cs](#s-programdata-cs)
    - [SettingsData.cs](#settingsdata-cs)
    - [TimetableButton.cs](#timetablebutton-cs)
    - [SaveManager.cs](#savemanager-cs)

  - [`Scripts`](#scripts)
    - [CopyPasteManager.cs](#copypastemanager-cs)
    - [PhotoManager.cs](#photomanager-cs)
    - [ConfirmationManager.cs](#confirmationmanager-cs)
    - [QuitButton.cs](#quitbutton-cs)

  - [`Scripts/Inspector Stuff`](#scripts-inspector-stuff)
    - [CommentInformationNote.cs](#commentinformationnote-cs)
    - [ReadOnlyAttribute.cs](#readonlyattribute-cs)
    - [ReadOnlyDrawer.cs](#readonlydrawer-cs)
  
  - [`Scripts/Polish`](#scripts-polish)
    - [InputFieldFixer.cs](#inputfieldfixer-cs)
    - [TabHandler.cs](#tabhandler-cs)
    - [VersionText.cs](#versiontext-cs)
  
  - [`Scripts/Polish/Animation Scripts`](#scripts-polish-animation-scripts)
    - [CustomAnimator.cs](#customanimator-cs)
    - [HamburgerButton.cs](#hamburgerbutton-cs)
  
  - [`TextMesh Pro/Validators`](#textmesh-pro-validators)
    - [ValidatorBase.cs](#validatorbase-cs)
    - [HexCodeValidator.cs](#hexcodevalidator-cs)
    - [TimeValidator.cs](#timevalidator-cs)

  - [`StreamingAssets`](#streamingassets)
    - [CopyToClipboard.ps1](#copytoclipboard-ps1)
    
  - [`Plugins/Android`](#plugins-android)
    - [FileProviderLib-release.aar](#fileproviderlib-release-aar)

- 04: [Packages Used](#packages-used)
  - [Unity Features](#unity-features)
  - [Unity Packages](#unity-packages)
  - [Libraries](#libraries)

## Overview
TimeyCells is made up of 67 scripts, with varying complexity — some are simple and help with animations or UI interactions, while others are full systems responsible for editing the timetable, saving data, copy-pasting, and more. The application was made with the Unity game engine, one of my favorite tools.
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

### Common Patterns Across Scripts
Many scripts share similar functions or code, so I’ll explain key patterns here:

#### Singletons
To make important scripts globally accessible, I use the singleton pattern, a common and recommended practice among Unity developers. This involves assigning a `static` instance reference in the `Awake()` method:
```cs
using UnityEngine;

public class ClassName : MonoBehaviour
{
    public static ClassName Instance;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        Instance = this;
    }

    public void DoSomething()
    {
        Debug.Log("Hello World!");
    }
}
```

With this setup, you can access the script from anywhere using `ClassName.Instance`, avoiding the need to create references manually or search for components at runtime.

Here's an example with Singletons:
```cs
    void Start()
    {
        ClassName.Instance.DoSomething()
    }
```

And here's an example without Singletons (less efficient):
```cs
    void Start()
    {
        // This is fine as long as you don't access the script frequently.
        FindObjectOfType<ClassName>().DoSomething();
    }
```
Singletons are used in many **manager** and **user editor** scripts.

#### Unity's Attributes
On many scripts, you'll likely see the following attributes:
```cs
  [Header("HeaderNameHere")]
  [Space(20)]
  [Space]
```
We use these to improve script readability in the Unity Inspector. Headers act like labeled sections, and `[Space]` adds vertical spacing to keep things neat.

Other common attributes include:
```cs
// This makes private variables visible and editable in the Unity Inspector.
[SerializeField] int variable;
```


```cs
/* Marks a class as serializable so Unity can display it in the Inspector,
even if the class isn't derived from MonoBehaviour.*/
[System.Serializable]
public class Item
{
  // Class contents here...
}
```

```cs
/* Used to quickly test functions via the Unity script's context menu.*/
[ContextMenu("Create!")]
public void Create()
{
  // Method contents here...
}
```

---

### `Scripts/Events - Event Types`

#### EventTypeItem.cs

**Description**<br/>
Event types are primarily used to group and visually distinguish events by applying a background and text color. Naming each type helps users easily identify and select the right one when editing an event.

**Properties**
- `int ItemID` — Unique identifier for the event type.
- `string TypeName` — The name of the event type.
- `Color TextColor` — The color used for the event's text.
- `Color BackgroundColor` — The color used for the event's background.

**Constructors**
- `EventTypeItem()` — Default constructor that initializes all fields to default/empty values.
- `EventTypeItem(TimetableData.EventTypeData data)` — Initializes an `EventTypeItem` from saved data (used by `SaveManager.cs`).

---

#### EventItem.cs

**Description**<br/>
Represents a single event preset in the timetable. Events can be assigned to timetable cells as a base configuration, making them useful for reusability. While you can override the default event directly, making events technically optional, using them can save time and improve consistency across cells.

**Properties**

- `int ItemID` — Unique identifier for the event.
- `string EventName` — The name of the event.
- `string Info1` — Optional user-provided info.
- `string Info2` — More optional user-provided info.
- `int EventType` — The event's type.
- `bool Favourite` — Marks the event as favourite.

**Constructors**

- `EventItem()` — Default constructor that initializes all fields to default/empty values.
- `EventItem(EventItem e)` — Copy constructor that duplicates an existing `EventItem`.

---

#### EventItemOverride.cs

**Description**<br/>
Used by CellInfo to store overrides and temporary overrides.

**Properties**<br/>
- `bool OverrideFavourite` — whether the timetable cell should use the overriden favourite value.

**Constructors**
- `EventItemOverride()` — Default constructor that initializes all fields to default/empty values.

---

#### EventManager.cs

**Description**<br/>
`EventManager.cs` is responsible for:
- Initializing default events and event types
- Creating and deleting events/event types
- Updating the UI graphics associated with each event and event type

**Properties**<br/>
- `static EventManager Instance` — Singleton reference to the EventManager.
- `CellInfoEditor CellInfoEditor` — Reference to the cell info editor for linking events with cell data.
- `EventCreator EventCreator` — Reference to the component responsible for event creation/editing UI.
- `EventItem DefaultNewEvent` — Template used when creating a new event.
- `EventTypeCreator EventTypeCreator` — Reference to the component responsible for event type creation/editing UI.
- `EventTypeItem DefaultNewEventType` — Template used when creating a new event type.
- `List<EventTypeItem> EventTypes` — All available event types.
- `List<EventItem> Events` — All created events.
- `TimetableCell CellPrefab` — UI prefab used to visually represent an event or event type.
- `Transform EventsParent` — Parent transform for event preview UI elements.
- `Transform EventSelectorsParent` — Parent transform for event selector buttons in the UI.
- `List<TimetableCell> EventPreviews` — Instantiated cells showing a preview of each event.
- `List<TimetableCell> EventSelectorPreviews` — Instantiated selector buttons for choosing events.
- `Transform EventTypesParent` — Parent transform for event type preview cells.
- `List<TimetableCell> EventTypePreviews` — Instantiated previews of all event types.

**Methods**
```cs
public void  InitializeLists()
```
  Initializes the default `EventItem` and `EventTypeItem`, adding them to their respective lists. These lists are typically cleared by `SaveManager.cs` before loading a timetable.

<br/><br/>

```cs
public void CreateNewEvent(out EventItem _item)
```
  Creates a new `EventItem` and assigns it to the `out` parameter `_item` so it can be configured or referenced elsewhere.

<br/>

```cs
public EventItem GetEvent(int ID)
```
  Searches the `Events` list for an item with a matching `ID`. Returns the item if found, or `null` if not.

<br/>

```cs
public EventItem GetEventIndex(int ID)
```
  Returns the index of the `EventItem` with the specified `ID` in the `Events` list. Returns `-1` if not found. Useful for deletion.

<br/>

```cs
public void DeleteEvent(int ID)
```
  Uses `GetEventIndex(ID)` to find and remove an event from the list. Logs a warning if the event doesn't exist.

<br/>

```cs
public void UpdateEventPreviews(bool updateTimetable)
```
  Updates the event UI previews to reflect the current list of events. If `updateTimetable` is `true`, timetable cells are updated accordingly.

<br/>

```cs
public void UpdateEventSelectorButtons()
```
  Updates the Event Selector UI used for quick cell assignments. This function ensures buttons reflect current events in both edit and normal modes.


<br/><br/>
  
```cs
public void CreateNewEventType(out EventTypeItem _item)
```
  Creates a new `EventTypeItem` and assigns it to the `out` parameter `_item`.

<br/>

```cs
public EventTypeItem GetEventType(int ID)
```
  Returns the `EventTypeItem` with a matching `ID`, or `null` if not found.

<br/>

```cs
public EventItem GetEventTypeIndex(int ID)
```
  Returns the index of the `EventTypeItem` with the given `ID`, or `-1` if not found.

<br/>

```cs
public void DeleteEventType(int ID)
```
  Uses `GetEventTypeIndex(ID)` to find and remove an event type. Logs a warning if none is found.

<br/>

```cs
public void UpdateEventTypePreviews(bool updateTimetable)
```
  Updates the event type UI previews to reflect changes. Also updates timetable cells if `updateTimetable` is `true`.

<br/><br/>

```cs
void SetButton(Button b, int id, bool EventType)
```
  Attaches click functionality to UI buttons so they open the appropriate event or event type editor.

---

#### EventCreator.cs

**Description**<br/>
This script allows the user to create and edit events.

**Properties**
- `int IDToModify` — The ID of the event currently being edited; negative if creating a new event.
- `TMP_Text TitleText` — The UI text element showing the current overlay's title.
- `TimetableCell PreviewCell` — The live preview cell used to reflect changes during editing.
- `TMP_InputField EventNameInput` — Input field for the event’s name.
- `TMP_InputField Info1Input` — Input field for the first line of additional info.
- `TMP_InputField Info2Input` — Input field for the second line of additional info.
- `TMP_Dropdown EventTypeDropdown` — Dropdown menu for selecting the event’s type.
- `Toggle FavouriteToggle` — Toggle for marking the event as a favorite.
- `Button DeleteButton` — Button used to delete the event, if it already exists.

**Methods**

```cs
public void OpenCreator(int ID)
```
Initializes the Creator UI for the event with the given ID, loading an existing event item. If the given ID is less than 0, it prepares a blank form for creating a new event.

<br/><br/>

```cs
public void ChangeEventName(string text)
```
A simple method called by a TMP_InputField. Sets the event's `EventName` property.

<br/>

```cs
public void ChangeInfo1Name(string text)
```
A simple method called by a TMP_InputField. Sets the event's `Info1` property.

<br/>

```cs
public void ChangeInfo2Name(string text)
```
A simple method called by a TMP_InputField. Sets the event's `Info2` property.

<br/>

```cs
public void ChangeEventType(int type)
```
A simple method called by a TMP_Dropdown. Sets the event's `EventType` property.

<br/>

```cs
public void ChangeIsFavourite(bool favourite)
```
 Sets the event's `Favourite` property, indicating whether it is marked as a favorite.

<br/><br/>

```cs
public void Confirm()
```
Applies all changes made in the UI to the event item. No changes affect the event until this method is called. 

> **NOTE:** Cancelling is as simple as disabling the gameobject overlay, so no dedicated function is needed.

<br/>

```cs
public void Delete(bool confirm)
```
Deletes the event. If confirm is false, a confirmation screen will appear to ensure you don't delete the event by accident. If true, the event gets immediately deleted.

---

#### EventTypeCreator.cs

**Description**<br/>
This script allows the user to create and edit event types.

**Properties**
- `int IDToModify` — The ID of the event type currently being edited. If negative, a new event type is being created. If zero, the default type is being edited (some fields are locked).


- `TMP_Text TitleText` — A UI text element that displays the title of the event type creator overlay, such as "Edit Event Type" or "Create Event Type", depending on the mode.

- `TimetableCell PreviewCell` — A live preview element showing how the event type will look, updating in real time as the user changes the name or colors.


- `TMP_InputField EventTypeNameInput` — The input field where users enter or edit the name of the event type.

- `Image ChangeTextColor` — A UI element representing the current text color of the event type. Clicking it opens the color editor to change the text color.

- `Image ChangeBackgroundColor` — A UI element representing the background color of the event type. Clicking it opens the color editor to change the background color.

- `Button DeleteButton` — The button used to delete an existing event type. It is disabled when editing the default event type or creating a new one.

**Methods**
```cs
public void OpenCreator(int ID)
```
Initializes the Creator UI for the event type with the given ID, loading an existing event type item. If the given ID is less than 0, it prepares a blank form for creating a new event type.

<br/>

```cs
public void CloseCreator()
```
Disables the Creator UI along with the color editor.

<br/><br/>

```cs
public void ChangeEventTypeName(string text)
```
A simple method called by a TMP_InputField. Sets the event's `TypeName` property.

<br/>

```cs
public void ActivateColorEditor(bool ChangeBackground)
```
Activates the color editor, which will change the event type's `TextColor` or `BackgroundColor` property, depending on the `ChangeBackground` parameter.

<br/>

```cs
public void Confirm()
```
Applies all changes made in the UI to the event type item. No changes affect the event type until this method is called. 


<br/>

```cs
public void Delete(bool confirm)
```
Deletes the event. If confirm is false, a confirmation screen will appear to ensure you don't delete the event by accident. If true, the event type gets immediately deleted.

---

### `Scripts/Timetable`

#### TimetableCell.cs

**Description**<br/>
Represents a single timetable cell and contains references to its visual and data components.

**Properties**
- `bool IsMultirow` — Indicates whether the parent column spans multiple rows.

- `RectTransform rect` — The cell's `RectTransform`.
- `Button SelfButton` — The cell's clickable button component.

- `Image BackgroundImage` — The background image of the cell.
- `TMP_Text EventNameText` — Displays the event name.
- `TMP_Text Info1Text` — Displays Info1.
- `TMP_Text Info2Text` — Displays Info2.
- `Image FavouriteImage` — Image that marks the cell as a favorite.

- `CellInfo Info` — The data associated with this cell.

---

#### CellInfo.cs

**Description**<br/>
Contains the data associated with a timetable cell, including the selected event, any overrides, and the event’s duration.

**Properties**
- `TimetableCell CellUI` — References the cell’s UI and related components.


- `int SelectedEventBase` — The index of the selected base event.
- `EventItemOverride Override` — Permanent override applied to the base event.

- `bool OverrideCommonLength` — Whether this cell overrides the default length.
- `TimeSpan NewLength` — Custom event length, used if `OverrideCommonLength` is `true`.



- `int TemporaryBase` — Temporary event base index.
- `EventItemOverride TemporaryOverride` — Temporary override applied to the event.

- `bool TempOverrideCommonLength` —  Whether the temporary event overrides the default length.
- `TimeSpan TempNewLength` — Custom temporary event length.


- `DateTime OverrideDate` — The date in which the temporary override was added.
- `int OverrideLength` — The default length of the temporary override, calculated
  externally by CellInfoEditor.cs.
- `int OverrideDelayWeeks` — How many weeks until the temporary override starts to apply.
- `int OverrideExtraLengthWeeks` — How many extra weeks until the temporary
  override expires.

**Methods**
```cs
public void SetupSelf(TimetableData.CellInfoData data)
```
Initializes the cell's data from the provided `data`. Primarily used when loading a timetable.

<br/>

```cs
public void SetSelfToSelectedEvent()
```
Called when `CellUI.button` is clicked while in edit mode. Assigns the currently selected event (`TimetableEditor.instance.SelectedID`) to the cell.

<br/><br/>

```cs
public void UpdateUI()
```
Refreshes the UI of `CellUI` to reflect the current data.

<br/>

```cs
void CheckIfTempExpired()
```
Checks whether the temporary override has expired and removes it if necessary.

---

#### TimetableGrid.cs

**Description**<br/>
Responsible for aligning cells correctly, adding/deleting columns and rows, and adding multi-row columns (cells that span across all rows).

**Properties**
- `Vector2 CellSize` — The size of each cell in the grid.
- `Vector2 Spacing` — The spacing between each cell in the grid.
- `int Rows` — Current number of rows.
- `int Columns` — Current number of columns.
- `int MaxRows` — Maximum allowed number of rows.
- `int MaxColumns` — Maximum allowed number of columns.

- `bool Center` — Whether the grid should center its content.
- `bool FitContent` — Whether the grid should automatically resize to fit its contents.
- `Vector2 Padding` — Additional padding applied when fitting to content.
- `bool DebugGrid` — Enables debug behavior for testing grid features. When true, everyting that uses `Destroy()` will use `DestroyImmediate()` instead. Automatically reset to false in `Start()`.
- `List<Column> ColumnsList` — List of all columns in the timetable.

**Subclasses**

- **TimetableGrid.Column**

  **Description**<br/>
  Nested class used by TimetableGrid.cs to represent a single column in the timetable.

  **Properties**
  - `bool IsMultirow` — Indicates whether the column spans multiple rows.
  - `List<TimetableCell> Children` — Stores all the cells contained in said column.

**Methods**
```cs
private void Start()
```
Called on the first frame by Unity. Disables `DebugGrid`.

<br/><br/>

```cs
[ContextMenu("Setup Timetable Grid!")]
public void Setup()
```
Clears existing columns and creates `Column` new ones which will contain `Row` of Children.

<br/><br/>

```cs
public void SpawnAddColumnButtons(bool rowspan)
```
Generates UI buttons between columns to allow the user to insert new columns at desired positions.

<br/>

```cs
public void SpawnDeleteColumnButtons()
```
Generates UI buttons on each column that allow the user to delete them.

<br/>

```cs
public void SpawnAddRowButtons()
```
Generates UI buttons between rows to allow the user to insert new rows.

<br/>

```cs
public void SpawnDeleteRowButtons()
```
Generates UI buttons on each row that allow the user to delete them. These buttons are slightly transparent to maintain weekday visibility.

<br/>

```cs
public void DestroyColumnButtons()
```
Destroys all column-related UI buttons.

<br/>

```cs
public void DestroyRowButtons()
```
Destroys all row-related UI buttons.

<br/><br/>

```cs
public void ClearAllCells()
```
Removes all timetable cells.

<br/>

```cs
public void UpdateAllCells()
```
Updates the UI for all cells in the timetable.

<br/>

```cs
void FitToContent()
```
Adjusts the size of the parent object to fit the entire grid. Call this after modifying the row/column count.

<br/><br/>

```cs
public Vector3 GetOffset()
```
Calculates the positional offset needed to center all timetable cells.

<br/>

```cs
public void AddAllOffsets()
```
Applies the calculated offset to all timetable cells.

<br/>

```cs
public void RemoveAllOffsets()
```
Removes the applied offset from all timetable cells — typically done before resizing the grid.

<br/><br/>

```cs
public void UpdateColumnTransform(Column column,int index)
```
Ensures the specified column contains the correct number of cells and aligns them properly.

<br/>

```cs
public void UpdateBreakTransform(int index)
```
Aligns multirow columns properly.

<br/>

```cs
public void UpdateAllTransforms(int start)
```
Updates the position and size of all cells starting from the given index.

<br/><br/>

```cs
public void AddColumn(int columnIndex)
```
Inserts a new column at the given index and updates the UI.

<br/>

```cs
public void AddMultirowColumn(int columnIndex)
```
Inserts a new multirow column at the given index and updates the UI.

<br/>

```cs
public void RemoveColumn(int columnIndex)
```
Deletes a column at the given index and updates the UI.

<br/>

```cs
public void SwapColumns(int IndexA, int IndexB)
```
Swaps the contents of two columns.

<br/>

```cs
public void ReplaceColumnWithMultirowAt(int index)
```
**Used only during save loading**. Replaces the column at the specified index with a multirow column.

<br/><br/>

```cs
public void AddRow(int rowIndex)
```
Inserts a new row at the given index and updates the UI.

<br/>

```cs
public void RemoveRow(int rowIndex)
```
Deletes a row at the given index and updates the UI.

<br/>

```cs
public void SwapRows(int IndexA, int IndexB)
```
Swaps the contents of two rows.

---

#### ScrollZoom.cs

**Description**<br/>
Allows the user to zoom and pan their timetable using mouse wheel scrolling on desktop or pinch gestures on mobile. Supports automatic horizontal or vertical scrolling when dragging near the edges of the viewport, useful for swapping rows or columns.


**Properties**
- `Camera MainCam` — The main camera. Used to calculate mouse position.

- `RectTransform ScrollView` — The scroll view container for detecting mouse position.
- `RectTransform Table` — The timetable UI element to scale (zoom).


- `float MinScale` — Minimum zoom level.
- `float MaxScale` — Maximum zoom level.
- `float ScrollSensitivity` — How quickly zoom changes per scroll or pinch.
- `bool mouseOver` — Tracks if the mouse is currently over the scroll view.


- `ScrollRect ScrollHandler` — The ScrollRect controlling scrolling behavior.
- `RectTransform Viewport` — The viewport RectTransform used for drag position calculations.



- `float DragSpeed` — Speed at which the scroll view auto-scrolls when dragging near edges.


- `Dragging` — Whether the user is currently dragging.
- `DragHorizontal` — Whether dragging is horizontal (`true`) or vertical (`false`).


**Methods**
```cs
public void OnPointerEnter(PointerEventData eventData)
```
Sets `mouseOver` to true when the pointer enters the ScrollView area, enabling zoom input.

<br/>

```cs
public void OnPointerExit(PointerEventData eventData)
```
Sets `mouseOver` to `false` when the pointer exits the ScrollView area, disabling zoom input.

<br/><br/>

```cs
void HandleDrag()
```
If `Dragging` is true, checks mouse position relative to the viewport edges and scrolls the content in the appropriate direction at a speed of `DragSpeed`.

- If `DragHorizontal` is true, scrolls left or right horizontally.
- Otherwise, scrolls up or down vertically.

<br/><br/>

```cs
void HandleScrollZoom()
```
If the mouse is over the ScrollView, zooms the `Table` content in or out based on mouse wheel input.

<br/>

```cs
void HandlePinchZoom()
```
Supports two-finger pinch zoom on touch devices. Calculates the difference between the current and previous distance between two touches to adjust the `Table` scale smoothly.

<br/>

```cs
void ClampZoom()
```
Clamps the Table scale to ensure it remains between MinScale and MaxScale. Called every frame to enforce limits regardless of zoom input.

<br/><br/>

```cs
void Update()
```
Called once per frame by Unity. Handles zooming and dragging behavior based on the current platform.
- Calls HandleDrag() to manage auto-scrolling when dragging near viewport edges.
- Uses platform-specific zoom input:
  - On mobile, runs HandlePinchZoom()
  - On desktop, runs HandleScrollZoom()
- Finally, calls ClampZoom() to limit zoom levels.

---

#### FreezeGrid.cs

**Description**<br/>
Implements Excel-like frozen panes for the timetable grid. Keeps the first row or first column fixed while the user scrolls through the rest of the grid.

**Properties**
- `FreezeModes { FreezeX, FreezeY }` — Specifies the axis to freeze:
  - FreezeX: Freezes the first column (vertical lock).
  - FreezeY: Freezes the first row (horizontal lock).

- `TimetableGrid Timetable` — Reference to the main timetable grid.
- `RectTransform TimetableViewportRect` — The scrollable viewport rect of the timetable.

- `RectTransform SelfRect` — The RectTransform of this frozen element's container.
- `RectTransform Child` — The actual frozen element (e.g., first row/column content).

- `FreezeModes FreezeMode` — Whether this instance freezes X (columns) or Y (rows).

- `Vector2 originalDelta` — The original sizeDelta of SelfRect (cached in `Start()`).
- `Vector2 originalViewportDelta` — The original sizeDelta of the timetable viewport
  (cached in `Start()`).

- `Vector3 WantedChildPos` — Computed world position the child should match.
- `Vector2 WantedChildSizeDelta` — Computed sizeDelta to keep dimensions in sync.
- `Vector3 WantedScale` — Computed scale to sync with the timetable grid.

**Methods**
```cs
void Start()
```
Caches the original sizeDelta values of `SelfRect` and `TimetableViewportRect`.
These baseline values are used to properly recalculate size and positioning during runtime scaling.

<br/>

```cs
void Update()
```
Called once per frame. Dynamically updates the frozen row or column's position, size, and scale to stay aligned with the scrolling and zooming grid.

- If `FreezeMode == FreezeY`:
  - Aligns the child’s **X-position** and **width** with the timetable’s.
  - Maintains its vertical position to simulate a frozen **row**.
  - Resizes and repositions `SelfRect` accordingly.
  - Adjusts the viewport height to maintain layout.
- If `FreezeMode == FreezeX`:
  - Aligns the child’s **Y-position** and **height** with the timetable’s.
  - Maintains its horizontal position to simulate a frozen **column**.
  - Resizes and repositions `SelfRect` accordingly.
  - Adjusts the viewport width to maintain layout.

---

### `Scripts/Time Management`

#### WeekDay.cs
**Description**<br/>
Represents a row in the timetable. A single row can be shared across multiple days of the week.

**Properties**
- `uint Days` — A bitmask representing which days the row is active. Values are read in binary (max 127) to allow multiple day combinations.

- `string DayName` — The label/name of the row.

- `TimeSpan StartTime` — When the row begins.
- `TimeSpan CommonLength` — Default duration for each cell in the row.

- `TimeSpan TempStartTime` — When the row begins temporarily.
- `TimeSpan TempCommonLength` — How long each cell in the row lasts by default temporarily.

- `DateTime OverrideDate` — The date in which the temporary override was added.
- `int OverrideLength` — The default length of the temporary override, calculated
  externally by WeekDayEditor.cs.
- `int OverrideDelayWeeks` — How many weeks until the temporary override starts to apply.
- `int OverrideExtraLengthWeeks` — How many additional weeks until the temporary
  override expires.

- `int OverrideMode` — Controls what gets overridden:
  - `0`: No Override
  - `1`: Override StartTime
  - `2`: Override CommonLength
  - `3`: Override All

**Constructors**
- `WeekDay(TimetableData.WeekDayData data)` — Initializes the row, using the provided `data`. Primarily used when loading a timetable.
- `WeekDay(string _DayName, uint _Days)` — Used to create default rows. Called by `SaveManager.cs` and `DayTimeManager.cs`.

**Methods**
```cs
public void CheckIfTempExpired()
```
Checks whether the temporary override period has expired. If so, it disables the override by resetting `OverrideExtraLengthWeeks` to `-1`.

---

#### WeekDayObject.cs
**Description**<br/>
The UI representation of a WeekDay.

**Properties**
- TMP_Text WeekDayName — Displays the name of the row.
- Button selfButton — Opens the `WeekDayEditor`.

---

#### TimeIndex.cs
**Description**<br/>
Stores configuration for column headers (labels) in the timetable.

**Properties**
- `bool IsCustomLabel` —  Whether the label uses custom text.
- `bool CountAsIndex` —  Whether the label should still be considered an index.
- `string CustomLabelName` — The custom text to display.

---

#### TimeIndexObject.cs
**Description**<br/>
Holds references to the TimeIndex label's UI representation. Modified `DayTimeManager.cs`

**Properties**
- TMP_Text IndexText — Displays the column number.
- TMP_Text TimeText — Displays the associated start time.

- Button button — Opens the `LabelEditor` when clicked.

---

#### DayTimeManager.cs
**Description**<br/>
Handles everything related to displaying and calculating time for a weekly timetable. Manages the current active time cell, supports custom time formats (12h/24h), and updates time-related UI elements in real-time.

**Properties**
- static DayTimeManager instance — Singleton reference to the current DayTimeManager.


- public TimetableGrid Grid — The grid displaying all timetable cells.

- public bool _24hFormat — Whether time is shown in 24-hour format.
- public bool EnglishFormat — Whether time punctuation uses English style (`.` instead of `:`).

- public Toggle _24hToggle — UI toggle to switch 24h display on or off.
- public Toggle EnglishToggle — UI toggle to switch English-style punctuation on or off.

- public GameObject Highlight — The object used to highlight the currently active cell.
- public RectTransform HighlightRect — RectTransform for resizing the highlight.
- public TMP_Text TimeLeftText — Text element displaying time left until the next event.

- public WeekdayEditor WeekdayEditor — UI editor for weekday configurations.
- public WeekDayObject WeekDayPrefab — Prefab used for weekday previews.
- public Transform WeekDaysParent — Parent transform holding weekday preview objects.

- public List<WeekDay> WeekDays — List of up to 7 weekday objects used in the timetable.

- public List<WeekDayObject> WeekDayPreviews — Instantiated previews for editing/visual feedback.



- public LabelEditor labelEditor — Editor for modifying index labels.
- public TimeIndexObject TimeIndexPrefab — Prefab for time/index label display.
- public Transform TimeIndexesParent — Parent transform for time index objects.

- public List<TimeIndexObject> TimeIndexPreviews — Instantiated UI elements for time/index labels.
- public List<TimeIndex> TimeLabels — Logical data structure for column index labels.


- DateTime wantedTime — Tracks when to update real-time data.

**Methods**
```cs
public void Setup()
```
Initializes the manager and clears all current weekday data. Called by `SaveManager.cs`.
<br/>

```cs
private void Update()
```
Runs every frame. Updates time-based highlights and remaining time display every second. Ensures current cell highlight is accurate based on real-world time. Hides highlights when no valid cell is active.

<br/><br/>

```cs
public int GetWeekDayIndex(int day)
```
Returns the index of the `WeekDay` object that contains the specified system day (0 = Sunday, 6 = Saturday). Returns `-1` if no match is found.

<br/>

```cs
public void UpdateWeekDays()
```
Refreshes the UI list of weekday previews based on `WeekDays`. Removes old objects and instantiates new preview buttons for editing.

<br/>

```cs
public void AddNewWeekday(int index)
```
Adds a new blank weekday at the specified index. Updates UI accordingly.

<br/>

```cs
public void RemoveWeekday(int index)
```
Removes the weekday at the specified index and updates UI elements.

<br/>

```cs
public void SwapWeekDays(int IndexA, int IndexB)
```
Swaps two weekdays in the list and updates the preview UI.

<br/><br/>

```cs
public void AddIndexLabel(int index)
```
Inserts a new blank `TimeLabel` (for column indexes) at the given position.

<br/>

```cs
public void RemoveIndexLabel(int index)
```
Removes a time label at the specified index.

<br/>

```cs
public void SwapIndexLabels(int IndexA, int IndexB)
```
Swaps two time/index labels in the list.

<br/><br/>

```cs
public string GetColumnIndexAt(int index)
```
Returns the index label (custom or default numeric) for a given column. Returns empty string if the cell is empty.

<br/><br/>

```cs
public TimeSpan GetCellCommonLength(int weekday)
```
Returns the default duration of a single cell for the specified weekday.

<br/>

```cs
public TimeSpan GetCellStartTime(int col, int weekday)
```
Calculates the start time of a specific cell by summing all durations before it, accounting for overrides.

<br/>

```cs
public TimeSpan TimeDiff(TimeSpan newStartTime, int col, int weekday)
```
Calculates the difference between a new start time and the actual start time of a given cell.

<br/><br/>

```cs
public CellInfo GetCurrentCellInfo(int weekdayindex, out TimeSpan diff)
```
Returns the current active `CellInfo` object for the given day index. Outputs the time remaining until the end of the event.

<br/>

```cs
public bool IsCellEmpty(int col, int weekday)
```
Checks if a cell is empty (no base or override event info present).

<br/><br/>

```cs
public void HideHighlights()
```
Disables the highlight and clears the countdown text.

<br/>

```cs
public void UpdateTimeIndexes()
```
Updates and redraws the time/index labels above the timetable columns. Ensures consistency between labels and columns.

<br/>

```cs
public string FormatTime(TimeSpan t)
```
Returns a formatted string of a time span based on 24h/12h settings and English punctuation.

<br/>

```cs
public static bool TryParseTime(string text, out DateTime result)
```
Attempts to parse a time string into a `DateTime` using multiple 12h/24h format options. Returns success state.

<br/>

```cs
public static bool TryParseLength(string hours, string minutes, out TimeSpan result)
```
Parses hour and minute strings into a valid TimeSpan, capping to max 23:59. Returns true if successful.

<br/><br/>

```cs
public void Set24h(bool is24)
```
Sets the 24-hour format preference and refreshes saved settings and labels.

<br/>

```cs
public void SetEnglish(bool english)
```
Sets English punctuation (colon/dot) and refreshes saved settings and labels.

---

### `Scripts/Drag N Swap`

#### DragHandle.cs

**Description**<br/>
A base class for draggable UI elements that support horizontal or vertical swapping within a UI grid. Implements drag functionality and swap logic, to be extended by specific drag behaviors.

**Properties**
- `DragHandleManager.SwapAxis SwapAxis` — The axis on which this handle is allowed to move (`Horizontal` or `Vertical`).
- `Vector3 startPos` — The starting local position of the drag handle (used to reset position on drag end).
- `int currIndex` — The current index of the handle in the list it's part of (used for determining swap position).

**Methods**
```cs
public void OnDrag(PointerEventData eventData)
```
Handles the movement of the drag handle during a drag gesture. Determines if a swap should be triggered based on the current and closest index.

<br/>

```cs
public void OnEndDrag(PointerEventData eventData)
```
Resets the drag handle back to its starting position and disables drag tracking.

<br/><br/>

```cs
public virtual void OnSwapDragged(int IndexA, int IndexB)
```
Called when a swap occurs between two indices. Can be overridden to perform custom logic (e.g. updating data).

<br/>

```cs
public virtual void OnSwap()
```
Used to update the visual state or text of the drag handle based on the current index.

---

#### TimeIndexDrag.cs

**Description**<br/>
Specialized `DragHandle` for horizontal handles representing time index columns in a grid. Updates the timetable column order and label during a swap.

**Properties**
- `TMP_Text IndexText` — The text UI element that displays the current column label.

**Methods**
```cs
public override void OnSwapDragged(int IndexA, int IndexB)
```
Swaps the corresponding columns in the timetable grid.

<br/>

```cs
public override void OnSwap()
```
Updates the label text of the time index using the current index.

---

#### WeekDayDrag.cs

**Description**<br/>
Specialized `DragHandle` for vertical handles representing weekdays in a timetable grid. Handles row swaps and updates the weekday label accordingly.

**Properties**
- `TMP_Text WeekDayText` — The text UI element that displays the weekday name.

**Methods**
```cs
public override void OnSwapDragged(int IndexA, int IndexB)
```
Swaps the corresponding rows in the timetable grid.

<br/>

```cs
public override void OnSwap()
```
Updates the label text to match the weekday name of the current index.

---

#### DragHandleManager.cs

**Description**<br/>
Central controller that manages drag-and-drop handle instantiation, swapping logic, UI canvas state toggling, and layout updates for both horizontal (columns) and vertical (rows) axes.

**Properties**
- `static DragHandleManager instance` — Singleton instance for global access.
- `enum SwapAxis {Horizontal, Vertical}` — Enum representing drag axis directions.
- `ScrollZoom ScrollViewManager` — Reference to the scroll view used for tracking drag activity.
- `DragHandle HorizontalDragPrefab` — Prefab for handles used to drag columns (horizontal).
- `DragHandle VerticalDragPrefab` — Prefab for handles used to drag rows (vertical).

- `CanvasGroup DaysOfWeekParent` — CanvasGroup for displaying the weekday labels.
- `CanvasGroup TimeIndexesParent` — CanvasGroup for displaying the time index labels.

- `CanvasGroup DaysOfWeekDRAGParent` — CanvasGroup that hosts weekday drag handles.
- `CanvasGroup TimeIndexesDRAGParent` — CanvasGroup that hosts time index drag handles.

- `Transform HorizontalParent` — Parent transform for horizontal drag handles.
- `Transform VerticalParent` — Parent transform for vertical drag handles.

- `CustomLayoutGroup HorizontalLayout` — Layout component for horizontal drag handles.
- `CustomLayoutGroup VerticalLayout` — Layout component for vertical drag handles.

- `List<DragHandle> HandlesVertical` — List of all currently active vertical drag handles.
- `List<DragHandle> HandlesHorizontal` — List of all currently active horizontal drag handles.

- `List<RectTransform> objects` — Miscellaneous tracked RectTransforms.

**Methods**
```cs
private void Awake()
```
Initializes the singleton instance.

<br/><br/>

```cs
public void StartSwap(bool horizontal)
```
Initializes the drag handles for either horizontal (columns) or vertical (rows) swapping. Disables interaction with static UI and prepares the layout for drag interaction.

<br/>

```cs
public void EndSwap()
```
Ends the swapping operation. Cleans up drag handles and restores static UI interactivity and visibility.

<br/><br/>

```cs
public void SwapHorizontal(int IndexA, int IndexB)
```
Swaps the positions of two horizontal handles and updates their start positions and indices.

<br/>

```cs
public void SwapVertical(int IndexA, int IndexB)
```
Swaps the positions of two vertical handles and updates their start positions and indices.

<br/><br/>

```cs
public int GetClosestIndex(Vector3 dragPos, SwapAxis axis)
```
Calculates the closest index in the list of drag handles based on the current drag position and axis.

---

### `Scripts/Help Tab`

#### GIFObject.cs

**Description**<br/>
A lightweight MonoBehaviour container used to display a video clip (originally a GIF) using a `RawImage`. Serves as the display surface for animated help content.

**Properties**
- `RawImage RawSelf` — The UI element used to display the rendered video frame.
- `VideoClip Clip` — he video clip (in .mp4 format) that will be shown as an animated help "GIF".

---

#### GIFHandler.cs

**Description**<br/>
Controls playback of help section videos in a performant way. Because Unity does not natively support GIFs, .mp4 clips are used. The system ensures that only videos currently visible in the viewport are played. Up to three simultaneous video players are supported to manage rendering efficiently.

**Properties**
- `RectTransform viewport` — The visible region of the scrollable UI where the GIFObjects are checked for visibility.

- `VideoPlayer[] videoPlayers` — Pool of reusable `VideoPlayer` components used to play videos.
- `List<GIFObject> GIFObjects` — List of all GIF objects that may be shown on screen.

- `UnityEvent[] onVideoPrepareEvents` — Internal array of UnityEvents to manage player ready callbacks.

- `Dictionary<GIFObject, VideoPlayer> ActiveGIFs` — Tracks currently playing GIFObjects and their associated `VideoPlayer`.

**Methods**
```cs
private void Start()
```
Initializes the event listeners for all `VideoPlayer` instances so they can trigger playback once a video is prepared.

<br/>

```cs
void Update()
```
Called once per frame. Checks which `GIFObjects` are visible in the `viewport`, stops playback and clears resources for out-of-view clips, and assigns videos to those now visible.

<br/><br/>

```cs
public bool VisibleGIF(RectTransform child)
```
Determines if a given `RectTransform` (child) is currently within the vertical bounds of the `viewport`.

<br/>

```cs
void AssignVideoToGIF(GIFObject gif)
```
Assigns an available `VideoPlayer` to the specified `GIFObject`. Prepares and plays the video, and assigns the output to the object's `RawImage`.

---

#### ContentButton.cs

**Description**<br/>
A simple component that represents a clickable UI button within the help section's table of contents. Displays a text label and handles user interaction to trigger navigation.

**Properties**
- `RectTransform selfRect` — The UI transform used for positioning and sizing the button within the layout.

- `Button button` — The underlying Unity UI button component that detects and handles user clicks.
- `TMP_Text text` — The text label displaying the title and format of the header (H1, H2, H3) in the table of contents.

---

#### TableOfContents.cs

**Description**<br/>
Dynamically generates a navigable table of contents UI based on provided headers. Each header becomes a button, which scrolls the help section to the corresponding content. Supports different formatting levels and customizable layout settings.

**Properties**
- `RectTransform SelfRect` — The root UI element that contains all the content buttons and defines the overall bounds of the table of contents.

- `ContentButton TabPrefab` — The prefab used to instantiate each button in the table of contents.
- `HelpSection helpSection` — The component that manages scrolling to a specific section of the help content when a button is clicked.


- `int H1Size` — The text size percentage for first-level headers (e.g., section titles).
- `int H2Size` — The text size percentage for second-level headers (e.g., subsections).
- `int H3Size` — The text size percentage for third-level headers (e.g., nested items).
- `float Spacing` — The vertical spacing between buttons in the layout.
- `bool CenterChildren` — Whether to vertically center all child buttons within the container.

- `List<HeaderShortcut> headers` — The list of headers to include in the table of contents, each paired with a UI target.
- `List<ContentButton> buttons` — Internally managed list of instantiated content buttons corresponding to the headers.

**Structs**
- **HelpSection.HeaderShortcut**
  
  **Description**<br/>
  A lightweight struct that links a header's name, level, and its associated UI element to scroll to.

  **Properties**
  - `string name` — The display name of the header.
  - `int level` — The nesting level (0 = top-level, 1 = sub, 2+ = nested).
  - `RectTransform wantedChild` — The UI transform to scroll to when the header is clicked.

  **Constructors**
  - `public HeaderShortcut(string _name, int _level, RectTransform _child)` — Creates a new header shortcut with the given name, level, and associated UI element.

**Methods**
```cs
public void Setup()
```
Builds the table of contents by instantiating buttons for each header in `headers`. Applies visual formatting based on header level and assigns click listeners to scroll to the correct section.

<br/>

```cs
public void UpdateLayout()
```
Triggers a coroutine that recalculates and repositions all buttons in the layout to ensure proper spacing and alignment.

<br/>

```cs
public void DelayedUpdateLayout()
```
Calculates the vertical positioning of each button in the list and optionally centers them in the container. Also adjusts the container's height to fit the content.

<br/>

```cs
IEnumerator Wait()
```
Waits until the end of the current frame to ensure the canvas layout is updated before recalculating the button positions. Prevents layout inconsistencies.

---

#### HelpLayoutGroup.cs

**Description**<br/>
A custom layout group that vertically arranges UI elements (RectTransforms) with consistent spacing and optional horizontal centering. Designed for dynamic or manually managed children where Unity's built-in layout components fall short.

**Properties**
- `RectTransform SelfRect` — The main RectTransform representing this layout group.

- `float Spacing` — Vertical space between each child element.
- `float PaddingX` — Extra horizontal padding applied when child width is smaller than the layout group.
- `bool CenterY` — If true, vertically centers all children within the group.


- `List<RectTransform> children` — A manually managed list of child RectTransforms to layout.

**Methods**
```cs
public void UpdateLayout()
```
Starts the coroutine that will update the layout on the next frame, after canvas updates.

<br/>

```cs
IEnumerator Wait()
```
Waits until the end of frame to ensure all UI changes are complete, then triggers the actual layout logic.

<br/>

```cs
public void DelayedUpdateLayout()
```
Performs the layout update: positions children vertically with spacing, optionally centers them horizontally and vertically, and resizes the layout group to fit content height.

---

#### HelpSection.cs

**Description**<br/>
A modular help/documentation renderer that parses lightweight markdown-like text into styled Unity UI elements. Supports custom headers, text formatting, inline media, and a dynamic table of contents. Acts as the central controller for rendering and managing help content.

**Properties**
- `bool ShouldSetup` — Determines if the help section should be re-rendered (usually only true once).


- `TableOfContents tableOfContents` — The reference to the table of contents manager that mirrors the document structure.

- `TMP_Text H1Text` — Preset template for top-level headers (`# Header`).
- `TMP_Text H2Text` — Preset template for second-level headers (`## Header`).
- `TMP_Text H3Text` — Preset template for third-level headers (`### Header`).

- `TMP_Text NormalText` — Preset template for normal body text.


- `Image UIImage` — Template used to instantiate static images.
- `GIFObject GifImage` — Template used to instantiate animated GIFs.

- `HelpLayoutGroup ContentLayoutGroup` — Custom layout group handling vertical spacing and alignment.
- `ScrollRect scrollRect` — The main scrollable area for the help content.
- `GIFHandler GIFHandler` — Manages playing/stopping of visible GIFs.

- `GIF[] gifs` — Predefined collection of named GIF assets.
- `IMG[] images` — Predefined collection of named image assets.
- `OBJ[] objects` — Predefined collection of named UI prefab objects.


- `List<GameObject> SpawnedObjects` — List of all dynamically created UI elements (for cleanup/resetting).


**Subclasses**
- **HelpSection.HelpMedia**
  
  **Description**<br/>
  Base class for media objects that can be referenced by name.

  **Properties**
  - `string name` — The unique identifier for the media element.



- **HelpSection.GIF**
  
  **Description**<br/>
  Represents a video-based animated GIF element.

  **Properties**
  - `float PixelsPerUnit` — The scaling factor used to size the GIF based on resolution.
  - `VideoClip GIFClip` — The video file used as the animated content.



- **HelpSection.IMG**
  
  **Description**<br/>
  Represents a static image asset.

  **Properties**
  - `Sprite IMGSprite` — The actual sprite used in the UI.



- **HelpSection.OBJ**
  
  **Description**<br/>
  Represents a reusable UI prefab object.

  **Properties**
  - `RectTransform UIObject` — The prefab object’s RectTransform to instantiate.



**Methods**
```cs
GIF GetGIF(string name)
```
Retrieves a `GIF` asset by its name. Logs a warning if the name does not exist.

<br/>

```cs
IMG GetIMG(string name)
```
Retrieves an `IMG` asset by its name. Logs a warning if the name does not exist.

<br/>

```cs
OBJ GetOBJ(string name)
```
Retrieves an `OBJ` asset by its name. Logs a warning if the name does not exist.

<br/><br/>

```cs
public void Setup()
```
Parses and renders the help text using lightweight markdown. Dynamically spawns header and body elements, inserts images, GIFs, and objects, populates the Table of Contents, and applies the current theme.

<br/>

```cs
public void ScrollToTarget(RectTransform target)
```
Scrolls the `ScrollRect` to the specified target element, centering it vertically in the viewport.

---

### `Scripts/Stylizing`

#### ColorStylePreset.cs

**Description**<br/>
Stores color palette data. Used by `ColorStylizer.cs`.

**Properties**
- `string PaletteName` — Name of the theme/preset.

- `Color PrimaryColor ` — Color for UI buttons.
- `Color SecondaryColor` — Color for UI backgrounds and text.
- `Color BackgroundColor` — Background color for the camera.

- `bool IsCustomPreset` — Whether the preset was user-created.

**Constructors**
- `public ColorStylePreset(SettingsData.CustomThemeData themeData)` — Constructs a custom preset using external data.
- `public ColorStylePreset()` — Default constructor with white colors and empty name.

---

#### ColorStylizer.cs

**Description**<br/>
Applies selected color presets (themes) to various UI and scene elements such as the camera background, buttons, and text. Can manage, delete, and apply both built-in and user-defined color themes.

**Properties**
- `int wantedPreset` — Index of the currently selected color preset.
- `PaletteDropdown paletteDropdown` — UI component used to display preset options.
- `List<ColorStylePreset> ColorStyles` — Available color presets.
- `Camera Camera` — Main camera to apply background color.
- `Image[] Backgrounds` — UI background elements to recolor.
- `Image[] Buttons` — Button elements to recolor.
- `TMP_Text[] Texts` — Text elements to recolor.

**Methods**
```cs
void Setup()
```
Initializes the stylizer by updating dropdown and applying the current theme.

<br/>

```cs
int GetIndex(ColorStylePreset preset)
```
Returns the index of a given preset in the list, or -1 if not found.

<br/>

```cs
void DeleteStyle(int index)
```
Removes a preset and adjusts the currently selected index as needed.

<br/><br/>

```cs
void ChangePreset(int index)
```
Sets the desired preset and saves the current settings.

<br/>

```cs
void GetElements()
```
Finds and stores all GameObjects tagged as `Styled/Button`, `Styled/Background`, or `Styled/Text`. Also temporarily activates inactive objects to ensure detection.

<br/>

```cs
void UpdateDropdown()
```
Refreshes the dropdown UI with the current list of presets and updates element references.

<br/><br/>

```cs
void ApplyCurrentTheme()
```
Applies the currently selected color preset to all relevant UI elements and camera.

<br/>

```cs
int CountBuiltInThemes()
```
Returns the number of built-in (non-custom) presets.

---

#### PaletteObject.cs

**Description**<br/>
A visual and functional representation of a color preset inside the UI. Acts like a toggle button and is instantiated by PaletteDropdown.

**Properties**
- `PaletteDropdown paletteDropdown` — Reference to the parent dropdown for callback.
- `int paletteIndex` — Index of the corresponding color preset.
- `Toggle toggle` — UI toggle component.


- `TMP_Text PaletteNameText` — Label displaying the preset's name.
- `Image BackgroundColorImage ` — Swatch showing background color.
- `Image SecondaryColorImag e` — Swatch showing secondary color.
- `Image PrimaryColorImage` — Swatch showing primary color.

**Methods**
```cs
public void SetPaletteDropdown(bool isOn)
```
Called when toggled on. Changes the selected palette.
If toggled on, sets this object as the selected preset in the dropdown.

---

#### PaletteDropdown.cs

**Description**<br/>
Controls the UI for selecting and displaying available color themes. Dynamically creates `PaletteObject` instances based on the `ColorStylePreset` list and keeps everything in sync.

**Properties**
- `PaletteObject Template` — The prefab used to create palette entries.
- `Transform PalettesParent` — The container that holds the instantiated 
  palette entries.
- `ColorStylizer Stylizer` — Reference to the script managing actual color application.

- `UnityEvent<int> onValueChanged` — Event triggered when a new palette is selected.
- `int value` — Current index of the selected preset.
- `List<PaletteObject> paletteChildren` — All currently instantiated palette buttons.

**Methods**
```cs
void Start()
```
Hides the template on initialization.

<br/>

```cs
public void Setup(ColorStylePreset[] presets)
```
Clears the dropdown, instantiates one `PaletteObject` per preset, and initializes them with color and name data.

<br/><br/>

```cs
public void ChangeValue(int newvalue)
```
Changes the selected preset, updates toggle states, and triggers `onValueChanged`.

<br/>

```cs
public void SetValueWithoutNotify(int newvalue)
```
Changes the preset index and toggle states **without** triggering the change event (useful during initialization).

---

#### CustomPaletteLister.cs

**Description**<br/>
Displays a list of custom color palettes (user-created). Instantiates PaletteObject buttons and opens the editor when clicked.

**Properties**
- `PaletteCreator PaletteCreator` — Reference to the editor UI used when modifying a palette.
- `PaletteObject Template` — Button prefab used to represent a palette visually.
- `Transform PalettesParent` — Parent transform that holds all instantiated palette buttons.
- `ColorStylizer Stylizer` — Core style manager that contains all color presets.
- `GameObject MessageIfNoThemes` — UI element shown if no custom themes are available.


**Methods**
```cs
public void Setup(ColorStylePreset[] presets)
```
Clears the list and recreates entries based on provided presets.

<br/>

```cs
public void AddCustomPalettes()
```
Filters the global preset list and adds only custom ones to the UI.

<br/>

```cs
public IEnumerator RefreshMessage()
```
Toggles a message if there are no custom themes (runs after layout update).

---

#### PaletteCreator.cs

**Description**<br/>
Handles creation, editing, and deletion of custom color palettes. Provides a UI for naming the palette and changing its colors.

**Properties**
- `int IDToModify` — Index of the palette being modified; `-1` if creating new.
- `int ColorToChange` — Index representing which color is currently being edited (0: Background, 1: Secondary, 2: Primary).


- `ColorStylePreset ThemeDefaults` — Default values when creating a new palette.


- `ColorStylizer Stylizer` — Reference to the global style manager.
- `CustomPaletteLister CustomPaletteLister` — UI component that lists
  all custom palettes.


- `PaletteObject PalettePreview` — Visual preview of the palette inside the editor.

- `TMP_Text TitleText` — Header showing whether user is editing or creating.


- `TMP_InputField PaletteNameInput` — Input field for naming the palette.


- `Image PrimaryColorImage`, `SecondaryColorImage`, `BackgroundColorImage` — UI color fields.
- `Button DeleteButton` — Deletes the current palette.

**Methods**
```cs
public void OpenCreator(int ID)
```
Opens the editor with either a new or existing palette loaded.

<br/>

```cs
public void CloseCreator()
```
Closes the editor and reapplies any live preview changes.

<br/><br/>

```cs
public void ActivateColorEditor(int colorToChange)
```
Opens the color editor for the selected color field.

<br/>

```cs
public void ChangePaletteName(string name)
```
Updates the preview label text as the user types a name.

<br/><br/>

```cs
public void Delete(bool confirm)
```
Prompts user to confirm and deletes the palette if confirmed.

<br/>

```cs
public void Confirm()
```
Saves changes to an existing palette or adds a new custom palette to the list. Refreshes the stylizer and saves to disk.

---

### `Scripts/Editors`

#### TimetableEditor.cs

**Description**<br/>
Handles editing operations on a timetable, including selecting events, assigning them to cells, changing timetable shape, and switching between edit/view modes. Manages UI visibility and input state across modes.

**Properties**
- `static TimetableEditor instance` — Singleton reference to the active instance of the editor.

- `bool Editing` — Indicates whether the timetable is currently in editing mode.
- `int SelectedID` — The currently selected event ID to assign to timetable cells.


- `DayTimeManager dayTimeManager` — Reference to the system managing days and time index previews.
- `TimetableGrid Grid` — Reference to the timetable grid that holds and organizes cells.


- `TimetableCell SelectedCellPreview` — UI element previewing the selected event's details.


- `TMP_Text TimetableNameText` — Text component displaying the timetable's name.
- `TMP_InputField TimetableNameInput` — Input field used to edit the timetable name in edit mode.


- `GameObject EventSelectorOverlay` — Overlay UI that displays event selection options.
- `Button SelectorCancelButton` — Button to cancel event selection.


- `Button TableDoneButton` — Button shown while editing timetable shape; exits table edit mode when clicked.

- `GameObject[] OtherButtons` — General-purpose buttons hidden while in edit mode.
- `CanvasGroup[] OtherGroups` — CanvasGroups that are disabled while editing.
- `GameObject[] EditorButtons` — Buttons shown only while editing.

**Methods**
```cs
private void Awake()
```
Initializes the singleton instance.

<br/><br/>

```cs
public void Setup()
```
Initializes the timetable editor. Resets selection, ends any active edit or table edit mode.

<br/><br/>

```cs
public void SelectEvent(int ID)
```
Selects an event by ID for cell assignment and hides the event selector overlay.

<br/>

```cs
public void UpdateSelectorPreview()
```
Updates the selected cell preview UI with the name, info, and style of the currently selected event.

<br/>

```cs
public void SetTimetableName(string text)
```
Sets the visible name of the timetable via the title `TMP_Text`.

<br/><br/>

```cs
public void BeginEditMode()
```
Enables editing mode. Hides general UI, enables input fields and editor-only buttons, and prepares cells for quick assignment.

<br/>

```cs
public void EndEditMode()
```
Disables editing mode. Restores regular UI and binds cells to open the manual editor when clicked.

<br/><br/>

```cs
public void BeginEditTable()
```
Prepares the timetable for row/column modification. Disables interactions and shows the "Done" button.

<br/>

```cs
public void EndEditTable()
```
Ends table shape editing mode. Restores interactions, destroys editing buttons, and re-binds cells for quick assignment.

<br/><br/>

```cs
public void BindCellsForQuickAssign()
```
Binds all timetable cells to assign the currently selected event with a single click.

<br/>

```cs
public void BindCellsForManualAssign()
```
Binds all timetable cells to open the manual cell editor (for detailed editing) on click.

---

#### CellInfoEditor.cs

**Description**<br/>
UI controller for editing cell information in a timetable, including both permanent overrides and temporary event modifications. Handles input bindings, UI state updates, and preview rendering.

**Properties**
- `int originalEvent` — Stores the ID of the base event originally associated with the selected cell.
- `int originalTempEvent` — Stores the ID of the temporary event originally associated with the selected cell.

- `int SelectedCellColumn` — The column index of the selected cell in the timetable grid.
- `int SelectedCellRow` — The row index of the selected cell in the timetable grid.



- `TimetableCell MainPreview` — Preview UI component representing the final computed cell state after overrides.
- `TimetableCell BasePreview` — Preview UI component showing the base event without overrides.


- `TMP_InputField EventNameOverride` — Input field for overriding the base event’s name.
- `TMP_InputField Info1Override` — Input field for overriding the base event’s first line of info.
- `TMP_InputField Info2Override` — Input field for overriding the base event’s second line of info.
- `TMP_Dropdown TypeOverride` — Dropdown for selecting the base event's overridden type.
- `TMP_Dropdown FavouriteOverride` — Dropdown for overriding the event's "favourite" status.

- `Toggle OverrideTimeToggle` — Toggles whether the base event time is overridden.
- `TMP_InputField StartTimeInput` — Input field for overriding the base event start time.
- `TMP_InputField LengthInputHours` — Input field for the hour component of the base event’s override length.
- `TMP_InputField LengthInputMinutes` — Input field for the minute component of the base event’s override length.




- `TabHandler tabs` — Handles switching between permanent and temporary override UI tabs.
- `GameObject CreateButton` — Button to create a temporary override.
- `GameObject DeleteButton` — Button to delete an existing temporary override.
- `GameObject ErrorText` — Warning UI element shown when editing an invalid day (e.g., zero active days).

- `GameObject TempPropertiesLayout` — Container for the temporary override UI.
- `GameObject TempPromptLayout` — Container for the "create temp override" prompt.
- `TimetableCell TempBasePreview` — Preview UI showing how the temporary override base looks.



- `Slider DelaySlider` — Slider for the number of weeks before the temporary override starts.
- `Slider LengthSlider` — Slider for the number of weeks the temporary override lasts.
- `TMP_InputField DelayInput` — Input field mirroring the delay slider value.
- `TMP_InputField LengthInput` — Input field mirroring the length slider value.

- `TMP_InputField TempEventNameOverride` — Input field for overriding the temporary event’s name.
- `TMP_InputField TempInfo1Override` — Input field for overriding the temporary event’s first line of info.
- `TMP_InputField TempInfo2Override` — Input field for overriding the temporary event’s second line of info.
- `TMP_Dropdown TempTypeOverride` — Dropdown for selecting the temporary event's type override.
- `TMP_Dropdown TempFavouriteOverride` — Dropdown for overriding the temporary event’s favourite status.

- `Toggle TempOverrideTimeToggle` — Toggles whether the temporary event has a custom time.
- `TMP_InputField TempStartTimeInput` — Input field for overriding the temporary event's start time.
- `TMP_InputField TempLengthInputHours` — Input for the hour portion of the temporary event’s length.
- `TMP_InputField TempLengthInputMinutes` — Input for the minute portion of the temporary event’s length.

- `DateTime OverrideDate` — Stores the date on which the override was created.


**Methods**
```cs
public void SelectCell(int column, int row)
```
Loads cell information from the grid, applies it to the UI, and determines whether to show temporary override controls or not. Initializes dropdowns, fields, toggles, and previews.

<br/>

```cs
public CellInfo GetSelectedInfo()
```
Returns the `CellInfo` of the currently selected cell in the timetable grid.

<br/><br/>

```cs
public void CreateTempOverride()
```
Initializes a new temporary override for the selected cell. Activates the proper UI layout, resets fields to defaults, and updates previews.

<br/>

```cs
public void DeleteTempOverride()
```
Removes the temporary override from the currently selected cell and reverts the UI layout. Updates the cell preview.

<br/><br/>

```cs
public void ParseStartTime(string text)
```
Validates and parses the start time input. If the time is invalid or earlier than the previous column's time, reverts to the original start time for the cell.

<br/>

```cs
public void ParseMinutes(string text)
```
Parses the input string as minutes. Caps it at 59 and assigns it to either the base or temp minutes field depending on layout state. Falls back to common length if invalid.

<br/>

```cs
public void ParseHours(string text)
```
Parses the input string as hours. Caps it at 23 and assigns it appropriately. Falls back to the default cell length if parsing fails.

<br/><br/>

```cs
public void ChangeInfoBase(int EventID)
```
Sets the selected event base for the current cell (either base or temp) and refreshes the UI. Closes the event selector overlay.

<br/>

```cs
public void ChangeInfoBase(int EventID, int TempEventID)
```
Sets both the base and temporary event IDs for the current cell and updates the UI accordingly. Also closes the event selector overlay.

<br/><br/>

```cs
public void ToggleOverrideTime(bool overridetime)
```
Toggles the override time input interactability based on the context (permanent vs temporary override). Syncs the toggle state and field interactability.

<br/><br/>

```cs
public void SetDelay(float Delay)
```
Updates the delay input field value based on the delay slider or value provided.

<br/>

```cs
public void UpdateDelaySlider(string Delay)
```
Parses and clamps the delay value, updating the delay slider accordingly. Defaults to 0 if parsing fails.

<br/>

```cs
public void SetLength(float Length)
```
Updates the length input field with the given value.

<br/>

```cs
public void UpdateLengthSlider(string Length)
```
Parses and clamps the length value, updating the length slider accordingly. Defaults to 0 on failure.

<br/><br/>

```cs
public void UpdatePreviews()
```
Updates all preview cells (`BasePreview`, `TempBasePreview`, and `MainPreview`) with the latest selected or overridden data. Applies fallback coloring and visibility based on whether temporary overrides are active. Reflects override fields if filled.

<br/><br/>

```cs
public void Cancel()
```
Cancels current edits by restoring original event and temporary event IDs. Hides the editor UI.

<br/>

```cs
public void Confirm()
```
Commits all changes made in the editor UI to the selected `CellInfo` object.

This includes:

- Setting permanent overrides for event name, info fields, event type, favourite state.

- Applying manual length overrides (hours/minutes) if enabled.

- Parsing and storing a custom start time; when editing the first column, it sets the weekday's start time instead.

- If editing a multi-row cell, updates preceding column entries to ensure proper duration tracking.

- Handles temporary overrides similarly for alternate event data (e.g., temporary name, type, favourite).

- Computes the difference in days between the override date and the cell's target weekday for scheduling offset.

- Applies temporary length and time overrides.

- If applicable, sets override values on multi-row entries for the previous column.

- Updates the state of the DayTimeManager and marks the UI and SaveManager for refresh.

And finally:

- Resets the selected cell indexes and override date.

- Deactivates the editor UI.

---

#### WeekdayEditor.cs

**Description**<br/>
Handles the UI and logic for editing weekly schedule entries, including their name, days, time slots, and temporary override configurations.

**Properties**
- `DateTime OverrideDate` — The date on which a temporary override becomes active.
- `int WeekdayIndex` — The index of the currently edited weekday in the WeekDays list.

- `WeekDayObject WeekdayPreview` — A visual preview element displaying the weekday's name and style.
- `TMP_InputField WeekdayName` — Input field for editing the weekday's name.
- `TMP_InputField StartTimeField` — Input field for the base start time of the weekday.
- `TMP_InputField CommonLengthFieldHours` — Input field for the hour component of the base length.
- `TMP_InputField CommonLengthFieldMinutes` — Input field for the minute component of the base length.
- `Toggle[] DayToggles` — Array of toggles representing selectable days of the week (Mon–Sun).


- `TabHandler TabHandler` — Manages tab selection between base and override editing modes.

- `GameObject Create` — UI group for creating a new override.
- `GameObject CreateButton` — Button that creates a temporary override.
- `GameObject ErrorText` — Error message shown when no days are selected on temporary override tab.
- `GameObject Override` — UI group for modifying an existing temporary override.
- `GameObject DeleteOverrideButton` — Button to remove an existing temporary override.

- `Slider DelaySlider` — Slider controlling the delay (in weeks) before the temporary override starts.
- `Slider LengthSlider` — Slider controlling the length (in weeks) of the temporary override.
- `TMP_InputField DelayInput` — Input field controlling the delay (in weeks) before the temporary override starts.
- `TMP_InputField LengthInput` — Input field controlling the length (in weeks) of the temporary override.

- `Toggle TempStartTimeToggle` — Toggles temporary override of start time.
- `TMP_InputField TempStartTimeField` — Input field for temporary override start time.

- `Toggle TempCommonLengthToggle` — Toggles temporary override of common length.
- `TMP_InputField TempCommonLengthFieldHours` — Input field for temporary override hours.
- `TMP_InputField TempCommonLengthFieldMinutes` — Input field for temporary override minutes.

**Methods**
```cs
public void OpenWeekday(int index)
```
Initializes the editor UI for the selected weekday and populates all fields.

<br/><br/>

```cs
public void ParseStartTime(string text)
```
Validates and parses the input for start time; reverts on invalid input.

<br/>

```cs
public void ParseMinutes(string text)
```
Validates and parses the minute field; clamps values to 0–59.

<br/>

```cs
public void ParseHours(string text)
```
Validates and parses the hour field; clamps values to 0–23.

<br/><br/>

```cs
public void UpdateWeekdayName(string text)
```
Updates the live preview of the weekday name in the UI.

<br/>

```cs
public void SetDelay(float text)
```
Updates the delay input field when the slider is moved.

<br/>

```cs
public void SetLength(float text)
```
Updates the length input field when the slider is moved.

<br/><br/>

```cs
public void UpdateDelaySlider(string Delay)
```
Parses and applies the delay input value to the slider.

<br/>

```cs
public void UpdateLengthSlider(string Length)
```
Parses and applies the length input value to the slider.

<br/>

```cs
public void UpdateCreateButton()
```
Toggles visibility of the create button and error text based on day selection.

<br/><br/>

```cs
public void SetStartTimeInteractable(bool enabled)
```
Enables or disables the override start time input field.

<br/>

```cs
public void SetCommonLengthInteractable(bool enabled)
```
Enables or disables the override length input fields.

<br/><br/>

```cs
public void CreateTemp()
```
Initializes a temporary override for the current weekday.

<br/>

```cs
public void DeleteTemp()
```
Removes the temporary override from the current weekday.

<br/><br/>

```cs
public void Confirm()
```
Applies all changes to the weekday object and closes the editor.

---

#### LabelEditor.cs

**Description**<br/>
Manages the UI and logic for editing time label properties, including custom naming and index inclusion for schedule visualization.

**Properties**
- `int objectToModify` — The index of the time label currently being edited.

- `TimeIndexObject TimeIndexPreview` — UI preview of the time label, showing label name and time.


- `Toggle CustomLabelToggle` — Enables or disables the use of a custom label name.
- `TMP_InputField CustomLabelInput` — Input field for entering a custom label.

- `Toggle CountAsIndexToggle` — Determines whether the custom label should be considered a valid time index in logic.

**Methods**
```cs
public void ActivateEditor(int objToEdit)
```
Initializes the editor with the data from the selected time label and populates UI fields.

<br/><br/>

```cs
public void IsCustomLabel(bool isTrue)
```
Toggles the interactivity of custom label fields based on whether a custom label is being used.

<br/>

```cs
public void SetCustomLabel(string text)
```
Updates the label preview in real time as the user types.

<br/><br/>

```cs
public void Confirm()
```
Saves changes to the selected label and updates all time indexes.

---

#### ColorEditor.cs

**Description**<br/>

**Properties**
- `static ColorEditor instance` — Singleton instance of the ColorEditor.

- `string oldPrompt` — Stores the last used prompt to detect editor reuse.
- `Color CurrentColor` — The color currently being edited.
- `Color PreviousColor` — The original color before editing started, used for cancellation.


- `CanvasGroup SelfGroup` — Controls the visibility and interactivity of the color editor UI.

- `Image[] imageRefs` — Array of images to apply the selected color to.
- `TMP_Text[] textRefs` — Array of text elements to apply the selected color to.


- `Slider HueSlider` — Slider for adjusting the hue component of the color.
- `Slider SaturationSlider` — Slider for adjusting saturation.
- `Slider ValueSlider` — Slider for adjusting brightness (value).

- `Slider AlphaSlider` — Slider for adjusting alpha (transparency).

- `TMP_InputField HexCodeField` — Input field for entering hex color codes.


- `Image ColorPreview` — Shows the current selected color.

- `TMP_Text PromptText` — Displays the prompt or title of the color editor.


- `Image HueValImage` — Gradient image for the hue/value selector background.
- `Image HueSatImage` — Overlay image affecting hue/saturation display.

- `Image SatValHueImage` — Main saturation/value image for hue display.
- `Image SatValImage` — Child saturation/value image.

- `Image ValHueSatImage` — Image for value/hue/saturation gradient display.

- `Image AlphaRGBImage` — Displays the RGB color for the alpha slider.

**Methods**
```cs
private void Awake()
```
Initializes the singleton instance.

<br/><br/>

```cs
public void Open(string prompt, Color color, params Image[] images)
```
Opens the color editor targeting an array of images with the specified initial color and prompt.

<br/>

```cs
public void Open(string prompt, Color color, params TMP_Text[] texts)
```
Opens the color editor targeting an array of text components with the specified initial color and prompt.

<br/><br/>

```cs
public void AssignNewImages(params Image[] images)
```
Assigns a new set of images to be updated with the selected color.

<br/>

```cs
public void AssignNewTexts(params TMP_Text[] texts)
```
Assigns a new set of text elements to be updated with the selected color.

<br/><br/>

```cs
public void SetColor(string hexCode)
```
Sets the current color from a hex string, updating sliders accordingly.

<br/>

```cs
public void UpdateColor()
```
Updates the current color based on slider values and refreshes UI previews and referenced elements.

<br/><br/>

```cs
public void UpdateRefs()
```
Applies the current color to all referenced images and texts.

<br/>

```cs
public void UpdateSliders()
```
Updates the sliders to reflect the current color’s HSV and alpha values.

<br/><br/>

```cs
public void ApplyColors()
```
Commits the current color to referenced elements and hides the editor.

<br/>

```cs
public void CancelColors()
```
Reverts all referenced elements to their original colors before editing and hides the editor.

<br/>

```cs
public void HideColorEditor()
```
Hides the color editor UI and clears referenced elements.

---

### `Scripts/Localization`

#### TMPLocalizer.cs

**Description**<br/>
Updates a `TMP_Text` component's value based on the selected language. Fetches localized text via `LocalizationSystem` using a specified key, with optional suffix text.

**Properties**
- `TMP_Text TMP_Text` — Reference to the text component to localize.
- `string key` — The key used to retrieve the localized string.
- `string extraText` — Additional text to append after the localized value.

**Methods**
```cs
public void UpdateText()
```
Updates the `TMP_Text` with the current language's localized value based on the key.

<br/>

```cs
private void OnDrawGizmos()
```
(Only in editor) Ensures the `TMP_Text` reference is auto-assigned for convenience.

---

#### TMPDropdownLocalizer.cs

**Description**<br/>
Updates the labels of a `TMP_Dropdown` based on localized keys. Used for dropdown UIs that should reflect the currently selected language.

**Properties**
- `TMP_Dropdown TMP_Dropdown` — Reference to the dropdown to localize.
- `string[] keys` — Keys for each option in the dropdown, matching the order of the dropdown options.


**Methods**
```cs
public void UpdateText()
```
Updates the text for all dropdown options and refreshes the caption to match the current value.

<br/>

```cs
private void OnDrawGizmos()
```
(Only in editor) Ensures the `TMP_Dropdown` reference is auto-assigned if missing.

---

#### LocalizationSystem.cs

**Description**<br/>
Handles language selection, translation retrieval from text files and notifies UI elements when the language changes.

**Properties**

`static LocalizationSystem instance` — Singleton instance.

`TMP_Dropdown LanguageDropdown` — UI dropdown for selecting languages.
`HelpSection helpSection` — Reference to help UI that reacts to language changes.


`int SelectedLanguage` —  Index of the currently selected language.

`bool DebugDictionary` — Logs key-value pairs when language is loaded.
`bool DebugObjects` — Logs keys and object names during lookups.


`string[] languageNames` — Names of all available languages.
`TextAsset[] textAssets` — Raw localization text files.
`Dictionary<string, string> stringPairs` — Key-value pairs for current language.

**Methods**
```cs
void Awake()
```
Initializes the singleton instance.

<br/>

```cs
public void Setup()
```
Loads available language files from Resources, parses names, and populates the dropdown.

<br/><br/>

```cs
public void SetLanguage(int langIndex)
```
Parses and loads localization data from the selected language file and updates all localized UI elements.

<br/>

```cs
public string GetText(string name, string key)
```
Returns the localized string associated with a given key. Logs the key if `DebugObjects` is enabled.

<br/><br/>

```cs
void UpdateLocalizers()
```
Finds all active and inactive `TMPLocalizer` and TMPDropdownLocalizer instances and updates their displayed text.

---

### `Scripts/Layout Groups`

#### CustomLayoutGroup.cs

**Description**<br/>
Aligns child elements linearly in a horizontal or vertical direction. Optionally resizes elements and spaces them with precision.

**Properties**
- `enum AlignmentModes { Horizontal, Vertical}` —  Direction of alignment.


- `RectTransform SelfRect` — Reference to this object's `RectTransform`.


- `AlignmentModes AlignmentMode` — Layout orientation.

- `bool AffectCellSizeX` — Whether to override child width.
- `float CellSizeX` — Width to apply if `AffectCellSizeX` is true.

- `bool AffectCellSizeY` — Whether to override child height.
- `float CellSizeY` —  Height to apply if `AffectCellSizeY` is true.

- `float Spacing` — Gap between child elements.


**Methods**
```cs
public void UpdateLayout()
```
Repositions and optionally resizes child elements, then centers them within the layout.

---

#### CenterAndFit.cs

**Description**<br/>
Aligns and optionally centers child elements in a line (horizontally or vertically), then resizes the parent `RectTransform` to fit the content.

**Properties**
- `RectTransform SelfRect` — Reference to this object's `RectTransform`.

- `CustomLayoutGroup.AlignmentModes AlignmentMode` — Layout orientation.

- `float Spacing` — Space between child elements.
- `bool Center` — Whether to center the content within the layout.


**Methods**
```cs
public void UpdateLayout()
```
Positions child elements, optionally centers them, and resizes the parent `RectTransform` to fit to the content.

---

#### CustomGridLayoutGroup.cs

**Description**<br/>
Arranges child elements in a flexible grid layout with optional headers and multi-row cells, similar to the timetable. Used by `PhotoManager.cs`.

**Properties**
- `RectTransform SelfRect` — Reference to this object's `RectTransform`.

- `int Rows` — Number of rows in the grid.
- `int Columns` — Number of columns in the grid.

- `Vector2 Spacing` — Space between grid cells (x = horizontal, y = vertical).
- `Vector2 Size` — Default size for grid cells.

- `Vector2 HeaderSize` — Size of the top row and left column headers.

- `List<int> MultirowIndexes` — List of child indexes that span multiple rows.

**Methods**
```cs
public void UpdateLayout()
```
Calculates grid cell positions and sizes, handles header dimensions, supports row-spanning cells, centers all elements, and resizes the parent to fit the grid.

---

### `Scripts/Saving`

#### TimetableData.cs

**Description**<br/>
Container class for all persistent timetable data, including layout structure, event definitions, and runtime overrides. Used for serialization and save/load operations.

**Properties**
- `string TimetableName` — Name of the timetable.
- `SerializableList<EventTypeData> EventTypes` — List of available event types.
- `SerializableList<EventItem> Events` — List of event items.
- `SerializableList<ColumnData> Columns` — Represents the vertical columns of the timetable grid.
- `SerializableList<WeekDayData> Weekdays` — Row labels for each day of the timetable.
  timetable (and core timing structure).
- `SerializableList<TimeIndex> Labels` — The labels on top of each  
  column.

**Subclasses**

- **SerializableList<T>**

  **Description**<br/>
  Wrapper around `List<T>` to make generic lists serializable in Unity's inspector and compatible with `JsonUtility`. Found this trick in the Unity Forums!

  **Properties**
  - `List<T> list` — The wrapped generic list.

- **TimetableData.WeekDayData**

  **Description**<br/>
  Stores data related to a weekday in the timetable, including its name, duration, start time, and override behaviors (temporary or scheduled changes to timing).

  **Properties**
  - `string WeekDayName` — Display name for the weekday.
  - `int Days` — Bitflag or enum value indicating which day(s) this data applies to.
  - `int[] StartTime` — Default start time (hours, minutes).
  - `int[] CommonLength` — Default session length (hours, minutes).
  - `int[] OverrideDate` — Date at which overrides become active (year, month, day).
  - `int OverrideLength` — Number of weeks the override lasts.
  - `int ExtraOverrideLengthWeeks` — Duration of override; -1 means no override.
  - `int OverrideDelayWeeks` — Delay before override becomes active.
  - `int OverrideMode` — Override behavior type (0 = none, 1 = time only, etc.).
  - `int[] TempStartTime` — Temporarily overridden start time (hours, minutes).
  - `int[] TempCommonLength` — Temporarily overridden session length (hours, minutes).

  **Constructors**
  - `WeekDayData()` — Default initializer with today's date and default values.
  - `WeekDayData(WeekDay Weekday)` — Creates a WeekDayData from an existing WeekDay object.

- **TimetableData.CellInfoData**

  **Description**<br/>
  Stores event selection and override data for a single cell in the timetable grid. Supports normal and temporary overrides, including event info, length, and favorite status.

  **Properties**
  - `int SelectedEvent` — ID/index of the selected base event.
  - `string EventNameOverride` — Override for the event's display name.
  - `string Info1Override`, `Info2Override` — Override strings for event details.
  - `int EventTypeOverride` — Override for the event's type/color.
  - `int OverrideFavourite` — 0 = no override, 1 = not favorite, 2 = favorite.
  - `bool OverrideCommonLength` — Whether the session length is overridden.
  - `int[] NewLength` — New length of the session if overridden (hours, minutes).
  - `int[] OverrideDate` — Start date for override (year, month, day).
  - `int OverrideLength` — Duration (in weeks) the override is active.
  - `int ExtraOverrideLengthWeeks` — Total override length; -1 disables.
  - `int OverrideDelayWeeks` — Delay (in weeks) before the override begins.
  - `int TempSelectedEvent` — Temporary event selection.
  - `string TempEventNameOverride` — Temporary override for name.
  - `string TempInfo1Override`, `TempInfo2Override` — Temporary override info.
  - `int TempEventTypeOverride` — Temporary event type override.
  - `int TempOverrideFavourite` — Temporary favorite override.
  - `bool TempOverrideCommonLength` — Whether length is temporarily overridden.
  - `int[] TempNewLength` — Temporary length override (hours, minutes).

  **Constructors**
  - `CellInfoData()` — Initializes fields with defaults and empty strings.
  - `CellInfoData(CellInfo c)` — Creates a CellInfoData from an existing CellInfo object.

- **TimetableData.ExtraCellInfoData**

  **Description**<br/>
  Extension of `CellInfoData` with additional string-based start time data for UI display or logging.

  **Properties**
  - `string StartTime` — Human-readable start time string.
  - `string TempStartTime` — Human-readable temporary start time string.
  (Plus all inherited `CellInfoData` properties.)

  **Constructors**
  - `ExtraCellInfoData()` — Default constructor, also initializes base class fields.

- **TimetableData.ColumnData**

  **Description**<br/>
  Represents one vertical column in the timetable grid. Contains per-cell data and whether it spans multiple rows.

  **Properties**
  - `SerializableList<CellInfoData> children` — List of cell data for this column.
  - `bool IsMultirow` — Whether this column spans multiple rows (used for merged rows).

  **Constructors**
  - `ColumnData()` — Initializes with empty children list.
  - `ColumnData(TimetableGrid.Column c)` — Initializes from a TimetableGrid.Column, copying child cell info.

- **TimetableData.EventTypeData**

  **Description**<br/>
  Stores metadata and visual information for a specific event type.

  **Properties**
  - `int ItemID` — Unique ID of the event type.
  - `string TypeName` — Display name of the type.
  - `float[] TextColor` — RGBA color for text (4 floats).
  - `float[] BackgroundColor` — RGBA color for background.

  **Constructors**
  - `EventTypeData()` — Default constructor with black text and white background.
  - `EventTypeData(EventTypeItem et)` — Initializes from an EventTypeItem.

**Constructors**
- `TimetableData() ` — Initializes all list properties and sets TimetableName to an empty string.

---

#### S_ProgramData.cs

**Description**<br/>
Legacy timetable data class used in the previous version of the app ("School Timetable"). Supports backwards compatibility by enabling conversion to the new TimetableData format.

**Properties**
- `List<LessonCellData> Cells` — A flat list of lesson cells (8 columns × 5 weekdays = 40 expected entries), each containing lesson metadata.
- `bool[] _7hDays = new bool[5]` — Array representing which days are 7-hour days (`true`) and which are 8-hour days (`false`).
- `int[] BreakLengths_7h = new int[3]` — Durations (in minutes) for the 3 breaks on 7-hour days.
- `int[] BreakLengths_8h = new int[3]` — Durations (in minutes) for the 3 breaks on 8-hour days.
- `int[] StartTime = new int[2]` — Time of day when lessons start, `[hour, minute]`. Default is `{7, 30}`.
- `int[] EndTime = new int[2]` — Time of day when lessons end, `[hour, minute]`. Default is `{13, 35}`.
- `int _7hDuration = 45` — Default lesson duration (in minutes) for 7-hour days.
- `int _8hDuration = 40 `— Default lesson duration (in minutes) for 8-hour days.
- `string FileName` — User-defined or system-generated name of the saved timetable.

**Subclasses**

- **S_ProgramData.LessonCellData**

  **Description**<br/>
  Holds information about a single lesson slot in the legacy timetable. These are arranged in a specific 8×5 grid pattern.

  **Properties**
  - `string LessonName` — Name/title of the lesson.
  - `string RoomIndex` — Room code or index.
  - `string TeacherName` — Name of the teacher.
  - `int LessonType` — Type of lesson:
    - `0`: Normal
    - `1`: Moving (different class location)
    - `2`: Gym
    - `3`: Support
  - `bool Tested` — Indicates if the lesson is an examined subject.
  - `bool Favourite` — Marks the lesson as a user’s favourite.

---

#### SettingsData.cs

**Description**<br/>
Represents all configurable settings used in the application, including user preferences for time format, language, theming, and the last opened file. Designed to be serialized and persisted between sessions.

**Properties**
- `string LastOpenedTimetable` — The filename or identifier of the last timetable that was opened.
- `bool Use24HFormat` — Indicates whether the application should display time in 24-hour format.
- `bool UseEnglishFormat` — Determines whether English-style date formatting (e.g., MM/DD/YYYY) is used.
- `SerializableList<SettingsData.CustomThemeData> CustomThemes` — A serializable list of user-defined themes available to the UI.
- `int CurrentTheme` — Index of the currently selected theme in the CustomThemes list.
- `int SelectedLanguage` — The index of the currently selected language for localization.

**Subclasses**
- **SettingsData.CustomThemeData**

  **Description**<br/>
  Encapsulates a custom user-defined UI theme with three RGBA color categories: primary, secondary, and background. Used to define appearance in a flexible and user-controllable way.

  **Properties**
  - `string ThemeName` — The name or label assigned to the theme.
  - `float[] PrimaryColor` — RGBA color used for primary UI accents (e.g., buttons, headers).
  - `float[] SecondaryColor` — RGBA color used for secondary UI elements or highlights.
  - `float[] BackgroundColor` — RGBA color used as the background color of the UI.
  **Constructors**
  - `CustomThemeData()` — Default constructor. Initializes all colors to white with 0 alpha.

  - `CustomThemeData(ColorStylePreset cs)` — Constructs a theme from an existing ColorStylePreset. Extracts RGBA components from the preset’s colors.

**Constructors**
`SettingsData()` — Default constructor. Initializes preferences to default values, including empty file path, 12-hour format, and default theme configuration.

---

#### TimetableButton.cs

**Description**<br/>
UI component representing a saved timetable file. Each instance of this component displays the file's name and provides options to load or delete it.

**Properties**
- `TMP_Text Text` — Displays the timetable's name.
- `Button Self` — Main button that loads the selected timetable when clicked.
- `Button DeleteButton` — Button that deletes the associated timetable when clicked.

---

#### SaveManager.cs

**Description**<br/>
Manages loading, saving, copying, and deletion of timetables. Also manages saving and loading user settings. Handles file system access, UI updates for saved timetables, and initializes persistent save paths. Also communicates with various systems such as the editor, day/time manager, event manager, and UI components.

**Properties**
- Basic
  - `static SaveManager instance` — Singleton instance for global access.
  - `string FilePath` — Root directory where all save data is stored.
  - `string TimetablesPath` — Relative path to the timetables subdirectory.
  - `string SettingsPath` — Relative path to the settings JSON file.
  - `string LastTimetable` — Name of the last opened timetable.
  - `static bool saved` — Flag indicating if the current timetable is saved.
  - `char[] reservedChars` — Array of characters not allowed in filenames.
  - `string[] reservedNames` — List of reserved Windows filenames that should not be used.
  - `List<TimetableButton> Buttons` — List of currently active timetable buttons.

- Serialized References (Unity Inspector)
  - `TimetableEditor TimetableEditor` — Reference to the timetable editor logic.
  - `DayTimeManager DayTimeManager` — Reference to the day and time slot manager.
  - `EventManager EventManager` — Reference to the timetable event manager.
  - `ColorStylizer Stylizer` — Reference to the color styling system.
  - `LocalizationSystem LocalizationSystem` — Reference to the localization/translation manager.
  - `GameObject OpenTimetableOverlay` — UI overlay for opening timetables.
  - `TimetableButton TimetableButtonPrefab` — Prefab used to spawn timetable buttons.
  - `Transform ButtonsParent` — Parent transform that holds all timetable button instances.
  - `Image UnsavedIndicator` — UI indicator for unsaved changes.
  - `class SaveProperties` — Class storing metadata from ExtraProperties.json.
  - `SaveProperties saveProperties` — Instance of the metadata used to determine file behavior (e.g., portable mode).

**Subclasses**
- **SaveManager.SaveProperties**

  **Description**<br/>
  Stores extra configuration flags loaded from ExtraProperties.json.

  **Properties**
  - `bool IsPortable` — Whether the app is running in portable mode (i.e., save location is relative to app directory)

**Methods**
```cs
void Awake()
```
Initializes the singleton instance.

<br/>

```cs
void Start()
```
Sets up save paths based on platform and `ExtraProperties.json`, loads the last opened timetable and associated settings, and generates buttons for saved timetables.

<br/>

```cs
void LoadButtons()
```
Ensures the timetable directory exists, clears any previously spawned buttons, and creates a `TimetableButton` for each saved timetable file. Assigns click listeners for loading and deleting.

<br/><br/>

```cs
public void ChangesMade()
```
Flags the current timetable as unsaved and enables the unsaved changes UI indicator.

<br/><br/>

```cs
public void CopyTimetableAsJson()
```
Copies the current timetable to the clipboard as a compact JSON string (without pretty print). Strips out placeholder/default data before serialization.

<br/>

```cs
public void PasteJsonAsTimetable(bool checkSave)
```
Imports a timetable from clipboard JSON. Supports both current format and legacy .timetable files. Shows a save prompt if needed before overwriting.

<br/><br/>

```cs
public TimetableData ConvertOldDataToNew(S_ProgramData old_data)
```
Converts legacy timetable data into the current format, mapping old cell types to new event types, creating columns, breaks, and time labels.

<br/>

```cs
int[] oldCellOrder = new int[]
{
    0, 30, 10, 20, 35, 5, 15, 25,
    31, 21, 11, 1, 36, 26, 16, 6,
    32, 12, 2, 22, 7, 37, 27, 17,
    33, 3, 13, 23, 8, 18, 38, 28,
    4, 14, 34, 24, 29, 9, 19, 39
};
public S_ProgramData SortOldCells(S_ProgramData olddata)
```
Reorders old timetable cells into the expected layout using the predefined order array `oldCellOrder`.

<br/><br/>

```cs
public void SaveTimetable()
```
Serializes the current timetable to JSON and saves it to disk, creating the file structure if needed. Removes default event and updates the save state.

<br/>

```cs
public void LoadTimetable(string timetable, bool checkSave)
```
Loads a saved timetable from disk. Prompts to save if unsaved changes exist. Applies timetable data to all relevant managers and UI components.

<br/>

```cs
public void DeleteTimetable(string timetable, bool confirm)
```
Deletes the specified timetable JSON file. Optionally asks for confirmation before deletion.

<br/>

```cs
public void LoadNewTimetable(bool checkSave)
```
Creates a new default timetable layout with predefined weekdays. Prompts to save if needed. Clears previous data and resets the grid.

<br/><br/>

```cs
public void SaveSettings()
```
Saves current app settings such as time format, language, theme, and custom color presets to disk.

<br/>

```cs
public void LoadSettings()
```
Loads settings from disk. Applies stored preferences and initializes theme, localization, and time format settings.

<br/><br/>

```cs
void ensureDirectoryExists(string dir)
```
Utility method to create the specified directory if it doesn’t exist.

<br/>

```cs
string removeReserved(string text)
```
Strips out reserved filename characters from a string and ensures it doesn’t match any Windows reserved names.

<br/><br/>

```cs
public void Quit(bool checkSave)
```
Exits the application. Prompts the user to save if there are unsaved changes.

---

### `Scripts`

#### CopyPasteManager.cs

**Description**<br/>
Manages copy-paste operations of simpler entities within the timetable system, such as cell info, events, event types, and color themes. It uses the system clipboard (`GUIUtility.systemCopyBuffer`) to store and retrieve JSON representations of these objects.
It **does not handle timetable copying/pasting**. That is handled by `SaveManager.cs`.

**Properties**
- `CellInfoEditor CellInfoEditor` — Manages editing of cell info data in the timetable.
- `EventCreator EventCreator` — Manages creation and editing of events.
- `EventTypeCreator EventTypeCreator` — Manages event type creation and editing.
- `PaletteCreator ThemeCreator` —  Manages color theme creation and editing.

**Methods**
```cs
public void CopyCellInfo()
```
Copies the currently edited cell info to the clipboard as JSON.
<br/>

```cs
public void PasteCellInfo()
```
Reads cell info data from the clipboard and pastes it into the editor if valid.

<br/><br/>

```cs
public void CopyEvent()
```
Copies the currently edited event to the clipboard as JSON.
<br/>

```cs
public void PasteEvent()
```
Reads event data from the clipboard and pastes it into the editor if valid.

<br/><br/>

```cs
public void CopyEventType()
```
Copies the currently edited event type to the clipboard as JSON.

<br/>

```cs
public void PasteEventType()
```
Reads event type data from the clipboard and pastes it into the editor if valid.

<br/><br/>

```cs
public void CopyColorTheme()
```
Copies the currently edited color theme to the clipboard as JSON.

<br/>

```cs
public void PasteColorTheme()
```
Reads color theme data from the clipboard and pastes it into the editor if valid.

---

#### PhotoManager.cs

**Description**<br/>
Manages the creation, display, and sharing of timetable photos. It sets up a grid of timetable cells and labels based on user-selected formats, captures a photo of the timetable layout, and enables sharing or copying of the photo depending on the platform.

**Properties**
- `Camera PhotoCamera` — Camera used to capture the timetable photo.
- `Canvas PhotoCanvas` — Canvas that contains the photo content.
- `RectTransform Content` — Container for the photo elements.
- `RawImage rawImg` — UI element to display the captured photo.
- `TMP_Dropdown FormatDropdown` — Dropdown to select the photo layout format.
- `CustomGridLayoutGroup Grid` — Layout manager for positioning cells and labels.
- `GameObject CornerPiecePrefab` — Prefab for the grid corner element.
- `WeekDayObject PhotoTimePrefab_` — Prefab for weekday/time labels.
- `CellInfo CellPrefab` — Prefab for timetable cells.
- `Button AndroidShareButton` — Button to share photo on Android.
- `Button WindowsCopyButton` — Button to copy photo on Windows.

**Methods**
```cs
private void Start()
```
Initializes platform-specific UI buttons. Enables the Windows copy button on Windows platforms, the Android share button on Android, and disables both on others.

<br/>

```cs
void SetupCells()
```
Clears previous photo elements and starts setting up the grid with new timetable data.

<br/>

```cs
IEnumerator ContinueSetup()
```
Finalizes setup after a frame delay. Instantiates labels and cells according to the selected format, adjusts layout, and triggers a photo capture.

<br/><br/>

```cs
public void Snap()
```
Captures the photo layout using the camera, stores it as a texture, and updates the UI with the captured image.

<br/>

```cs
public void CopyPhotoToClipboard()
```
(On Windows) Saves the captured photo as a PNG and executes a PowerShell script to copy it to the clipboard.

<br/>

```cs
public void SharePhoto()
```
(On Android) Saves the photo as a PNG and opens the Android share sheet to share the image.

---

#### ConfirmationManager.cs

**Description**<br/>
Manages display of confirmation dialogs with customizable titles, descriptions, and buttons. Supports dynamic button creation with associated actions and adjusts UI layout accordingly.

**Properties**
- `GameObject ConfirmationOverlay` — The overlay GameObject that shows/hides the confirmation dialog.
- `TMP_Text TMP_Title` — Text component for the dialog title.
- `TMP_Text TMP_Desc` — Text component for the dialog description.
- `ContentSizeFitter DescriptionTextSizeFitter` — Adjusts vertical fitting for the description text to handle dynamic sizing.
- `CenterAndFit DescriptionParent` — Custom component managing layout updates of the description container.
- `Button ButtonPrefab` — Prefab used to instantiate buttons dynamically.
- `Transform ButtonsParent` — Parent transform under which buttons are instantiated.
- `List<Button> Buttons` — List tracking current active buttons in the dialog.

**Subclasses**
- **ConfirmationManager.ButtonPrompt**

  **Description**<br/>
  Represents a button prompt with label and callback action.

  **Properties**
  - `string ButtonName` — Label text shown on the button.
  - `UnityAction Action` — Callback to invoke when the button is clicked.

  **Constructors**
  - `public ButtonPrompt(string text, UnityAction action)` — Initializes a new button prompt with the specified text and action.

**Methods**
```cs
public void ShowConfirmation(string title, string desc, params ButtonPrompt[] buttons)
```
Displays the confirmation dialog with a given title, description, and an arbitrary number of buttons. Clears previous buttons, instantiates new ones, assigns their labels and actions, and ensures the overlay is visible.

<br/>

```cs
IEnumerator PrepareLayout()
```
Coroutine that handles layout recalculations for the description text and its container, allowing proper resizing of the confirmation dialog UI elements.

---

#### QuitButton.cs

**Description**<br/>
Disables the parent UI object at runtime if the application is running on a mobile platform. This is typically used to hide the quit button on mobile builds, where quitting the app manually is unnecessary.

**Methods**
```cs
void Start()
```
Called automatically. Checks the platform and disables the parent object if running on a mobile device.

---

### `Scripts/Inspector Stuff`

#### CommentInformationNote.cs

**Description**<br/>
A Unity Editor-only component made by Alan Mattanó from the Unity Forums. Allows you to add notes or comments to GameObjects in the Unity Editor, making it easier to communicate essential information
about GameObjects or their components to other developers.

**Properties**
- `string comment` —  A multi-line text field holding the note or comment content.

**Methods**
```cs
void Awake()
```
Clears the comment string and immediately destroys this component at runtime, ensuring it only exists in the Editor.

---

#### ReadOnlyAttribute.cs

**Description**<br/>
A simple marker attribute class with no logic, used to tag fields in scripts as read-only in the Unity Inspector. Unity recognizes classes that inherit from `PropertyAttribute` 
as custom attributes usable in editor scripts.

---

#### ReadOnlyDrawer.cs

**Description**<br/>
A custom property drawer that renders any field tagged with `[ReadOnly]` as disabled (non-editable) in the Unity Inspector, 
preventing modifications while still displaying the field's value.

**Methods**
```cs
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
```
Draws the property in the Inspector in a disabled state, making it uneditable but visible.

---

### `Scripts/Polish`

#### InputFieldFixer.cs

**Description**<br/>
Provides helper functionality for `TMP_InputField` to sanitize text and ensure proper validator setup. Specifically removes zero-width spaces that visually interfere with the Roboto font and links the input field to its validator.

**Properties**
- `TMP_InputField TMPField` — The target input field to monitor and validate.
- `TMP_Text PreviewText` —  Text element that displays the cleaned-up version of the input, with zero-width spaces removed.

**Methods**
```cs
public void UpdateTextPreview(string text)
```
Removes zero-width space characters (`\u200b`) from the input and updates the preview text. This prevents Roboto font from showing visual artifacts.

<br/>

```cs
private void Start()
```
Automatically assigns the `TMPField` to its `ValidatorBase` validator on start, allowing custom validation logic to access the input field’s character limit.

---

#### TabHandler.cs

**Description**<br/>
Controls a UI tab system by toggling visibility of associated tab content and invoking custom events on selection. Allows tabs to be dynamically assigned in the Inspector and initialized at runtime.

**Properties**
- `bool DefaultToZero` — If true, the first tab is automatically selected on start.

- `Tab[] tabs` — Array of Tab objects, each representing a button-content pair and an optional event.

- `UnityEvent OnSelectTab` — Global event invoked any time a tab is selected.

**Subclasses**
- **TabHandler.Tab**

  **Description**<br/>
  Represents a single tab consisting of a button, its associated content, and a custom event triggered when the tab is selected.

  **Properties**
  - `Button TabButton` — The UI button that triggers tab selection.
  - `GameObject TabObject` — The GameObject (usually a panel) that is activated when the tab is selected.
  - `UnityEvent TabSpecificEvent` — Optional event that runs when this specific tab is selected.

**Methods**
```cs
public void SelectTab(int index)
```
Activates the tab at the given index, enabling its content and disabling others. Triggers the selected tab’s event and the global `OnSelectTab` event. Safely ignores out-of-range indices.

<br/>

```cs
private void Start()
```
Initializes the tab system. If `DefaultToZero` is true, selects the first tab. Also attaches `SelectTab(index)` calls to each tab's button onClick listener.

---

#### VersionText.cs

**Description**<br/>
Updates a UI text element to display the current application version. Typically used to show version info in settings or splash screens.

**Properties**
- `TMP_Text text` — Reference to the TextMeshPro UI component that displays the version number.

**Methods**
```cs
void Start()
```
Sets `text.text` to `"v"` followed by `Application.version`, e.g., `"v1.0.0"`.

---

### `Scripts/Polish/Animation Scripts`

#### CustomAnimator.cs

**Description**<br/>
Wraps Unity's `Animator` to simplify playing animation states while tracking the current state. Prevents redundant state changes.

**Properties**
- `Animator Animator` — The Animator component driving the animation.
- `string currState` — Internally stored name of the current animation state
  (used to prevent replaying the same animation).

**Methods**
```cs
public void ChangeState(string newstate)
```
Plays the given animation if it differs from the current one. Updates internal `currState`.

<br/>

```cs
public void SetState(string newstate,int layer ,float progress)
```
Forcefully sets an animation to a specific point in its timeline. Updates `currState` and manually applies it with `Animator.Update(0f)` to ensure immediate effect.

---

#### HamburgerButton.cs

**Description**<br/>
Controls a hamburger-style toggle button and related animated UI content. Handles state switching and ensures animations are visually correct even after being disabled/re-enabled.

**Properties**
- `CustomAnimator buttonAnimator` — Animator for the hamburger icon itself.
- `CustomAnimator contentAnimator` — Animator for the associated content panel.

- `string ButtonOpenAnim` — Animation played when button opens (e.g., "Burger to X").
- `string ButtonCloseAnim` — Animation played when button closes (e.g., "X to Burger").

- `string ContentOpenAnim` — Animation played when showing the content panel.
- `string ContentCloseAnim` — Animation played when hiding the content panel.

- `bool isOpen` — Internal state tracking whether the menu is open.
- `bool CloseMenuOnDisable` — If `true`, menu resets to closed when disabled and re-enabled.

**Methods**
```cs
private void OnEnable()
```
Ensures the correct animation state is restored when the object is re-enabled. Resets to closed if `CloseMenuOnDisable` is true.

<br/>

```cs
public void OpenOrClose()
```
Toggles between open and closed states. Triggers both button and content animations.

<br/>

```cs
public void SetOpenState(bool open)
```
Explicitly sets the open/closed state (without toggling). Avoids duplicate state changes.

---

### `TextMesh Pro/Validators`

#### ValidatorBase.cs

**Description**<br/>
This class inherits from `TMP_InputValidator` and serves as a base class for a couple custom validators.

**Properties**
- `TMP_InputField InputField` — Used primarily by subclasses to restore the character limit, since custom validators override the default character limit behavior.

**Methods**
```cs
public void AssignInputField(TMP_InputField _inputField)
```
Assigns the given `_inputField` to the `InputField` property. Called by InputFieldFixer.cs.

<br/>

```cs
public override char Validate(ref string text, ref int pos, char ch)
```
Automatically called by the `TMP_InputField` as the user types. This method contains the custom validation logic.

---

#### HexCodeValidator.cs

**Description**<br/>
Inherits from `ValidatorBase.cs`. This class is used by the hex color code input field in the UI Color Editor to restrict input to valid hexadecimal characters.

**Methods**
```cs
public override char Validate(ref string text, ref int pos, char ch)
```
Automatically called by the `TMP_InputField` as the user types.
- Accepts only the characters: `0-9`, `a-f`, `A-F`.
- Accepts `#` only at the first position (`pos == 0`).
- Blocks any other characters and prevents overflow beyond the input's character limit.

---

#### TimeValidator.cs

**Description**<br/>
Inherits from `ValidatorBase.cs`. This validator is used by input fields in the WeekDay Editor and CellInfo Editor when entering start times.

**Methods**
```cs
public override char Validate(ref string text, ref int pos, char ch)
```
Automatically called by the TMP_InputField during input.

- Accepts numeric digits: `0-9`.
- Accepts time-specific characters: `A`, `P`, `M`, `a`, `p`, `m`.
- Accepts separators: `:`, `.`, and space (` `).
- Blocks any other characters and prevents overflow beyond the input's character limit.

---

### `StreamingAssets`

#### CopyToClipboard.ps1

**Description**<br/>
PowerShell script used on Windows to copy a generated timetable image to the clipboard. Called by `PhotoManager.cs` when the user taps the "Copy" button.

---

### `Plugins/Android`

#### FileProviderLib-release.aar

**Description**<br/>
Android AAR library that provides `FileProvider` functionality, enabling secure file sharing via the Android share sheet. Used by `PhotoManager.cs` when invoking the system share UI for timetable screenshots.

---

## Packages Used

### Unity Features
- 2D Features
  - 2D Animation
  - 2D Aseprite Importer
  - 2D Common
  - 2D Pixel Perfect
  - 2D PSD Importer
  - 2D Sprite
  - 2D SpriteShape
  - 2D Tilemap Editor
  - 2D Tilemap Extras

  NOTE: Some of these packages aren't used but Unity counts all of these as a group, so they can't be removed separetely.

### Unity Packages
- Burst
- Collections
- Custom NUnit
- Mathematics
- Mono Cecil
- Performance testing API
- Test Framework
- Timeline
- Unity UI
- Visual Studio Editor

### Libraries
- TextMeshPro — Located in `Assets/TextMesh Pro`. Used to display UI related to text.