namespace messenger
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Готово = new Guna.UI.WinForms.GunaButton();
            this.Телефон = new Guna.UI.WinForms.GunaTextBox();
            this.Пароль = new Guna.UI.WinForms.GunaTextBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 33);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 35.25F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.label1.Location = new System.Drawing.Point(12, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 124);
            this.label1.TabIndex = 1;
            this.label1.Text = "Добро \r\nпожаловать!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.label2.Location = new System.Drawing.Point(18, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Нет аккаунта?";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Underline);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.label3.Location = new System.Drawing.Point(18, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 28);
            this.label3.TabIndex = 3;
            this.label3.Text = "Зарегистрироваться";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.label4.Location = new System.Drawing.Point(442, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 46);
            this.label4.TabIndex = 6;
            this.label4.Text = "Авторизация";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Готово
            // 
            this.Готово.AnimationHoverSpeed = 0.07F;
            this.Готово.AnimationSpeed = 0.03F;
            this.Готово.BackColor = System.Drawing.Color.Transparent;
            this.Готово.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.Готово.BorderColor = System.Drawing.Color.Black;
            this.Готово.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Готово.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Готово.ForeColor = System.Drawing.Color.White;
            this.Готово.Image = null;
            this.Готово.ImageSize = new System.Drawing.Size(20, 20);
            this.Готово.Location = new System.Drawing.Point(726, 539);
            this.Готово.Name = "Готово";
            this.Готово.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.Готово.OnHoverBorderColor = System.Drawing.Color.Black;
            this.Готово.OnHoverForeColor = System.Drawing.Color.Transparent;
            this.Готово.OnHoverImage = null;
            this.Готово.OnPressedColor = System.Drawing.Color.Black;
            this.Готово.Radius = 7;
            this.Готово.Size = new System.Drawing.Size(160, 42);
            this.Готово.TabIndex = 8;
            this.Готово.Text = "Готово";
            this.Готово.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Готово.Click += new System.EventHandler(this.gunaButton1_Click);
            // 
            // Телефон
            // 
            this.Телефон.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Телефон.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Телефон.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Телефон.BorderSize = 2;
            this.Телефон.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Телефон.FocusedBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Телефон.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.Телефон.FocusedForeColor = System.Drawing.SystemColors.ControlText;
            this.Телефон.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Телефон.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.Телефон.Location = new System.Drawing.Point(395, 305);
            this.Телефон.Name = "Телефон";
            this.Телефон.PasswordChar = '\0';
            this.Телефон.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Телефон.Size = new System.Drawing.Size(338, 37);
            this.Телефон.TabIndex = 9;
            this.Телефон.Text = "Телефон";
            // 
            // Пароль
            // 
            this.Пароль.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Пароль.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Пароль.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Пароль.BorderSize = 2;
            this.Пароль.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Пароль.FocusedBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.Пароль.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.Пароль.FocusedForeColor = System.Drawing.SystemColors.ControlText;
            this.Пароль.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Пароль.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.Пароль.Location = new System.Drawing.Point(395, 370);
            this.Пароль.Name = "Пароль";
            this.Пароль.PasswordChar = '\0';
            this.Пароль.Size = new System.Drawing.Size(338, 37);
            this.Пароль.TabIndex = 10;
            this.Пароль.Text = "Пароль";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(933, 639);
            this.Controls.Add(this.Пароль);
            this.Controls.Add(this.Телефон);
            this.Controls.Add(this.Готово);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "я";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Guna.UI.WinForms.GunaButton Готово;
        private Guna.UI.WinForms.GunaTextBox Телефон;
        private Guna.UI.WinForms.GunaTextBox Пароль;
    }
}

