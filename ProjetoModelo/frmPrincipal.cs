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
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }
        DateTime login;
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            login = DateTime.Now;
            timer1.Enabled = true;
            Left = 0;
            Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;

            Usuario usuario = new Usuario
            {
                Id = Global.IdUsuarioLogado
            };

            usuario.Consultar();
            lblUsuario.Text = $"Usuário: {usuario.Nome}";
            lblServidor.Text = $"Servidor: {Global.Servidor}";
            lblBanco.Text = $"Banco: {Global.Banco}";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - login;
            lblTempo.Text =
                $"Tempo: {ts.Hours.ToString("00")}:" +
                $"{ts.Minutes.ToString("00")}:" +
                $"{ts.Seconds.ToString("00")}";
        }
        private void AbrirForm(Form form)
        {
            foreach (Form filho in this.MdiChildren)
            {
                if (filho.Name == form.Name)
                {
                    filho.Activate();
                    return;
                }
            }
            form.MdiParent = this;
            form.Show();

        }
        private void mnuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirForm(new frmUsuario());
        }
        private void mnuCliente_Click(object sender, EventArgs e)
        {
            AbrirForm(new frmCliente());
        }

        private void mnuSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja encerrar a aplicação?",
                "Projeto Modelo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
