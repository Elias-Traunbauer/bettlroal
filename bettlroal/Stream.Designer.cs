
namespace bettlroal
{
    partial class Stream
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
            this.pbStream = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbStream)).BeginInit();
            this.SuspendLayout();
            // 
            // pbStream
            // 
            this.pbStream.Location = new System.Drawing.Point(12, 12);
            this.pbStream.Name = "pbStream";
            this.pbStream.Size = new System.Drawing.Size(776, 426);
            this.pbStream.TabIndex = 0;
            this.pbStream.TabStop = false;
            // 
            // Stream
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbStream);
            this.Name = "Stream";
            this.Text = "Stream";
            this.Load += new System.EventHandler(this.Stream_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbStream)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pbStream;
    }
}