using Gtk;
using PurityEngine;

namespace PurityEditor {
    public class Editor {
        Window window;

        public void Main(string[] args) {
            InitUI();
        }

        public void InitUI() {
            Application.Init();
            window = new Window("Purity Editor");
            window.SetDefaultSize(800, 600);

            AssetEditor editor = new AssetEditor();
            editor.asset = new Asset();

            //Create toolbar
            Toolbar toolbar = new Toolbar();
            ToolButton newButton = new ToolButton(Stock.New);
            newButton.Clicked += delegate {
                editor.asset = new PurityEngine.Image();
            };
            ToolButton openButton = new ToolButton(Stock.Open);
            ToolButton saveButton = new ToolButton(Stock.Save);
            ToolButton saveAsButton = new ToolButton(Stock.SaveAs);
            ToolButton quitButton = new ToolButton(Stock.Quit);

            toolbar.Add(newButton);
            toolbar.Add(openButton);
            toolbar.Add(saveButton);
            toolbar.Add(saveAsButton);
            toolbar.Add(quitButton);

            //Create statusbar
            Statusbar statusbar = new Statusbar();
            statusbar.Push(1, "Statusbar message");
            //Create vbox
            VBox vbox = new VBox(false, 0);
            vbox.PackStart(toolbar, false, false, 0);
            vbox.PackStart(editor.Editor(), true, true, 0);
            vbox.PackStart(statusbar, false, false, 0);
            //Add vbox to window
            window.Add(vbox);
            window.ShowAll();
            Application.Run();
        }
    }

    public class AssetEditor {
        public Asset asset;

        public Widget Editor() {
            if (asset is PurityEngine.Image) {
                return new Label("placeholder");
            } else {
                // Center VBox
                VBox centerBox = new VBox();
                HBox horizontalCenterBox = new HBox();
                HBox box = new HBox(false, 10);
                // Add error icon
                Gtk.Image errorIcon = new Gtk.Image(Stock.DialogError, IconSize.Button);
                errorIcon.SetSizeRequest(30, 30);
                box.PackStart(errorIcon, false, false, 0);
                // Add error message
                Label errorMessage = new Label("Unsuported asset type");
                box.PackStart(errorMessage, false, false, 0);
                horizontalCenterBox.CenterWidget = box;
                // Add box to center box
                centerBox.CenterWidget = horizontalCenterBox;
                // Return center box
                return centerBox;
            }
        }
    }
}