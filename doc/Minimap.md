# Minimap of Phrike-Scenarios

## How to add the Minimap Controller to the Scenario

Drag and Drop the Minimap Blueprint into the current Level.
![Minimap_Wiki_1](https://gitlab.com/OperationPhrike/phrike/uploads/78deb9df9770b18d76473d32ee256fb9/Minimap_Wiki_1.png)

## Parameters of the Minimap
![Minimap_Wiki_2](https://gitlab.com/OperationPhrike/phrike/uploads/97527101ae7757f0dab2c7ea3207fcb2/Minimap_Wiki_2.png)

* **Length:** The total Length of the measurestick in Unreal Units
* **Segments:** The total Amounts of Segments on the stick
* **Enable Minimap:** When enabled, the measurestick is the primary Camera and Input Actor. *DISABLE before packaging the scenario.*
* **Move Factor:** How fast you move when steering the camera
* **Ortho Width Modif:** The capture size of the camera

## Frequent Problems
1. If the Minimap doesn't work, see if you use the correct GameMode and Player Controller.
