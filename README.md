![.NETFramework](https://github.com/e-X-plorer/VisualStudioExtension/workflows/.NETFramework/badge.svg)

[**Video showcase**](https://www.youtube.com/watch?v=zani-Uus3Gs)

# Diagnostic analyzer
Diagnostic analyzer which suggests making classes partial if they exceed user-defined boundaries.

*Criteria:*
 - Class length in lines (if the class is too long, diagnostic appears)
 - Class member count (if the class has too many members, diagnostic appears)

By clicking on the lightbulb and selecting the "Make partial" code fix, you will see an original class made partial get separated into two files (the original one and the new one).

# Member division dialog
Select `Move class members...` option in the `Tools` menu to invoke a dialog that will allow you to select a class from the currently active file, class members to move to a separate file, and the name of that new file.

# Setting dialog
In the same `Tools` menu you can select the `Partial class analyzer` option to change the analyzer settings.
