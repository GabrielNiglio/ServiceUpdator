using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionesManager;
namespace Pruebas
{
    internal class Program
    {
        static void Main(string[] args)
        {

            GestorConexiones gestorConexiones = new GestorConexiones();


            var conexiones = gestorConexiones.leerConexiones();

            foreach (var conexion in conexiones)
            {
                Console.WriteLine($"Nombre: {conexion.Key}");
                Console.WriteLine($"Servidor: {conexion.Value.server}");
                Console.WriteLine($"Base de Datos: {conexion.Value.database}");
                Console.WriteLine($"Contraseña: {conexion.Value.password}");
                Console.WriteLine();
            }

            Console.ReadLine();




        }
    }
}
