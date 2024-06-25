using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProjetoModelo
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
        }
        bool load = false;
        Cliente cliente = new Cliente();
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void frmCliente_Load(object sender, EventArgs e)
        {
            CarregarGridCliente();
            CarregarEstados();
            load = true;
        }
        private void CarregarEstados()
        {
            try
            {
                cboEstado.DataSource = Global.ConsultarEstados();
                cboEstado.DisplayMember = "Estado";
                cboEstado.ValueMember = "Id";
                cboEstado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CarregarCidades()
        {
            if (!load)
            {
                return;
            }

            try
            {
                int estado = Convert.ToInt32(cboEstado.SelectedValue);
                cboCidade.DataSource = Global.ConsultarCidades(estado);
                cboCidade.DisplayMember = "Cidade";
                cboCidade.ValueMember = "Id";
                cboCidade.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidades();
        }
        private void LimparCampos()
        {
            cliente = new Cliente();
            txtPesquisa.Clear();
            txtNome.Clear();
            dtpDataNascimento.Value = DateTime.Parse("01/01/1900");
            txtCPF.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            txtBairro.Clear();
            txtCEP.Clear();
            txtCelular.Clear();
            rdbMasculino.Checked = true;
            rdbFeminino.Checked = false;
            cboCidade.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            txtPesquisa.Focus();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void txtCPF_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text);
        }
        private void txtCEP_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text);
        }
        private void rdbNome_CheckedChanged(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void txtPesquisa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rdbCPF.Checked)
            {
                e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text);
            }
        }
        private void CarregarGridCliente()
        {
            try
            {
                grdDados.DataSource = cliente.Consultar();
                //Ocultando colunas
                grdDados.Columns[0].Visible = false;
                grdDados.Columns[2].Visible = false;
                grdDados.Columns[4].Visible = false;
                grdDados.Columns[5].Visible = false;
                grdDados.Columns[7].Visible = false;
                //Definindo cabeçalhos
                grdDados.Columns[1].HeaderText = "Nome";
                grdDados.Columns[3].HeaderText = "CPF";
                grdDados.Columns[6].HeaderText = "Celular";
                //Definindo largura das colunas
                grdDados.Columns[1].Width = 250;
                grdDados.Columns[3].Width = 100;
                grdDados.Columns[6].Width = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            cliente = new Cliente();
            if (rdbNome.Checked)
            {
                cliente.Nome = txtPesquisa.Text;
                CarregarGridCliente();
            }
            else if (rdbCPF.Checked && txtPesquisa.Text.Length == 11)
            {
                cliente.CPF = txtPesquisa.Text;
                CarregarGridCliente();
            }
        }
        private string ValidarPreenchimento()
        {
            string msgErro = string.Empty;
            try
            {
                if (txtNome.Text == string.Empty)
                {
                    msgErro = "Preencha o campo NOME.\n";
                }
                if (dtpDataNascimento.Value == DateTime.Parse("01/01/1900"))
                {
                    msgErro += "Preencha a DATA DE NASCIMENTO.\n";
                }
                if (txtCPF.Text == string.Empty)
                {
                    msgErro += "Preencha o CPF.\n";
                }
                else
                {
                    Cliente c = new Cliente();
                    c.CPF = txtCPF.Text;
                    c.Consultar();
                    if (cliente.Id == 0 && c.Id != 0 ||
                        cliente.Id != 0 && c.Id != 0 && cliente.Id != c.Id)
                    {
                        msgErro += "Cliente já existente.\n";
                    }
                }
                if (txtEmail.Text == string.Empty)
                {
                    msgErro += "Preencha o campo E-MAIL\n";
                }
                else
                {
                    try
                    {
                        MailAddress ma = new MailAddress(txtEmail.Text);                        
                    }
                    catch
                    {
                        msgErro += "Campo EMAIL inválido.\n";
                    }
                }
                if (txtEndereco.Text == string.Empty)
                {
                    msgErro += "Preencha o campo ENDEREÇO\n";
                }
                if (txtNumero.Text == string.Empty)
                {
                    msgErro += "Preencha o campo NÚMERO.\n";
                }
                if (txtBairro.Text == string.Empty)
                {
                    msgErro += "Preencha o campo BAIRRO.\n";
                }
                if (txtCEP.Text == string.Empty)
                {
                    msgErro += "Preencha o campo CEP.\n";
                }
                if (cboCidade.SelectedIndex==-1)
                {
                    msgErro += "Selecione o campo CIDADE.\n";
                }
                if (cboEstado.SelectedIndex == -1)
                {
                    msgErro += "Selecione o campo ESTADO.\n";
                }
                if (txtCelular.Text == string.Empty)
                {
                    msgErro += "Preencha o campo CELULAR.\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return msgErro;
        }
    }
}
