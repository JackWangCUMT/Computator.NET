﻿using System.Collections.Generic;
using System.Windows.Forms;
using Computator.NET.UI.Menus.Commands;

namespace Computator.NET.UI.Views
{
    public partial class ToolBarView : UserControl, IToolbarView
    {
        public ToolBarView()
        {
            InitializeComponent();
        }

        public void SetCommands(IEnumerable<IToolbarCommand> commands)
        {
            toolStrip1.Items.Clear();
            foreach (var command in commands)
            {
                if (command == null)
                {
                    toolStrip1.Items.Add(new ToolStripSeparator());
                    continue;
                }
                var button = new ToolStripButton
                {
                    Checked = command.Checked,
                    CheckOnClick = command.CheckOnClick,
                    ToolTipText = command.ToolTip.Replace(@"&", ""), //hack for accelerator keys
                    Text = command.Text,
                    Image = command.Icon,
                    Enabled = command.IsEnabled,
                    Visible = command.Visible,
                    ImageScaling = ToolStripItemImageScaling.None,
                    DisplayStyle = ToolStripItemDisplayStyle.Image
                };


                var c = command; // create a closure around the command
                command.PropertyChanged += (o, s) =>
                {
                    button.CheckOnClick = c.CheckOnClick;
                    button.Checked = c.Checked;
                    button.ToolTipText = c.ToolTip;

                    button.Visible = c.Visible;
                    button.Text = c.Text;
                    button.Image = c.Icon;
                    button.Enabled = c.IsEnabled;
                };
                button.Click += (sender, args) => c.Execute();
                toolStrip1.Items.Add(button);
            }
        }
    }


    public interface IToolbarView
    {
        void SetCommands(IEnumerable<IToolbarCommand> commands);
    }
}