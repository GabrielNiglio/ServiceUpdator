using CinetConfigurationSerices.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinetConfigurationSerices
{
    public class WindowsConfigurationHelper : IEnviromentConfigurationHelper
    {
        public Task<List<string>> getInstanciasSqlServer()
        {
            var subKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SQL Server", false);


            var names = subKey.GetSubKeyNames().ToList();

            var instancias = new List<string>();

            foreach (var name in names)
            {
                var subsubkey = subKey.OpenSubKey(name, false);

                var nombresDeSubkeys= subsubkey.GetSubKeyNames().ToList();

                if (nombresDeSubkeys.Contains("MSSQLServer"))
                {
                    instancias.Add(name);
                }

            }

            return Task.FromResult(instancias); 
        }

        public string getNombreEquipo()
        {
            return System.Environment.MachineName;
        }

        public Task<String> runCMD(string comand)
        {

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Verb = "runas";
            cmd.Start();

            cmd.StandardInput.WriteLine(comand);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            var respuesta = cmd.StandardOutput.ReadToEnd();

            return Task.FromResult(respuesta);
        }

        public async Task setODBC(string name, string server, string database)
        {

            var registro = Registry.CurrentUser.OpenSubKey("SOFTWARE", true)
            .OpenSubKey("ODBC", true)
            .OpenSubKey("ODBC.INI", true);



            registro.CreateSubKey(name);
            var odbc = registro.OpenSubKey(name, true);
            var odbc_datasources = registro.OpenSubKey("ODBC Data Sources", true);

            odbc_datasources.SetValue(name, "SQL Server");

            odbc.SetValue("Database", database);
            odbc.SetValue("Driver", "C:\\WINDOWS\\system32\\SQLSRV32.dll");
            odbc.SetValue("Language", "Español");
            odbc.SetValue("LastUser", "sa");
            odbc.SetValue("Server", server);


            
        }

        public async Task setUpdaterReg(string ruta, string aplicativo, String equipo = "")
        {
            if (equipo.Equals(""))
            {
                equipo = "C:\\CINET";
            }
            else
            {
                equipo = $"\\\\{equipo}";
            }

            var subkeys = new List<string> { "VB and VBA Program Settings", "CINET", "UPDATER", aplicativo };

            var registro = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);

            foreach (var sub in subkeys)
            {
                registro.CreateSubKey(sub);
                registro = registro.OpenSubKey(sub, true);
            }

            registro.SetValue("Ejecutable", $"{aplicativo}.exe");
            registro.SetValue("Rutadesde1", $"{equipo}\\CINET\\ACTUALIZACIONES\\{ruta}\\{aplicativo}.zip");
            registro.SetValue("Rutahasta", $"C:\\CINET\\{aplicativo}");
            registro.SetValue("Tiempopausa", "5");
            registro.SetValue("ULTIMOUPDATE", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));


        }



    }

}
