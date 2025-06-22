# TimeyCells
### By Dimitrios Klis (aka. Jimm)

TimeyCells is a new version of my previous app, School Timetable. It was built from the ground up with one goal in mind: Making organization as dynamic and customizable as possible.

# Download
Grab the latest version of **TimeyCells** here:<br/>
[![Latest Release](https://img.shields.io/github/v/release/Dimitris-Klis/TimeyCells)](https://github.com/Dimitris-Klis/TimeyCells/releases/latest)

---

# Source Code Documentation
After Reading the User Manual, Developers and YH4F Judges can learn how the project works by exploring the [Source Code Docs](./SOURCE-CODE-DOCS.md).

---

# User Manual
## Table of Contents
- 01: [Introduction](#introduction)

- 02: [Events, Event Types](#events-event-types)
  - [Creating Event Types](#creating-event-types)
  - [Editing Event Types](#editing-event-types)
  - [Creating Events](#creating-events)
  - [Editing Events](#editing-events)

- 03: [Editing your timetable](#editing-your-timetable)
  - [Renaming your timetable](#renaming-your-timetable)
  - [Adding/Deleting Columns & Multirows](#addingdeleting-columns--multirows)
  - [Adding/Deleting Rows](#addingdeleting-rows)
  - [Assigning events to your timetable](#assigning-events-to-your-timetable)
  - [Swapping Columns](#swapping-columns)
  - [Swapping Rows](#swapping-rows)

- 04: [Manual Editing](#manual-editing)
  - [Editing Weekdays](#editing-weekdays)
  - [Editing Cells](#editing-cells)
  - [Editing Labels](#editing-labels)
  - [Temporary Overrides](#temporary-overrides)
    - [Creating Temporary Weekdays](#creating-temporary-weekdays)
    - [Creating Temporary Cells](#creating-temporary-cells)
    - [Deleting Temporary Overrides](#deleting-temporary-overrides)

- 05: [Saving, Loading & Creating Timetables](#saving-loading--creating-timetables)
  - [Saving your work](#saving-your-work)
  - [Loading a timetable](#loading-a-timetable)
  - [Creating a new timetable](#creating-a-new-timetable)

- 06: [Copying & Pasting Data](#copying--pasting-data)

- 07: [Backwards Compatibility](#backwards-compatibility)

- 08: [Sharing a Photo](#sharing-a-photo)
  - [Sharing photos on Windows](#sharing-photos-on-windows)
  - [Sharing photos on Android](#sharing-photos-on-android)

- 09: [Settings](#settings)
  - [Time Format](#time-format)
  - [Language](#language)
  - [Themes](#themes)

- 10: [Portable Mode (PC Only)](#portable-mode-pc-only)

---
## Introduction:
_TimeyCells_ is a new version of my previous app, _School Timetable_. The original _School Timetable_ application had very little customizability and was only meant to be used by Cypriot students. In contrast, _TimeyCells_ was built from the ground up with one goal in mind: Making organization as dynamic and customizable as possible.
<br/><br/>
You can create events, event types, cells which span across multiple rows and swap rows and columns. You can even change the colors of the application to one of the predefined palettes or make your own color palette! You can specify the length or the start time of a specific lesson or event. Finally, you can temporarily overwrite events and you're also informed of the current event that is taking place and how much time is left for the next event to occur.
<br/>
## Events, Event Types:
In order to create events and event types, you need to swap to the `Edit Events` tab:<br/>
![01  Burger menu - create event type (with transition) 15fps](https://github.com/user-attachments/assets/2384d600-f042-4c0f-8e06-3681f488b3e4)
<br/><br/>
### Creating Event Types:
Event Types are used to categorize the events that happen throughout the week. You can change the colors of the text and background. Event Types also need a name.<br/>
![04  Event Type Creation 15fps](https://github.com/user-attachments/assets/8fa32507-2f65-46d9-9f7a-49f123c55c83)
<br/><br/>
### Editing Event Types:
To edit an event type, simply click the one you want to edit. You can also edit the colors of the default event.<br/>
![05  Event Type Editing 15fps](https://github.com/user-attachments/assets/bd6a0bf7-9be0-4aa1-8f60-c932e131639a)
<br/><br/>
### Creating Events:
Events have 5 properties:
- **Name:** The name of the event.
- **Info1:** Meant for extra details like the place in which the event will happen, or the name of the person organizing the event.
- **Info2:** Also meant for extra details.
- **Event Type:** Meant to categorize your event, which helps with figuring out how important the lesson/event is.
- **Favourite:** A simple toggle to classify an event as something you really enjoy.<br/>
![06  Event Creation 15fps](https://github.com/user-attachments/assets/f7e6c000-fd54-4118-a73d-d1ffc7c583f7)
<br/><br/>
### Editing Events
You can edit events by simply clicking on them.
<br/><br/>
## Editing your Timetable
To begin editing your timetable, click the pencil icon.<br/>
![07  Edit Timetable 15fps](https://github.com/user-attachments/assets/407aeb3a-d327-44b7-994d-47b4822450eb)
<br/><br/>
### Renaming your Timetable:
![08  Renaming your Timetable 15fps](https://github.com/user-attachments/assets/1b10abeb-d669-4469-9783-cf32ed9050ea)
<br/><br/>
### Adding/Deleting Columns & Multirows:
You can add up to 40 columns, which should be more than enough to satisfy the busiest of schedules. You can also add Multirows, which are columns that use the same event for each weekday.<br/>
![09  Renaming your Timetable 15fps](https://github.com/user-attachments/assets/10d43df3-5c14-4857-8ed7-f02c9adb726d)
<br/><br/>
### Adding/Deleting Rows:
Rows represent weekdays. You can add up to 7 rows, one for each day of the week.<br/>
![10  Adding, Deleting Rows 15fps](https://github.com/user-attachments/assets/0b77c6dd-b1e5-417f-bca3-79f3dac0eb9a)
<br/><br/>
### Assigning Events to your Timetable:
While editing, select the event you want to assign. Then, simply click the cell you want to change for a quick assignment.<br/>
![11  Assigning Events 15fps](https://github.com/user-attachments/assets/1a12bb7a-e6b5-42fa-ba8d-8ffaa7061e3e)
<br/><br/>
### Swapping Columns:
![12  Swapping Columns 15fps](https://github.com/user-attachments/assets/8d83f93c-b46e-4f52-a917-080339220335)
<br/><br/>
### Swapping Rows:
![13  Swapping Rows 15fps](https://github.com/user-attachments/assets/5fd67207-3354-4ba1-a892-8a5220ee0f2a)
<br/><br/><br/>
## Manual Editing
### Editing Weekdays:
Weekdays have the following properties:
- **Name:** The name of your weekday.
- **Start Time:** The time when the first event of the day begins.
- **Common Length:** The default length for each cell.
- **Days:** The days when the weekday will take place.
![14  Editing Weekdays 15fps](https://github.com/user-attachments/assets/fa30f18a-28d4-4ccb-bf54-1177e808e0dc)
<br/><br/>
### Editing Cells:
While not editing, clicking on a cell will bring up the cell editor. There you can manually change the assigned event or override some of the properties. You can also override the length and start time. If not overridden, the length will be defaulted into the row's weekday common length.<br/>
![15  Manually Editing Cells 15fps](https://github.com/user-attachments/assets/803d7977-cc7b-40e0-9eb3-7ecb3e0465a2)
<br/><br/>
### Editing Labels
Labels are the smaller cells located at the top of the column. They display the start time and an index for each cell of the current day. A label will not be displayed if the cell of the current day is empty or the current day doesn't correspond to a weekday.
<br/><br/>
You can edit the labels to always display text.<br/>
![20  Custom Labels 15fps](https://github.com/user-attachments/assets/34f41dca-0478-4c76-baa0-36fe1e4234e6)
<br/>You can also edit them to count as an index, which means that the rest of the indexes will be displayed as:<br/>
`1, 2, 3, 4, TEXT, 6, 7, 8`<br/>
Instead of:<br/>
`1, 2, 3, 4, TEXT, 5, 6, 7, 8`
<br/><br/>
Here's how this timetable's labels would look on a Tuesday:
![Tuesday Labels](https://github.com/user-attachments/assets/ca47b3b5-dbfa-45fe-a295-4ca3693fe524)
<br/>_* The final label is blank because the final cell of Tuesday is blank._
<br/><br/>
### Temporary Overrides:
Temporary Overrides replace a weekday's or cell's default properties with new ones for a specified number of weeks. You can also delay the override for a specified number of weeks. If the override length is set to 0, the temporary override will expire the next time that the weekday or the weekday of that cell occurs.</br>
_NOTE: If a weekday has multiple days assigned to it, the override will expire on the furthest day_
<br/><br/>
#### Creating Temporary Weekdays:
![16  Temp Weekday 15fps](https://github.com/user-attachments/assets/f847c0f1-8003-4ec8-a116-975af2b177c4)
<br/><br/>
#### Creating Temporary Cells:
![17  Temp Cell 15fps](https://github.com/user-attachments/assets/c83e03f9-b66e-4a34-95f5-53da43a19093)
<br/><br/>
#### Deleting Temporary Overrides:
To delete a temporary override, simply click the trash icon and confirm your changes.<br/>
![18  Temp Deletion 15fps](https://github.com/user-attachments/assets/53f3f05f-02c1-494f-9627-f4040631234a)
<br/><br/>
## Saving, Loading & Creating Timetables
### Saving your work:
This is the `Save` button:<br/>
![22  Save Button](https://github.com/user-attachments/assets/00e05a2d-b5ad-4bb3-9e29-637556809e3a)
<br/><br/>When you have unsaved changes, the `Save` button will get highlighted. Make sure to save often!<br/>
![23  Save Button Hilighted](https://github.com/user-attachments/assets/6cc4f120-9960-4472-aad0-4d96fe81d038)
<br/><br/>
### Loading a timetable:
This is the `Load` button:<br/>
![24  Load Button](https://github.com/user-attachments/assets/d5e614d2-eeed-4dbe-ac66-b7258cacb78f)
<br/><br/>
When you click it, you'll be able to load or delete one of your saved timetables:<br/>
![Open Timetable](https://github.com/user-attachments/assets/5144657f-65eb-49a4-96fb-c0807a98b522)
<br/><br/>
When deleting anything a confirmation prompt will always appear:<br/>
![Delete Prompt](https://github.com/user-attachments/assets/2ff7bc80-df4b-4b52-a598-90d12a300d69)
<br/><br/>
### Creating a new timetable:
This is the `Create New` button. When clicked, a new blank timetable is created.<br/>
![25  New](https://github.com/user-attachments/assets/59f644dd-39c2-4259-8945-e4394a899e72)
<br/><br/>
If you want to use an already existing timetable as a template, you can simply rename and save it:<br/>
![19  New Timetable 15fps](https://github.com/user-attachments/assets/26047030-1a92-4a1f-97ff-104050916b5e)
<br/><br/>
## Copying & Pasting Data
This is the `Copy` Button:<br/>
![26  Copy](https://github.com/user-attachments/assets/b6524551-9e5d-44e6-adb6-414ca9f5e6bf)
<br/><br/>
And this is the `Paste` Button:<br/>
![27  Paste](https://github.com/user-attachments/assets/0eb52e86-8373-49e0-882c-c6b2d78dec0d)
<br/><br/>
With the use of these 2 buttons, repetitive parts of the timetable can be edited much more quickly.
<br/><br/>
You can also use these buttons to share your timetable with other people. When copying, the timetable data is copied as text. The text can then be sent to anyone.
<br/><br/>
When the other person receives the text, if they own the application, they can copy the text you sent and click the ` Paste ` button to paste the timetable.
<br/><br/>
## Backwards Compatibility
_School Timetable_, the predecessor of _TimeyCells_ also has this Copy-Paste sharing feature. Therefore, in the event that a user of the old application wants to convert to the new one, they can simply copy their timetable from _School Timetable_ and paste it directly into _TimeyCells_!
<br/><br/>
## Sharing a Photo
_TimeyCells_ also allows you to share your timetable to a person without the app by just sharing a photo instead.
<br/><br/>
By pressing the ` camera ` button:<br/>
![28  photoIcon](https://github.com/user-attachments/assets/6a27dae9-c9f3-41ce-a571-b6f5cfcc1322)
<br/>You can share the detailed info of the timetable:<br/>
![Share Photo Info](https://github.com/user-attachments/assets/2e42eebc-893e-481c-aa6c-71056e132f6d)
<br/><br/>
Or you can share the times instead:<br/>
![Share Photo Time](https://github.com/user-attachments/assets/f420b5b8-c955-4b15-a17f-54baeb6cf49f)
<br/><br/>
### Sharing photos on Windows:
On Windows, you'll get the option to copy the photo with the `Copy` button.<br/>
![26  Copy](https://github.com/user-attachments/assets/b6524551-9e5d-44e6-adb6-414ca9f5e6bf)
<br/>This button simply copies the image to the clipboard.
<br/><br/>
### Sharing photos on Android:
On android, you'll instead get the option to share the photo with the `Share` button.<br/>
![29  Share](https://github.com/user-attachments/assets/24161cd5-ff7d-43c8-a41d-bc4b2819cd9a)
<br/>
This button will activate android's share sheet, which will look something like this:<br/>
![Android Share Sheet](https://github.com/user-attachments/assets/0b038864-84f6-47aa-b176-0271eef34abe)
<br/><br/>
## Settings
![02  Burger menu - Settings](https://github.com/user-attachments/assets/2f70b3b6-e246-4fa0-8705-1423c7ef4a73)
### Time Format:
You can set the time to a 24 hour format with this toggle:<br/>
![24h Toggle](https://github.com/user-attachments/assets/59c68a20-9a86-44e3-8db7-1e9e9b19713d)
<br/><br/>
If you're an English user, you may want your time to look like:<br/>
`12.00` instead of `12:00`. There's a toggle for that too:<br/>
![English Toggle](https://github.com/user-attachments/assets/566cc935-3f1e-4c51-b225-89391dd291f6)
<br/><br/>
### Language:
At the moment, there are only 2 supported languages: English and Greek (Ελληνικά), the only languages I know.
<br/><br/>
### Themes:
Themes are used to change the app's colors. There are currently 11 pre-defined themes. 
<br/><br/>
You can also create your own themes:<br/>
![21  Color Themes 15fps](https://github.com/user-attachments/assets/b5d70873-7a10-4504-bdb9-f01ab1b23aba)
<br/><br/>
## Portable Mode (PC Only)
To enable Portable Mode, you must modify `ExtraProperties.json`, which is located in<br/>
`APPFOLDER\TimeyCells_Data\StreamingAssets\`, with a text editor and replace <br/>
`"IsPortable": false` with `"IsPortable": true`.
<br/><br/>
When Portable Mode is enabled, all files will be stored in the application's directory instead of the PC's `%AppData%`. This is useful if you prefer to store the app inside a USB or any other storage device that you connect to multiple computers.
<br/><br/>
## Help
The app has a similar user manual built in. To access it, you need to swap to the `Help` tab:<br/>
![03  Burger menu - Help](https://github.com/user-attachments/assets/48549294-ea8c-424b-a61a-bdcd9f1e4836)

---
## Done Reading?
Explore the [Source Code Docs](./SOURCE-CODE-DOCS.md)!
