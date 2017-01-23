using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security.Principal;
using System.Security.AccessControl;
using System;
using System.Runtime.InteropServices ;
using System.Threading;
using System.Drawing;



namespace H08.Models
{
    public class H08
    {
        public string base64String;
        public H08()
        {
            try
            {
                Ftp.Client theFTP = new Ftp.Client("ftp://10.8.3.193", "admin", "123comparex");
                theFTP.ChangeWorkingDirectory("DVUGMS");
                theFTP.ChangeWorkingDirectory("SINOPTIC");
                theFTP.ChangeWorkingDirectory("H08");
                theFTP.ChangeWorkingDirectory("types");
               
                string pathLast = theFTP.ListDirectory().Where(x => x.Substring(0, 3).Contains("cld")).OrderByDescending(x => x.ToLower()).First();

                string fileTemp = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
                theFTP.DownloadFile(pathLast, fileTemp);


                using (Image image = Image.FromFile(fileTemp))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        this.base64String = base64String;
                    }
                }
                System.IO.File.Delete(fileTemp);
            }
            catch (Exception ex)
            {
                this.base64String = ex.Message;
            }
        }
    }
}