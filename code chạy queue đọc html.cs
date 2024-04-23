using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace ConsoleApp6
{
    internal class Program
    {
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private Queue<string> cardQueue = new Queue<string>();
        private void button_Click(object sender, EventArgs e)
        {
                ThemTheVaoHangDoi();
                KiemTraDinhDangThe();
        }
        private void ThemTheVaoHangDoi()
        {
            string text = textBox1.Text.ToString();
            StringBuilder cardContent = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    if (cardContent.Length > 0) 
                    {
                        cardQueue.Enqueue(cardContent.ToString());
                        cardContent.Clear();
                    }
                    cardContent.Append(text[i]); 
                }
                else if (text[i] != '>')
                {
                    cardContent.Append(text[i]); 
                }
                else 
                {
                    cardContent.Append(text[i]); 
                    if (cardContent.Length > 2 && !cardContent.ToString().Contains("<>"))
                    {
                        cardQueue.Enqueue(cardContent.ToString());
                    }
                    cardContent.Clear();
                    while (i + 1 < text.Length && text[i + 1] == '>')
                    {
                        i++;
                    }
                }
            }
            if (cardContent.Length > 0)
            {
                cardQueue.Enqueue(cardContent.ToString());
            }
        }
        private void KiemTraDinhDangThe()
        {
            StringBuilder output = new StringBuilder();

            while (cardQueue.Count > 0)
            {
                string card = cardQueue.Dequeue();
                if (KiemTraTheHopLe(card))
                {
                    output.AppendLine(card);
                }
                else
                {
                    output.AppendLine($"Định dạng không hợp lệ: {card}");
                }
            }
            textBox2.Text = output.ToString();
        }
        private bool KiemTraTheHopLe(string card)
        {
            return card.Length > 2 && !card.Contains("<>"); 
        }
    }
}