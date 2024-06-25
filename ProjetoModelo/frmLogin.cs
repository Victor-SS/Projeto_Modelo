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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string senhaCriptografada = Global.Criptografar(txtSenha.Text);
                Usuario usuario = new Usuario();
                usuario.Login = txtUsuario.Text;
                usuario.Consultar();
                if (usuario.Id == 0)
                {
                    MessageBox.Show("Usuário e/ou senha inválidos", "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }                
                if (!usuario.Autenticar(senhaCriptografada))
                {
                    MessageBox.Show("Usuário e/ou senha inválidos", "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!usuario.Ativo)
                {
                    MessageBox.Show("Usuário inativo", "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show($"Bem vindo {usuario.Nome}.", "Login",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
                Global.IdUsuarioLogado = usuario.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro --> " + ex.Message, "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
