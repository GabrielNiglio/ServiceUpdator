
using ConexionesManager;
using DataAccess;
using InstaladorComanda.DataAccessExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;

namespace InstaladorComanda
{
    public class ConnectionFactory
    {
        private string conexComanda;
        private string conexBackoffice;
        private string conexPdv;

        private string tipoConexion;


        private void leerDatosConexionFromArchivo()
        {




            var obj = (new GestorConexiones()).leerConexiones();
            try
            {


                DatosConexion cComa = obj["comanda"];
                this.conexComanda = generarStringConexion(cComa);
            }
            catch { }
            try
            {

                var cBko = obj["backoffice"];
                this.conexBackoffice = generarStringConexion(cBko);

            }
            catch { }
            try
            {

                var cPdv = obj["pdv"];
                this.conexPdv = generarStringConexion(cPdv);


            }
            catch { }

        }

        public ConnectionFactory()
        {

            tipoConexion = ConfigurationManager.AppSettings["conexion"]?.ToLower() ?? "archivo";
            if (tipoConexion.Equals("odbc"))
            {
                this.conexBackoffice = generarStringConexionODBC("backoffice", 1);
                this.conexComanda = generarStringConexionODBC("comanda", 1);
                this.conexPdv = generarStringConexionODBC("cinet_pdv", 1);
            }
            else
            {
                this.leerDatosConexionFromArchivo();
            }
        }


        private string generarStringConexion(string server, string database, int passwordId)
        {


            string usuario = "sa";
            string password;
            if (passwordId == 1)
            {
                password = "cinettorcel";
            }
            else
            {
                password = "";
            }

            return $"DATA SOURCE={server};" +
                $"Database={database};" +
                $"Uid={usuario};" +
                $"Pwd={password};";

        }

        private string generarStringConexionODBC(string ODBC, int passwordId)
        {


            string usuario = "sa";
            string password;
            if (passwordId == 1)
            {
                password = "cinettorcel";
            }
            else
            {
                password = "";
            }

            return $"DSN={ODBC};" +
                $"Uid={usuario};" +
                $"Pwd={password};";

        }



        private string generarStringConexion(DatosConexion datos)
        {
            if (datos == null)
            {
                return null;
            }

            return generarStringConexion(datos.server, datos.database, datos.password);


        }


        public ConexionGeneral connect(string server, string database, int passwordId)
        {
            string conectionString = generarStringConexion(server, database, passwordId);

            IDbConnection conn = new SqlConnection(conectionString);
            // IConexion conn = DaoFactory.GetFactory(conectionString);
            return new ConexionGeneral(conn);
        }


        private IDbConnection constructorConexion(string conexStr)
        {
            switch (tipoConexion)
            {
                case "odbc":
                    return new OdbcConnection(conexStr);
                default:
                    return new SqlConnection(conexStr);
            }

        }

        public ConexionGeneral connectBackoffice()
        {

            //IConexion conn = DaoFactory.GetFactory(conexBackoffice);
            return new ConexionGeneral(constructorConexion(conexBackoffice));

        }
        public ConexionGeneral connectComanda()
        {

            return new ConexionGeneral(constructorConexion(conexComanda));

            IDbConnection conn = new SqlConnection(conexComanda);
            //IConexion conn = DaoFactory.GetFactory(conexComanda);
            return new ConexionGeneral(conn);
        }

        public ConexionGeneral connectPdv()
        {


            return new ConexionGeneral(constructorConexion(conexPdv));
            IDbConnection conn = new SqlConnection(conexPdv);
            //IConexion conn = DaoFactory.GetFactory(conexPdv);
            return new ConexionGeneral(conn);
        }

        public void setConexionComanda(string conexComanda)
        {
            this.conexComanda = conexComanda;
        }

        public void setConexionBackoffice(string conexBackoffice)
        {
            this.conexBackoffice = conexBackoffice;
        }

        public void setConexionPdv(string conexPdv)
        {
            this.conexPdv = conexPdv;
        }
    }
}
