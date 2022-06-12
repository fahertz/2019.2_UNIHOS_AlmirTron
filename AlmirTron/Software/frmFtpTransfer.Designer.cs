namespace AlmirTron.Software
{
    partial class frmAlmirTron_v3
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
            this.btnWorkingStopped = new System.Windows.Forms.Button();
            this.pcbBotInfo = new System.Windows.Forms.PictureBox();
            this.lsbLogList = new System.Windows.Forms.ListBox();
            this.lblLogList = new System.Windows.Forms.Label();
            this.lblErrorList = new System.Windows.Forms.Label();
            this.lsbErrorList = new System.Windows.Forms.ListBox();
            this.btnSolveErrors = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.timerPedido = new System.Windows.Forms.Timer(this.components);
            this.timerRetorno = new System.Windows.Forms.Timer(this.components);
            this.timerWorking = new System.Windows.Forms.Timer(this.components);
            this.timerNota = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pcbBotInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnWorkingStopped
            // 
            this.btnWorkingStopped.BackColor = System.Drawing.Color.Green;
            this.btnWorkingStopped.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkingStopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkingStopped.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnWorkingStopped.Location = new System.Drawing.Point(368, 302);
            this.btnWorkingStopped.Name = "btnWorkingStopped";
            this.btnWorkingStopped.Size = new System.Drawing.Size(200, 74);
            this.btnWorkingStopped.TabIndex = 14;
            this.btnWorkingStopped.Text = "Working...";
            this.btnWorkingStopped.UseVisualStyleBackColor = false;
            this.btnWorkingStopped.Click += new System.EventHandler(this.btnWorkingStopped_Click);
            // 
            // pcbBotInfo
            // 
            this.pcbBotInfo.BackColor = System.Drawing.Color.Transparent;
            this.pcbBotInfo.BackgroundImage = global::AlmirTron.Properties.Resources.AlmirTron;
            this.pcbBotInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbBotInfo.Location = new System.Drawing.Point(368, 163);
            this.pcbBotInfo.Name = "pcbBotInfo";
            this.pcbBotInfo.Size = new System.Drawing.Size(200, 133);
            this.pcbBotInfo.TabIndex = 17;
            this.pcbBotInfo.TabStop = false;
            // 
            // lsbLogList
            // 
            this.lsbLogList.FormattingEnabled = true;
            this.lsbLogList.Location = new System.Drawing.Point(19, 47);
            this.lsbLogList.Name = "lsbLogList";
            this.lsbLogList.Size = new System.Drawing.Size(331, 433);
            this.lsbLogList.TabIndex = 18;
            // 
            // lblLogList
            // 
            this.lblLogList.AutoSize = true;
            this.lblLogList.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogList.Location = new System.Drawing.Point(13, 13);
            this.lblLogList.Name = "lblLogList";
            this.lblLogList.Size = new System.Drawing.Size(117, 31);
            this.lblLogList.TabIndex = 19;
            this.lblLogList.Text = "Log List";
            // 
            // lblErrorList
            // 
            this.lblErrorList.AutoSize = true;
            this.lblErrorList.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorList.Location = new System.Drawing.Point(579, 18);
            this.lblErrorList.Name = "lblErrorList";
            this.lblErrorList.Size = new System.Drawing.Size(134, 31);
            this.lblErrorList.TabIndex = 21;
            this.lblErrorList.Text = "Error List";
            // 
            // lsbErrorList
            // 
            this.lsbErrorList.FormattingEnabled = true;
            this.lsbErrorList.Location = new System.Drawing.Point(585, 52);
            this.lsbErrorList.Name = "lsbErrorList";
            this.lsbErrorList.Size = new System.Drawing.Size(331, 433);
            this.lsbErrorList.TabIndex = 20;
            this.lsbErrorList.SelectedIndexChanged += new System.EventHandler(this.lsbErrorList_SelectedIndexChanged);
            // 
            // btnSolveErrors
            // 
            this.btnSolveErrors.Location = new System.Drawing.Point(841, 23);
            this.btnSolveErrors.Name = "btnSolveErrors";
            this.btnSolveErrors.Size = new System.Drawing.Size(75, 23);
            this.btnSolveErrors.TabIndex = 22;
            this.btnSolveErrors.Text = "Solve Errors";
            this.btnSolveErrors.UseVisualStyleBackColor = true;
            this.btnSolveErrors.Click += new System.EventHandler(this.btnSolveErrors_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnOptions.Location = new System.Drawing.Point(368, 47);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(200, 35);
            this.btnOptions.TabIndex = 25;
            this.btnOptions.Text = "Options";
            this.btnOptions.UseVisualStyleBackColor = false;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // timerPedido
            // 
            this.timerPedido.Tick += new System.EventHandler(this.timerPedido_Tick);
            // 
            // timerRetorno
            // 
            this.timerRetorno.Tick += new System.EventHandler(this.timerRetorno_Tick);
            // 
            // timerWorking
            // 
            this.timerWorking.Tick += new System.EventHandler(this.timerWorking_Tick);
            // 
            // timerNota
            // 
            this.timerNota.Tick += new System.EventHandler(this.timerNota_Tick);
            // 
            // frmAlmirTron_v3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 497);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnSolveErrors);
            this.Controls.Add(this.lblErrorList);
            this.Controls.Add(this.lsbErrorList);
            this.Controls.Add(this.lblLogList);
            this.Controls.Add(this.lsbLogList);
            this.Controls.Add(this.pcbBotInfo);
            this.Controls.Add(this.btnWorkingStopped);
            this.Name = "frmAlmirTron_v3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AlmirTron 3.0";
            this.Load += new System.EventHandler(this.frmAlmirTron_v3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcbBotInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnWorkingStopped;
        private System.Windows.Forms.PictureBox pcbBotInfo;
        private System.Windows.Forms.ListBox lsbLogList;
        private System.Windows.Forms.Label lblLogList;
        private System.Windows.Forms.Label lblErrorList;
        private System.Windows.Forms.ListBox lsbErrorList;
        private System.Windows.Forms.Button btnSolveErrors;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Timer timerPedido;
        private System.Windows.Forms.Timer timerRetorno;
        private System.Windows.Forms.Timer timerWorking;
        private System.Windows.Forms.Timer timerNota;
    }
}