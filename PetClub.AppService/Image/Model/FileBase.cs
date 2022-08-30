using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Image.Model
{
    public abstract class FileBase
    {
        public FileBase()
        {
            FileName = Guid.NewGuid().ToString();
        }

        public abstract string Folder { get; }

        public abstract string FileType { get; }

        public string FileName { get; set; }

        public abstract string FilePath();

        [Required(ErrorMessage = "O arquivo é obrigatório.")]
        public IFormFile Value { get; set; }

        public string GetUrl(string host)
        {
            return $"{host}{FilePath()}";
        }
    }
}
