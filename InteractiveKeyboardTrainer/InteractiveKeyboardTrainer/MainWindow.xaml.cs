using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace InteractiveKeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer sessionTimer; // таймер для запуска тренажера
        private int elapsedTime = 0; // затраченное время
        private int enteredChar = 0; // счетчик только для введенных символов
        private string generatedText; // строка для генерации символов
        public MainWindow()
        {
            InitializeComponent();
            Loaded += KeyHandler_Load; // подключение словаря для неинтерактивных клавиш для подгрузки в главное окно
            InitializeSession();
        }
        private void KeyHandler_Load(object sender, RoutedEventArgs e)
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler(this);
            KeyDown += keyboardHandler.OnKeyDown;
            KeyUp += keyboardHandler.OnKeyUp;
        }
        private void Preview_OnDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                // Если нажали Tab или Enter, перемещаем фокус в InputTxt
                InputTxt.Focus();
                e.Handled = true;
            }
            else if (InputTxt.IsFocused)
            {
                InputTxt.KeyDown += InputTxt_KeyDown;
                InputTxt.TextChanged += InputBox_Changed;
            }
        }
        private void InitializeSession()
        {
            SetButtonState(StartTrainer, true);
            SetButtonState(StopTrainer, false);
        }
        private void InputTxt_KeyDown(object sender, KeyEventArgs e)
        {
            bool ShiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            char typedChar = KeyToChar(e.Key, ShiftPressed);

            if (typedChar != '\0')
            {
                InputTxt.AppendText(typedChar.ToString());
                string userText = new TextRange(InputTxt.Document.ContentStart, InputTxt.Document.ContentEnd).Text;
                int currentErrors = CalculateErrors(generatedText, userText, out int missedChars);
                error_amount.Text = $"{currentErrors}";
                enteredChar = userText.Length;
                HighlightChar(generatedText, userText);
                InputTxt.CaretPosition = InputTxt.Document.ContentEnd;
                e.Handled = true;
            }
        }
        private void InputBox_Changed(object sender, TextChangedEventArgs e)
        {
            string refText = new TextRange(TxtTrainer.Document.ContentStart, TxtTrainer.Document.ContentEnd).Text.Replace("\n", Environment.NewLine);
            string typedText = new TextRange(InputTxt.Document.ContentStart, InputTxt.Document.ContentEnd).Text.Replace("\n", Environment.NewLine);

            HighlightChar(refText, typedText);
        }
        private void DifficultyValue_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int complexitylvl = (int)e.NewValue;
            diff_text.Text = complexitylvl.ToString();

            generatedText = GenerateRndString(complexitylvl, 25); // Меняется только уровень сложности, кол-во символов - по умолчанию
            InitializeTrainerText(generatedText);
        }
        private void SymbolValue_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int symb_overhaul = (int)e.NewValue;
            symb_text.Text = symb_overhaul.ToString();

            generatedText = GenerateRndString(1, symb_overhaul); // Меняется только кол-во символов, уровень сложности - по умолчанию
            InitializeTrainerText(generatedText);
        }
        private void EventStarted_Click(object sender, RoutedEventArgs e)
        {
            SetButtonState(StartTrainer, false);
            SetButtonState(StopTrainer, true);

            generatedText = GenerateRndString((int)LvL_Setter.Value, 50);
            InitializeTrainerText(generatedText);
            StartSessionTimer();
        }
        private void EventStopped_Click(object sender, RoutedEventArgs e)
        {
            sessionTimer?.Stop();

            string userText = new TextRange(InputTxt.Document.ContentStart, InputTxt.Document.ContentEnd).Text;
            ShowSessionResults(generatedText, userText, elapsedTime);

            SetButtonState(StartTrainer, true);
            SetButtonState(StopTrainer, false);
        }
        private void Sensivity_Click(object sender, RoutedEventArgs e)
        {
            if (CaseSensivity.IsChecked == true)
                InputTxt.KeyUp += EnforceUpperCase;
            else if (CaseSensivity.IsChecked == false)
                InputTxt.KeyUp -= EnforceUpperCase;
        }
        private string GenerateRndString(int complexitylvl, int length)
        {
            string lowerCase = "abcdefghijklmnopqrstuvwxyz"; // буквы нижнего регистра
            string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // буквы верхнего регистра
            string numbers = "0123456789"; // цифры
            string specialChars = "!@#$%^&*()-_=+[{]};:'\",<.>/?"; // спец. символы
            string ordinarySpace = " ";
            string Spaces = " \t\n"; // символы табуляции

            string availableChars = lowerCase + ordinarySpace; // Пусть это будет базовая сложность
            if (complexitylvl >= 2) availableChars += upperCase + numbers + ordinarySpace; // для средней
            if (complexitylvl >= 3) availableChars += specialChars + Spaces; // для сложной

            StringBuilder result = new StringBuilder();
            Random rnd = new Random();
            for (int i=0; i < length; i++)
            {
                int index = rnd.Next(availableChars.Length);
                result.Append(availableChars[index]);
            }
            return result.ToString();
        }
        private void InitializeTrainerText(string text)
        {
            if (TxtTrainer == null || string.IsNullOrEmpty(text)) return;

            TxtTrainer.Document.Blocks.Clear(); // Очистка тек. текста

            Paragraph paragraph = new Paragraph();
            foreach (char c in generatedText)
            {
                // Создаём новый Run для каждого символа и устанавливаем черный цвет по умолчанию
                Run run = new Run(c.ToString())
                {
                    Foreground = Brushes.Black
                };
                paragraph.Inlines.Add(run);
            }

            TxtTrainer.Document.Blocks.Add(paragraph);
        }
        private char KeyToChar(Key key, bool ShiftPressed)
        {
            string lowerCase = "abcdefghijklmnopqrstuvwxyz"; // нижние регистры
            string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // верхние регистры
            string numbers = "0123456789"; // цифры
            string ShiftedChars = "!@#$%^&*()_+{}:|\"<>?"; // Спец. символы верхнего регистра
            string UnshiftedChars = "1234567890-=[];',./"; // Спец. символы нижнего регистра

            // Клавиши латинского алфавита (от A до Z)
            if (key >= Key.A && key <= Key.Z)
                return ShiftPressed ? upperCase[(int)key - (int)Key.A] : lowerCase[(int)key - (int)Key.A];
            // Клавиши цифр (от 0 до 9)
            if (key >= Key.D0 && key <= Key.D9)
                return ShiftPressed ? ShiftedChars[(int)key - (int)Key.D0] : numbers[(int)key - (int)Key.D0];
            // Спец. Oem клавиши и прочее
            if (key >= Key.Oem1 && key <= Key.OemTilde)
                return ShiftPressed ? ShiftedChars[(int)key - (int)Key.Oem1] : UnshiftedChars[(int)key - (int)Key.Oem1];
            if (key == Key.Space) return ' ';
            if (key == Key.Enter) return '\n';
            if (key == Key.Tab) return '\t';

            return '\0';
        }
        private void HighlightChar(string targetedText, string typedText)
        {
            TxtTrainer.Document.Blocks.Clear();
            var paragraph = new Paragraph();

            for (int i = 0; i < targetedText.Length; i++)
            {
                char currentRefChar = targetedText[i];
                Run charRun = new Run(currentRefChar.ToString());

                if (i < typedText.Length)
                {
                    char currentTypedChar = typedText[i];
                    charRun.Foreground = currentTypedChar == currentRefChar ? Brushes.Green : Brushes.Red;
                }
                else if (i == typedText.Length)
                {
                    charRun.Foreground = Brushes.Gray;
                }
                else
                {
                    charRun.Foreground = Brushes.Black;
                }
                paragraph.Inlines.Add(charRun);
            }
            TxtTrainer.Document.Blocks.Add(paragraph);
        }
        private void StartSessionTimer()
        {
            sessionTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            sessionTimer.Tick += (s, e) => { elapsedTime++; speed_amount.Text = $"{elapsedTime} сек"; symbol_amount.Text = $"{enteredChar}"; };
            elapsedTime = 0;
            enteredChar = 0;
            sessionTimer.Start();
        }
        private int CalculateErrors(string refText, string userText, out int missedChars)
        {
            int errorCount = 0;
            missedChars = 0;
            int length = Math.Min(refText.Length, userText.Length);

            for (int i = 0; i < length; i++)
            {
                if (refText[i] != userText[i])
                    errorCount++;
            }
            if (userText.Length < refText.Length)
                missedChars = refText.Length - userText.Length;
            errorCount += Math.Abs(refText.Length - userText.Length);

            return errorCount;
        }
        private double CalculateSpeed(string userText, int elapsedTime)
        {
            return elapsedTime > 0 ? userText.Length / (elapsedTime / 60.0) : 0;
        }
        private void SetButtonState(UIElement button, bool isEnabled)
        {
            button.IsEnabled = isEnabled;
            button.Opacity = isEnabled ? 1.0 : 0.5;
        }
        private void ResetTrainer()
        {
            TxtTrainer.Document.Blocks.Clear(); // очистка текстового поля для генерированных строк
            InputTxt.Document.Blocks.Clear(); // очистка текстового поля для ввода
            generatedText = ""; // опустошаем весь текст
            elapsedTime = 0;
            enteredChar = 0;
            sessionTimer = null; // сброс всего таймера
            error_amount.Text = "0"; // сброс в нуль
            speed_amount.Text = "0 сек"; // сброс в нуль секунд
        }
        private void ShowSessionResults(string refText, string userText, int elapsedTime)
        {
            int errorCount = CalculateErrors(refText, userText, out int missedChars);
            if (string.IsNullOrEmpty(userText))
            {
                errorCount = 0;
                missedChars = refText.Length;
            }
            double speed = CalculateSpeed(userText, elapsedTime);
            MessageBox.Show($"Ошибок: {errorCount} \nПропущенных символов: {missedChars} \nСкорость: {speed:F1} символов/мин", "Результат Тренировки", MessageBoxButton.OK);
            ResetTrainer();
        }
        private void EnforceUpperCase(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Back && e.Key != Key.Enter)
            {
                var TextPointer = InputTxt.CaretPosition.GetPositionAtOffset(-1, LogicalDirection.Forward);
                if (TextPointer != null)
                {
                    var lastChar = TextPointer.GetTextInRun(LogicalDirection.Forward);
                    if (!string.IsNullOrEmpty(lastChar) && char.IsLower(lastChar[0]))
                    {
                        TextPointer.DeleteTextInRun(-1);
                        TextPointer.InsertTextInRun(char.ToUpper(lastChar[0]).ToString());
                        InputTxt.CaretPosition = TextPointer.GetPositionAtOffset(1, LogicalDirection.Forward);
                    }
                }
            }
        }
    }
}
