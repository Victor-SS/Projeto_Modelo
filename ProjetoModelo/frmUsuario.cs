using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoModelo
{
    public partial class frmUsuario : Form
    {
        public frmUsuario()
        {
            InitializeComponent();
        }

        Usuario usuario = new Usuario();
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void LimparCampos()
        {
            usuario = new Usuario();
            txtPesquisa.Clear();
            txtUsuario.Clear();
            txtNome.Clear();
            txtSenha.Clear();
            txtConfirmacao.Clear();
            rdbAtivo.Checked = true;
            rdbInativo.Checked = false;
            txtPesquisa.Focus();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            usuario = new Usuario();
            usuario.Nome = txtPesquisa.Text;
            CarregarGridUsuario();
        }
        private void CarregarGridUsuario()
        {
            try
            {
                grdDados.DataSource = usuario.Consultar();
                grdDados.Columns[0].Visible = false;
                grdDados.Columns[3].Visible = false;
                grdDados.Columns[4].Visible = false;
                //Cabeçalho das colunas
                grdDados.Columns[1].HeaderText = "Usuário";
                grdDados.Columns[2].HeaderText = "Nome";
                //Largura das colunas
                grdDados.Columns[1].Width = 100;
                grdDados.Columns[2].Width = 250;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmUsuario_Load(object sender, EventArgs e)
        {
            CarregarGridUsuario();
        }
        private void grdDados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                usuario = new Usuario();
                usuario.Id = Convert.ToInt32(grdDados.SelectedRows[0].Cells[0].Value);
                usuario.Consultar();
                PreencherFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PreencherFormulario()
        {
            txtUsuario.Text = usuario.Login;
            txtNome.Text = usuario.Nome;
            txtSenha.Text = usuario.Password;
            txtConfirmacao.Text = usuario.Password;
            rdbAtivo.Checked = usuario.Ativo;
            rdbInativo.Checked = !usuario.Ativo;
        }
        private void PreencherClasse()
        {
            usuario.Login = txtUsuario.Text;
            usuario.Nome = txtNome.Text;
            if (usuario.Password != txtSenha.Text)
            {
                usuario.Password = Global.Criptografar(txtSenha.Text);
            }
            usuario.Ativo = rdbAtivo.Checked;
        }
        private string ValidarPreenchimento()
        {
            try
            {
                string msgErro = string.Empty;
                if (txtUsuario.Text == string.Empty)
                {
                    msgErro = "Preencha o USUÁRIO.\n";
                }
                else
                {
                    Usuario u = new Usuario();
                    u.Login = txtUsuario.Text;
                    u.Consultar();
                    if (usuario.Id == 0 && u.Id != 0 ||
                        usuario.Id != 0 && u.Id != 0 && usuario.Id != u.Id)
                    {
                        msgErro += "Usuário já existente.\n";
                    }
                }
                if (txtNome.Text == string.Empty)
                {
                    msgErro += "Preencha o NOME.\n";
                }
                if (txtSenha.Text == string.Empty)
                {
                    msgErro += "Preencha a SENHA.\n";
                }
                else if (txtSenha.Text != txtConfirmacao.Text)
                {
                    msgErro += "Confirmação da senha não confere.\n";
                }
                return msgErro;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagemErro = ValidarPreenchimento();
                if (mensagemErro != string.Empty)
                {
                    MessageBox.Show(mensagemErro, "Erro de Preenchimento",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                PreencherClasse();
                usuario.Gravar();
                MessageBox.Show("Usuário gravado com sucesso!",
                    "Cadastro de Usuários",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarGridUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
