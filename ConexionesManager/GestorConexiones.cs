using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConexionesManager
{
    public class DatosConexion
    {
        public string server { get; set; }
        public string database { get; set; }
        public int password { get; set; }



    }


    public class GestorConexiones
    {

        public Dictionary<string, DatosConexion> leerConexiones()
        {

            var aes = obtenerAes();
            string pathArchivo = "C:\\cinet\\config";
            string cifrado;
            using (StreamReader sw = new StreamReader($"{pathArchivo}\\auto.dll"))
            {

                 cifrado = sw.ReadToEnd();
            }

            using(var decryptor = aes.CreateDecryptor())
            {
                byte[] datosCifrados = Convert.FromBase64String(cifrado);
                byte[] datosDescifrados = decryptor.TransformFinalBlock(
                    datosCifrados,
                    0,
                    datosCifrados.Length
                );
                string json = Encoding.UTF8.GetString(datosDescifrados);
                return JsonConvert.DeserializeObject<Dictionary<string, DatosConexion>>(json);
            }   
        }

        public void guardarConexiones(Dictionary<string, DatosConexion> datosConexion)
        {

            string pathArchivo = "C:\\cinet\\config";
            Directory.CreateDirectory(pathArchivo);




            using (StreamWriter sw = new StreamWriter($"{pathArchivo}\\auto.dll"))
            {
                var  aes = obtenerAes();



                string json = JsonConvert.SerializeObject(datosConexion);

                using (var encryptor = aes.CreateEncryptor())
                {


                    byte[] datos = Encoding.UTF8.GetBytes(json);
                    byte[] cifrado = encryptor.TransformFinalBlock(
                        datos,
                        0,
                        datos.Length
                    );

                    string resultado = Convert.ToBase64String(cifrado);

                    sw.Write(resultado);
                }
            }
        }

        private Aes obtenerAes()
        {


            Aes aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes("7ff929b1d208753a4a9aad49133c8d37");

            return aes;

        }

        public void actualizarDeArchivoBaseSiNoExiste()
        {

            string pathArchivo = "C:\\cinet\\config";
            if (File.Exists($"{pathArchivo}\\auto.dll"))
            {
                return;
            }

            Dictionary<string, DatosConexion> x;
            using (StreamReader sw = new StreamReader($"{pathArchivo}\\conexiones.json"))
            {

                string json = sw.ReadToEnd();
                x = JsonConvert.DeserializeObject<Dictionary<string, DatosConexion>>(json);
            }

            GestorConexiones gc = new GestorConexiones();

            gc.guardarConexiones(x);

        }
    }
}
