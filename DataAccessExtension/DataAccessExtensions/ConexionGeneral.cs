using DataAccess;
using InstaladorComanda.Builders;
using InstaladorComanda.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaladorComanda.DataAccessExtensions
{
    public class ConexionGeneral : IDisposable
    {
        private readonly IDbConnection conn;

        public ConexionGeneral(IDbConnection conn)
        {
            this.conn = conn;

            conn.Open();
        }

        public void Dispose()
        {
            conn.Close();
        }

        public long guardarHparamLoc(string app, string version)
        {
            try
            {
                string sSql = "INSERT INTO HPARAMLOC (EMPRESA,LOCAL,PARAMETRO,VALOR,FECHATRANS,EQUIPO,CAJA) " +
                       $"VALUES ('MOSTAZA',(select para_valor from parametros where para_codigo = 'NOMLOCAL')," +
                       $"'{app}','{version}',getdate(),'{Environment.MachineName}','0');";
               
                return ejecutarAccion(sSql);

            }
            catch { return 0 ; }

        }

        public DataTable cmdToDataTAble(string query)
        {

            DataTable dt = new DataTable();
            if (conn is SqlConnection sqlConn)
            {
                using (var com = sqlConn.CreateCommand())
                {
                    com.CommandText = query;
                    var adp = new SqlDataAdapter(com);
                    adp.Fill(dt);
                    return dt;

                }

            }
            else if (conn is OdbcConnection odbcConn)
            {
                using (var com = odbcConn.CreateCommand())
                {
                    com.CommandText = query;
                    var adp = new OdbcDataAdapter(com);

                    adp.Fill(dt);
                    return dt;
                }
            }

            return null;
        }

        public DataTable getDt(string query)
        {
            return this.cmdToDataTAble(query);



        }

        public string getString(string query)
        {

            using (IDbCommand comm = conn.CreateCommand())
            {
                comm.CommandText = query;
                var rd = comm.ExecuteReader();

                if (rd.Read())
                {
                    return rd[0].ToString();
                }
                return null;

            }

        }

        public long ejecutarAccion(string query)
        {
            using (IDbCommand comm = conn.CreateCommand())
            {
                comm.CommandText = query;
                return comm.ExecuteNonQuery();

            }

        }


        private long ejecutarAccionInner(QueryBuilder builder, bool disparaExcepcion = true)
        {
            return ejecutarAccion(builder.toQuery());
        }


        private long ejecutarAccionIgnoringExceptions(QueryBuilder builder)
        {

            try
            {

                return ejecutarAccionInner(builder);
            }
            catch
            {
                return -1;
            }
        }


        public long ejecutarAccion(QueryBuilder builder, bool disparaExcepcion = true)
        {

            if (disparaExcepcion)
            {
                return ejecutarAccionInner(builder);
            }
            else
            {
                return ejecutarAccionIgnoringExceptions(builder);
            }

        }


        public List<TResult> getConsultaAsObject<TResult>(string query, Func<DataRow, TResult> resultSelector)
        {
            List<TResult> list = new List<TResult>();
            DataTable dataTable = getDt(query);
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(resultSelector(row));
            }

            return list;

        }

        public TResult getFirstRowAsObject<TResult>(string query, Func<DataRow, TResult> resultSelector)
        {
            TResult objeto = default;
            var lista = this.getConsultaAsObject(query, resultSelector);

            return lista.FirstOrDefault();


            //DataTable dataTable = getDt(query);
            //if (dataTable.Rows.Count > 0)
            //{
            //    objeto = resultSelector(dataTable.Rows[0]);
            //}


            //return objeto;

        }




        public long ejecutarIfExists(string condicion, QueryBuilder trueQuery, QueryBuilder falseQuery)
        {
            string sQuery = $"IF EXISTS ({condicion}) {trueQuery.toQuery()} else {falseQuery.toQuery()}";
            return this.ejecutarAccion(sQuery);
        }




    }


    public class ConexionDataAccessx : IDisposable
    {
        private readonly IConexion conn;

        public ConexionDataAccessx(IConexion conn)
        {
            this.conn = conn;

            conn.Conectar();
        }

        public void Dispose()
        {
            conn.Desconectar();
        }

        public long guardarHparamLoc(string app, string version)
        {
            string sSql = "INSERT INTO HPARAMLOC (EMPRESA,LOCAL,PARAMETRO,VALOR,FECHATRANS,EQUIPO,CAJA) " +
                   $"VALUES ('MOSTAZA',(select para_valor from parametros where para_codigo = 'NOMLOCAL')," +
                   $"'{app}','{version}',getdate(),'{Environment.MachineName}','0');";

            return ejecutarAccion(sSql, ignoraErrores: true);
        }

        public DataTable getDt(string query, bool ignoraErrores = false)
        {
            return ejecutar(query, () => conn.getConsulta(query), ignoraErrores);
        }

        public string getString(string query, bool ignoraErrores = false)
        {
            return ejecutar(query, () => conn.getEscalar(query), ignoraErrores).ToString();

        }

        public long ejecutarAccion(string query, bool ignoraErrores = false)
        {
            return ejecutar(query, () => conn.ejecutarAccion(query), ignoraErrores);
        }

        public long ejecutarAccion(QueryBuilder builder, bool ignoraErrores = false)
        {
            return ejecutarAccion(builder.toQuery(), ignoraErrores);
        }


        public List<TResult> getConsultaAsObject<TResult>(string query, Func<DataRow, TResult> resultSelector)
        {
            List<TResult> list = new List<TResult>();
            DataTable dataTable = getDt(query);
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(resultSelector(row));
            }

            return list;

        }

        public TResult getFirstRowAsObject<TResult>(string query, Func<DataRow, TResult> resultSelector)
        {
            TResult objeto = default;
            var lista = this.getConsultaAsObject(query, resultSelector);

            return lista.FirstOrDefault();


            //DataTable dataTable = getDt(query);
            //if (dataTable.Rows.Count > 0)
            //{
            //    objeto = resultSelector(dataTable.Rows[0]);
            //}


            //return objeto;

        }

        private T ejecutar<T>(string query, Func<T> accion, bool isIgnoraErrores)
        {

            T resultado = accion();


            if (!isIgnoraErrores && conn.RESULTADO < 0)
            {
                throw new DataAccessException(conn.MsjError, query);
            }

            return resultado;
        }


        public long ejecutarIfExists(string condicion, QueryBuilder trueQuery, QueryBuilder falseQuery)
        {
            string sQuery = $"IF EXISTS ({condicion}) {trueQuery.toQuery()} else {falseQuery.toQuery()}";
            return this.ejecutarAccion(sQuery);
        }




    }
}
