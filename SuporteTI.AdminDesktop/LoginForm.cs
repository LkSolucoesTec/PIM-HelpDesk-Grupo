using SuporteTI.AdminDesktop.Services;
using System;
using System.Drawing; // <-- Adicionado para a Imagem
using System.IO;      // <-- Adicionado para o Caminho
using System.Windows.Forms;

namespace SuporteTI.AdminDesktop
{
    public partial class LoginForm : Form
    {
        private readonly ApiClient _apiClient;

        public LoginForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient();

            // Valores padrão para teste rápido
            txtLogin.Text = "gerencia";
            txtSenha.Text = "gerencia123";

            // --- A CORREÇÃO DA IMAGEM ---
            try
            {
                // Carrega a imagem da pasta "Images" que copiamos
                pictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Images", "iasistente.png"));
            }
            catch (Exception ex)
            {
                // Se a imagem falhar, não quebra o app
                MessageBox.Show("Mascote não encontrado. " + ex.Message, "Aviso de Recurso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            btnLogin.Text = "Entrando...";

            try
            {
                var user = await _apiClient.LoginAsync(txtLogin.Text, txtSenha.Text);

                var adminPanel = new AdminPanelForm();
                adminPanel.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
                btnLogin.Text = "Entrar";
            }
        }
    }
}