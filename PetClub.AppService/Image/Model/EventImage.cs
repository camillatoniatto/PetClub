using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Image.Model
{
    public class EventImage : ImageBase
    {
        public override int Width => 560;

        public override int Height => 360;

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
