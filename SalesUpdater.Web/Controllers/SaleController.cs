using AutoMapper;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Contracts.Services;
using SalesUpdater.Web.Data.Models;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using X.PagedList;

namespace SalesUpdater.Web.Data.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;

        private readonly IMapper _mapper;

        private readonly int _pageSize;

        private readonly int _numberOfRecordsToCreateSchedule;

        public SaleController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;

            _mapper = mapper;

            _pageSize = int.Parse(ConfigurationManager.AppSettings["itemsPerPage"]);

            _numberOfRecordsToCreateSchedule =
                int.Parse(ConfigurationManager.AppSettings["numberOfRecordsToCreateSchedule"]);
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                ViewBag.SaleFilter = new SaleViewFilterModel();

                var salesCoreModels = await _saleService.GetPagedListAsync(page ?? 1, _pageSize);

                var salesViewModels =
                        _mapper.Map<IPagedList<SaleViewModel>>(salesCoreModels);

                return View(salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> ShowGraph(int? page)
        {
            try
            {
                var salesForGraphCoreModels =
                    await _saleService.GetPagedListAsync(page ?? 1, _numberOfRecordsToCreateSchedule, null,
                        SortDirection.Descending);

                var salesForGraphViewModels =
                    _mapper.Map<IEnumerable<SaleViewModel>>(salesForGraphCoreModels).ToList();

                var uniqueProducts = salesForGraphViewModels.Select(x => x.Product.Name).Distinct();

                var productSalesQuantity = uniqueProducts
                    .Select(product => salesForGraphViewModels.Count(x => x.Product.Name == product));

                ViewBag.UniqueProducts = uniqueProducts;
                ViewBag.ProductSalesQuantity = productSalesQuantity;

                return PartialView("~/Views/Sale/Partial/_Chart.cshtml");
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Find(SaleViewFilterModel saleViewFilterModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var coreModels = await _saleService.GetPagedListAsync(saleViewFilterModel.Page ?? 1, _pageSize)
                        .ConfigureAwait(false);

                    var viewModels = _mapper.Map<IPagedList<SaleViewModel>>(coreModels);

                    return PartialView("Partial/_SaleTable", viewModels);
                }


                var salesCoreModels = await _saleService.Filter(
                    _mapper.Map<SaleCoreFilterModel>(saleViewFilterModel), _pageSize);

                var salesViewModels = _mapper.Map<IPagedList<SaleViewModel>>(salesCoreModels);

                ViewBag.SaleFilterClientFirstNameValue = saleViewFilterModel.ClientName;
                ViewBag.SaleFilterClientLastNameValue = saleViewFilterModel.ClientSurname;
                ViewBag.SaleFilterDateFromValue = saleViewFilterModel.DateFrom;
                ViewBag.SaleFilterDateToValue = saleViewFilterModel.DateTo;
                ViewBag.SaleFilterManagerLastNameValue = saleViewFilterModel.ManagerSurname;
                ViewBag.SaleFilterProductNameValue = saleViewFilterModel.ProductName;
                ViewBag.SaleFilterSumFromValue = saleViewFilterModel.SumFrom;
                ViewBag.SaleFilterSumToValue = saleViewFilterModel.SumTo;

                return PartialView("Partial/_SaleTable", salesViewModels);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return PartialView("Partial/_SaleTable");
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
        public async Task<ActionResult> Create(SaleViewModel sale)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }

                await _saleService.AddAsync(_mapper.Map<SaleDTO>(sale)).ConfigureAwait(false);

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
                var saleCoreModel = await _saleService.GetAsync(id).ConfigureAwait(false);

                var saleViewModel = _mapper.Map<SaleViewModel>(saleCoreModel);

                return View(saleViewModel);
            }
            catch (Exception exception)
            {
                ViewBag.Error = exception.Message;

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(SaleViewModel sale)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sale);
                }

                await _saleService.UpdateAsync(_mapper.Map<SaleDTO>(sale)).ConfigureAwait(false);

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
                await _saleService.DeleteAsync(id).ConfigureAwait(false);

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