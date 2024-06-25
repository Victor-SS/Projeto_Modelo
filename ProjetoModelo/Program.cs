using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoModelo
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Global.LerAppConfig();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin form = new frmLogin();
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                Application.Run(new frmPrincipal());
            }            
        }
    }
}
