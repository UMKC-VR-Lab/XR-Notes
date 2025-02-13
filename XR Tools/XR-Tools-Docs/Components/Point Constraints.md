Determines the closest point to a target point while satisfying specific spatial constraints.

### **1. Point Within a Radius of Another Point**
- **Constraint**: Distance from the reference point ≤ radius.
- **Solution**: Use vector math to clamp the distance from the reference point:
    - If the point is outside the radius, move it to the closest point on the sphere's surface.

### **2. Point on the Line Between Two Points**
- **Constraint**: The point must be on the line segment defined by two endpoints.
- **Solution**:
    - Project the target point onto the line defined by the two points.
    - Clamp the projected point to the segment if necessary.

### **3. Point Within a Shape on a Plane**
- **Constraint**: Point lies on a plane defined by multiple points and within a 2D shape on the plane's surface.
- **Solution**:
    - Project the target point onto the plane.
    - Check if the projected point is inside the shape using a 2D point-in-polygon test.

### **4. Point on the Surface of a Mesh**
- **Constraint**: Point must remain on the surface of a 3D mesh.
- **Solution**:
    - Use closest-point-on-mesh algorithms, such as:
        - **Ray casting**: Shoot a ray toward the target point.
        - **Distance Field Search**: Use spatial partitioning like octrees or bounding volume hierarchies (BVH).
