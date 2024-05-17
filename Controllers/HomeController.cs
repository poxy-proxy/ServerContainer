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
using System.Text;

namespace ServerContainer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Entities db = new Entities();

       // String PathServer = @"D:\vs projects\DE\";
        String PathServer = @"D:\vs projects\ServerContainer\";
        TaskCompletionSource<bool> eventHandled = new TaskCompletionSource<bool>();
        public ActionResult Index()
        {
            if (System.IO.File.Exists(PathServer + @"App_Data\DBInfo\" + User.Identity.Name + @"\InfoDB.txt")){
                string file = System.IO.File.ReadAllText(PathServer + @"App_Data\DBInfo\" + User.Identity.Name + @"\InfoDB.txt");
                return View((object)file);
            }
            return View();
        }

        public ActionResult Catalog(string status="Сайт не добавлен",string reference="")
        {
            DirectoryInfo dirInfo = new DirectoryInfo(PathServer+@"App_Data\Projects\");// + User.Identity.Name + @"\" );
            List<DirectoryInfo> dirs = dirInfo.GetDirectories().ToList();
            List<FileModel> modelList = new List<FileModel>();
            foreach(var dir in dirs)
            {
               
                FileModel model = new FileModel();
                model.Name = dir.Name;
               
                model.Status = status;
                model.Reference = reference;
                if (dir.GetDirectories().Length > 0)
                {
                    var dirChild = dir.GetDirectories().ToList().First();
                    model.CreatedOn = dirChild.CreationTime;
                    var modelDB = db.ApplicationPort.Where(ap => ap.NameApplication == dirChild.Name).ToList();
                    if (!(modelDB is null) && modelDB.Count() > 0)
                    {
                        model.Status = modelDB.First().Status;
                        model.Reference = @"http://172.19.2.63:" + modelDB.First().Port + "/";
                        
                        //model.Reference = modelDB.First().Port;
                    }
                    else
                    {
                        model.Status = "Несобрано";
                        model.Reference = @"";
                    }
                }
            
                modelList.Add(model);
            }
          
            return View(modelList);
        
        }

        public ActionResult DB(string type="",string status="")
        {
            if (Session["StatusMySQL"] == null)
            {
                Session["StatusMySQL"] = "Не запущено";
            }

            if (Session["StatusPostgreSQL"] == null)
            {
                Session["StatusPostgreSQL"] = "Не запущено";
            }

            List<FileModel> modelList = new List<FileModel>();
            FileModel model = new FileModel();
            model.Name = "MySQL";
            model.Reference = "<a href=\"http://172.19.2.63:2206/\" target=\"_blank\">http://172.19.2.63:2206/</a>-mysql <a href=\"http://172.19.2.63:8080/\" target=\"_blank\">http://172.19.2.63:8080/</a>-phpmyadmin";
            model.Status = Session["StatusMySQL"].ToString();
            modelList.Add(model);
            model = new FileModel();
            model.Name = "PostgreSQL";
            model.Reference = "<a href=\"http://172.19.2.63:5432/\" target=\"_blank\">http://172.19.2.63:5432/</a>-postgreSQL <a href=\"http://172.19.2.63:5050/\" target=\"_blank\">http://172.19.2.63:5050/</a>-pgAdmin";
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
                        psi.WorkingDirectory = PathServer + @"App_Data\Dockerfiles\mysql";
                        Session["StatusMySQL"] = "Запущено";
                    }
                    else
                    {
                        psi.WorkingDirectory = PathServer + @"App_Data\Dockerfiles\postgresql";
                        Session["StatusPostgreSQL"] = "Запущено";
                    }
                    psi.FileName = "cmd.exe";
                    psi.Arguments = @"/c docker-compose up -d";

                    p.StartInfo = psi;
                    p.EnableRaisingEvents = true;
                    p.Exited += new EventHandler(myProcess_ExitedDB);

                    p.Start();
                }
                catch (Exception ex)
                {
                    RedirectToAction("DB", "Home");
                }
                //System.Threading.Thread.Sleep(100000);
                await Task.WhenAny(eventHandled.Task, Task.Delay(6660000));

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
                        psi.WorkingDirectory = PathServer + @"App_Data\Dockerfiles\mysql";
                        Session["StatusMySQL"] = "Не запущено";
                    }
                    else
                    {
                        psi.WorkingDirectory = PathServer + @"App_Data\Dockerfiles\postgresql";
                        Session["StatusPostgreSQL"] = "Не запущено";
                    }
                    psi.FileName = "cmd.exe";
                    psi.Arguments = @"/c docker-compose down";

                    p.StartInfo = psi;
                    p.EnableRaisingEvents = true;
                    p.Exited += new EventHandler(myProcess_ExitedDB);
                    p.Start();
                }
                catch (Exception ex)
                {
                    RedirectToAction("DB", "Home");
                }
                //System.Threading.Thread.Sleep(100000);
                  await Task.WhenAny(eventHandled.Task, Task.Delay(660000));

                return RedirectToAction("DB", "Home");
            }
        }

        public ActionResult MySite(string status = "Остановлен", string reference = "")
        {
            DirectoryInfo dirInfo = new DirectoryInfo(PathServer+@"App_Data\Projects\" + User.Identity.Name + @"\");
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
                    model.Reference = @"http://172.19.2.63:" + modelDB.First().Port + "/";
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
            SubjectList.Add(new SelectListItem { Text = "NodeJS", Value = "NodeJS" });
            SubjectList.Add(new SelectListItem { Text = "PHP", Value = "PHP" });
            SubjectList.Add(new SelectListItem { Text = "Django", Value = "Django" });
            ViewBag.SubjectList = SubjectList;
            return View(modelList);

        }


        public ActionResult LogSite(string name = "")
        {
            // Указываем путь к файлу, в который будем записывать вывод
            string outputFile = PathServer + @"App_Data\Projects\" + User.Identity.Name+@"\LogSite.txt";

            // Создаем новый процесс
            Process process = new Process();

            // Настройка параметров запуска процесса
          
            process.StartInfo.UseShellExecute = false; // Обязательно установите это в false для перенаправления вывода
            process.StartInfo.RedirectStandardOutput = true; // Перенаправляем стандартный вывод процесса
            process.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            process.StartInfo.WorkingDirectory = PathServer + @"App_Data\Projects\" + User.Identity.Name;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c docker logs "+name+"_cont";
            process.Start();

            // Читаем вывод процесса и записываем его в файл
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                writer.Write(process.StandardOutput.ReadToEnd());
            }

            // Дожидаемся завершения процесса
            process.WaitForExit();
            byte[] bytes = System.IO.File.ReadAllBytes(outputFile);
            return File(bytes, MimeMapping.GetMimeMapping("LogSite.txt"), "LogSite.txt");

        }


        public ActionResult Tasks()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(PathServer+@"App_Data\Tasks\");
            //List<DirectoryInfo> dirs = dirInfo.GetDirectories().ToList();
            List<FileModel> modelList = new List<FileModel>();
            foreach (var dir in dirInfo.GetFiles())
            {
                FileModel model = new FileModel();
                model.Name = dir.Name;
                model.CreatedOn = dir.CreationTime;
                


                modelList.Add(model);
            }
          
            return View(modelList);

        }


        public async Task<ActionResult> RunMySite(string name,string subject)
        {
            try
            {
                if (subject.Contains("PHP"))
                {
                    System.IO.File.Copy(PathServer + @"App_Data\Dockerfiles\Dockerfile-PHP",
                        PathServer + @"App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile", true);
                }

                if (subject.Contains("Django"))
                {
                    System.IO.File.Copy(PathServer + @"App_Data\Dockerfiles\requirements.txt",
                        PathServer + @"App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\requirements.txt", true);
                    System.IO.File.Copy(PathServer + @"App_Data\Dockerfiles\Dockerfile-Django",
                       PathServer + @"App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile", true);
                }

                if (subject.Contains("NodeJS"))
                {
                    System.IO.File.Copy(PathServer + @"App_Data\Dockerfiles\Dockerfile-NodeJS",
                        PathServer + @"App_Data\Projects\" + User.Identity.Name + @"\" + name + @"\Dockerfile", true);
                }
                Process p;
                using (p = new Process())
                {
                    try
                    {
                        System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
                        psi.WorkingDirectory = PathServer + @"App_Data\Projects\" + User.Identity.Name + @"\" + name;
                        psi.FileName = "cmd.exe";
                        psi.Arguments = @"/c docker build -t " + name + " .";
                        psi.CreateNoWindow = true;
                        Session["Name"] = name;
                        Session["Subject"] = subject;
                        p.EnableRaisingEvents = true;
                      //  p.Exited += new EventHandler(myProcess_Exited);
                        p.Exited += (sender, e) => myProcess_Exited(sender, e, name, subject);
                        p.StartInfo = psi;

                        p.Start();
                        await WriteLogAsync(@"/c docker build -t " + name + " .", PathServer + @"App_Data\LogSite.txt");
                    }
                    catch (Exception ex)
                    {
                        await WriteLogAsync(ex.Message + " " + ex.StackTrace, PathServer + @"App_Data\Errors.txt");
                        RedirectToAction("MySite", "Home");

                    }
                    try
                    {
                        //System.Threading.Thread.Sleep(100000);
                        await Task.WhenAny(eventHandled.Task, Task.Delay(9990000));
                    }
                    catch (Exception ex)
                    {
                        await WriteLogAsync(ex.Message + " " + ex.StackTrace, PathServer + @"App_Data\Errors.txt");
                        RedirectToAction("MySite", "Home");

                    }
                }
            }
            catch (Exception ex)
            {
                await WriteLogAsync(ex.Message + " " + ex.StackTrace, PathServer + @"App_Data\Errors.txt");
                RedirectToAction("MySite", "Home");

            }

            return RedirectToAction("MySite", "Home");
            
        }

        public static async Task WriteLogAsync(string message, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                await writer.WriteLineAsync($"{DateTime.Now}: {message}");
            }
        }


        // Handle Exited event and display process information.
        private async void myProcess_Exited(object sender, System.EventArgs e,string name,string subject)
        {
           // string name = Session["Name"].ToString();
           // string subject = Session["Subject"].ToString();
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
            psi.Arguments = @"/c docker run --name "+name+"_cont -p " + port + ":" + port2 + " " + name;
            psi.CreateNoWindow = true;
            var p = Process.Start(psi);
            await WriteLogAsync(@"/c docker run -p " + port + ":" + port2 + " " + name, PathServer + @"App_Data\LogSite.txt");
            //System.Threading.Thread.Sleep(10000);
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            runModel.Status = "Запущено";
            runModel.Port = port;
            db.SaveChanges();
            eventHandled.TrySetResult(true);
        }


        private void myProcess_ExitedDB(object sender, System.EventArgs e)
        {

            eventHandled.TrySetResult(true);
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                DirectoryInfo folder = new DirectoryInfo(PathServer+@"App_Data\Projects\" + User.Identity.Name);
                Directory.Delete(PathServer+@"App_Data\Projects\" + User.Identity.Name, recursive: true);
                Directory.CreateDirectory(PathServer+@"App_Data\Projects\" + User.Identity.Name);
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid()+"_"+upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(PathServer+@"Files\" + fileName);

                string archiveFilePath = PathServer+@"Files\" + fileName; //полный путь к файлу архива
                string targetFolder = PathServer+@"App_Data\Projects\" + User.Identity.Name + @"\" + fileName.Substring(0, fileName.Length - 4); ;//путь к папке в которую будет распакован архиа
                bool overriteFiles = true; //перезаписать файлы, если они уже есть в папке нахначения
                ZipFile.ExtractToDirectory(archiveFilePath, targetFolder);
            }
            return RedirectToAction("MySite", "Home");
        }

        [HttpPost]
        public ActionResult UploadTask(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
              
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(PathServer+@"App_Data\Tasks\" + fileName);

            }
            return RedirectToAction("Tasks", "Home");
        }

        public ActionResult DownloadTask(string name)
        {
          
            byte[] bytes = System.IO.File.ReadAllBytes(PathServer+@"App_Data\Tasks\" + name);
            return File(bytes, MimeMapping.GetMimeMapping(name), name);
        }

        public ActionResult DownloadSolution(string name)
        {
            string path = PathServer + @"App_Data\ZipFiles\" + User.Identity.Name + @"\";

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }

            ZipFile.CreateFromDirectory(PathServer + @"App_Data\Projects\" + name, path + name + ".zip");
            name += ".zip";
            byte[] bytes = System.IO.File.ReadAllBytes(path + name);
            return File(bytes, MimeMapping.GetMimeMapping(name), name);
        }

        public async Task<ActionResult> DownloadDump(string name)
        {
            var process = new Process();
            process.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            process.StartInfo.WorkingDirectory = PathServer + @"App_Data\Projects\" + name;
            process.StartInfo.FileName = "cmd.exe";
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(myProcess_ExitedDB);
            process.StartInfo.Arguments = "/c docker exec -u root -it postgres-server pg_dump -U postgres "+name.ToLower()+"db  > "+name+".sql";
            process.Start();
            await Task.WhenAny(eventHandled.Task, Task.Delay(9990000));
         
            byte[] bytes = System.IO.File.ReadAllBytes(PathServer + @"App_Data\Projects\" + name+@"\"+name+".sql");
            name += ".sql";
            return File(bytes, MimeMapping.GetMimeMapping(name), name);
        }



        public ActionResult DeleteTask(string name)
        {
            string fullPath = PathServer+@"App_Data\Tasks\" + name;
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            return RedirectToAction("Tasks", "Home");
        }

        public ActionResult StopMySite(string name, string subject)
        {
            var runModel = db.ApplicationPort.First(ap => ap.NameApplication == name);
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = @"C:\Windows\System32";
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/c docker stop " + name + "_cont && docker rm " + name + "_cont && docker rmi "+name;

            var p = Process.Start(psi);
            //System.Threading.Thread.Sleep(10000);
            runModel.Status = "Остановлено";
            runModel.Port = "";
            db.SaveChanges();

            return RedirectToAction("MySite", "Home");

        }

        public ActionResult Build(string name)
        {
            //string strCommand;
            //strCommand = @"docker run -it --rm -v /app/node_modules -p 3001:3000 -e CHOKIDAR_USEPOLLING=true " + name;
            //System.Diagnostics.Process.Start(PathServer+@"App_Data\Projects\hello-world\cmd.exe", strCommand);
            //System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo();
            //psi.WorkingDirectory = @"C:\Windows\System32";
            //psi.FileName = "cmd.exe";
            //psi.Arguments = PathServer+@"App_Data\Projects\"+name+ " docker build -t "+name+" .";
            System.Diagnostics.ProcessStartInfo psi = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
            psi.WorkingDirectory = PathServer+@"App_Data\Projects\"+User.Identity.Name + @"\"+name;
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
            //System.Diagnostics.Process.Start(PathServer+@"App_Data\Projects\hello-world\cmd.exe", strCommand);
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
            return RedirectToAction("Catalog","Home", new { status = "Запущено",reference= "http://172.19.2.63:3001/" });

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