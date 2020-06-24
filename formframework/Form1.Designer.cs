namespace formframework
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.DisplayImage = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Return = new System.Windows.Forms.RadioButton();
            this.Circle = new System.Windows.Forms.RadioButton();
            this.Square = new System.Windows.Forms.RadioButton();
            this.Triangle = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Status = new System.Windows.Forms.Label();
            this.Stage1 = new System.Windows.Forms.PictureBox();
            this.Stage2 = new System.Windows.Forms.PictureBox();
            this.Stage3 = new System.Windows.Forms.PictureBox();
            this.Stage4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Stage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage4)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayImage
            // 
            this.DisplayImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DisplayImage.Location = new System.Drawing.Point(12, 12);
            this.DisplayImage.Name = "DisplayImage";
            this.DisplayImage.Size = new System.Drawing.Size(300, 300);
            this.DisplayImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.DisplayImage.TabIndex = 0;
            this.DisplayImage.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Return
            // 
            this.Return.AutoSize = true;
            this.Return.Location = new System.Drawing.Point(6, 19);
            this.Return.Name = "Return";
            this.Return.Size = new System.Drawing.Size(57, 17);
            this.Return.TabIndex = 1;
            this.Return.TabStop = true;
            this.Return.Text = "Return";
            this.Return.UseVisualStyleBackColor = true;
            this.Return.CheckedChanged += new System.EventHandler(this.Return_CheckedChanged);
            // 
            // Circle
            // 
            this.Circle.AutoSize = true;
            this.Circle.Location = new System.Drawing.Point(6, 88);
            this.Circle.Name = "Circle";
            this.Circle.Size = new System.Drawing.Size(51, 17);
            this.Circle.TabIndex = 2;
            this.Circle.TabStop = true;
            this.Circle.Text = "Circle";
            this.Circle.UseVisualStyleBackColor = true;
            this.Circle.CheckedChanged += new System.EventHandler(this.Return_CheckedChanged);
            // 
            // Square
            // 
            this.Square.AutoSize = true;
            this.Square.Location = new System.Drawing.Point(6, 65);
            this.Square.Name = "Square";
            this.Square.Size = new System.Drawing.Size(59, 17);
            this.Square.TabIndex = 3;
            this.Square.TabStop = true;
            this.Square.Text = "Square";
            this.Square.UseVisualStyleBackColor = true;
            this.Square.CheckedChanged += new System.EventHandler(this.Return_CheckedChanged);
            // 
            // Triangle
            // 
            this.Triangle.AutoSize = true;
            this.Triangle.Location = new System.Drawing.Point(6, 42);
            this.Triangle.Name = "Triangle";
            this.Triangle.Size = new System.Drawing.Size(63, 17);
            this.Triangle.TabIndex = 4;
            this.Triangle.TabStop = true;
            this.Triangle.Text = "Triangle";
            this.Triangle.UseVisualStyleBackColor = true;
            this.Triangle.CheckedChanged += new System.EventHandler(this.Return_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Return);
            this.groupBox1.Controls.Add(this.Circle);
            this.groupBox1.Controls.Add(this.Square);
            this.groupBox1.Controls.Add(this.Triangle);
            this.groupBox1.Location = new System.Drawing.Point(318, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 130);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choices";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(318, 149);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(44, 13);
            this.Status.TabIndex = 6;
            this.Status.Text = "Nothing";
            // 
            // Stage1
            // 
            this.Stage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stage1.Location = new System.Drawing.Point(525, 13);
            this.Stage1.Name = "Stage1";
            this.Stage1.Size = new System.Drawing.Size(300, 300);
            this.Stage1.TabIndex = 7;
            this.Stage1.TabStop = false;
            // 
            // Stage2
            // 
            this.Stage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stage2.Location = new System.Drawing.Point(831, 12);
            this.Stage2.Name = "Stage2";
            this.Stage2.Size = new System.Drawing.Size(300, 300);
            this.Stage2.TabIndex = 8;
            this.Stage2.TabStop = false;
            // 
            // Stage3
            // 
            this.Stage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stage3.Location = new System.Drawing.Point(525, 319);
            this.Stage3.Name = "Stage3";
            this.Stage3.Size = new System.Drawing.Size(300, 300);
            this.Stage3.TabIndex = 9;
            this.Stage3.TabStop = false;
            // 
            // Stage4
            // 
            this.Stage4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Stage4.Location = new System.Drawing.Point(831, 318);
            this.Stage4.Name = "Stage4";
            this.Stage4.Size = new System.Drawing.Size(300, 300);
            this.Stage4.TabIndex = 10;
            this.Stage4.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1399, 645);
            this.Controls.Add(this.Stage4);
            this.Controls.Add(this.Stage3);
            this.Controls.Add(this.Stage2);
            this.Controls.Add(this.Stage1);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DisplayImage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DisplayImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Stage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Stage4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DisplayImage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton Return;
        private System.Windows.Forms.RadioButton Circle;
        private System.Windows.Forms.RadioButton Square;
        private System.Windows.Forms.RadioButton Triangle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.PictureBox Stage1;
        private System.Windows.Forms.PictureBox Stage2;
        private System.Windows.Forms.PictureBox Stage3;
        private System.Windows.Forms.PictureBox Stage4;
    }
}

