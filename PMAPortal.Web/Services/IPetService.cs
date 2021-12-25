using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IPetService
    {
        Task CreatePet(Pet pet);
        Task DeletePet(long petId);
        IEnumerable<Pet> GetPets();
        Task<Pet> GetPet(long id);
    }
}
