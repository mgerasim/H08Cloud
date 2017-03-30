using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace H08.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            try
            {

                H08.Models.H08 theH08 = new H08.Models.H08("types");
                ViewBag.Types = theH08.base64String;

                H08.Models.H08 theTemp = new H08.Models.H08("temperature");
                ViewBag.Temperature = theTemp.base64String;

                H08.Models.H08 theHeight = new H08.Models.H08("height");
                ViewBag.Height = theHeight.base64String;

            }
            catch (Exception er)
            {
                ViewBag.Error = er.Message;
            }

            return View();
        }

        public ActionResult Load(string Catalog)
        {
            List<String> theList = new List<String>();
            ViewBag.Catalog = Catalog;
            try
            {
                Ftp.Client theFTP = new Ftp.Client("ftp://10.8.3.193", "admin", "123comparex");
                theFTP.ChangeWorkingDirectory("DVUGMS");
                theFTP.ChangeWorkingDirectory("SINOPTIC");
                theFTP.ChangeWorkingDirectory("H08");
                theFTP.ChangeWorkingDirectory(Catalog);

                int i = 0;
                foreach(var item in theFTP.ListDirectory().Where(x => x.Substring(0, 3).Contains("cld")).OrderByDescending(x => x.ToLower()) )
                {
                    i++;
                    if (i == 10)
                    {
                        break;
                    }
                    string fileTemp = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
                    theFTP.DownloadFile(item, fileTemp);

                    using (Image image = Image.FromFile(fileTemp))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            theList.Add(base64String);
                        }
                    }
                    System.IO.File.Delete(fileTemp);
                }
                theList.Reverse();

            }
            catch (Exception er)
            {
                ViewBag.Error = er.Message;
            }

            return View(theList);
        }

    }
}
