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
            PurityEditor.Editor editor = new PurityEditor.Editor();
            editor.Main(args);
            return;
            
            if (args.Contains("--test")) {
                UnitTest.InitializeUnitTests();
                UnitTest.Run();
                return;
            }

            Graphics.Graphics.GetOpenGL();

            new Universe(); // Initialize universe. Ignore info, it does do something, it assigns the instance.

            Universe.instance.root.components = Universe.instance.root.components.Append(new TestComponent()).ToArray();

            while (true) {
                Universe.instance.Update();
                Thread.Sleep(1);
            }
        }
    }

    class TestComponent : Component {
        public void Update() {
        }
    }
}
