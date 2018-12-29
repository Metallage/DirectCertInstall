using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;


namespace DirectCertInstall
{
    class CertInstaller
    {
        /// <summary>
        /// Дирректория с сертификатами
        /// </summary>
        private string incomeDir;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="incomeDir">Дирректория с сертификатами</param>
        public CertInstaller(string incomeDir)
        {
            this.incomeDir = incomeDir;
        }


        /// <summary>
        /// Установить сертификаты из дирректории
        /// </summary>
        public void InstallNow()
        {
            InstallCerts(FormCertArray(incomeDir));
        }

        /// <summary>
        /// Устанавливает сертификаты в Корневое и промежуточное хранилище
        /// </summary>
        /// <param name="certs"></param>
        private void InstallCerts(X509Certificate2Collection certs)
        {

            X509Store rootStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            
            rootStore.Open(OpenFlags.ReadWrite);
            rootStore.AddRange(certs);
            rootStore.Close();

            X509Store authorityStore = new X509Store(StoreName.CertificateAuthority, StoreLocation.LocalMachine);

            authorityStore.Open(OpenFlags.ReadWrite);
            authorityStore.AddRange(certs);
            authorityStore.Close();

        }

        

        /// <summary>
        /// Формирует список сертификатов для установки (пока не умеет работать с crl)
        /// </summary>
        /// <param name="certsDir">Дирректория с сертификатами</param>
        /// <returns>Список сертификатов</returns>
        private X509Certificate2Collection FormCertArray(string certsDir)
        {
            X509Certificate2Collection certs = new X509Certificate2Collection();

            DirectoryInfo certsIncome = new DirectoryInfo(certsDir);

            if(certsIncome.Exists)
            {
                foreach(FileInfo cert in certsIncome.GetFiles())
                {
                    if((cert.Extension.ToLower() == ".cer")/*||(cert.Extension.ToLower() == ".crl")*/|| (cert.Extension.ToLower() == ".crt"))
                    {
                        certs.Add(new X509Certificate2(cert.FullName));
                    }
                    else if ((cert.Extension.ToLower() == ".crl"))
                    {
                        Process crlCert = new Process();
                        ProcessStartInfo ps = new ProcessStartInfo();
                        ps.FileName = "CertMgr.Exe";
                        ps.Arguments = "/add -all "+cert.FullName+" -s -r localMachine CA";
                        ps.CreateNoWindow=true;
                        crlCert.StartInfo = ps;
                        crlCert.Start();
                        crlCert.WaitForExit();

                    }
                }
            }

            return certs;
        }

    }
}
