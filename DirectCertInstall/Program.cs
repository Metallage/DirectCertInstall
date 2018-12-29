using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectCertInstall
{
    class Program
    {
        static void Main(string[] args)
        {

            string certDir;

            if (args.Length > 0)
            {
                int i = 0;
                foreach (string arg in args)
                {
                    switch (arg.ToLower())
                    {
                        case "-dir":
                            if(args.Length> i)
                            {
                                certDir = args[i + 1];
                                CertInstaller ci = new CertInstaller(certDir);
                                ci.InstallNow();
                            }
                            else
                            {
                                PrintHelp();
                            }
                            break;

                    }
   
                }


            }
            else
            {
                PrintHelp();
            }


        }

        private static void PrintHelp()
        {
            Console.WriteLine("ключ -dir задаёт путь к файлам сертификатов");
            Console.WriteLine(@"Например -dir c:\temp\certs\");

        }
    }
}
