namespace SuporteTI.AdminDesktop
{
    partial class ConfiguracoesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.listBoxCategorias = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNovaCategoria = new System.Windows.Forms.TextBox();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxCategorias
            // 
            this.listBoxCategorias.FormattingEnabled = true;
            this.listBoxCategorias.ItemHeight = 20;
            this.listBoxCategorias.Location = new System.Drawing.Point(12, 12);
            this.listBoxCategorias.Name = "listBoxCategorias";
            this.listBoxCategorias.Size = new System.Drawing.Size(328, 304);
            this.listBoxCategorias.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nova Categoria:";
            // 
            // txtNovaCategoria
            // 
            this.txtNovaCategoria.Location = new System.Drawing.Point(12, 358);
            this.txtNovaCategoria.Name = "txtNovaCategoria";
            this.txtNovaCategoria.Size = new System.Drawing.Size(225, 27);
            this.txtNovaCategoria.TabIndex = 2;
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Location = new System.Drawing.Point(243, 357);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(97, 29);
            this.btnAdicionar.TabIndex = 3;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Location = new System.Drawing.Point(219, 409);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(121, 29);
            this.btnExcluir.TabIndex = 4;
            this.btnExcluir.Text = "Excluir Selecionado";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // ConfiguracoesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 450);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.txtNovaCategoria);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxCategorias);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfiguracoesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurações de Parâmetros";
            this.Load += new System.EventHandler(this.ConfiguracoesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBoxCategorias;
        private Label label1;
        private TextBox txtNovaCategoria;
        private Button btnAdicionar;
        private Button btnExcluir;
    }
}