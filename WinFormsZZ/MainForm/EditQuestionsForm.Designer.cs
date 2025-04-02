namespace MainForm
{
    partial class EditQuestionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonОК = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxQuestions = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBoxCategories = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonОК
            // 
            this.buttonОК.Location = new System.Drawing.Point(9, 299);
            this.buttonОК.Name = "buttonОК";
            this.buttonОК.Size = new System.Drawing.Size(75, 23);
            this.buttonОК.TabIndex = 0;
            this.buttonОК.Text = "ОК";
            this.buttonОК.UseVisualStyleBackColor = true;
            this.buttonОК.Click += new System.EventHandler(this.buttonОК_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(147, 299);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxQuestions);
            this.panel1.Location = new System.Drawing.Point(243, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 331);
            this.panel1.TabIndex = 2;
            // 
            // textBoxQuestions
            // 
            this.textBoxQuestions.Location = new System.Drawing.Point(13, 9);
            this.textBoxQuestions.Multiline = true;
            this.textBoxQuestions.Name = "textBoxQuestions";
            this.textBoxQuestions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxQuestions.Size = new System.Drawing.Size(337, 313);
            this.textBoxQuestions.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBoxCategories);
            this.panel2.Controls.Add(this.buttonОК);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Location = new System.Drawing.Point(3, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 331);
            this.panel2.TabIndex = 3;
            // 
            // listBoxCategories
            // 
            this.listBoxCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxCategories.FormattingEnabled = true;
            this.listBoxCategories.ItemHeight = 24;
            this.listBoxCategories.Location = new System.Drawing.Point(9, 9);
            this.listBoxCategories.Name = "listBoxCategories";
            this.listBoxCategories.Size = new System.Drawing.Size(213, 244);
            this.listBoxCategories.TabIndex = 2;
            this.listBoxCategories.SelectedIndexChanged += new System.EventHandler(this.listBoxCategories_SelectedIndexChanged);
            // 
            // EditQuestionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 354);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "EditQuestionsForm";
            this.Text = "Редактор вопросов";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonОК;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxQuestions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBoxCategories;
    }
}