# PhotoSort
PhotoSort is a C# Windows form application. It is for merging photos (& movies) from multiple cameras into one folder by date & time from EXIF (or MediInfo from movies). 

![alt text](https://github.com/fialot/PhotoSort/blob/master/PhotoSort.png)


## Features:

- Merging photos (& movies) from multiple cameras to one folder

- EXIF DateTime shift for photos in input folder

- Optional set File Creation Time from EXIF time

- Optional set shifted time to EXIF tag

### Used file mask symbols:

**%i** - file index

**%i4** - file index with fixes number length (for example: **%i4** -> 0001)

**%N** - old file name

**%F** - parrent folder name

**%yyyy** - photo year (4 digits)

**%yy** - photo year (last 2 digits)

**%y** - photo year

**%MM** - photo month (2 digits)

**%M** - photo month

**%dd** - photo day (2 digits)

**%d** - photo day

**%hh** - photo hour (2 digits)

**%h** - photo hour

**%mm** - photo minutes (2 digits)

**%m** - photo minutes

**%ss** - photo seconds (2 digits)

**%s** - photo seconds

### In this project is used:

- **ExifLib** 1.7.0 by SimonMcKenzie (https://www.codeproject.com/Articles/36342/ExifLib-A-Fast-Exif-Data-Extractor-for-NET)

- **ExifLibNet** 1.1.9 by oozcitak (https://github.com/oozcitak/exiflibrary)

- **ObjectListView.Official** 2.9.1 (http://objectlistview.sourceforge.net/cs/index.html)

- **MediaInfo.Wrapper** 17.12.0 by yartat (https://github.com/yartat/MP-MediaInfo)

