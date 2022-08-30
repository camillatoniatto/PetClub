using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Image.Model
{
    public class PerfilImage : ImageBase
    {
        public override int Width => 200;

        public override int Height => 200;

        public override string Folder { get => "perfil"; }

        public override string EmptyImage()
        {
            return "/img/logo_Upload.svg";
        }
        public override string FilePath()
        {
            return string.Concat($"{Folder}/{FileName}.jpeg");
        }
    }
}
