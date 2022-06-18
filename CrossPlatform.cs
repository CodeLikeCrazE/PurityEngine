using System;
using System.Dynamic;
using PurityEngine;

namespace PurityEngine.CrossPlatform {
    [Obsolete("Don't use any of this, it's not really supported any more, just use PurityEngine.Graphics.")]
    public class Platform {
        public static PlatformImplementations GetPlatform() {
            throw new NotImplementedException("Cross platform implementation generation has not been implemented.");
        }
    }

    [Obsolete("Don't use any of this, it's not really supported any more, just use PurityEngine.Graphics.")]
    public interface PlatformImplementations {
        public string CreateWindow(double x, double y, double w, double h);
        public void SetPixel(string id, int x, int y, Color color);
        public void ApplyImage(string id, Image image) {
            for (int x = 0; x < image.width; x++) {
                for (int y = 0; y < image.height; y++) {
                    SetPixel(id,x,y,image.colors[x,y]);
                }
            }
        }
        public int GetScreenWidth();
        public int GetScreenHeight();
        public int GetWindowWidth(string id);
        public int GetWindowHeight(string id);
    }
}