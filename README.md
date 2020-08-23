# YoutubeAdSkipper
Skips Youtube ads by searching for "Skip ad" sequence from supplied screenshot image.

## Usage
Replace ScreenshotImageUri string in App.xaml.cs to a real location on your system.<br>
Replace GoogleApiCredentialsPath in Helpers/GoogleApi to a real json credentials file.<br>
Build and Run the solution. Press Alt+S hotkey to activate the app. Right click the system tray icon to exit.<br>
Best to create a startup rule to start the app on computer startup.

# Description
The app runs in the background and can be exited using the system tray icon.<br>
Functionality can be diveded into these parts:<br>
- Take a screenshot, save it
- Pass the saved screenshot file to Google's VisionAPI to annotate text from the image
- Search returned text for "Skip ad" or "Skip Ads" sequence. (this part is the weakest point of the app)
- Simulate a mouse click on "Skip ad" location (location for each word sequence is returned by the API)