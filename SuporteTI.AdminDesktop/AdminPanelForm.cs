using SuporteTI.AdminDesktop.Services;
using SuporteTI.Shared.Models.Dto;
using System; // Necessário
using System.Collections.Generic; // Necessário para List<>
using System.Threading.Tasks; // Necessário para Task
using System.Windows.Forms;

namespace SuporteTI.AdminDesktop
{
    public partial class AdminPanelForm : Form
    {
        private readonly ApiClient _apiClient;
        private List<UserListDto> _listaUsuarios;

        public AdminPanelForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            // Vincula o evento manualmente ou via designer (garanta que só tem um vínculo)
            cmbPapel.SelectedIndexChanged += CmbPapel_SelectedIndexChanged;
        }

        private void CmbPapel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // --- CORREÇÃO DO ERRO (NullReferenceException) ---
            // Se a seleção for nula (ex: ao limpar o formulário), paramos aqui.
            if (cmbPapel.SelectedItem == null) return;

            var papel = cmbPapel.SelectedItem.ToString();

            if (papel == "Usuario")
            {
                txtSetor.Enabled = true;
                txtEspecialidade.Enabled = false;
                txtEspecialidade.Text = "";
            }
            else if (papel == "Tecnico")
            {
                txtSetor.Enabled = false;
                txtSetor.Text = "";
                txtEspecialidade.Enabled = true;
            }
            else // Gerencia
            {
                txtSetor.Enabled = false;
                txtEspecialidade.Enabled = false;
            }
        }

        private async void AdminPanelForm_Load(object sender, EventArgs e)
        {
            // Verifica se o usuário está logado antes de acessar a propriedade
            string nomeUsuario = ApiClient.CurrentUser?.Nome ?? "Administrador";
            lblBoasVindas.Text = $"Bem-vindo(a), {nomeUsuario}!";
            await CarregarUsuarios();
        }

        private async Task CarregarUsuarios()
        {
            try
            {
                _listaUsuarios = await _apiClient.GetUsuariosAsync();
                dataGridViewUsuarios.DataSource = null;
                dataGridViewUsuarios.DataSource = _listaUsuarios;

                if (dataGridViewUsuarios.Columns["Id"] != null)
                    dataGridViewUsuarios.Columns["Id"].Width = 50;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao carregar dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ABA DE CADASTRO ---
        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text) ||
                cmbPapel.SelectedItem == null)
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var novoUsuario = new UserCreateDto
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    Senha = txtSenha.Text,
                    Papel = cmbPapel.SelectedItem.ToString(),
                    Setor = txtSetor.Text,
                    Especialidade = txtEspecialidade.Text,
                    Telefone = txtTelefone.Text
                };

                await _apiClient.CreateUsuarioAsync(novoUsuario);

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparFormulario();
                await CarregarUsuarios();
                tabControl.SelectedTab = tabPageLista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao salvar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            txtNome.Text = "";
            txtEmail.Text = "";
            txtSenha.Text = "";
            // Isso dispara o evento SelectedIndexChanged. 
            // A correção no início desse método evita o crash.
            cmbPapel.SelectedIndex = -1;
            txtSetor.Text = "";
            txtEspecialidade.Text = "";
            txtTelefone.Text = "";
        }

        // --- ABA DA LISTA ---
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuarioSelecionado = (UserListDto)dataGridViewUsuarios.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Excluir '{usuarioSelecionado.Nome}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteUsuarioAsync(usuarioSelecionado.Id);
                    MessageBox.Show("Usuário excluído.", "Sucesso");
                    await CarregarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AdminPanelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var configForm = new ConfiguracoesForm();
            configForm.ShowDialog();
        }
    }
}