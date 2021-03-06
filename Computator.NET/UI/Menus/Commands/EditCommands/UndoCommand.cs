using Computator.NET.UI.Controls.CodeEditors;
using Computator.NET.UI.Interfaces;

namespace Computator.NET.UI.Menus.Commands.EditCommands
{
    internal class UndoCommand : CommandBase
    {
        private readonly ICanFileEdit customFunctionsCodeEditor;
        private readonly IMainForm mainFormView;
        private readonly ICanFileEdit scriptingCodeEditor;

        public UndoCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor,
            IMainForm mainFormView)
        {
            //this.Icon = Resources.copyToolStripButtonImage;
            Text = MenuStrings.undoToolStripMenuItem_Text;
            ToolTip = MenuStrings.undoToolStripMenuItem_Text;
            ShortcutKeyString = "Ctrl+Z";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            if ((int) SharedViewState.Instance.CurrentView < 4)
                mainFormView.SendStringAsKey("^Z"); //expressionTextBox.Undo();
            else if ((int) SharedViewState.Instance.CurrentView == 4)
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.Undo();
                else
                    mainFormView.SendStringAsKey("^Z");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.Undo();
                else
                    mainFormView.SendStringAsKey("^Z");
            }

            mainFormView.SendStringAsKey("^Z");
        }
    }
}