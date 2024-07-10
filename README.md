# VR-Unity-Template

VR-Unity-Template is an open-source solution for kickstarting VR application development using Unity. This template leverages the [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/index.html) and boasts UI support for VR, enhanced user teleportation, player collision with obstacles, physical emulated hands, and interactive doors!

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/0_preview.PNG" align="center" style="width: 80%" />
</div>

## Features

### VR UI Components
Enjoy VR-native UI elements such as Buttons, Dropdowns, and Sliders, seamlessly integrated with VR controllers. Simply point and click using the "Trigger" button on your VR controller to interact with UI elements.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/1-ui.gif" align="center" style="width: 80%" />
</div>

### Enhanced Teleportation
Teleport with ease by holding the Right Controller Thumbstick forward, pointing to your desired location, and releasing. Teleportation is restricted to designated "Teleport" layers, ensuring a smooth experience while navigating your VR environment.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/2-teleport.gif" align="center" style="width: 80%" />
</div>

### Player Collision Avoidance
Navigate your VR world without fear of collisions. The player is intelligently pushed away from walls and obstacles, preventing awkward interactions. Controller movements are also governed by physics, ensuring they behave realistically within the environment.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/3-obstacleCollision.gif" align="center" style="width: 80%" />
</div>

### Object Interaction
Interact with objects in two distinct ways: XR Grab Interactable and XR Physical Grab Interactable. XR Physical Grab Interactables behave realistically, preventing them from passing through obstacles, while XR Grab Interactables offer a smoother interaction experience.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/4-grabbingObjects.gif" align="center" style="width: 80%" />
</div>

### Interactive Doors
Add immersive doors to your VR environment. Though currently undergoing refinement for smoother operation, simply grasp the door handle to open it and explore new areas within your virtual world.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/5-door.gif" align="center" style="width: 80%" />
</div>

### Object Manipulation
Utilize physics-based hands to manipulate objects within your VR space. Push objects with attached rigid bodies to create dynamic interactions and enhance the realism of your virtual environment.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/vr-Unity-Template/showdown/6-pushingObjects.gif" align="center" style="width: 80%" />
</div>

### Hand Tracking
There is another scene with Hand Tracking Input instead of controllers. It needs to be updated to have UI interaction and physic-based hands, but the rest of features are working.
 - To grab objects, use a pinch gesture.
 - To teleport, open the palm, aim to the location you want to teleport and then pinch to trigger the teleportation.

## Getting Started
Ensure you have the following packages installed via the Unity Package Manager:
- XR Interaction Toolkit
- XR Plugin Management

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/_tutorial-VR-Unity-Template/1.png" align="center" style="width: 80%" />
</div>
<br>

Once installed, navigate to "Edit/Project Settings/XR Plug-in Management" and select the appropriate Plug-in Providers based on your VR hardware. For the Hand Tracking, it is necessary to use OpenXR.

<div align="center">
  <img src="https://github.com/oscardelgado02/oscardelgado02/blob/main/images/_tutorial-VR-Unity-Template/2.png" align="center" style="width: 80%" />
</div>
<br>

Connect your VR headset to your PC, ensure relevant software (e.g., Oculus App) is running, and dive into the exciting world of VR development!
