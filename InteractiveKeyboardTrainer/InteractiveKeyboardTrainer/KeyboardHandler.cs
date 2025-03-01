using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InteractiveKeyboardTrainer
{
    // Класс для перечисления обработок клавиш при нажатии KeyDown и отпускании KeyUp
    public class KeyboardHandler
    {
        private readonly Dictionary<Key, Action> keyDownActions;
        private readonly Dictionary<Key, Action> keyUpActions;
        bool ShiftPressed => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        bool CapsLockPressed => Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled;
        public KeyboardHandler(MainWindow mainWindow)
        {
            keyDownActions = new Dictionary<Key, Action>
            {
                {Key.OemTilde, () => HighlightButton(mainWindow.btn_Tilda, ShiftPressed || CapsLockPressed ? "~" : "`") },
                {Key.D0, () => HighlightButton(mainWindow.btn_Zero, ShiftPressed || CapsLockPressed ? ")" : "0") },
                {Key.D1, () => HighlightButton(mainWindow.btn_One, ShiftPressed || CapsLockPressed ? "!" : "1") },
                {Key.D2, () => HighlightButton(mainWindow.btn_Two, ShiftPressed || CapsLockPressed ? "@" : "2") },
                {Key.D3, () => HighlightButton(mainWindow.btn_Three, ShiftPressed || CapsLockPressed ? "#" : "3") },
                {Key.D4, () => HighlightButton(mainWindow.btn_Four, ShiftPressed || CapsLockPressed ? "$" : "4") },
                {Key.D5, () => HighlightButton(mainWindow.btn_Five, ShiftPressed || CapsLockPressed ? "%" : "5") },
                {Key.D6, () => HighlightButton(mainWindow.btn_Six, ShiftPressed || CapsLockPressed ? "^" : "6") },
                {Key.D7, () => HighlightButton(mainWindow.btn_Seven, ShiftPressed || CapsLockPressed ? "&" : "7") },
                {Key.D8, () => HighlightButton(mainWindow.btn_Eight, ShiftPressed || CapsLockPressed ? "*" : "8") },
                {Key.D9, () => HighlightButton(mainWindow.btn_Nine, ShiftPressed || CapsLockPressed ? "(" : "9") },
                {Key.A, () => HighlightButton(mainWindow.btn_A, ShiftPressed || CapsLockPressed ? "A" : "a") },
                {Key.B, () => HighlightButton(mainWindow.btn_B, ShiftPressed || CapsLockPressed ? "B" : "b") },
                {Key.C, () => HighlightButton(mainWindow.btn_C, ShiftPressed || CapsLockPressed ? "C" : "c") },
                {Key.D, () => HighlightButton(mainWindow.btn_D, ShiftPressed || CapsLockPressed ? "D" : "d") },
                {Key.E, () => HighlightButton(mainWindow.btn_E, ShiftPressed || CapsLockPressed ? "E" : "e") },
                {Key.F, () => HighlightButton(mainWindow.btn_F, ShiftPressed || CapsLockPressed ? "F" : "f") },
                {Key.G, () => HighlightButton(mainWindow.btn_G, ShiftPressed || CapsLockPressed ? "G" : "g") },
                {Key.H, () => HighlightButton(mainWindow.btn_H, ShiftPressed || CapsLockPressed ? "H" : "h") },
                {Key.I, () => HighlightButton(mainWindow.btn_I, ShiftPressed || CapsLockPressed ? "I" : "i") },
                {Key.J, () => HighlightButton(mainWindow.btn_J, ShiftPressed || CapsLockPressed ? "J" : "j") },
                {Key.K, () => HighlightButton(mainWindow.btn_K, ShiftPressed || CapsLockPressed ? "K" : "k") },
                {Key.L, () => HighlightButton(mainWindow.btn_L, ShiftPressed || CapsLockPressed ? "L" : "l") },
                {Key.M, () => HighlightButton(mainWindow.btn_M, ShiftPressed || CapsLockPressed ? "M" : "m") },
                {Key.N, () => HighlightButton(mainWindow.btn_N, ShiftPressed || CapsLockPressed ? "N" : "n") },
                {Key.O, () => HighlightButton(mainWindow.btn_O, ShiftPressed || CapsLockPressed ? "O" : "o") },
                {Key.P, () => HighlightButton(mainWindow.btn_P, ShiftPressed || CapsLockPressed ? "P" : "p") },
                {Key.Q, () => HighlightButton(mainWindow.btn_Q, ShiftPressed || CapsLockPressed ? "Q" : "q") },
                {Key.R, () => HighlightButton(mainWindow.btn_R, ShiftPressed || CapsLockPressed ? "R" : "r") },
                {Key.S, () => HighlightButton(mainWindow.btn_S, ShiftPressed || CapsLockPressed ? "S" : "s") },
                {Key.T, () => HighlightButton(mainWindow.btn_T, ShiftPressed || CapsLockPressed ? "T" : "t") },
                {Key.U, () => HighlightButton(mainWindow.btn_U, ShiftPressed || CapsLockPressed ? "U" : "u") },
                {Key.V, () => HighlightButton(mainWindow.btn_V, ShiftPressed || CapsLockPressed ? "V" : "v") },
                {Key.W, () => HighlightButton(mainWindow.btn_W, ShiftPressed || CapsLockPressed ? "W" : "w") },
                {Key.X, () => HighlightButton(mainWindow.btn_X, ShiftPressed || CapsLockPressed ? "X" : "x") },
                {Key.Y, () => HighlightButton(mainWindow.btn_Y, ShiftPressed || CapsLockPressed ? "Y" : "y") },
                {Key.Z, () => HighlightButton(mainWindow.btn_Z, ShiftPressed || CapsLockPressed ? "Z" : "z") },
                {Key.Oem1, () => HighlightButton(mainWindow.btn_SemiColon, ShiftPressed || CapsLockPressed ? ":" : ";") },
                {Key.Oem2, () => HighlightButton(mainWindow.btn_Question, ShiftPressed || CapsLockPressed ? "?" : "/") },
                {Key.Oem4, () => HighlightButton(mainWindow.btn_SBLeft, ShiftPressed || CapsLockPressed ? "{" : "[") },
                {Key.Oem5, () => HighlightButton(mainWindow.btn_Pipe, ShiftPressed || CapsLockPressed ? "|" : "\\") },
                {Key.Oem6, () => HighlightButton(mainWindow.btn_SBRight, ShiftPressed || CapsLockPressed ? "}" : "]") },
                {Key.Oem7, () => HighlightButton(mainWindow.btn_Quote, ShiftPressed || CapsLockPressed ? "\"" : "'") },
                {Key.OemComma, () => HighlightButton(mainWindow.btn_Comma, ShiftPressed || CapsLockPressed ? "<" : ",") },
                {Key.OemPeriod, () => HighlightButton(mainWindow.btn_Dot, ShiftPressed || CapsLockPressed ? ">" : ".") },
                {Key.OemPlus, () => HighlightButton(mainWindow.btn_Equal, ShiftPressed || CapsLockPressed ? "+" : "=") },
                {Key.OemMinus, () => HighlightButton(mainWindow.btn_Minus, ShiftPressed || CapsLockPressed ? "_" : "-") },
                {Key.Space, () => HighlightButton(mainWindow.btn_Space, "Space") },
                {Key.Back, () => HighlightButton(mainWindow.btn_BS, "BackSpace") },
                {Key.Tab, () => HighlightButton(mainWindow.btn_Tab, "Tab") },
                {Key.LeftAlt, () => HighlightButton(mainWindow.btn_LeftAlt, "Alt") },
                {Key.LeftCtrl, () => HighlightButton(mainWindow.btn_LeftCtrl, "Ctrl") },
                {Key.LeftShift, () => HighlightButton(mainWindow.btn_LeftShift, "Shift") },
                {Key.RightAlt, () => HighlightButton(mainWindow.btn_RightAlt, "Alt") },
                {Key.RightCtrl, () => HighlightButton(mainWindow.btn_RightCtrl, "Ctrl") },
                {Key.RightShift, () => HighlightButton(mainWindow.btn_RightShift, "Shift") },
                {Key.Enter, () => HighlightButton(mainWindow.btn_Enter, "Enter") },
                {Key.LWin, () =>HighlightButton(mainWindow.btn_LeftWin, "Win") },
                {Key.RWin, () => HighlightButton(mainWindow.btn_RightWin, "Win") }
            };
            keyUpActions = new Dictionary<Key, Action>
            {
                {Key.OemTilde, () => ResetHighlight(mainWindow.btn_Tilda, "LightCoral") },
                {Key.D0, () => ResetHighlight(mainWindow.btn_Zero, "Yellow") },
                {Key.D1, () => ResetHighlight(mainWindow.btn_One, "LightCoral") },
                {Key.D2, () => ResetHighlight(mainWindow.btn_Two, "LightCoral") },
                {Key.D3, () => ResetHighlight(mainWindow.btn_Three, "Yellow") },
                {Key.D4, () => ResetHighlight(mainWindow.btn_Four, "LightGreen") },
                {Key.D5, () => ResetHighlight(mainWindow.btn_Five, "LightBlue") },
                {Key.D6, () => ResetHighlight(mainWindow.btn_Six, "LightBlue") },
                {Key.D7, () => ResetHighlight(mainWindow.btn_Seven, "HotPink") },
                {Key.D8, () => ResetHighlight(mainWindow.btn_Eight, "HotPink") },
                {Key.D9, () => ResetHighlight(mainWindow.btn_Nine, "LightCoral") },
                {Key.A, () => ResetHighlight(mainWindow.btn_A, "LightCoral") },
                {Key.B, () => ResetHighlight(mainWindow.btn_B, "LightBlue") },
                {Key.C, () => ResetHighlight(mainWindow.btn_C, "LightGreen") },
                {Key.D, () => ResetHighlight(mainWindow.btn_D, "LightBlue") },
                {Key.E, () => ResetHighlight(mainWindow.btn_E, "LightGreen") },
                {Key.F, () => ResetHighlight(mainWindow.btn_F, "LightBlue") },
                {Key.G, () => ResetHighlight(mainWindow.btn_G, "LightBlue") },
                {Key.H, () => ResetHighlight(mainWindow.btn_H, "HotPink") },
                {Key.I, () => ResetHighlight(mainWindow.btn_I, "LightCoral") },
                {Key.J, () => ResetHighlight(mainWindow.btn_J, "HotPink") },
                {Key.K, () => ResetHighlight(mainWindow.btn_K, "LightCoral") },
                {Key.L, () => ResetHighlight(mainWindow.btn_L, "Yellow") },
                {Key.M, () => ResetHighlight(mainWindow.btn_M, "HotPink") },
                {Key.N, () => ResetHighlight(mainWindow.btn_N, "HotPink") },
                {Key.O, () => ResetHighlight(mainWindow.btn_O, "Yellow") },
                {Key.P, () => ResetHighlight(mainWindow.btn_P, "LightGreen") },
                {Key.Q, () => ResetHighlight(mainWindow.btn_Q, "LightCoral") },
                {Key.R, () => ResetHighlight(mainWindow.btn_R, "LightBlue") },
                {Key.S, () => ResetHighlight(mainWindow.btn_S, "Yellow") },
                {Key.T, () => ResetHighlight(mainWindow.btn_T, "LightBlue") },
                {Key.U, () => ResetHighlight(mainWindow.btn_U, "HotPink") },
                {Key.V, () => ResetHighlight(mainWindow.btn_V, "LightBlue") },
                {Key.W, () => ResetHighlight(mainWindow.btn_W, "Yellow") },
                {Key.X, () => ResetHighlight(mainWindow.btn_X, "Yellow") },
                {Key.Y, () => ResetHighlight(mainWindow.btn_Y, "HotPink") },
                {Key.Z, () => ResetHighlight(mainWindow.btn_Z, "LightCoral") },
                {Key.Oem1, () => ResetHighlight(mainWindow.btn_SemiColon, "LightGreen") },
                {Key.Oem2, () => ResetHighlight(mainWindow.btn_Question, "LightGreen") },
                {Key.Oem4, () => ResetHighlight(mainWindow.btn_SBLeft, "LightGreen") },
                {Key.Oem5, () => ResetHighlight(mainWindow.btn_Pipe, "LightGreen") },
                {Key.Oem6, () => ResetHighlight(mainWindow.btn_SBRight, "LightGreen") },
                {Key.Oem7, () => ResetHighlight(mainWindow.btn_Quote, "LightGreen") },
                {Key.OemComma, () => ResetHighlight(mainWindow.btn_Comma, "LightCoral") },
                {Key.OemPeriod, () => ResetHighlight(mainWindow.btn_Dot, "Yellow") },
                {Key.OemPlus, () => ResetHighlight(mainWindow.btn_Equal, "LightGreen") },
                {Key.OemMinus, () => ResetHighlight(mainWindow.btn_Minus, "LightGreen") },
                {Key.Space, () => ResetHighlight(mainWindow.btn_Space, "Orange") },
                {Key.Back, () => ResetHighlight(mainWindow.btn_BS, "White") },
                {Key.Tab, () => ResetHighlight(mainWindow.btn_Tab, "White") },
                {Key.LeftAlt, () => ResetHighlight(mainWindow.btn_LeftAlt, "White") },
                {Key.LeftCtrl, () => ResetHighlight(mainWindow.btn_LeftCtrl, "White") },
                {Key.LeftShift, () => ResetHighlight(mainWindow.btn_LeftShift, "White") },
                {Key.RightAlt, () => ResetHighlight(mainWindow.btn_RightAlt, "White") },
                {Key.RightCtrl, () => ResetHighlight(mainWindow.btn_RightCtrl, "White") },
                {Key.RightShift, () => ResetHighlight(mainWindow.btn_RightShift, "White") },
                {Key.Enter, () => ResetHighlight(mainWindow.btn_Enter, "White") },
                {Key.LWin, () => ResetHighlight(mainWindow.btn_LeftWin, "White") },
                {Key.RWin, () => ResetHighlight(mainWindow.btn_RightWin, "White") }
            };
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownActions.TryGetValue(e.Key, out var action))
                action();
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (keyUpActions.TryGetValue(e.Key, out var action))
                action();
        }
        private void HighlightButton(Button button, string content)
        {
            button.Background = new SolidColorBrush(Colors.DarkGray);
            button.Content = content;
        }
        private void ResetHighlight(Button button, string hexColor)
        {
            var Converter = new BrushConverter();
            button.Background = (Brush)Converter.ConvertFromString(hexColor);
        }
    }
}
