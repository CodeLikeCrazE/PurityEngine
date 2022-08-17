using System;
using System.Linq;
using PurityEngine;
using PurityEngine.Graphics;
using System.Threading;
using PurityEngine.UnitTests;

namespace PurityEngine.ProgramMain
{
    class Program
    {
        static void Main(string[] args)
        {
            bool EditorMode = false;

            if (args.Contains("--editor")) {
                EditorMode = true;
            }

            if (EditorMode) {
                PurityEditor.Editor editor = new PurityEditor.Editor();
                editor.Main(args);
                return;
            }
            
            if (args.Contains("--test")) {
                UnitTest.Run();
                return;
            }

            //Graphics.Graphics.GetOpenGL();
            Graphics.Graphics.GetCairo();

            new Universe(); // Initialize universe. Ignore info, it does do something, it assigns the instance.

            Universe.instance.root.components = Universe.instance.root.components.Append(new TestComponent()).ToArray();

            while (true) {
                Universe.instance.Update();
                Thread.Sleep(1);
                Console.WriteLine(Serializer.Serialize(Universe.instance));
            }
        }
    }

    class TestComponent : Component {
        double time = 0;

        public void Update() {
            time += Time.deltaTime;
            Console.WriteLine(time);
        }
    }
}
