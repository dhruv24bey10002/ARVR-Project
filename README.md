# AR Technical Equipment Manual & Interactive Maintenance HUD

**Course/Module:** AR/VR Development (Module 3 - Mobile AR & Digital Twins)
**Developer:** Dhruv Chaudhary
**Project Type:** Mobile Augmented Reality (Android)
**Frameworks Used:** Unity 3D, Universal Render Pipeline (URP), AR Foundation, Google ARCore

---

## 🔗 Project Links
* **GitHub Repository:** [ARVR-Project](https://github.com/dhruv24bey10002/ARVR-Project)
* **Demo Video:** [Insert Link to YouTube/Drive Video Here]

---

## 🎯 Problem Statement
In industrial maintenance and electronics repair, technicians frequently rely on 2D paper manuals, blueprints, or PDF diagrams to understand complex 3D hardware. Continuously shifting focus between a reference document and the physical equipment leads to cognitive overload, increased repair time, and a higher risk of error. There is a critical need for an interactive, "in-situ" diagnostic tool that overlays 3D digital twins and maintenance steps directly onto the physical hardware in real-time.

---

## ⚙️ Project Overview & Overall Working
This project is a Mobile AR application designed to act as an interactive technical manual. 

**How it works:**
1. **Target Recognition:** The user launches the app on an Android smartphone and points the camera at a physical printed reference diagram (e.g., a 2D image of a motherboard or circuit board).
2. **Digital Twin Anchoring:** Utilizing ARCore's Image Tracking subsystem, the app instantly detects the physical image, calculates its real-world pose, and spawns a 3D digital twin directly on top of the paper, scaled perfectly (1 Unity unit = 1 physical meter).
3. **Interactive UI (HUD):** A responsive 2D Canvas UI overlays the screen, providing real-time status updates ("Equipment Connected") and interaction buttons.
4. **Exploded View Animation:** Tapping the "Toggle Exploded View" button triggers a C#-driven animation state machine. The digital twin physically separates into sub-components (e.g., the capacitor lifts off the baseboard), allowing the technician to inspect internal layouts.
5. **Inspection Torch:** Tapping the "Toggle Torch" button activates a localized 3D spotlight within the AR scene, casting a beam onto the virtual components to highlight normal maps and material depth, simulating a technician's inspection flashlight.

---

## 🏗️ Technical Architecture & Mechanics
* **AR Subsystems:** `ARTrackedImageManager` handles 2D feature extraction and 6-DoF tracking. `ARCameraManager` handles the physical camera feed.
* **Rendering:** Universal Render Pipeline (URP) with the `AR Background Renderer Feature` injected to allow real-world camera passthrough.
* **State Machine:** Unity `Animator` using boolean parameters (`isExploded`) to transition smoothly between default and exploded mechanical states.
* **UI Scaling:** `CanvasScaler` configured to "Scale With Screen Size" to ensure touch targets remain mathematically consistent on high-DPI Android devices.

---

## 🛠️ Step-by-Step Implementation Guide

### Phase 1: Project & Build Configuration
1. Created a Unity 3D URP project.
2. Installed **AR Foundation** and **Google ARCore XR Plugin** via the Package Manager.
3. Switched the build platform to Android. Configured Player Settings: changed Scripting Backend to `IL2CPP`, set target architecture to `ARM64`, removed the `Vulkan` Graphics API, and set Minimum API Level to 24.

### Phase 2: AR Environment Setup
1. Removed the default Main Camera and added an **AR Session** and **XR Origin (Mobile AR)**.
2. Created a **Reference Image Library** (`EquipmentLibrary`) and added the 2D motherboard image, specifying its exact physical width in meters (e.g., `0.2m`) to ensure 1:1 AR scaling.
3. Added the `AR Tracked Image Manager` to the XR Origin and linked the library.

### Phase 3: Digital Twin & Animation
1. Constructed an `Equipment_Prefab` using basic 3D primitives (`BaseBoard` as a blue cube, `Capacitor` as a red cylinder).
2. Attached a `Spot Light` pointing downward to act as an inspection tool.
3. Created an Animation Clip (`Explode.anim`) that translates the Capacitor upwards on the Y-axis over 1 second.
4. Configured an Animator Controller with two states (Empty/Assembled and Exploded), transitioning based on the `isExploded` boolean parameter. Disabled "Has Exit Time" for immediate UI responsiveness.

### Phase 4: UI & Scripting
1. Created a C# script `ImageTargetHUDManager` to instantiate the Prefab over the tracked image and pass its reference to the UI.
2. Created `EquipmentHUDController` to handle UI button clicks (`onClick.AddListener`), updating the Animator boolean and toggling the Spotlight intensity.
3. Built a UI Canvas with `TextMeshPro` buttons securely anchored to the bottom corners of the screen.

### Phase 5: URP Rendering Fixes
1. Located the `Mobile_Renderer` asset in the URP global settings.
2. Added the **AR Background Renderer Feature** to prevent the URP skybox from blocking the ARCore camera feed.

---

## 🚧 Challenges & Resolutions
* **Camera Feed Occlusion:** The initial build resulted in a solid yellow screen. **Resolution:** Identified a conflict with the Universal Render Pipeline. Fixed by injecting the `AR Background Renderer Feature` into the URP Mobile Renderer and disabling Vulkan.
* **Microscopic UI on Device:** UI elements rendered correctly in the editor but were unclickable on the mobile device. **Resolution:** Transitioned the Canvas Scaler from absolute pixels to relative scaling (`Scale With Screen Size`) and applied bottom-corner RectTransform anchoring.
* **Washed-out Materials:** The Inspection Torch initially turned the 3D models pure white. **Resolution:** Added distinct colored materials and lowered the Spotlight intensity, preventing shader blowout and allowing depth perception.

---

## 🚀 Future Scope
To elevate this prototype to a production-ready enterprise application, the following upgrades are recommended:
1. **High-Fidelity CAD Integration:** Replace the primitive placeholder shapes with highly detailed `.fbx` or `.obj` mechanical models utilizing PBR (Physically Based Rendering) materials.
2. **Multi-Target Subsystem:** Expand the Image Library to recognize multiple different machine parts simultaneously, spawning different tool manuals depending on what the user scans.
3. **IoT Data Integration:** Connect the app to a cloud database to fetch live diagnostic data (e.g., temperature, voltage) and display it floating next to the AR model.
4. **Step-by-Step Spatial Instructions:** Implement a UI stepper that guides the user through a repair process, animating one specific screw or wire at a time with floating 3D text arrows.
