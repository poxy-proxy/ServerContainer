using ServerContainer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ServerContainer.Controllers
{
    public class HomeController : Controller
    {
        Entities db = new Entities();
        String NameBuf = "";
        TaskCompletionSource<bool> eventHandled = new TaskCompletionSource<bool>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Catalog(string status="Остановлен",string reference="")
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"D:\vs projects\ServerContainer\App_Data\Projects\");// + User.Identity.Name + @"\" );
            List<DirectoryInfo> dirs = dirInfo.GetDirectories().ToList();
            List<FileModel> modelList = new List<FileModel>();
            foreach(var dir in dirs)
            {
                var dirChild = dir.GetDirectories().ToList().First();
                FileModel model = new FileModel();
                model.Name = dir.Name;
                model.CreatedOn = dir.CreationTime;
                model.Status = status;
                model.Reference = reference;
                var modelDB=db.ApplicationPort.Where(ap => ap.NameApplication == dirChild.Name).ToList();
                if(!(modelDB is null) && modelDB.Count() > 0)
                {
                    model.Status = modelDB.First().Status;
                    model.Reference = @"http://localhost:" + modelDB.First().Port + "/";
                    //model.Reference = modelDB.First().Port;
                }
                else
                {
                    model.Status = "Несобрано";
                   model.Reference = @"";
                }
            
                modelList.Add(model);
            }
          
            return View(modelList);
        
        }

        public ActionResult DB(string type="",string status="")
        {
            if (Session["StatusMySQL"] == null)
            {
                Session["StatusMySQL"] = "Не запущена";
            }

            if (Session["StatusPostgreSQL"] == null)
            {
                Session["StatusPostgreSQL"] = "Не запущена";
            }

            List<FileModel> modelList = new List<FileModel>();
            FileModel model = new FileModel();
            model.Name = "MySQL";
            model.Reference = "localhost:2206-mysql localhost:8080-phpmyadmin";
            model.Status = Session["StatusMySQL"].ToString();
            modelList.Add(model);
            model = new FileModel();
            model.Name = "PostgreSQL";
            model.Reference = "localhost:5432-postgresql";
            model.Status = Session["StatusPostgreSQL"].ToString();
            modelList.Add(model);
            return View(modelList);

        }

        public async Task<ActionResult> RunDB(string name)
        {

            
            Process p;
            using (p = new Process())
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
                    if (name == "MySQL")
                    {
                        psi.WorkingDirectory = @"D:\примеры сайтов диплом\mysql";
                        Session["StatusMySQL"] = "Запущено";
                    }
                    else
                    {
                        psi.WorkingDirectory = @"D:\примеры сайтов диплом\postgresql";
                        Session["StatusPostgreSQL"] = "Запущено";
                    }
                    psi.FileName = "cmd.exe";
                    psi.Arguments = @"/c docker-compose up -d";

                    p.StartInfo = psi;

                    p.Start();
                }
                catch (Exception ex)
                {
                    RedirectToAction("DB", "Home");
                }
                //System.Threading.Thread.Sleep(100000);
              //  await Task.WhenAny(eventHandled.Task, Task.Delay(660000));

                return RedirectToAction("DB", "Home");
            }
        }

        public async Task<ActionResult> StopDB(string name)
        {


            Process p;
            using (p = new Process())
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
                    if (name == "MySQL")
                    {
                        psi.WorkingDirectory = @"D:\примеры сайтов диплом\mysql";
                        Session["StatusMySQL"] = "Запущено";
                    }
                    else
                    {
                        psi.WorkingDirectory = @"D:\примеры сайтов диплом\postgresql";
                        Session["StatusPostgreSQL"] = "Запущено";
                    }
                    psi.FileName = "cmd.exe";
                    psi.Arguments = @"/c docker-compose up -d";

                    p.StartInfo = psi;

                    p.Start();
                }
                catch (Exception ex)
                {
                    RedirectToAction("DB", "Home");
                }
                //System.Threading.Thread.Sleep(100000);
                //  await Task.WhenAny(eventHandled.Task, Task.Delay(660000));

                return RedirectToAction("DB", "Home");
            }
        }

        public ActionResult MySite(string status = "Остановлен", string reference = "")
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\");
            List<DirectoryInfo> dirs = dirInfo.GetDirectories().ToList();
            List<FileModel> modelList = new List<FileModel>();
            foreach (var dir in dirs)
            {
                FileModel model = new FileModel();
                model.Name = dir.Name;
                model.CreatedOn = dir.CreationTime;
                model.Status = status;
                model.Reference = reference;
                var modelDB = db.ApplicationPort.Where(ap => ap.NameApplication == dir.Name).ToList();
                if (!(modelDB is null) && modelDB.Count() > 0)
                {
                    model.Status = modelDB.First().Status;
                    model.Reference = @"http://localhost:" + modelDB.First().Port + "/";
                    //model.Reference = modelDB.First().Port;
                }
                else
                {
                    model.Status = "Несобрано";
                    model.Reference = @"";
                }

                
                modelList.Add(model);
            }
            List<SelectListItem> SubjectList = new List<SelectListItem>();
            SubjectList.Add(new SelectListItem { Text = "PHP", Value = "PHP" });
            SubjectList.Add(new SelectListItem { Text = "NodeJS", Value = "NodeJS" });
            SubjectList.Add(new SelectListItem { Text = "Django", Value = "Django" });
            ViewBag.SubjectList = SubjectList;
            return View(modelList);

        }
      

        public async Task<ActionResult> RunMySite(string name,string subject)
        {
           
            if (subject.Contains("PHP"))
            {
                System.IO.File.Copy(@"D:\vs projects\ServerContainer\App_Data\Dockerfiles\Dockerfile-PHP",
                    @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile",true);
            }

            if (subject.Contains("Django"))
            {
                System.IO.File.Copy(@"D:\vs projects\ServerContainer\App_Data\Dockerfiles\requirements.txt",
                    @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\requirements.txt", true);
                System.IO.File.Copy(@"D:\vs projects\ServerContainer\App_Data\Dockerfiles\Dockerfile-Django",
                   @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile", true);
            }

            if (subject.Contains("NodeJS"))
            {
                System.IO.File.Copy(@"D:\vs projects\ServerContainer\App_Data\Dockerfiles\Dockerfile-NodeJS",
                    @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile", true);
            }
            Process p;
            using (p = new Process())
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
                    psi.WorkingDirectory = @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + name;
                    psi.FileName = "cmd.exe";
                    psi.Arguments = @"/c docker build -t " + name + " .";

                    Session["Name"] = name;
                    Session["Subject"] = subject;
                    p.EnableRaisingEvents = true;
                    p.Exited += new EventHandler(myProcess_Exited);
                    p.StartInfo = psi;

                    p.Start();
                }
                catch (Exception ex)
                {
                    RedirectToAction("MySite", "Home");
                }
                //System.Threading.Thread.Sleep(100000);
                await Task.WhenAny(eventHandled.Task, Task.Delay(660000));

                return RedirectToAction("MySite", "Home");
            }
        }

        // Handle Exited event and display process information.
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            string name = Session["Name"].ToString();
            string subject = Session["Subject"].ToString();
            var modelDB = db.ApplicationPort.Where(ap => ap.NameApplication == name);
            if (!(modelDB is null) && modelDB.Count() > 0)
            {
                modelDB.First().Status = "Собрано";

            }
            else
            {
                ApplicationPort model = new ApplicationPort();
                model.NameApplication = name;
                model.Status = "Собрано";
                db.ApplicationPort.Add(model);
            }
            db.SaveChanges();
            modelDB = db.ApplicationPort.Where(ap => ap.Status == "Запущено").OrderByDescending(ap => ap.Id);
            string port = "3001";
            string port2 = "3000";
            if (subject.Contains("PHP") || subject.Contains("Django"))
            {
                port2 = "80";

            }
            if (!(modelDB is null) && modelDB.Count() > 0)
            {
                port = (int.Parse(modelDB.First().Port) + 1).ToString();
            }
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = @"C:\Windows\System32";
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c docker run -p " + port + ":" + port2 + " " + name;

            var p = Process.Start(psi);
            System.Threading.Thread.Sleep(10000);
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            runModel.Status = "Запущено";
            runModel.Port = port;
            db.SaveChanges();
            eventHandled.TrySetResult(true);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                DirectoryInfo folder = new DirectoryInfo(@"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name);
                Directory.Delete(@"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name, recursive: true);
                Directory.CreateDirectory(@"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name);
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(@"D:\vs projects\ServerContainer\Files\" + fileName);

                string archiveFilePath = @"D:\vs projects\ServerContainer\Files\" + fileName; //полный путь к файлу архива
                string targetFolder = @"D:\vs projects\ServerContainer\App_Data\Projects\" + User.Identity.Name + @"\" + fileName.Substring(0, fileName.Length - 4); ;//путь к папке в которую будет распакован архиа
                bool overriteFiles = true; //перезаписать файлы, если они уже есть в папке нахначения
                ZipFile.ExtractToDirectory(archiveFilePath, targetFolder);
            }
            return RedirectToAction("MySite", "Home");
        }

        public ActionResult StopMySite(string name, string subject)
        {
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = @"C:\Windows\System32";
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c C:\Users\DNS\Desktop\stopByPort.sh " + runModel.Port;

            var p = Process.Start(psi);
            System.Threading.Thread.Sleep(10000);
            runModel.Status = "Собрано";
            runModel.Port = "";
            db.SaveChanges();

            return RedirectToAction("MySite", "Home");

        }

        public ActionResult Build(string name)
        {
            //string strCommand;
            //strCommand = @"docker run -it --rm -v /app/node_modules -p 3001:3000 -e CHOKIDAR_USEPOLLING=true " + name;
            //System.Diagnostics.Process.Start(@"D:\vs projects\ServerContainer\App_Data\Projects\hello-world\cmd.exe", strCommand);
            //System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            //psi.WorkingDirectory = @"C:\Windows\System32";
            //psi.FileName = "cmd.exe";
            //psi.Arguments = @"D:\vs projects\ServerContainer\App_Data\Projects\"+name+ " docker build -t "+name+" .";
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
            psi.WorkingDirectory = @"D:\vs projects\ServerContainer\App_Data\Projects\"+User.Identity.Name + @"\"+name;
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c docker build -t " + name + " .";



            var p = Process.Start(psi);
            System.Threading.Thread.Sleep(10000);
            var modelDB = db.ApplicationPort.Where(ap => ap.NameApplication == name);
            if (!(modelDB is null) && modelDB.Count()>0)
            {
                modelDB.First().Status="Собрано";

            }
            else
            {
                ApplicationPort model = new ApplicationPort();
                model.NameApplication = name;
                model.Status = "Собрано";
                db.ApplicationPort.Add(model);
            }
            db.SaveChanges();
            return RedirectToAction("Catalog", "Home");

        }

        public ActionResult Run(string name)
        {
            //string strCommand;
            //strCommand = @"docker run -it --rm -v /app/node_modules -p 3001:3000 -e CHOKIDAR_USEPOLLING=true " + name;
            //System.Diagnostics.Process.Start(@"D:\vs projects\ServerContainer\App_Data\Projects\hello-world\cmd.exe", strCommand);
            var modelDB = db.ApplicationPort.Where(ap => ap.Status == "Запущено").OrderByDescending(ap => ap.Id);
            string port = "3001";
            if (!(modelDB is null) && modelDB.Count() > 0)
            {
                port = (int.Parse(modelDB.First().Port) + 1).ToString();
            }
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = @"C:\Windows\System32";
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c docker run -it --rm -v /app/node_modules -p "+port+":3000 -e CHOKIDAR_USEPOLLING=true "+name;
            
            var p = Process.Start(psi);
            System.Threading.Thread.Sleep(10000);
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            runModel.Status = "Запущено";
            runModel.Port = port;
            db.SaveChanges();
            return RedirectToAction("Catalog","Home", new { status = "Запущено",reference= "http://localhost:3001/" });

        }

        public ActionResult Stop(string name)
        {
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = @"C:\Windows\System32";
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c C:\Users\DNS\Desktop\stopByPort.sh "+runModel.Port;

            var p = Process.Start(psi);
            System.Threading.Thread.Sleep(10000);
            runModel.Status = "Собрано";
            runModel.Port = "";
            db.SaveChanges();
            return RedirectToAction("Catalog", "Home", new { status = "Остановлен", reference = "" });

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}