namespace WindowsFormsWebKit
{
    partial class Form2
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
            this.btn_receive_email = new System.Windows.Forms.Button();
            this.richText_content = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_receive_email
            // 
            this.btn_receive_email.Location = new System.Drawing.Point(641, 12);
            this.btn_receive_email.Name = "btn_receive_email";
            this.btn_receive_email.Size = new System.Drawing.Size(75, 23);
            this.btn_receive_email.TabIndex = 0;
            this.btn_receive_email.Text = "Receive";
            this.btn_receive_email.UseVisualStyleBackColor = true;
            this.btn_receive_email.Click += new System.EventHandler(this.btn_receive_email_Click);
            // 
            // richText_content
            // 
            this.richText_content.Location = new System.Drawing.Point(12, 24);
            this.richText_content.Name = "richText_content";
            this.richText_content.Size = new System.Drawing.Size(567, 427);
            this.richText_content.TabIndex = 1;
            this.richText_content.Text = "";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 463);
            this.Controls.Add(this.richText_content);
            this.Controls.Add(this.btn_receive_email);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_receive_email;
        private System.Windows.Forms.RichTextBox richText_content;
    }
}