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
using System.IO;

namespace EncryptString
{
    public partial class Form1 : Form
    {
        //byte[] iv = Convert.FromBase64String("v2Lat7Oxru5CnDSIbfAT / w ==");

        // Создаём вектор инициализации
        byte[] iv;
        // Создаём объект Rijndael
        Rijndael myRijndael = Rijndael.Create();
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox4.Text = "";
                // Получаем значение из поля "Исходный текст"
                string text = textBox1.Text;
                // Конвертируем текст в двоичное представление
                string bites = TextToBinary(text);
                string textKey = textBox7.Text;

                // Используем объект Rijndael
                using (myRijndael)
                {
                    // Если ключ пуст
                    // выводим сообщение
                    if (textBox7.Text == "")
                        MessageBox.Show("Требуется ключ!");
                    else
                    {
                        // Шифруем биты в байты
                        byte[] bytes = EncryptStringToBytes(bites, Convert.FromBase64String(textKey), iv);
                        // Конверитуем зашифрованные байты в Base64
                        string encrypted = Convert.ToBase64String(bytes);
                        // Заносим Base64 в поля "Шифр"
                        textBox3.Text = encrypted;
                        textBox6.Text = encrypted;
                    }
                }
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем ключ шифрования
                string textKey = textBox7.Text;
                // Получаем значение из поля шифр и убираем пробелы
                string encrypted = textBox6.Text.Replace(" ", "");
                // Конвертируем зашифрованные Base64 в зашифрованные байты
                byte[] bytes = Convert.FromBase64String(encrypted);
                string roundtrip = "";

                // Используем объект Rijndael
                using (myRijndael)
                {
                    // Дешифруем байты в биты
                    roundtrip = DecryptStringFromBytes(bytes, Convert.FromBase64String(textKey), iv);
                }
                // Преобразовываем биты в текст
                string text = BinaryToText(roundtrip);
                // Записываем биты в поле "Исходный текст"
                textBox4.Text = text;
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Невозможно расшифровать этот шифр этим ключом");
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Генерируем ключ и вектор инициализации
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            iv = myRijndael.IV;
            // Конвертируем ключ шифрования в Base64
            textBox7.Text = Convert.ToBase64String(myRijndael.Key);
        }
    }
}
