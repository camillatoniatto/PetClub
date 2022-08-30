using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.Image.Model
{
    public class DocumentFile : FileBase
    {
        public override string Folder => throw new NotImplementedException();

        public override string FileType => throw new NotImplementedException();

        public override string FilePath()
        {
            throw new NotImplementedException();
        }
    }
}
