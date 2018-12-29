using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace DirectCertInstall
{
    class CertInstaller
    {
        private string incomeDir;

        public CertInstaller(string incomeDir)
        {
            this.incomeDir = incomeDir;
        }

        public void InstallNow()
        {
            InstallCerts(FormCertArray(incomeDir));
        }

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
                }
            }

            return certs;
        }

    }
}
