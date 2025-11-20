using SuporteTI.AdminDesktop.Services;
using SuporteTI.Shared.Models;
using System;
using System.Windows.Forms;

namespace SuporteTI.AdminDesktop
{
    public partial class ConfiguracoesForm : Form
    {
        private readonly ApiClient _apiClient;

        public ConfiguracoesForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        private async void ConfiguracoesForm_Load(object sender, EventArgs e)
        {
            await CarregarCategorias();
        }

        private async Task CarregarCategorias()
        {
            try
            {
                var categorias = await _apiClient.GetCategoriasAsync();
                listBoxCategorias.DataSource = null;
                listBoxCategorias.DataSource = categorias;
                listBoxCategorias.DisplayMember = "Nome"; // Mostra o nome
                listBoxCategorias.ValueMember = "Id"; // Guarda o ID
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao carregar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNovaCategoria.Text)) return;
            try
            {
                await _apiClient.CreateCategoriaAsync(txtNovaCategoria.Text);
                txtNovaCategoria.Text = "";
                await CarregarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao salvar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (listBoxCategorias.SelectedItem == null) return;

            var categoria = (Categoria)listBoxCategorias.SelectedItem;

            if (MessageBox.Show($"Tem certeza?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteCategoriaAsync(categoria.Id);
                    await CarregarCategorias();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao excluir", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}