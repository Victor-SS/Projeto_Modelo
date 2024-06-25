using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoModelo
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }
        public bool Ativo { get; set; }
        public Usuario()
        {
            Id = 0;
            Login = string.Empty;
            Nome = string.Empty;
            Password = string.Empty;
            Ativo = false;
        }

        AcessoBD acesso = new AcessoBD();
        DataTable dt = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>();
        string sql = string.Empty;

        public DataTable Consultar()
        {
            try
            {
                parameters.Clear();
                sql = "select id, login, nome, password, ativo \n";
                sql += "from tblUsuario \n";
                if (Id != 0)
                {
                    sql += "where id = @id \n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (Login != string.Empty)
                {
                    sql += "where login = @login \n";
                    parameters.Add(new SqlParameter("@login", Login));
                }
                else if (Nome != string.Empty)
                {
                    sql += "where nome like @nome \n";
                    parameters.Add(new SqlParameter("@nome", '%' + Nome + '%'));
                }
                dt = acesso.Consultar(sql, parameters);
                if (Id != 0 || Login != string.Empty && dt.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]);
                    Login = dt.Rows[0]["login"].ToString();
                    Nome = dt.Rows[0]["nome"].ToString();
                    Password = dt.Rows[0]["password"].ToString();
                    Ativo = Convert.ToBoolean(dt.Rows[0]["ativo"]);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Autenticar(string senha)
        {
            return senha == Password;
        }
        public void Gravar()
        {
            try
            {
                parameters.Clear();
                if (Id==0)
                {
                    sql = "insert into tblUsuario \n";
                    sql += "(login, nome, password, ativo)\n";
                    sql += "values \n";
                    sql += "(@login, @nome, @password, @ativo)";
                }
                else
                {
                    sql = "update tblUsuario\n";
                    sql += "set \n";
                    sql += "login     = @login, \n";
                    sql += "nome	  = @nome, \n";
                    sql += "password  = @password, \n";
                    sql += "ativo	  = @ativo\n";
                    sql += "where id  = @id \n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                parameters.Add(new SqlParameter("@login", Login));
                parameters.Add(new SqlParameter("@nome", Nome));
                parameters.Add(new SqlParameter("@password", Password));
                parameters.Add(new SqlParameter("@ativo", Ativo));
                acesso.Executar(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
