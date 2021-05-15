using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LawFirmRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StorageController : Controller
    {
        private readonly StorageLogic storageLogic;
        private readonly BlankLogic blankLogic;

        public StorageController(StorageLogic storageLogic, BlankLogic blankLogic)
        {
            this.storageLogic = storageLogic;
            this.blankLogic = blankLogic;
        }

        [HttpGet]
        public List<StorageViewModel> GetStorageList() => storageLogic.Read(null)?.ToList();

        [HttpPost]
        public void CreateOrUpdateStorage(StorageBindingModel model) => storageLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteStorage(StorageBindingModel model) => storageLogic.Delete(model);

        [HttpPost]
        public void Replenishment(ReplenishStorageBindingModel model) => storageLogic.Replenishment(model);

        [HttpGet]
        public StorageViewModel GetStorage(int storageId) => storageLogic.Read(new StorageBindingModel { Id = storageId })?[0];

        [HttpGet]
        public List<BlankViewModel> GetBlankList() => blankLogic.Read(null);
    }
}