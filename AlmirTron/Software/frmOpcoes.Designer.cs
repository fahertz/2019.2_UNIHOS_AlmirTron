namespace AlmirTron.Software
{
    partial class frmOpcoes
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
            this.lblEndereco_Servidor = new System.Windows.Forms.Label();
            this.txtEnderecoServidor = new System.Windows.Forms.TextBox();
            this.lblOpcoes = new System.Windows.Forms.Label();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEndereco_Servidor
            // 
            this.lblEndereco_Servidor.AutoSize = true;
            this.lblEndereco_Servidor.Location = new System.Drawing.Point(9, 54);
            this.lblEndereco_Servidor.Name = "lblEndereco_Servidor";
            this.lblEndereco_Servidor.Size = new System.Drawing.Size(95, 13);
            this.lblEndereco_Servidor.TabIndex = 0;
            this.lblEndereco_Servidor.Text = "Endereço Servidor";
            // 
            // txtEnderecoServidor
            // 
            this.txtEnderecoServidor.Location = new System.Drawing.Point(12, 70);
            this.txtEnderecoServidor.Name = "txtEnderecoServidor";
            this.txtEnderecoServidor.Size = new System.Drawing.Size(139, 20);
            this.txtEnderecoServidor.TabIndex = 1;
            // 
            // lblOpcoes
            // 
            this.lblOpcoes.AutoSize = true;
            this.lblOpcoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpcoes.Location = new System.Drawing.Point(8, 9);
            this.lblOpcoes.Name = "lblOpcoes";
            this.lblOpcoes.Size = new System.Drawing.Size(143, 39);
            this.lblOpcoes.TabIndex = 2;
            this.lblOpcoes.Text = "Opções";
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(407, 148);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 3;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(326, 148);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(12, 114);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(193, 20);
            this.txtEmail.TabIndex = 5;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(12, 98);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(87, 13);
            this.lblEmail.TabIndex = 6;
            this.lblEmail.Text = "Email notificacao";
            // 
            // frmOpcoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 183);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.lblOpcoes);
            this.Controls.Add(this.txtEnderecoServidor);
            this.Controls.Add(this.lblEndereco_Servidor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpcoes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opções";
            this.Load += new System.EventHandler(this.frmOpcoes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEndereco_Servidor;
        private System.Windows.Forms.TextBox txtEnderecoServidor;
        private System.Windows.Forms.Label lblOpcoes;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
    }
}