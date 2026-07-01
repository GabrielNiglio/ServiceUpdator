
using BaseDatos;
using BaseDatos.Builders;
using CapaServicios.Classes;
using InstaladorComanda.Builders;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaladorComanda.DataAccessExtensions
{
    public static class UpdaterConfigExtensions
    {

        private static string NOMBRE_TABLA = "updater_config";

        private static void agregarDatoIfNotNull(this ConexionGeneral me, string app, string parametro, bool valor)
        {
            me.agregarDatoIfNotNull(app, parametro, valor ? "S" : "N");
        }


        private static void agregarDatoIfNotNull(this ConexionGeneral me, string app, string parametro, string valor)
        {
            QueryBuilder insert = new InsertQueryBuilder(NOMBRE_TABLA)
                .addTupla("UPD_APLICACION", app)
                .addTupla("UPD_PARAMETRO", parametro)
                .addTupla("UPD_VALOR", valor);


            QueryBuilder update = new UpdateQueryBuilder(NOMBRE_TABLA)
                .addSet("UPD_VALOR", valor)
                .addWhere("UPD_APLICACION", app)
                .addWhere("UPD_PARAMETRO", parametro);


            string select = $"select * from {NOMBRE_TABLA} " +
                $" where UPD_PARAMETRO = '{parametro}'" +
                $" and UPD_APLICACION = '{app}'";

            me.ejecutarIfExists(select, update, insert);



        }

        public static List<RegistroUpdater> GetRegistroUpdaters(this ConexionGeneral me)
        {

            string query = $"select * from {NOMBRE_TABLA} order by UPD_APLICACION";


            var configuracionesUpdater = me.getConsultaAsObject(query, row => new
            {
                app = row["UPD_APLICACION"].ToString(),
                parametro = row["UPD_PARAMETRO"].ToString(),
                valor = row["UPD_VALOR"].ToString()
            });

            List<RegistroUpdater> listado = configuracionesUpdater
              .GroupBy(o => o.app.ToUpper())
                .Select((apli) =>
                {
                    RegistroUpdater registroUpdater = new RegistroUpdater();
                    registroUpdater.aplicacio = apli.Key;
                    foreach (var paramUpd in apli)
                    {
                        registroUpdater.setParametro(paramUpd.parametro, paramUpd.valor);
                    }

                    return registroUpdater;

                }).ToList();






            return listado;

        }


        public static void agregarRegistroUpdater(this ConexionGeneral me, RegistroUpdater registro)
        {
            me.agregarDatoIfNotNull(registro.aplicacion, "rutaDesde", registro.rutaDesde);
            me.agregarDatoIfNotNull(registro.aplicacion, "rutaHasta", registro.rutaHasta);
            me.agregarDatoIfNotNull(registro.aplicacion, "rutaDescarga", registro.rutaDescarga);
            me.agregarDatoIfNotNull(registro.aplicacion, "ejecutable", registro.ejecutable);
            me.agregarDatoIfNotNull(registro.aplicacion, "esServicio", registro.esServicio);
            me.agregarDatoIfNotNull(registro.aplicacion, "forzado", registro.forzado);
            me.agregarDatoIfNotNull(registro.aplicacion, "unico", registro.unico);
        }
    }
}
