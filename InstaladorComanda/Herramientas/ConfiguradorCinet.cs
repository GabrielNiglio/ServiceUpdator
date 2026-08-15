using BaseDatos;
using BaseDatos.Builders;
using CapaServicios.Classes;
using ConexionesManager;
using InstaladorComanda.Builders;
using InstaladorComanda.DataAccessExtensions;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InstaladorComanda.Herramientas
{
    internal class ConfiguradorCinet
    {

        private readonly string nombreServer;
        private readonly string nombreEquipo;
        private readonly string ruta;
        private readonly bool isServer;
        private readonly ConnectionFactory factory;
        private Dictionary<string, DatosConexion> datosConexion;

        public ConfiguradorCinet()
        {

            this.factory = new ConnectionFactory();

            using (var conn = factory.connectBackoffice())
            {

                string query = "select SERVERPROPERTY('MachineName') AS [SERVER], " +
                "(select para_valor from PARAMETROS where PARA_CODIGO = 'UPDRUTA') AS [RUTA]";

                var respuesta = conn.getFirstRowAsObject(query, row => new { server = row["server"], ruta = row["ruta"] });

                this.nombreServer = respuesta.server.ToString().Trim();
                this.ruta = respuesta.ruta.ToString().Trim();
                this.nombreEquipo = Environment.MachineName.Trim();

                string nombreServerLimpio = this.nombreServer.ToUpper();
                string nombreEquipoLimpio = Environment.MachineName.ToUpper();

                this.isServer = (nombreServerLimpio.Equals(nombreEquipoLimpio));

            }

        }




        internal void crearTablas()
        {


            TableBuilder TablaParametosPDV = new TableBuilder("PARAMETROS_PDV")
                 .addColumna("PARA_ID", "INT", false, true, true)
                 .addColumna("PARA_CODIGO", "VARCHAR(10)", false)
                 .addColumna("PARA_VALOR", "VARCHAR(150)", true)
                 .addColumna("PARA_FECHA", "DATETIME", false)
                 .addColumna("PARA_DELETE", "CHAR(1)", true);

            TableBuilder tablaUpdater = new TableBuilder("UPDATER_CONFIG")
                .addColumna("UPD_APLICACION", "VARCHAR(20)", false, true)
                .addColumna("UPD_PARAMETRO", "VARCHAR(20)", false, true)
                .addColumna("UPD_VALOR", "VARCHAR(255)", true);

            using (var conn = factory.connectBackoffice())
            {

                conn.ejecutarAccion(TablaParametosPDV, disparaExcepcion: false);

                conn.ejecutarAccion(tablaUpdater, disparaExcepcion: false);


            }





        }

        private void addParametroPDV(ConexionGeneral conn, string codigo, string valor, bool elimina = false)
        {
            string paraDelete = elimina ? "S" : null;


            QueryBuilder sParametro = new InsertQueryBuilder("Parametros_PDV")
                 .addTupla("PARA_CODIGO", codigo)
                 .addTupla("PARA_VALOR", valor)
                 .addTupla("PARA_DELETE", paraDelete)
                 .addTupla("PARA_FECHA", new SqlFunction("GETDATE()"));

            conn.ejecutarAccion(sParametro, disparaExcepcion: true);

        }

        private void addParametro(ConexionGeneral conn, string codigo, string descripcion, string valor)
        {
            QueryBuilder insert = new InsertQueryBuilder("Parametros")
                  .addTupla("PARA_CODIGO", codigo)
                  .addTupla("PARA_DESCRIPCION", descripcion)
                  .addTupla("PARA_VALOR", valor);

            QueryBuilder update = new UpdateQueryBuilder("Parametros")
                .addSet("PARA_VALOR", valor)
                .addWhere("PARA_CODIGO", codigo);

            string select = $"select * from parametros where para_codigo = '{codigo}'";

            conn.ejecutarIfExists(select, update, insert);
        }


        private RegistroUpdater crearRegUpdServ(string app, string exe)
        {
            return this.crearRegitroUpdater(
                app: app,
                exe: exe,
                esServicio: true,
                esForzado: false,
                esUnico: true
                );
        }

        private RegistroUpdater crearRegUpdAppUnica(string app, string exe)
        {
            return this.crearRegitroUpdater(
             app: app,
             exe: exe,
             esServicio: false,
             esForzado: true,
             esUnico: true
             );
        }

        private RegistroUpdater crearRegitroUpdater(string app, string exe,
            bool esServicio, bool esUnico, bool esForzado)
        {
            RegistroUpdater reg = new RegistroUpdater()
            {
                aplicacion = app,
                ejecutable = exe,
                esServicio = esServicio,
                forzado = esForzado,
                unico = esUnico,
                rutaDesde = $"\\\\{this.nombreServer}\\CINET\\ACTUALIZACIONES\\{this.ruta}\\{app}.zip",
                rutaHasta = $"C:\\CINET\\{app}",
                rutaDescarga = ConfigurationManager.AppSettings["rutaDescarga"] + $"/mensaje/Aplicativos/zip/{app}"
            };

            return reg;
        }


        private RegistroUpdater getRegistroPorAplicativo(string aplicativo)
        {
            switch (aplicativo.ToUpper())
            {
                //Servicios
                case "WSCOBROS": return crearRegUpdServ("WsCobros", "WsCobros.exe");
                case "WSCOMANDA": return crearRegUpdServ("WSComanda", "WSComanda.exe");
                case "CINETMOZOS": return crearRegUpdServ("CinetMozos", "MozosApi.exe");
                case "APICINETGENERAL": return crearRegUpdServ("ApiCinetGeneral", "ApiCinetGeneral.exe");
                case "WSUPDATORLAUNCHER": return crearRegUpdServ("WSUpdatorLauncher", "WSUpdatorLauncher.exe");

                //Aplicativos actualizacionForzada
                case "CUPDATOR": return crearRegUpdAppUnica("CUpdator", "CUpdator.exe");

                //Aplicativos Unicos
                case "SERVICEUPDATOR":
                    return crearRegitroUpdater(
                    "ServiceUpdator", "ServiceUpdator.exe",
                    esUnico: true, esServicio: false, esForzado: false
                 );

                default: throw new Exception("No existe: " + aplicativo);
            }
        }


        public void insertRegUpdater(List<string> aplicativos)
        {
            using (var connBackoffice = factory.connectBackoffice())
            {
                foreach (string aplicativo in aplicativos)
                {

                    RegistroUpdater wsComanda = getRegistroPorAplicativo(aplicativo);
                    connBackoffice.agregarRegistroUpdater(wsComanda);

                }

            }
        }

        public void crearArchivoConfigConexiones()
        {

            string sCodClave = ConfigurationManager.AppSettings.Get("clave");

            int codClave = 1;

            int.TryParse(sCodClave, out codClave);


            var odbcs = Registry.CurrentUser.OpenSubKey("SOFTWARE")
                  .OpenSubKey("ODBC")
                  .OpenSubKey("ODBC.INI");

            var bko = odbcs.OpenSubKey("backoffice");
            var comanda = odbcs.OpenSubKey("comanda");
            var pdv = odbcs.OpenSubKey("cinet_pdv");

            if (bko is null)
            {
                throw new KeyNotFoundException("No existe la ODBC de backoffice");
            }

            if (comanda is null)
            {
                throw new KeyNotFoundException("No existe la ODBC de comanda");
            }

            datosConexion = new Dictionary<string, DatosConexion>();

            datosConexion["comanda"] = new DatosConexion();
            datosConexion["backoffice"] = new DatosConexion();
            datosConexion["pdv"] = new DatosConexion();

            try
            {

                datosConexion["backoffice"].server = bko.GetValue("Server")?.ToString() ?? "backoffice";
                datosConexion["backoffice"].database = bko.GetValue("Database")?.ToString() ?? "backoffice";
                datosConexion["backoffice"].password = codClave;
            }
            catch (NullReferenceException ex) { throw new KeyNotFoundException("No existe la odbc 'Backoffice'."); }
            try
            {
                datosConexion["pdv"].server = pdv.GetValue("Server")?.ToString() ?? "pdv";
                datosConexion["pdv"].database = pdv.GetValue("Database")?.ToString() ?? "pdv";
                datosConexion["pdv"].password = codClave;
            }
            catch (NullReferenceException ex) { throw new KeyNotFoundException("No existe la odbc 'cinet_pdv'."); }

            try
            {
                datosConexion["comanda"].server = comanda.GetValue("Server")?.ToString() ?? "comanda";
                datosConexion["comanda"].database = comanda.GetValue("Database")?.ToString() ?? "comanda";
                datosConexion["comanda"].password = codClave;
            }
            catch (NullReferenceException ex) { throw new KeyNotFoundException("No existe la odbc 'Comanda'."); }

            string pathArchivo = "C:\\cinet\\config";
            Directory.CreateDirectory(pathArchivo);
            using (StreamWriter sw = new StreamWriter($"{pathArchivo}\\conexiones.json"))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                sw.Write(JsonConvert.SerializeObject(datosConexion, settings));
            }



            (new GestorConexiones()).guardarConexiones(datosConexion);





        }

        public void insertarParametrosWSComanda()
        {

            string basePathApi = $"http://{this.nombreEquipo}:5801";
            string endpoint = "/WSPDV/marcharcomanda";

            using (var connBackoffice = factory.connectBackoffice())
            {

                addParametroPDV(connBackoffice, "ZONAEFIJA", null, true);
                addParametroPDV(connBackoffice, "WSMARPATH", basePathApi + endpoint);

            }

            using (var connComanda = factory.connectComanda())
            {
                addParametro(connComanda, "COMNICANAL", "Usa onmicanal", "S");
                addParametro(connComanda, "WSMARPATH", "Servicio de omnicomanda para Panel DVY", basePathApi);
            }


        }

        internal void modificarAppsettingsWSComanda()
        {
            string rutaAppSettings = "C:\\cinet\\WSComanda\\appsettings.json";
            string json;
            using (StreamReader sr = new StreamReader(rutaAppSettings))
            {
                json = sr.ReadToEnd();
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            dict["PDV"] = $"Server={datosConexion["backoffice"].server};database={datosConexion["backoffice"].database}";
            dict["COMANDA"] = $"Server={datosConexion["comanda"].server};database={datosConexion["comanda"].database}";

            using (StreamWriter sw = new StreamWriter(rutaAppSettings))
            {
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                string jsonModificado = JsonConvert.SerializeObject(dict, Formatting.Indented);
                sw.Write(jsonModificado);
            }


        }


        public int getClaveConfigurada()
        {

            string sCodClave = ConfigurationManager.AppSettings.Get("clave");

            int codClave = 1;

            int.TryParse(sCodClave, out codClave);

            return codClave;
        }

        internal void actualizarParametrosPdv(bool usaDbRemo = true, string serverRemoto = "", string baseRemota = "")
        {

            int codClave = getClaveConfigurada();

            using (var connPdv = usaDbRemo ? factory.connect(serverRemoto, baseRemota, codClave) : factory.connectPdv())
            using (var connBko = factory.connectBackoffice())
            {
                string ULTPARAMID = "ULTPARAMID";
                var queryParametroUlt = $"select * from parametros where para_codigo = '{ULTPARAMID}'";
                var parametroUlt = connPdv.getFirstRowAsObject(queryParametroUlt, row => new
                {
                    parametro = row["para_codigo"],
                    valor = long.Parse(row["para_valor"].ToString())
                });


                long ultModif = parametroUlt?.valor ?? 0;



                var queryParamPorActu = $"select * from parametros_pdv where para_id > {ultModif}";


                var listaParametros = connBko.getConsultaAsObject(queryParamPorActu, row => new
                {
                    id = row["para_id"].ToString(),
                    codigo = row["para_codigo"].ToString(),
                    valor = row["para_valor"].ToString(),
                    elimina = row["para_delete"].ToString().Equals("S")
                });

                foreach (var param in listaParametros)
                {
                    if (param.elimina)
                    {
                        var delQuery = $"delete from parametros where para_codigo = '{param.codigo}'";
                        connPdv.ejecutarAccion(delQuery);
                    }
                    else
                    {
                        addParametro(connPdv, param.codigo, param.codigo, param.valor);
                    }
                    addParametro(connPdv, ULTPARAMID, "Ultima actualizacion auto", param.id);
                }
            }

        }

        internal void insertComandas()
        {

            using (var connComanda = factory.connectComanda())
            {
                for (int i = 3; i <= 6; i++)
                {
                    QueryBuilder insertComanda = new InsertQueryBuilder("comanda")
                        .addTupla("CO_COMANDA", i)
                        .addTupla("CO_HABILITADO", 0);

                    QueryBuilder insertCarril = new InsertQueryBuilder("CARRILES")
                        .addTupla("CA_COMANDA", i)
                        .addTupla("CA_CARRIL_DESDE", 99)
                        .addTupla("CA_CARRIL_HASTA", 99);

                    QueryBuilder insertZonas = new InsertQueryBuilder("CARRILES")
                        .addTupla("ZO_ID", i)
                        .addTupla("ZO_HABILITADO", 0)
                        .addTupla("zo_dvy", 0)
                        .addTupla("zo_auto", null);

                    connComanda.ejecutarAccion(insertComanda, disparaExcepcion: false);
                    connComanda.ejecutarAccion(insertCarril, disparaExcepcion: false);
                    connComanda.ejecutarAccion(insertZonas, disparaExcepcion: false);

                }

            }

        }

        internal List<string> getBasesPdv(string rutaServer)
        {

            int codClave = getClaveConfigurada();

            using (var connMaster = factory.connect(rutaServer, "master", codClave))
            {
                string query = "select name from sys.databases where name like '%cinet_pdv%'";
                return connMaster.getConsultaAsObject(query, row => row["name"].ToString());
            }

        }

        internal List<string> getEquiposLocal()
        {
            List<string> equipos = new List<string>();
            using (var connBko = factory.connectBackoffice())
            {
                string minDias = ConfigurationManager
                    .AppSettings["diaslocales"]?.ToString() ?? "20";

                string query = $"select distinct equipo from HParamLoc " +
                      $"where datediff(day, FECHATRANS, getdate()) < {minDias} " +
                      "and equipo like '%#%' " +
                      "order by equipo";

                equipos = connBko.getConsultaAsObject(query, row => row["equipo"].ToString())
                     .Select(name => name.Split('#')[0].Trim())
                     .Distinct()
                     .ToList();
            }

            return equipos;
        }
    }
}
