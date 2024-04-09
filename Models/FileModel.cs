using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerContainer.Models
{
    public class FileModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }
        public string FileType { get; set; }

        [Display(Name = "Ссылка")]
        public string Reference { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; }
        public string UploadedBy { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime? CreatedOn { get; set; }

    }
}