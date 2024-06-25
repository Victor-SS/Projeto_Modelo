using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.Transactions;

namespace ProjetoModelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string Celular { get; set; }
        public int UsuarioId { get; set; }
        public Endereco Endereco { get; set; }
        public Cliente()
        {
            Id = 0;
            Nome = string.Empty;
            DataNascimento = DateTime.MinValue;
            CPF = string.Empty;
            Email = string.Empty;
            Sexo = string.Empty;
            Celular = string.Empty;
            UsuarioId = 0;
            Endereco = new Endereco();
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
                sql = "select id, nome, data_nascimento, CPF, email,\n";
                sql += "sexo, celular, usuarioId \n";
                sql += "from tblCliente \n";
                if (Id != 0)
                {
                    sql += "where id = @id \n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (CPF != string.Empty)
                {
                    sql += "where cpf = @cpf \n";
                    parameters.Add(new SqlParameter("@cpf", CPF));
                }
                else if (Nome != string.Empty)
                {
                    sql += "where nome like @nome \n";
                    parameters.Add(new SqlParameter("@nome", '%' + Nome + '%'));
                }
                dt = acesso.Consultar(sql, parameters);
                if (Id != 0 || (CPF != string.Empty && dt.Rows.Count > 0))
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]);
                    Nome = dt.Rows[0]["nome"].ToString();
                    DataNascimento = Convert.ToDateTime(dt.Rows[0]["data_nascimento"]);
                    CPF = dt.Rows[0]["cpf"].ToString();
                    Email = dt.Rows[0]["email"].ToString();
                    Sexo = dt.Rows[0]["sexo"].ToString();
                    Celular = dt.Rows[0]["celular"].ToString();
                    UsuarioId = Convert.ToInt32(dt.Rows[0]["usuarioId"]);
                    Endereco.ClienteId = Id;
                    Endereco.Consultar();
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Gravar()
        {
            try
            {
                using (TransactionScope transacao = new TransactionScope())
                {
                    parameters.Clear();
                    if (Id == 0)
                    {
                        sql = "insert into tblCliente \n";
                        sql += "(nome, data_nascimento, CPF, \n";
                        sql += "email, sexo, celular, usuarioId)\n";
                        sql += "values \n";
                        sql += "(@nome, @data_nascimento, @CPF, \n";
                        sql += "@email, @sexo, @celular, @usuarioId);\n";
                        sql += "select @@IDENTITY";
                    }
                    else
                    {
                        sql = "update tblCliente \n";
                        sql += "set \n";
                        sql += "nome = @nome, \n";
                        sql += "data_nascimento = @data_nascimento, \n";
                        sql += "CPF = @CPF, \n";
                        sql += "email  = @email, \n";
                        sql += "sexo  = @sexo, \n";
                        sql += "celular = @celular, \n";
                        sql += "usuarioId = @usuarioId \n";
                        sql += "where id = @id \n";
                        parameters.Add(new SqlParameter("@id", Id));
                    }
                    parameters.Add(new SqlParameter("@nome", Nome));
                    parameters.Add(new SqlParameter("@data_nascimento", DataNascimento));
                    parameters.Add(new SqlParameter("@CPF", CPF));
                    parameters.Add(new SqlParameter("@email", Email));
                    parameters.Add(new SqlParameter("@sexo", Sexo));
                    parameters.Add(new SqlParameter("@celular", Celular));
                    parameters.Add(new SqlParameter("@usuarioId", UsuarioId));

                    if (Id == 0)
                    {
                        Id = acesso.Executar(parameters, sql);
                    }
                    else
                    {
                        acesso.Executar(sql, parameters);
                    }
                    Endereco.ClienteId = Id;
                    Endereco.Gravar();
                    transacao.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
    }
}
    }
}
