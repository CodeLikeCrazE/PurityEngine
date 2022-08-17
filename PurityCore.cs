using System.Threading.Tasks;
using System.Threading;
using System;

namespace PurityEngine {

    /// <summary>
    ///  The class that represents the universe.
    /// </summary>
    public class Universe : Asset {
        /// <summary>
        ///  The instance of the universe.
        /// </summary>
        public static Universe instance;
        /// <summary>
        ///  The root entity.
        /// </summary>
        public Node root = new Node();
        public Universe() {
            instance = this;
        }
        /// <summary>
        ///  Finds an object with a component
        /// <summary>
        public T Get<T>() {
            return root.Get<T>();
        }
        ///  Updates the universe.
        /// </summary>
        public void Update() {
            Time.TimeIteration();
            root.Update();
            root.AsyncUpdate();
        }
    }

    /// <summary>
    /// The class for anything in the game
    /// </summary>
    public class Node : Asset {
        /// <summary>
        /// The node's transform
        /// </summary>
        public Transform transform = new Transform();
        /// <summary>
        /// The node's child nodes
        /// </summary>
        public Node[] children = new Node[0];
        /// <summary>
        /// The node's components
        /// </summary>
        public Component[] components = new Component[0];
        /// <summary>
        /// The node's parent node
        /// </summary>
        public Node parent;
        /// <summary>
        /// The universe that owns this node
        /// </summary>
        public Universe universe;
        private bool initialized;

        public Node() {

        }

        /// <summary>
        /// Calls the start function an all the components
        /// </summary>
        public void Start() {
            for (int i = 0; i < components.Length; i++) {
                try {
                    components[i].node = this;
                    ((dynamic)components[i]).Start();
                } catch {}
            }
        }

        /// <summary>
        /// Calls the update function an all the components
        /// </summary>
        public void Update() {
            if (!initialized) {
                initialized = true;
                Start();
            }
            for (int i = 0; i < components.Length; i++) {
                components[i].node = this;
                try {
                    ((dynamic)components[i]).Update();
                } catch {}
            }
            for (int i = 0; i < children.Length; i++) {
                children[i].universe = universe;
                children[i].Update();
            }
        }

        /// <summary>
        /// Calls the async update function an all the components (asynchronously)
        /// </summary>
        public void AsyncUpdate() {
            for (int i = 0; i < components.Length; i++) {
                if (components[i].GetType().GetMethod("AsyncUpdate") != null) {
                    Thread th = new Thread((ParameterizedThreadStart)components[i].GetType().GetMethod("AsyncUpdate").CreateDelegate(typeof(ParameterizedThreadStart)));
                    th.Start();
                }
            }
            for (int i = 0; i < children.Length; i++) {
                children[i].AsyncUpdate();
            }
        }

        /// <summary>
        /// Gets a component of a specific type
        /// </summary>
        public T Get<T>() {
            for (int i = 0; i < components.Length; i++) {
                if (components[i] is T) {
                    return (T)(dynamic)components[i];
                }
            }
            for (int i = 0; i < children.Length; i++) {
                if (children[i].Get<T>() != null) {
                    return children[i].Get<T>();
                }
            }
            return default(T);
        }
    }

    /// <summary>
    /// The Time class, which allows you to get time information.
    /// </summary>
    public class Time {
        /// <summary>
        /// The time in seconds since the start of the game
        /// </summary>
        public static double time = 0;
        
        /// <summary>
        /// The time's scaling factor
        /// </summary>
        public static double timeScale = 1;

        /// <summary>
        /// The amount of time since the last frame in seconds
        /// </summary>
        public static double deltaTime;
        private static double previousTime;

        public static void TimeIteration() {
            double t = DateTime.UtcNow.Subtract(System.DateTime.UnixEpoch).TotalSeconds;
            if (previousTime == 0) {
                previousTime = t;
            }
            deltaTime = (t - previousTime) * timeScale;
            previousTime = t;
            time += deltaTime;
        }
    }
    
    /// <summary>
    /// The base of all functionality for Nodes
    /// </summary>
    public class Component {
        /// <summary>
        /// The node attached to this component
        /// </summary>
        public Node node;
        /// <summary>
        /// The transform attached to the node
        /// </summary>
        public Transform transform {
            get {
                return node.transform;
            }
        }
        /// <summary>
        /// The universe the node lives in
        /// </summary>
        public Universe universe {
            get {
                return node.universe;
            }
        }
        
        public Component() {
        }

