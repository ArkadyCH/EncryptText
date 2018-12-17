using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptString
{
    public partial class Form1 : Form
    {
        // Создаём вектор инициализации
        byte[] iv = Convert.FromBase64String("v2Lat7Oxru5CnDSIbfAT / w ==");

        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            // Получаем значение из поля "Исходный текст"
            string text = textBox1.Text;
            // Конвертируем текст в двоичное представление
            string bites = TextToBinary(text);

            // Создаём объект Rijndael
            using (Rijndael myRijndael = Rijndael.Create())
            {
                // Если мы не задали ключ, то генерируем новый
                // иначе берём его из поля
                if (textBox7.Text != "")
                    myRijndael.Key = Convert.FromBase64String(textBox7.Text);
                else
                    myRijndael.GenerateKey();
                // Конвертируем ключ шифрования в Base64
                textBox7.Text = Convert.ToBase64String(myRijndael.Key);
                // Шифруем биты в байты
                byte[] bytes = EncryptStringToBytes(bites, myRijndael.Key, iv);
                // Конверитуем зашифрованные байты в Base64
                string encrypted = Convert.ToBase64String(bytes);
                // Заносим Base64 в поля "Шифр"
                textBox3.Text = encrypted;
                textBox6.Text = encrypted;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Получаем ключ шифрования
            string textKey = textBox7.Text;
            // Получаем значение из поля шифр и убираем пробелы
            string encrypted = textBox6.Text.Replace(" ", "");
            // Конвертируем зашифрованные Base64 в зашифрованные байты
            byte[] bytes = Convert.FromBase64String(encrypted);
            // Дешифруем байты в биты
            string roundtrip = DecryptStringFromBytes(bytes, Convert.FromBase64String(textKey), iv);
            // Преобразовываем биты в текст
            string text = BinaryToText(roundtrip);
            // Записываем биты в поле "Исходный текст"
            textBox4.Text = text;
        }
    }
}
