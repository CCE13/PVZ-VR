SP IXTC 360 Editor was created by 
SINGAPORE POLYTECHNIC,
Department of Technology Innovation and Enterprise, 
Interactive Expierence Technological Centre.

# SP IXTC 360 Editor 
-Create a 360 interactive virtual tour of an evironment with static images
-Create a 360 interactive video tour
-Scrub and Save out a single 360 image from a 360 video
-Input hotspots and information popouts

# Controllers
-Hold ALT key + move mouse to maneuveour viewpoint
-Hold ALT key + move mouse over direction arrows, click left-mouse button to transport from one location sphere to another.
-Esc button to preview data (time duration looking at Hotspot)

# After Importing Package
-Two new tabs wil appear in the menu bar, "Google Vr" & "SP IXTC 360"
-Sample scenes and 360 images/videos are available in the package
-For you own project, make sure your desired 360 images/video are available in you computer

# Usage
..................................360 interactive Virtual tour..............................................

[Creating the location spheres]
1)SP IXTC 360 > Create 360 Sphere (window editor will appear)
2)360 Image Sphere Tab > Browse Button (select your first location image) 
3)Rename your Sphere
4)Create Sphere Button (creates a sphere with your image in the scene)
5)Repeat steps 1 - 4 with as many location images as you have (created sphere will always be at the same spot, 
shifting them around in the scene will not affect the outcome and is highly advisable)

[Connecting the location spheres]
6)Add Arrow Button > Cick at any part of within a sphere to create direction arrows ( indicates the direction to the next connecting sphere)
7)[Hierachy] 
  - Select the sphere that the direction arrow was created in (The sphere will have children attached to it)
  - Click on SP_Teleport(Clone)
8)[Inspector] 
  - The inspector will indicate an empty "Target Sphere" slot
  - Input a sphere in the slot (A sphere which you want the arrow to lead to)
  - Arrow will turn from red to white ( a connection has been made)
9)Repeat steps 6 - 8 to map out the whole environment with directional arrows

[Creating Infomation within the sphere]
10)SP IXTC 360 > Input Information (window editor will appear)
11)Fill up the information, details and image first
12)Select a spot of the sphere where you want your popout to appear.(e.g. Machines, Hardware )

[Building the tour]
13) Select the sphere you wish to begin your tour with
    [Inspector]
    -Click "Select as Start Position"
14)Go into game mode and Navigate around your 360 interactive virtual tour
15)File > Build Settings > Build and Run

..................................360 interactive Video tour..............................................
-Same step as the Virtual Tour, except that you use the "Create 360 Video" Tab.
-On sphere creation, texture would be black, Go into game mode once, in order to activate and display video on sphere.
-Transcode Video to Quater Res(1440 x 720) at import settings for optimal performance.

..................................Save out single 360 image frame..........................................
[To enable user to save out as many photos they need from a 360 video, and use it in a 360 interactive virtual tour]
1)SP IXTC 360 > Capture 360 image (window editor + game panel will appear)
2)Drag a sphere(containing a video) created from the Create 360 Sphere editor into empty object field
3)Change game panel to Display 2 
4)Toggle play button to stop and play video
4)Stop Video First before scrubbing timeline till desired frame
5)Click Save Image






