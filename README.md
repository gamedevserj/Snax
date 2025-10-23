# Snax

Unity 2023.2.22f1  
Run Hub scene, movement with WASD, can be rebinded - on object called **PuzzleController** there is a component with fields for inputs.  

The scene object called **ZoneStreamer** contains component where you can adjust zone centers and enter/exit radii. It also has settings for turning on/off the simulated load delay and adjust delay duration. The player is considered as being inside zone when it's center position is within zone's radius.  

The grid size can be changed by using **GridController** object in scene. Set the grid size and then click the "Create grid" button on the component. 

Object count before/after one load/unload cycle  
before: 9.39k  
after: 9.40k

Addressables screenshot  

<img src="https://raw.githubusercontent.com/gamedevserj/Snax/refs/heads/main/AddressablesScreenshot.png">
