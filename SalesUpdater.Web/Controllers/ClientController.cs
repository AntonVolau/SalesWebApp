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

namespace SalesUpdater.Web.Data.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["numberOfRecordsPerPage"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.ClientFilter = new ClientFilterViewModel();

                var clientsCoreModels = await _clientService.GetUsingPagedListAsync(page ?? 1, _pageSize);

                var clientsViewModels =
                        _mapper.Map<IPagedList<ClientViewModel>>(clientsCoreModels);

                return View(clientsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(ClientFilterViewModel clientFilterViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    var coreModels = await _clientService
                        .GetUsingPagedListAsync(clientFilterViewModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<ClientViewModel>>(coreModels);

                    return PartialView("Partial/_ClientTable", viewModels);
                }
                #endregion

                #region Filter
                var clientsCoreModels = await _clientService.Filter(
                    _mapper.Map<ClientFilterCoreModel>(clientFilterViewModel), _pageSize);

                var clientsViewModels = _mapper.Map<IPagedList<ClientViewModel>>(clientsCoreModels);
                #endregion

                #region Filling ViewBag
                ViewBag.ClientFilterFirstNameValue = clientFilterViewModel.Name;
                ViewBag.ClientFilterLastNameValue = clientFilterViewModel.Surname;
                #endregion

                return PartialView("Partial/_ClientTable", clientsViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_ClientTable");
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
        public async Task<ActionResult> Create(ClientViewModel clientViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(clientViewModel);
                }
                #endregion

                await _clientService.AddAsync(_mapper.Map<ClientDTO>(clientViewModel))
                    .ConfigureAwait(false);

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
                var clientCoreModel = await _clientService.GetAsync(id).ConfigureAwait(false);

                var clientViewModel = _mapper.Map<ClientViewModel>(clientCoreModel);

                return View(clientViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(ClientViewModel clientViewModel)
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    return View(clientViewModel);
                }
                #endregion

                await _clientService.UpdateAsync(_mapper.Map<ClientDTO>(clientViewModel))
                    .ConfigureAwait(false);

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
                await _clientService.DeleteAsync(id).ConfigureAwait(false);

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