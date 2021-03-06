using System;
using System.Windows.Forms;
using Computator.NET.DataTypes;
using Computator.NET.DataTypes.Localization;

namespace Computator.NET.UI.Menus.Commands.HelpCommands
{
    internal class ThanksToCommand : CommandBase
    {
        public ThanksToCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            Text = MenuStrings.ThanksTo_Text;
            ToolTip = MenuStrings.ThanksTo_Text;
        }


        public override void Execute()
        {
            MessageBox.Show(
                GlobalConfig.betatesters + Environment.NewLine + Environment.NewLine + GlobalConfig.translators +
                Environment.NewLine + Environment.NewLine +
                GlobalConfig.libraries + Environment.NewLine + Environment.NewLine +
                GlobalConfig.others, Strings.SpecialThanksTo);
        }
    }
}