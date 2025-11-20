namespace SuporteTI.AdminDesktop
{
    partial class AdminPanelForm
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
            tabControl = new TabControl();
            tabPageLista = new TabPage();
            btnExcluir = new Button();
            dataGridViewUsuarios = new DataGridView();
            tabPageCadastro = new TabPage();
            btnSalvar = new Button();
            groupBox1 = new GroupBox();
            txtTelefone = new TextBox();
            label7 = new Label();
            txtEspecialidade = new TextBox();
            label6 = new Label();
            txtSetor = new TextBox();
            label5 = new Label();
            cmbPapel = new ComboBox();
            label4 = new Label();
            txtSenha = new TextBox();
            label3 = new Label();
            txtEmail = new TextBox();
            label2 = new Label();
            txtNome = new TextBox();
            label1 = new Label();
            lblBoasVindas = new Label();
            btnConfig = new Button();
            tabControl.SuspendLayout();
            tabPageLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).BeginInit();
            tabPageCadastro.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageLista);
            tabControl.Controls.Add(tabPageCadastro);
            tabControl.Location = new Point(15, 56);
            tabControl.Margin = new Padding(4, 4, 4, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(950, 491);
            tabControl.TabIndex = 0;
            // 
            // tabPageLista
            // 
            tabPageLista.Controls.Add(btnConfig);
            tabPageLista.Controls.Add(btnExcluir);
            tabPageLista.Controls.Add(dataGridViewUsuarios);
            tabPageLista.Location = new Point(4, 34);
            tabPageLista.Margin = new Padding(4, 4, 4, 4);
            tabPageLista.Name = "tabPageLista";
            tabPageLista.Padding = new Padding(4, 4, 4, 4);
            tabPageLista.Size = new Size(942, 453);
            tabPageLista.TabIndex = 0;
            tabPageLista.Text = "Lista de Usuários";
            tabPageLista.UseVisualStyleBackColor = true;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExcluir.Location = new Point(779, 395);
            btnExcluir.Margin = new Padding(4, 4, 4, 4);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(154, 48);
            btnExcluir.TabIndex = 1;
            btnExcluir.Text = "Excluir Selecionado";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // dataGridViewUsuarios
            // 
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsuarios.Location = new Point(8, 8);
            dataGridViewUsuarios.Margin = new Padding(4, 4, 4, 4);
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.Name = "dataGridViewUsuarios";
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.RowHeadersWidth = 51;
            dataGridViewUsuarios.RowTemplate.Height = 29;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.Size = new Size(925, 380);
            dataGridViewUsuarios.TabIndex = 0;
            // 
            // tabPageCadastro
            // 
            tabPageCadastro.Controls.Add(btnSalvar);
            tabPageCadastro.Controls.Add(groupBox1);
            tabPageCadastro.Location = new Point(4, 34);
            tabPageCadastro.Margin = new Padding(4, 4, 4, 4);
            tabPageCadastro.Name = "tabPageCadastro";
            tabPageCadastro.Padding = new Padding(4, 4, 4, 4);
            tabPageCadastro.Size = new Size(942, 453);
            tabPageCadastro.TabIndex = 1;
            tabPageCadastro.Text = "Cadastrar Novo";
            tabPageCadastro.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            btnSalvar.Location = new Point(785, 381);
            btnSalvar.Margin = new Padding(4, 4, 4, 4);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(148, 61);
            btnSalvar.TabIndex = 1;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtTelefone);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(txtEspecialidade);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(txtSetor);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cmbPapel);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtSenha);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtNome);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(8, 8);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(925, 366);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dados do Usuário (Baseado nas fichas do PIM)";
            // 
            // txtTelefone
            // 
            txtTelefone.Location = new Point(524, 171);
            txtTelefone.Margin = new Padding(4, 4, 4, 4);
            txtTelefone.Name = "txtTelefone";
            txtTelefone.Size = new Size(376, 31);
            txtTelefone.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(524, 142);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(81, 25);
            label7.TabIndex = 12;
            label7.Text = "Telefone:";
            // 
            // txtEspecialidade
            // 
            txtEspecialidade.Location = new Point(524, 284);
            txtEspecialidade.Margin = new Padding(4, 4, 4, 4);
            txtEspecialidade.Name = "txtEspecialidade";
            txtEspecialidade.Size = new Size(376, 31);
            txtEspecialidade.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(524, 255);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(194, 25);
            label6.TabIndex = 10;
            label6.Text = "Especialidade (Técnico):";
            // 
            // txtSetor
            // 
            txtSetor.Location = new Point(26, 284);
            txtSetor.Margin = new Padding(4, 4, 4, 4);
            txtSetor.Name = "txtSetor";
            txtSetor.Size = new Size(376, 31);
            txtSetor.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(26, 255);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(133, 25);
            label5.TabIndex = 8;
            label5.Text = "Setor (Usuário):";
            // 
            // cmbPapel
            // 
            cmbPapel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPapel.FormattingEnabled = true;
            cmbPapel.Items.AddRange(new object[] { "Usuario", "Tecnico", "Gerencia" });
            cmbPapel.Location = new Point(524, 71);
            cmbPapel.Margin = new Padding(4, 4, 4, 4);
            cmbPapel.Name = "cmbPapel";
            cmbPapel.Size = new Size(376, 33);
            cmbPapel.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(524, 42);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(77, 25);
            label4.TabIndex = 6;
            label4.Text = "Papel (*)";
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(26, 171);
            txtSenha.Margin = new Padding(4, 4, 4, 4);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(376, 31);
            txtSenha.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 142);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(83, 25);
            label3.TabIndex = 4;
            label3.Text = "Senha (*)";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(26, 222);
            txtEmail.Margin = new Padding(4, 4, 4, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(376, 31);
            txtEmail.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 194);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(143, 25);
            label2.TabIndex = 2;
            label2.Text = "E-mail (Login) (*)";
            // 
            // txtNome
            // 
            txtNome.Location = new Point(26, 72);
            txtNome.Margin = new Padding(4, 4, 4, 4);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(376, 31);
            txtNome.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 44);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(168, 25);
            label1.TabIndex = 0;
            label1.Text = "Nome Completo (*)";
            // 
            // lblBoasVindas
            // 
            lblBoasVindas.AutoSize = true;
            lblBoasVindas.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblBoasVindas.Location = new Point(15, 11);
            lblBoasVindas.Margin = new Padding(4, 0, 4, 0);
            lblBoasVindas.Name = "lblBoasVindas";
            lblBoasVindas.Size = new Size(205, 32);
            lblBoasVindas.TabIndex = 1;
            lblBoasVindas.Text = "Bem-vindo(a), ...";
            // 
            // btnConfig
            // 
            btnConfig.Location = new Point(8, 395);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(172, 34);
            btnConfig.TabIndex = 2;
            btnConfig.Text = "Configurações";
            btnConfig.UseVisualStyleBackColor = true;
            btnConfig.Click += btnConfig_Click;
            // 
            // AdminPanelForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 562);
            Controls.Add(lblBoasVindas);
            Controls.Add(tabControl);
            Margin = new Padding(4, 4, 4, 4);
            Name = "AdminPanelForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Painel Administrativo - Gestão de Usuários";
            FormClosed += AdminPanelForm_FormClosed;
            Load += AdminPanelForm_Load;
            tabControl.ResumeLayout(false);
            tabPageLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsuarios).EndInit();
            tabPageCadastro.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPageLista;
        private TabPage tabPageCadastro;
        private Label lblBoasVindas;
        private DataGridView dataGridViewUsuarios;
        private Button btnExcluir;
        private GroupBox groupBox1;
        private Label label1;
        private ComboBox cmbPapel;
        private Label label4;
        private TextBox txtSenha;
        private Label label3;
        private TextBox txtEmail;
        private Label label2;
        private TextBox txtNome;
        private Label label5;
        private TextBox txtSetor;
        private TextBox txtEspecialidade;
        private Label label6;
        private TextBox txtTelefone;
        private Label label7;
        private Button btnSalvar;
        private Button btnConfig;
    }
}