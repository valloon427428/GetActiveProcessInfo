# Get Active Process/Window Info

C# console app that gets current active process/window info including Handle, PID, WindowTitle, WindowClass, Processname, Filename using Windows API.
And it takes screenshots for active window in 2 ways - using PrintWindow API and classic method.

<img src="assets/2024-01-30 15.23.43.png" alt="CMD Window on PrintWindow API" />
<img src="assets/2024-01-30 15.23.43-2.png" alt="CMD Window on classic method" />
<img src="assets/2024-01-30 15.23.33.png" alt="Notepad Window on PrintWindow API" />
<img src="assets/2024-01-30 15.23.33-2.png" alt="Notepad Window on classic method" />


## PS
The PrintWindow win32 api will capture a window bitmap even if the window is covered by other windows or if it is off screen. But it does not work on some kind of applications.
(https://stackoverflow.com/a/911225/23280849)

