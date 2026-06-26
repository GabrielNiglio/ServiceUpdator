
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;


public class BabyServerLocal
{

    public class ArchivoPorRuta
    {
        public ArchivoPorRuta(string nombre, string ruta)
        {
            this.nombre = nombre;
            this.ruta = ruta;
        }

        public string nombre { get; }

        public string ruta { get; }

    }

    public class Archivo
    {
        public byte[] contenido { get; }

        public string nombre { get; }

        public Archivo(string nombre, byte[] contenido)
        {
            this.nombre = nombre;
            this.contenido = contenido;
        }

        public Archivo(string nombre, string contenido)
        {
            this.nombre = nombre;
            this.contenido = Encoding.UTF8.GetBytes(contenido);
        }

        public Archivo(string nombre, StreamReader sr)
            : this(nombre, sr.ReadToEnd())
        {
        }
    }

    private readonly HttpListener listener;

    private int puerto = 80;

    private Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, object>> acciones;

    public BabyServerLocal()
    {
        listener = new HttpListener();
        acciones = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, object>>();
    }

    public void setPuerto(int puerto)
    {
        this.puerto = puerto;
    }

    public void addEndpoint(string ruta, Func<HttpListenerRequest, HttpListenerResponse, object> accion)
    {
        acciones[ruta] = accion;
    }

    public void iniciar(CancellationToken cancellation)
    {
        

        listener.Prefixes.Clear();
        try
        {
            listener.Prefixes.Add($"http://*:{puerto}/");
        }
        catch
        {
            listener.Prefixes.Add($"http://localhost:{puerto}/");
        }

        listener.Start();
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                object obj2 = "NO ENCONTRADO";
                string key = request.RawUrl.Split('?')[0];
                if (acciones.ContainsKey(key))
                {
                    obj2 = acciones[key](request, response);
                }

                byte[] array = null;
                
                if (obj2 is string s)
                {
                    array = Encoding.UTF8.GetBytes(s);
                }
                else if (obj2 is Archivo archivo)
                {
                    array = archivo.contenido;
                    response.Headers.Add("Content-Disposition", "attachment; filename=\"" + archivo.nombre + "\"");
                }
                else if (obj2 is ArchivoPorRuta archivoo)
                {
                   
                    array = File.ReadAllBytes(archivoo.ruta);
                    response.Headers.Add("Content-Disposition", "attachment; filename=\"" + archivoo.nombre + "\"");
                }
                else
                {
                    response.Headers.Add("Content-Type", "application/json");
                    string s2 = JsonConvert.SerializeObject(obj2);
                    array = Encoding.UTF8.GetBytes(s2);
                }

                response.ContentLength64 = array.Length;
                Stream outputStream = response.OutputStream;
                outputStream.Write(array, 0, array.Length);
                outputStream.Close();
            }
            catch
            {
            }
        }
    }
}