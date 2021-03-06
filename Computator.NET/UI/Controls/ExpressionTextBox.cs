﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Computator.NET.Data;
using Computator.NET.DataTypes;
using Computator.NET.DataTypes.SettingsTypes;
using Computator.NET.Natives;
using Computator.NET.Properties;

namespace Computator.NET.UI.Controls
{
    internal class ExpressionTextBox : TextBox, INotifyPropertyChanged, IExpressionTextBox
    {
        private AutocompleteMenu.AutocompleteMenu _autocompleteMenu;

        public ExpressionTextBox()
        {
            InitializeComponent();

            GotFocus += ExpressionTextBox_GotFocus;
            MouseDoubleClick += Control_MouseDoubleClick;
            SetFont(Settings.Default.ExpressionFont);
            SizeChanged +=
                (o, e) =>
                {
                    _autocompleteMenu.MaximumSize = new Size(Size.Width, _autocompleteMenu.MaximumSize.Height);
                };

            Settings.Default.PropertyChanged += (o, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Settings.Default.FunctionsOrder):
                        RefreshAutoComplete();
                        break;

                    case nameof(Settings.Default.ExpressionFont):
                        SetFont(Settings.Default.ExpressionFont);
                        break;
                }
            };

            if (!DesignMode)
            {
                RefreshAutoComplete();
                SharedViewState.Instance.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(SharedViewState.IsExponent)) _showCaret();
                };
            }
        }


        public bool Sort
            => Settings.Default.FunctionsOrder == FunctionsOrder.Alphabetical;

        public bool IsInDesignMode
        {
            get
            {
                var isInDesignMode = LicenseManager.UsageMode ==
                                     LicenseUsageMode.Designtime ||
                                     Debugger.IsAttached;

                if (!isInDesignMode)
                {
                    using (var process = Process.GetCurrentProcess())
                    {
                        return process.ProcessName.ToLowerInvariant().Contains("devenv");
                    }
                }

                return true;
            }
        }

        public override string Text
        {
            get { return base.Text.Replace('*', SpecialSymbols.DotSymbol); }
            set { base.Text = value.Replace('*', SpecialSymbols.DotSymbol); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Control_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SharedViewState.Instance.IsExponent = false;
        }

        private void ExpressionTextBox_GotFocus(object sender, EventArgs e)
        {
            _showCaret();
        }

        /// <summary>
        ///     test case:
        ///     tg(x)·H(x)+2.2312·root(z,2)+zᶜᵒˢ⁽ᶻ⁾+xʸ+MathieuMc(1,2,y,x)
        /// </summary>
        private void _showCaret()
        {
            var blob = TextRenderer.MeasureText("x", Font);
            if (SharedViewState.Instance.IsExponent)
                NativeMethods.CreateCaret(Handle, IntPtr.Zero, 2, blob.Height/2);
            else
                NativeMethods.CreateCaret(Handle, IntPtr.Zero, 2, blob.Height);
            NativeMethods.ShowCaret(Handle);
        }

        private void InitializeComponent()
        {
            KeyPress += ExpressionTextBox_KeyPress;
            _autocompleteMenu = new AutocompleteMenu.AutocompleteMenu();
            _autocompleteMenu.SetAutocompleteMenu(this, _autocompleteMenu);
        }

        public void SetFont(Font font)
        {
            if (font.FontFamily.Name == "Cambria" && !IsInDesignMode)
            {
                Font = CustomFonts.GetMathFont(font.Size);
                _autocompleteMenu.Font = CustomFonts.GetMathFont(font.Size);
            }
            else
            {
                Font = font;
                _autocompleteMenu.Font = font;
            }
        }

        private void RefreshAutoComplete()
        {
            var array = AutocompletionData.GetAutocompleteItemsForExpressions(true);
            if (Sort)
                Array.Sort(array, (a, b) => a.Text.CompareTo(b.Text));
            _autocompleteMenu.SetAutocompleteItems(array);
            //RefreshSize();

            //this.autocompleteMenu.deserialize();
        }

        public void RefreshSize()
        {
            _autocompleteMenu.MaximumSize = new Size(Size.Width, _autocompleteMenu.MaximumSize.Height);
        }

        private void ExpressionTextBox_KeyPress(object s, KeyPressEventArgs e)
        {
            if (SharedViewState.Instance.IsExponent)
            {
                if (SpecialSymbols.AsciiForSuperscripts.Contains(e.KeyChar))
                {
                    e.KeyChar = SpecialSymbols.AsciiToSuperscript(e.KeyChar);
                }
            }

            if (IsOperator(e.KeyChar))
            {
                if (e.KeyChar == SpecialSymbols.ExponentModeSymbol)
                {
                    SharedViewState.Instance.IsExponent = !SharedViewState.Instance.IsExponent;
                    _showCaret();
                    // EventAggregator.Instance.Publish<ExponentModeChangedEvent>(new ExponentModeChangedEvent(SharedViewState.Instance.IsExponent));
                    e.Handled = true;
                    //return;
                }

                if (e.KeyChar == '*')
                {
                    e.KeyChar = SpecialSymbols.DotSymbol;
                    //for (int i = 0; i < this.AutoCompleteCustomSource.Count; i++)
                    // this.AutoCompleteCustomSource[i] += Text + e.KeyChar;
                }
            }
        }

        private static bool IsOperator(char c)
        {
            if (c == '*' || c == '/' || c == '+' || c == '-' || c == '(' || c == '^' || c == '!')
                return true;
            return false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}