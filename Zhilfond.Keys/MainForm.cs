using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zhilfond.Keys
{
    public partial class MainForm : Form
    {
        LoginForm _loginForm;

        public MainForm(LoginForm loginForm)
        {
            _loginForm = loginForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != textBox2.Text || textBox1.Text == "")
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            GenerateKeys();
        }

        private void GenerateKeys()
        {
            string path = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Выберите путь для сохранения ключей";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;


                Cursor = Cursors.WaitCursor;
                try
                {
                    KeyGenerator kg = new KeyGenerator();
                    string pub_cert, priv_key;
                    kg.Process(path, _loginForm.Username, textBox1.Text, out pub_cert, out priv_key);

                    KeyUploader ku = new KeyUploader(_loginForm.Token, _loginForm.Username, _loginForm.Password);
                    ku.Process(pub_cert, priv_key);

                    MessageBox.Show("Операция выполнена успешно");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при генерации ключей.\n Необходимо обратиться к системному администратору.\n Текст ошибки: " + ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }
    }
}
