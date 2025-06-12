# TimeyCells
### By Dimitrios Klis (aka. Jimm)

TimeyCells is a new version of my previous app, School Timetable. It was built from the ground up with one goal in mind: Making organization as dynamic and customizable as possible.

# Download
Grab the latest version of **TimeyCells** here:<br/>
[![Latest Release](https://img.shields.io/github/v/release/Dimitris-Klis/TimeyCells)](https://github.com/Dimitris-Klis/TimeyCells/releases/latest)

---

# Source Code Documentation
Developers can explore annotated source files and module-level details here:  
[View the Source Code Docs](./SOURCE-CODE-DOCS.md)

---

# User Manual
## Table of Contents
- 01: [Introduction](#introduction)<br/>

- 02: [Events, Event Types](#events-event-types)<br/>
  - [Creating Event Types](#creating-event-types)<br/>
  - [Editing Event Types](#editing-event-types)<br/>
  - [Creating Events](#creating-events)<br/>
  - [Editing Events](#editing-events)<br/>

- 03: [Editing your timetable](#editing-your-timetable)<br/>
  - [Renaming your timetable](#renaming-your-timetable)<br/>
  - [Adding/Deleting Columns & Multirows](#addingdeleting-columns--multirows)<br/>
  - [Adding/Deleting Rows](#addingdeleting-rows)<br/>
  - [Assigning events to your timetable](#assigning-events-to-your-timetable)<br/>
  - [Swapping Columns](#swapping-columns)<br/>
  - [Swapping Rows](#swapping-rows)<br/>

- 04: [Manual Editing](#manual-editing)<br/>
  - [Editing Weekdays](#editing-weekdays)<br/>
  - [Editing Cells](#editing-cells)<br/>
  - [Editing Labels](#editing-labels)<br/>
  - [Temporary Overrides](#temporary-overrides)<br/>
    - [Creating Temporary Weekdays](#creating-temporary-weekdays)<br/>
    - [Creating Temporary Cells](#creating-temporary-cells)<br/>
    - [Deleting Temporary Overrides](#deleting-temporary-overrides)<br/>

- 05: [Saving, Loading & Creating Timetables](#saving-loading--creating-timetables)<br/>
  - [Saving your work](#saving-your-work)<br/>
  - [Loading a timetable](#loading-a-timetable)<br/>
  - [Creating a new timetable](#creating-a-new-timetable)<br/>

- 06: [Copying & Pasting Data](#copying--pasting-data)<br/>

- 07: [Backwards Compatibility](#backwards-compatibility)<br/>

- 08: [Sharing a Photo](#sharing-a-photo)<br/>
  - [Sharing photos on Windows](#sharing-photos-on-windows)<br/>
  - [Sharing photos on Android](#sharing-photos-on-android)<br/>

- 09: [Settings](#settings)<br/>
  - [Time Format](#time-format)<br/>
  - [Language](#language)<br/>
  - [Themes](#themes)<br/>

- 10: [Portable Mode (PC Only)](#portable-mode-pc-only)<br/><br/>

---
## Introduction:
_TimeyCells_ is a new version of my previous app, _School Timetable_. The original _School Timetable_ application had very little customizability and was only meant to be used by Cypriot students. In contrast, _TimeyCells_ was built from the ground up with one goal in mind: Making organization as dynamic and customizable as possible.
<br/><br/>
You can create events, event types, cells which span across multiple rows and swap rows and columns. You can even change the colors of the application to one of the predefined palettes or make your own color palette! You can specify the length or the start time of a specific lesson or event. Finally, you can temporarily overwrite events and you're also informed of the current event that is taking place and how much time is left for the next event to occur.
<br/>
## Events, Event Types:
In order to create events and event types, you need to swap to the `Edit Events` tab:<br/>
![01  Burger menu - create event type (with transition)](https://github.com/user-attachments/assets/6b38371b-b96c-4b45-a96b-239f1610bc73)
<br/><br/>
### Creating Event Types:
Event Types are used to categorize the events that happen throughout the week. You can change the colors of the text and background. Event Types also need a name.<br/>
![04  Event Type Creation](https://github.com/user-attachments/assets/25067571-b445-457b-87aa-a335d33a2bc6)
<br/><br/>
### Editing Event Types:
To edit an event type, simply click the one you want to edit. You can also edit the colors of the default event.<br/>
![05  Event Type Editing](https://github.com/user-attachments/assets/0e4e27cb-bf63-4151-ba1b-8b03f32aecc7)
<br/><br/>
### Creating Events:
Events have 5 properties:
- **Name:** The name of the event.
- **Info1:** Meant for extra details like the place in which the event will happen, or the name of the person organizing the event.
- **Info2:** Also meant for extra details.
- **Event Type:** Meant to categorize your event, which helps with figuring out how important the lesson/event is.
- **Favourite:** A simple toggle to classify an event as something you really enjoy.<br/>
![06  Event Creation](https://github.com/user-attachments/assets/6e5c3a8a-ccbe-4a6a-abcf-d98ca26e47b9)
<br/><br/>
### Editing Events
You can edit events by simply clicking on them.
<br/><br/>
## Editing your Timetable
To begin editing your timetable, click the pencil icon.<br/>
![07  Edit Timetable](https://github.com/user-attachments/assets/56df73dc-d037-433b-81db-fb6d1e908251)
<br/><br/>
### Renaming your Timetable:
![08  Renaming your Timetable](https://github.com/user-attachments/assets/7bf022fe-9c76-4ef0-a31a-fe6ddaef236a)
<br/><br/>
### Adding/Deleting Columns & Multirows:
You can add up to 40 columns, which should be more than enough to satisfy the busiest of schedules. You can also add Multirows, which are columns that use the same event for each weekday.<br/>
![09  Adding, Deleting Columns](https://github.com/user-attachments/assets/ed9adc73-5447-41e7-91e1-40d449b60feb)
<br/><br/>
### Adding/Deleting Rows:
Rows represent weekdays. You can add up to 7 rows, one for each day of the week.<br/>
![10  Adding, Deleting Rows](https://github.com/user-attachments/assets/052a726a-2b4d-470f-954f-102e2994f3ab)
<br/><br/>
### Assigning Events to your Timetable:
While editing, select the event you want to assign. Then, simply click the cell you want to change for a quick assignment.<br/>
![11  Assigning Events](https://github.com/user-attachments/assets/d2a75d36-1032-48cc-9c40-5cba2fc613b8)
<br/><br/>
### Swapping Columns:
![12  Swapping Columns](https://github.com/user-attachments/assets/b8a0b1f0-2af6-494e-9254-22c156936c1b)<br/>
<br/><br/>
### Swapping Rows:
![13  Swapping Rows](https://github.com/user-attachments/assets/c207cfa4-bcdd-4511-bf86-b1678180ca67)<br/>
<br/><br/>
## Manual Editing
### Editing Weekdays:
Weekdays have the following properties:
- **Name:** The name of your weekday.
- **Start Time:** The time when the first event of the day begins.
- **Common Length:** The default length for each cell.
- **Days:** The days when the weekday will take place.
![14  Editing Weekdays](https://github.com/user-attachments/assets/b39817c8-8c5f-411f-837a-6cc56336e4c2)
<br/><br/>
### Editing Cells:
While not editing, clicking on a cell will bring up the cell editor. There you can manually change the assigned event or override some of the properties. You can also override the length and start time. If not overridden, the length will be defaulted into the row's weekday common length.
<br/>![15  Manually Editing Cells](https://github.com/user-attachments/assets/52ac2787-f8e7-4213-a256-8f3c080747bb)
<br/><br/>
### Editing Labels
Labels are the smaller cells located at the top of the column. They display the start time and an index for each cell of the current day. A label will not be displayed if the cell of the current day is empty or the current day doesn't correspond to a weekday.
<br/><br/>
You can edit the labels to always display text.<br/>
![20  Custom Labels](https://github.com/user-attachments/assets/a4a4fe29-52a3-4b9c-99b3-47d9f21b2a13)<br/>
You can also edit them to count as an index, which means that the rest of the indexes will be displayed as:
`1, 2, 3, 4, TEXT, 6, 7, 8`<br/>
Instead of:<br/>
`1, 2, 3, 4, TEXT, 5, 6, 7, 8`
<br/><br/>
Here's how thιs timetable's labels would look on a Tuesday:
![Tuesday Labels](https://github.com/user-attachments/assets/ca47b3b5-dbfa-45fe-a295-4ca3693fe524)<br/>
_* The final label is blank because the final cell of Tuesday is blank._
<br/><br/>
### Temporary Overrides:
Temporary Overrides replace a weekday's or cell's default properties with new ones for a specified number of weeks. You can also delay the override for a specified number of weeks. If the override length is set to 0, the temporary override will expire the next time that the weekday or the weekday of that cell occurs.</br>
_NOTE: If a weekday has multiple days assigned to it, the override will expire on the furthest day_
<br/><br/>
#### Creating Temporary Weekdays:
![16  Temp Weekday](https://github.com/user-attachments/assets/6118355e-fe8d-4629-b00d-806ccf90a7f3)
<br/><br/>
#### Creating Temporary Cells:
![17  Temp Cell](https://github.com/user-attachments/assets/2bec3d8a-f49a-4f57-b83b-ee160a66e2dc)
<br/><br/>
#### Deleting Temporary Overrides:
To delete a temporary override, simply click the trash icon and confirm your changes.<br/>
![18  Temp Deletion](https://github.com/user-attachments/assets/b18fd083-b8e4-4aa9-bc98-9a877256fdb1)
<br/><br/>
## Saving, Loading & Creating Timetables
### Saving your work:
This is the `Save` button:<br/>
![Save Button](https://github.com/user-attachments/assets/d02e531d-01c7-4d07-9320-7aaa581eaf0c)<br/><br/>
When you have unsaved changes, the `Save` button will get highlighted. Make sure to save often!<br/>
![Highlighted Save Button](https://github.com/user-attachments/assets/e8521649-a2c0-4159-bf0b-789bf5666217)
<br/><br/>
### Loading a timetable:
This is the `Load` button:<br/>
![Load Button](https://github.com/user-attachments/assets/112492a7-dae3-4a8c-ab5b-997e1e0de8cd)
<br/><br/>
When you click it, you'll be able to load or delete one of your saved timetables:<br/>
![Open Timetable](https://github.com/user-attachments/assets/5144657f-65eb-49a4-96fb-c0807a98b522)
<br/><br/>
When deleting anything a confirmation prompt will always appear:<br/>
![Delete Prompt](https://github.com/user-attachments/assets/2ff7bc80-df4b-4b52-a598-90d12a300d69)
<br/><br/>
### Creating a new timetable:
This is the `Create New` button. When clicked, a new blank timetable is created.<br/>
![Create New Button](https://github.com/user-attachments/assets/6e08a1ae-de54-441e-bddb-3f0d07ac592c)
<br/><br/>
If you want to use an already existing timetable as a template, you can simply rename and save it:<br/>
![19  New Timetable](https://github.com/user-attachments/assets/b94cbadf-3450-49fc-89ca-9703e94e4d37)
<br/><br/>
## Copying & Pasting Data
This is the `Copy` Button:<br/>
![Copy Button](https://github.com/user-attachments/assets/151ebad6-d351-406a-9ddb-730e1765c69a)
<br/><br/>
And this is the `Paste` Button:<br/>
![Paste Button](https://github.com/user-attachments/assets/511d8269-98d4-4ee6-a602-b33534b54432)
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
![photoIcon](https://github.com/user-attachments/assets/73656b45-7d0e-4621-8e1b-f137868efa2f)
<br/>You can share the detailed info of the timetable:<br/>
![Share Photo Info](https://github.com/user-attachments/assets/2e42eebc-893e-481c-aa6c-71056e132f6d)
<br/><br/>
Or you can share the times instead:<br/>
![Share Photo Time](https://github.com/user-attachments/assets/f420b5b8-c955-4b15-a17f-54baeb6cf49f)
<br/><br/>
### Sharing photos on Windows:
On Windows, you'll get the option to copy the photo with the `Copy` button.<br/>
![Copy Button](https://github.com/user-attachments/assets/151ebad6-d351-406a-9ddb-730e1765c69a)
<br/>This button simply copies the image to the clipboard.
<br/><br/>
### Sharing photos on Android:
On android, you'll instead get the option to share the photo with the `Share` button.<br/>
![Share Button](https://github.com/user-attachments/assets/b9b3a383-f01b-4e92-9b70-2d994e34b251)
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
![21  Color Themes](https://github.com/user-attachments/assets/55b1ab41-dce7-4ad7-aca0-ec7a5b3e82b7)
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
