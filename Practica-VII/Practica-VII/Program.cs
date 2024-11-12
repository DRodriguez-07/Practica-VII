using System;
using System.IO;
using System.Net;

namespace FTPSYSTEM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Crear instancia del cliente FTP
                ftp ftpClient = new ftp("ftp://demo.wftpserver.com:21/", "demo", "demo");

                // Subir un archivo
                ftpClient.upload("etc/test.txt", @"C:\Users\LeurisDeLosSantos\Desktop\test.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    internal class ftp
    {
        private string host;
        private string user;
        private string pass;
        private int bufferSize = 2048;

        public ftp(string hostIP, string userName, string password)
        {
            host = hostIP;
            user = userName;
            pass = password;
        }

        public void upload(string remoteFile, string localFile)
        {
            try
            {
                // Crear una solicitud FTP
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + remoteFile);

                // Establecer credenciales
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                // Opciones comunes para FTP
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                // Abrir archivo local para lectura
                using (FileStream localFileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read))
                {
                    // Crear flujo de datos hacia el servidor FTP
                    using (Stream ftpStream = ftpRequest.GetRequestStream())
                    {
                        byte[] byteBuffer = new byte[bufferSize];
                        int bytesSent;

                        while ((bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize)) > 0)
                        {
                            ftpStream.Write(byteBuffer, 0, bytesSent);
                        }
                    }
                }

                Console.WriteLine("Archivo subido exitosamente a " + host + "/" + remoteFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al subir el archivo: " + ex.Message);
            }
        }
    }
}
