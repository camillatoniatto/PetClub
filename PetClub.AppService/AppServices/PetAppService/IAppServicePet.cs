using PetClub.AppService.ViewModels.Pet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.AppService.AppServices.PetAppService
{
    public interface IAppServicePet
    {
        Task<string> CreatePet(CreatePetViewModel model);
        Task<List<GetPetViewModel>> GetAllPets();
        Task<GetPetViewModel> GetPetById(string idPet);
        Task<List<GetPetViewModel>> GetPetsUser(string idUser);
        Task UpdatePet(UpdatePetViewModel model);
        Task DeletePet(string idPet);
        Task<List<GetPetViewModel>> GetAllPetsClient(string idUser);
    }
}
