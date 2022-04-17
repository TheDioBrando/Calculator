using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            number1 = new StringBuilder();
            number2= new StringBuilder();
            operation = null;


        }

        StringBuilder number1, number2;
        string? operation;

        /// <summary>
        /// Присваивает полю первого или второго числа значение нажатой кнопки цифры
        /// </summary>
        private void BtnDigit_Click(object sender, RoutedEventArgs e)
        {
            var digit = (sender as Button).Content.ToString();
            Lbl.Content += digit;
            if (operation == null)
            {
                number1.Append(digit);
                return;
            }
            else
                number2.Append(digit);
        }

        /// <summary>
        /// Присваивает оператору значение плюс/минус или указывает знак числа
        /// </summary>
        private void BtnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            var oper = (sender as Button).Content.ToString();
            
            if (number1.Length == 0)
            {
                Lbl.Content += oper;
                number1.Append(oper);
                return;
            }
            if(number1.Length >0 && operation==null)
            {
                switch (oper)
                {
                    case "+":
                        operation = "sum";
                        Lbl.Content += "\n + \n";
                        break;
                    case "-":
                        operation = "minus";
                        Lbl.Content += "\n - \n";
                        break;
                }
                return;
            }
            btnDelete_Click(sender, e);
        }

        /// <summary>
        /// Присваивает оператору значение умножения/деления
        /// </summary>
        private void BtnMulDiv_Click(object sender, RoutedEventArgs e)
        {
            if (operation == null && number1.Length>0 && number2.Length==0)
            {
                var str = (sender as Button).Content.ToString();
                switch (str)
                {
                    case "*":
                        operation = "mult";
                        Lbl.Content += "\n * \n";
                        break;
                    case "/":
                        operation = "div";
                        Lbl.Content += "\n / \n";
                        break;
                }
            }
        } 

        /// <summary>
        /// Присваивает числу десятичную часть. Невозможно назначить две десятичные части одному числу
        /// </summary>
        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            Lbl.Content += ",";
            if(operation == null && number1.Length>0 && !IsStringContain(number1,','))
            {
                number1.Append(',');
                return;
            }
            if (operation != null && number2.Length>0 && !IsStringContain(number2,','))
            {
                number2.Append(',');
                return;
            }
            btnDelete_Click(sender, e);
        }


        /// <summary>
        /// Содержит ли строка определенный знак
        /// </summary>
        /// <param name="strBuilder"></param>
        /// <param name="sign"></param>
        private bool IsStringContain(StringBuilder strBuilder, char sign)
        {
            var str=strBuilder.ToString(); 
            foreach(var c in str)
            {
                if (c == sign)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Стирает последний символ в окне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(Lbl.Content.ToString() !="")
            {
                var content = Lbl.Content.ToString();
                content=content.Remove(content.Length - 1);
                Lbl.Content = content;
                if(number2.Length>0)
                {
                    number2.Length--;
                    return;
                }
                if (operation != null)
                {
                    operation = null;
                    return;
                }
                if (number1.Length>0)
                {
                    number1.Length--;
                    return;
                }
                return;
            }
                Reset_Click(sender,e);
        }

        /// <summary>
        /// Полностью стирает панель калькулятора и обнуляет значения полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Lbl.Content = "";
            number1.Clear();
            number2.Clear();
            operation = null;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Выводит результат выражения и присваивает первому числу результат.
        /// Если какое-то из значений не ввели, то просит заполнить выражение полностью.
        /// Если делить на нуль, выводит сообщение о недопустимости и стирает нуль.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            if(number1.Length==0 || number2.Length==0 || operation == null)
            {
                MessageBox.Show("Please, complete expression!");
                return;
            }

            double number1AsInt, number2AsInt;
            number1AsInt = double.Parse(number1.ToString());
            number2AsInt = double.Parse(number2.ToString());

            switch (operation)
            {
                case "mult": Lbl.Content = number1AsInt * number2AsInt;
                    break;
                case "div":
                    if (number2AsInt == 0)
                    {
                        MessageBox.Show("Don't able to divide by zero");
                        number2.Clear();
                        btnDelete_Click(sender, e);
                        return;
                    }
                    Lbl.Content = number1AsInt / number2AsInt;
                    break;
                case "sum":
                    Lbl.Content = number1AsInt + number2AsInt;
                    break;
                case "minus":
                    Lbl.Content = number1AsInt - number2AsInt;
                    break;
            }
            number1.Clear();
            number1.Append(Lbl.Content.ToString());
            number2.Clear();
            operation = null;
        }
    }
}
