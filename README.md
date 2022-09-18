# Touch
Unix equivalent of the "touch" command for Windows.

## Usage
``touch <filename> <flags>``

**Examples:**

Create "notes.txt" file in the current directory:

``touch notes.txt``

Create "settings.json" file in the current directory, and open it with the default editor:

``touch -o settings.json``

## Flags:

```none
-o              Open file after creating/updating with default editor
-a              Change access/write time on file when file already exists
-c              Do not create any files
```