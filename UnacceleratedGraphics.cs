using Gtk;
using System;

namespace PurityEngine.Graphics {
    public class UnacceleratedGraphicsSystem : GraphicsSystem {
        Window window;
        public UnacceleratedGraphicsSystem() {
            Application.Init ();
 
            window = new Window("PurityEngine");
            window.DeleteEvent += delegate { Application.Quit(); };

            window.ShowAll();
 
            Application.Run ();
        }
    }
}
