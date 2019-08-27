namespace WinFormsApp
{
    partial class SearchBoxForm
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
            this.SearchBoxComponent = new BackgroundWorkerComponents.SearchBoxComponent();
            this.SuspendLayout();
            // 
            // SearchBoxComponent
            // 
            this.SearchBoxComponent.Location = new System.Drawing.Point(12, 12);
            this.SearchBoxComponent.Name = "SearchBoxComponent";
            this.SearchBoxComponent.Size = new System.Drawing.Size(297, 79);
            this.SearchBoxComponent.TabIndex = 0;
            this.SearchBoxComponent.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SearchBoxComponent_DoWork);
            this.SearchBoxComponent.StartClicked += new System.Func<object>(this.SearchBoxComponent_StartClicked);
            // 
            // SearchBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 99);
            this.Controls.Add(this.SearchBoxComponent);
            this.Name = "SearchBoxForm";
            this.Text = "Search box";
            this.ResumeLayout(false);

        }

        #endregion

        private BackgroundWorkerComponents.SearchBoxComponent SearchBoxComponent;
    }
}

