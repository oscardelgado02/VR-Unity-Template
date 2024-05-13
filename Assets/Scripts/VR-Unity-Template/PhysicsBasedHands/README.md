## Attribution
The scripts included inside [PhysicsBasedHands](https://github.com/oscardelgado02/VR-Prototype---Prospective-Memory-Training/edit/main/Assets/Scripts/PhysicsBasedHands) folder are property of [Amebous Labs](https://amebouslabs.medium.com/developing-physics-based-vr-hands-in-unity-cca4643c296b).

## Modifications
[PhysicsHand.cs](https://github.com/oscardelgado02/VR-Prototype---Prospective-Memory-Training/blob/main/Assets/Scripts/PhysicsBasedHands/PhysicsHand.cs) was modified.

- All the attributes inside the script are now private instead of public.
- It was added a functionality to change the layer of grabbed objects when they are grabbed by the XRDirectInteractor. There are two new fields: *defaultObjectLayer* and *grabbedObjectLayers*. When an object is grabbed, if its layer is inside *defaultObjectLayer* at position i, then it changes its layer to *grabbedObjectLayers* at position i. This way, you can change to a layer that does not collide with the "Hands" layer, and it avoids conflicts with collisions. You can add as many layers as you want, and you can custom the layers inside "Edit > Project Settings > Physics"