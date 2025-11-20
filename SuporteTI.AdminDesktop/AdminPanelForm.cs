using SuporteTI.AdminDesktop.Services;
using SuporteTI.Shared.Models.Dto;
using System.Data;
using System.Windows.Forms;

namespace SuporteTI.AdminDesktop
{
    public partial class AdminPanelForm : Form
    {
        private readonly ApiClient _apiClient;
        private List<UserListDto> _listaUsuarios; // Cache local

        public AdminPanelForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
            cmbPapel.SelectedIndexChanged += CmbPapel_SelectedIndexChanged;
        }
        private void CmbPapel_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        // Evento que roda quando o formulário é exibido
        private async void AdminPanelForm_Load(object sender, EventArgs e)
        {
            lblBoasVindas.Text = $"Bem-vindo(a), {ApiClient.CurrentUser.Nome}!";
            await CarregarUsuarios();
        }

        private async Task CarregarUsuarios()
        {
            try
            {
                // 1. Busca os dados da API
                _listaUsuarios = await _apiClient.GetUsuariosAsync();

                // 2. Preenche o DataGridView (a tabela)
                dataGridViewUsuarios.DataSource = null;
                dataGridViewUsuarios.DataSource = _listaUsuarios;
                dataGridViewUsuarios.Columns["Id"].Width = 50; // Ajusta colunas
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
                // 1. Monta o DTO com os dados do formulário
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

                // 2. Envia para a API
                await _apiClient.CreateUsuarioAsync(novoUsuario);

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Limpa o formulário e recarrega a lista
                LimparFormulario();
                await CarregarUsuarios();
                tabControl.SelectedTab = tabPageLista; // Muda para a aba da lista
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
                MessageBox.Show("Por favor, selecione um usuário na lista para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Pega o usuário selecionado
            var usuarioSelecionado = (UserListDto)dataGridViewUsuarios.SelectedRows[0].DataBoundItem;

            // 2. Confirmação
            var confirm = MessageBox.Show($"Tem certeza que deseja excluir '{usuarioSelecionado.Nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // 3. Manda deletar na API
                    await _apiClient.DeleteUsuarioAsync(usuarioSelecionado.Id);
                    MessageBox.Show("Usuário excluído com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 4. Recarrega a lista
                    await CarregarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Fecha o App ao sair
        private void AdminPanelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var configForm = new ConfiguracoesForm();
            configForm.ShowDialog(); // Abre o novo formulário
        }
    }
}