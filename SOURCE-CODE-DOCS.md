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

#### TimetableGrid.Column
**Description**<br/>
Nested class used by TimetableGrid.cs to represent a single column in the timetable.

**Properties**
- `bool IsMultirow` — Indicates whether the column spans multiple rows.
- `List<TimetableCell> Children` — Stores all the cells contained in said column.

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

### `Scripts/Stylizing`

#### ColorStylePreset.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

#### ColorStylizer.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

#### PaletteObject.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

#### PaletteDropdown.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

#### PaletteLister.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

#### PaletteCreator.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**

---

### `Scripts`

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

#### ScriptName.cs

**Description**<br/>

**Properties**

**Constructors**

**Methods**