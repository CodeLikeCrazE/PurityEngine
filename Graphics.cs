using PurityEngine.Internal.XNAStuff;

namespace PurityEngine.Graphics {
    public class Graphics {
        public static GraphicsSystem graphicsSystem;

        public static GraphicsSystem GetMonoGame() {
            return new MonoGameGraphicsSystem();
        }

        public static GraphicsSystem GetOpenGL() {
            return new GLGraphicsSystem();
        }
    }

    public class MonoGameGraphicsSystem : GraphicsSystem {
        Game1 game1;
        public MonoGameGraphicsSystem() {
            game1 = new Game1();
            game1.Run();
            Graphics.graphicsSystem = this;
        }
    }

    public class GLGraphicsSystem : GraphicsSystem {
        PurityEngine.Internal.GLStuff.GLGraphicsCore core = new PurityEngine.Internal.GLStuff.GLGraphicsCore();
        public GLGraphicsSystem() {

        }
    }
    
    public interface GraphicsSystem {

    }
}