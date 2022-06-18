using System.Threading.Tasks;
using System.Threading;
using System;

namespace PurityEngine {

    /// <summary>
    ///  The class that represents the universe.
    /// </summary>
    public class Universe {
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
    public class Node {
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
        private bool initialized;

        public Node() {

        }

        /// <summary>
        /// Calls the start function an all the components
        /// </summary>
        public void Start() {
            for (int i = 0; i < components.Length; i++) {
                if (components[i].GetType().GetMethod("Start") != null) {
                    components[i].GetType().GetMethod("Start").Invoke(components[i],null);
                }
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
                components[i].transform = transform;
                if (components[i].GetType().GetMethod("Update") != null) {
                    components[i].GetType().GetMethod("Update").Invoke(components[i],null);
                }
            }
            for (int i = 0; i < children.Length; i++) {
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
        public Component? GetComponent<T>() {
            for (int i = 0; i < components.Length; i++) {
                if (components[i] is T) {
                    return components[i];
                }
            }
            return null;
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
        static double previousTime;

        public static void TimeIteration() {
            double t = System.DateTime.UtcNow.Subtract(System.DateTime.UnixEpoch).TotalSeconds;
            if (previousTime == 0) {
                previousTime = t;
            }
            deltaTime = (t - previousTime) * timeScale;
            previousTime = t;
            time += deltaTime;
        }
    }
    
    public class Component {
        public Node node;
        public Transform transform;

        public Component() {
        }

        public Component? GetComponent<T>() {
            return node.GetComponent<T>();
        }
    }

    public class Color {
        public double r;
        public double g;
        public double b;

        public Color() {

        }
    }

    public class Image : Asset, Texture {
        public Color[,] colors;
        public int width;
        public int height;

        public Image() {

        }

        public bool IsReadOnly() {
            return true;
        }

        public float WidthToHeightRatio() {
            return (float)width / (float)height;
        }

        public Color GetColorAtCoordinate(double x, double y) {
            return colors[(int)x, (int)y];
        }

        public void SetColor(double x, double y, Color color) {
            throw new System.Security.SecurityException("Cannot set colors on an image");
        }
    }

    public interface Texture {
        public bool IsReadOnly();
        public float WidthToHeightRatio();
        public Color GetColorAtCoordinate(double x, double y);
        public void SetColor(double x, double y, Color c);
    }

    public class Transform {
        public Vector3D position;
        public Vector3D rotation;
        public Vector3D scale;

        public Transform() {

        }
    }

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
    }

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
    }

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
    }
}