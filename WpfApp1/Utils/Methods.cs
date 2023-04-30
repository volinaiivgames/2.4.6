using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Utils
{
    class Methods
    {
        public static bool VerifyName(string login)
        {
            string errorMsg = "";
            string pattern = @"^[a-zA-Z0-9_-]{3,16}$";
            if (!Regex.IsMatch(login, pattern)) errorMsg = "Имя должен содержать от 3 до 16 символов, состоять из латинских букв, цифр, знаков _ и -, и не содержать пробелов.";

            if (errorMsg.Length != 0) MessageBox.Show(errorMsg);
            else return true;
            return false;
        }

        public static bool VerifyIP(string IPvalue)
        {
            string errorMsg = "";
            string pattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (IPvalue == "0.0.0.0" || IPvalue == "255.255.255.255") errorMsg = $"Это специальный IP адрес и не может быть использован.";
            else if (!Regex.IsMatch(IPvalue, pattern)) errorMsg = $"недопустимый IP адрес.";

            if (errorMsg.Length != 0) MessageBox.Show(errorMsg);
            else return true;
            return false;
        }

        public static void ValidTextBox(TextBox textbox, string text, bool lost)
        {
            if (textbox.Text.Equals(text) && !lost)
            {
                textbox.Text = string.Empty;
                textbox.Foreground = Brushes.Black;
            }
            else if (string.IsNullOrEmpty(textbox.Text) && lost)
            {
                textbox.Text = text;
                textbox.Foreground = Brushes.Gray;
            }
        }

        public static bool IsValidTextBox(TextBox textbox)
        {
            if (textbox.Foreground == Brushes.Gray) return false;
            return true;
        }

        public static void LoadSmiles(ListBox listBox)
        {
            foreach (var item in Inits.Smiles)
            {
                listBox.Items.Add(item);
            }
        }
    }
}
