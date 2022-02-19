using Microsoft.AspNetCore.Http;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class PetService:IPetService
    {
        private readonly IRepository<Pet> petRepo;
        private readonly ILoggerService<PetService> logger;
        private readonly IHttpContextAccessor accessor;

        public PetService(IRepository<Pet> petRepo,
         ILoggerService<PetService> logger,
         IHttpContextAccessor accessor)
        {
            this.petRepo = petRepo;
            this.logger = logger;
            this.accessor = accessor;
        }

        // add Pet
        public async Task CreatePet(Pet pet)
        {
            if (pet == null)
            {
                throw new AppException("Pet object cannot be null");
            }

            if (await petRepo.AnyAsync(m => m.Name.ToLower() == pet.Name.ToLower()))
            {
                throw new AppException($"A pet with name '{pet.Name}' already exist");
            }

            var currentUser = accessor.HttpContext.GetUserSession();
            pet.CreatedBy = currentUser.Id;
            pet.CreatedDate = DateTimeOffset.Now;

            await petRepo.Insert(pet, false);

            //log action
            await logger.LogActivity(ActivityActionType.CREATE_PET, currentUser.Email, $"Created pet with name {pet.Name}");
        }

        // delete Pet
        public async Task DeletePet(long petId)
        {
            var pet = await petRepo.GetById(petId);
            if (pet == null)
            {
                throw new AppException($"Invalid Pet id {petId}");
            }
            else
            {

                var _pet = pet.Clone<Pet>();
                await petRepo.Delete(petId, false);

                var currentUser = accessor.HttpContext.GetUserSession();
                // log activity
                await logger.LogActivity(ActivityActionType.DELETE_PET, currentUser.Email, petRepo.TableName, _pet, new Pet(),
                    $"Deleted pet with name {_pet.Name}");

            }
        }

        public IEnumerable<Pet> GetPets()
        {
            return petRepo.GetAll().OrderBy(m => m.Name);
        }

        public async Task<Pet> GetPet(long id)
        {
            return await petRepo.GetById(id);
        }
    }
}
