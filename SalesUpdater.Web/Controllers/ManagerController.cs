using AutoMapper;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Contracts.Services;
using SalesUpdater.Web.Data.Models;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using X.PagedList;

namespace SalesUpdater.Web.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        public ManagerController(IManagerService managerService, IMapper mapper)
        {
            _managerService = managerService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["itemsPerPage"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.ManagerFilter = new ManagerViewFilterModel();

                var managersCoreModels = await _managerService.GetPagedListAsync(page ?? 1, _pageSize);

                var managersViewModels =
                        _mapper.Map<IPagedList<ManagerViewModel>>(managersCoreModels);

                return View(managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(ManagerViewFilterModel managerViewFilterModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _managerService
                        .GetPagedListAsync(managerViewFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<ManagerViewModel>>(coreModels);

                    return PartialView("Partial/_ManagerTable", viewModels);
                }
                #endregion

                #region Filter
                var managersCoreModels = await _managerService.Filter(
                    _mapper.Map<ManagerCoreFilterModel>(managerViewFilterModel), _pageSize);

                var managersViewModels = _mapper.Map<IPagedList<ManagerViewModel>>(managersCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.ManagerFilterLastNameValue = managerViewFilterModel.Surname;
                #endregion

                return PartialView("Partial/_ManagerTable", managersViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ManagerTable");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ManagerViewModel managerViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(managerViewModel);
                }
                #endregion

                await _managerService.AddAsync(_mapper.Map<ManagerDTO>(managerViewModel)).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var managerCoreModel = await _managerService.GetAsync(id).ConfigureAwait(false);

                var managerViewModel = _mapper.Map<ManagerViewModel>(managerCoreModel);

                return View(managerViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(ManagerViewModel manager)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(manager);
                }
                #endregion

                await _managerService.UpdateAsync(_mapper.Map<ManagerDTO>(manager)).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _managerService.DeleteAsync(id).ConfigureAwait(false);

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return RedirectToAction("Index");
            }
        }
    }
}