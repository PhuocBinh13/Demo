using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
        private RichTextBox txtHtml;
        private RichTextBox txtContent;
        private Button btnParse;

        public Form1()
        {
            InitializeComponent();
            InitializeControls();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Thêm mã cần thực thi khi form được tải vào đây (nếu cần)
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = openFileDialog.FileName;
                    string htmlContent = File.ReadAllText(fileName);
                    txtHtml.Text = htmlContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở tập tin: " + ex.Message);
                }
            }
        }
        private void InitializeMenuStrip()
{
    MenuStrip menuStrip = new MenuStrip();
    ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File");
    ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Mở");

    openMenuItem.Click += mnuOpen_Click;
    fileMenuItem.DropDownItems.Add(openMenuItem);
    menuStrip.Items.Add(fileMenuItem);

    this.Controls.Add(menuStrip);
    menuStrip.Location = new Point(0, 0);
}
        private void InitializeControls()
        {
            // Label "BỘ ĐỌC NỘI DUNG TÀI LIỆU HTML"
            Label lblTitle = new Label();
            lblTitle.Text = "BỘ ĐỌC NỘI DUNG TÀI LIỆU HTML";
            lblTitle.Font = new Font(lblTitle.Font, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 10);
            this.Controls.Add(lblTitle);

            // Label để mô tả RichTextBox nhập HTML
            Label lblHtml = new Label();
            lblHtml.Text = "Nhập các thẻ HTML:";
            lblHtml.Location = new Point(20, 40);
            lblHtml.ForeColor = Color.Blue;
            this.Controls.Add(lblHtml);

            // RichTextBox để nhập HTML
            txtHtml = new RichTextBox();
            txtHtml.Location = new Point(20, 60);
            txtHtml.Size = new Size(400, 120);
            txtHtml.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtHtml);

            // Label để mô tả RichTextBox hiển thị kết quả
            Label lblResult = new Label();
            lblResult.Text = "Kết quả phân tích:";
            lblResult.Location = new Point(20, 190);
            lblResult.ForeColor = Color.Green;
            this.Controls.Add(lblResult);

            // RichTextBox để hiển thị kết quả
            txtContent = new RichTextBox();
            txtContent.Location = new Point(20, 210);
            txtContent.Size = new Size(400, 100);
            txtContent.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtContent);

            // Button để thực hiện phân tích HTML
            btnParse = new Button();
            btnParse.Text = "Phân tích HTML";
            btnParse.Location = new Point(20, 330);
            btnParse.BackColor = Color.Orange;
            btnParse.ForeColor = Color.White;
            btnParse.FlatStyle = FlatStyle.Flat;
            btnParse.FlatAppearance.BorderSize = 0;
            btnParse.Click += btnParse_Click;
            this.Controls.Add(btnParse);
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            string html = txtHtml.Text;

            // Kiểm tra tính hợp lệ của HTML và hiển thị kết quả
            try
            {
                string result = ParseHtml(html);
                txtContent.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cú pháp HTML: {ex.Message}");
            }
        }

        private string ParseHtml(string html)
        {
            Stack<string> tagStack = new Stack<string>();
            Queue<string> contentQueue = new Queue<string>();

            int i = 0;
            while (i < html.Length)
            {
                if (html[i] == '<')
                {
                    // Thẻ mở
                    int j = html.IndexOf('>', i);
                    if (j == -1)
                        throw new Exception("Thiếu ký tự '>'");

                    string tag = html.Substring(i + 1, j - i - 1);
                    if (!tag.StartsWith("/"))
                    {
                        tagStack.Push(tag);
                    }
                    else
                    {
                        // Thẻ đóng
                        string openingTag = tagStack.Pop();
                        if (openingTag != tag.Substring(1))
                            throw new Exception("Thẻ đóng không khớp với thẻ mở");
                    }

                    i = j + 1;
                }
                else
                {
                    // Nội dung trong thẻ
                    int j = html.IndexOf('<', i);
                    if (j == -1)
                    {
                        // Đây là nội dung cuối cùng của tài liệu
                        string content = html.Substring(i).Trim();
                        if (!string.IsNullOrEmpty(content))
                            contentQueue.Enqueue(content);
                        break;
                    }
                    else
                    {
                        string content = html.Substring(i, j - i).Trim();
                        if (!string.IsNullOrEmpty(content))
                            contentQueue.Enqueue(content);
                        i = j;
                    }
                }
            }

            // Kiểm tra tính hợp lệ của stack tag
            if (tagStack.Count > 0)
                throw new Exception("Thiếu thẻ đóng");

            // Tạo nội dung kết quả
            string result = "";
            while (contentQueue.Count > 0)
            {
                result += contentQueue.Dequeue() + Environment.NewLine;
            }

            return result;
        }
    }
}