        /// <summary>
        /// Gets a component of a specific type
        /// </summary>
        public T Get<T>() {
            return node.Get<T>();
        }
    }

    /// <summary>
    /// The class for colors
    /// </summary>
    public class Color {
        /// <summary>
        /// The red component of a color
        /// </summary>
        public double r;
        /// <summary>
        /// The green component of a color
        /// </summary>
        public double g;
        /// <summary>
        /// The blue component of a color
        /// </summary>
        public double b;

        public Color(double r, double g, double b) {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }

    /// <summary>
    /// The class for images
    /// </summary>
    public class Image : Asset, Texture {
        /// <summary>
        /// The array of colors
        /// </summary>
        public Color[,] colors;
        /// <summary>
        /// The width of the image
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the image
        /// </summary>
        public int height;

        public Image() {

        }

        public float WidthToHeightRatio() {
            return (float)width / (float)height;
        }

        public Color GetColorAtCoordinate(double x, double y) {
            return colors[(int)x, (int)y];
        }
    }

    /// <summary>
    /// The interface for images with dynamic resolutions
    /// </summary>
    public interface Texture {
        /// <summary>
        /// The width relative to the height
        /// </summary>
        public float WidthToHeightRatio();
        /// <summary>
        /// Gets the color at a coordinate
        /// </summary>
        public Color GetColorAtCoordinate(double x, double y);
    }

    /// <summary>
    /// The class for the position of a component in the world
    /// </summary>
    public class Transform {
        /// <summary>
        /// The position of the object
        /// </summary>
        public Vector3D position;
        /// <summary>
        /// The rotation of the object
        /// </summary>
        public Vector3D rotation;
        /// <summary>
        /// The scale of the object
        /// </summary>
        public Vector3D scale;

        public Transform() {

        }
    }

    /// <summary>
    /// A 2D position
    /// </summary>
    public class Vector2D {
        public double x;
        public double y;

        public Vector2D() {

        }

        public Vector2D(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector3D(Vector2D value) {
            return new Vector3D(value.x,value.y,0);
        }

        public static implicit operator Vector4D(Vector2D value) {
            return new Vector4D(value.x,value.y,0,0);
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x + b.x, a.y + b.y);
        }
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x - b.x, a.y - b.y);
        }
        public static Vector2D operator *(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x * b.x, a.y * b.y);
        }
        public static Vector2D operator /(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x / b.x, a.y / b.y);
        }
        public static Vector2D operator *(Vector2D a, double b) {
            return new Vector2D(a.x * b, a.y * b);
        }
        public static Vector2D operator /(Vector2D a, double b) {
            return new Vector2D(a.x / b, a.y / b);
        }
    }

    /// <summary>
    /// A 3D position
    /// </summary>
    public class Vector3D {
        public double x;
        public double y;
        public double z;

        public static implicit operator Vector2D(Vector3D value) {
            return new Vector2D(value.x,value.y);
        }

        public static implicit operator Vector4D(Vector3D value) {
            return new Vector4D(value.x,value.y,0,0);
        }

        public Vector3D() {

        }

        public Vector3D(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3D operator *(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vector3D operator /(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static Vector3D operator *(Vector3D a, double b) {
            return new Vector3D(a.x * b, a.y * b, a.z * b);
        }
        public static Vector3D operator /(Vector3D a, double b) {
            return new Vector3D(a.x / b, a.y / b, a.z * b);
        }
    }

    /// <summary>
    /// A 4D position
    /// </summary>
    public class Vector4D {
        public double x;
        public double y;
        public double z;
        public double w;

        public static implicit operator Vector2D(Vector4D value) {
            return new Vector2D(value.x,value.y);
        }

        public static implicit operator Vector3D(Vector4D value) {
            return new Vector3D(value.x,value.y,0);
        }

        public Vector4D() {

        }

        public Vector4D (double x, double y, double z, double w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector4D operator +(Vector4D a, Vector4D b)
        {
            return new Vector4D(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }
        public static Vector4D operator -(Vector4D a, Vector4D b)
        {
            return new Vector4D(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }
        public static Vector4D operator *(Vector4D a, Vector4D b)
        {
            return new Vector4D(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }
        public static Vector4D operator /(Vector4D a, Vector4D b)
        {
            return new Vector4D(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        }
        public static Vector4D operator *(Vector4D a, double b) {
            return new Vector4D(a.x * b, a.y * b, a.z * b, a.w * b);
        }
        public static Vector4D operator /(Vector4D a, double b) {
            return new Vector4D(a.x / b, a.y / b, a.z / b, a.w / b);
        }
    }
}