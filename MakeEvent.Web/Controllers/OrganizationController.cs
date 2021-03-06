﻿using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MakeEvent.Business.Models;
using MakeEvent.Web.Models.Organization;
using MakeEvent.Business.Services.Interfaces;
using MakeEvent.Common.Models;
using MakeEvent.Web.Attributes;
using MakeEvent.Web.Models.Common;

namespace MakeEvent.Web.Controllers
{
    [RequireHttps, Localized, OrganizationAuthorize]
    public class OrganizationController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IOrganizationService  _organizationService;
        private readonly IImageService         _imageService;

        public OrganizationController(
            IAuthorizationService authorizationService, 
            IOrganizationService organizationService,
            IImageService imageService)
        {
            _authorizationService = authorizationService;
            _organizationService  = organizationService;
            _imageService         = imageService;
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Category");

            var organization = _organizationService.Get(id).Data;
            var image = (organization != null && organization.ImageId.HasValue)
                ? _imageService.Get(organization.ImageId.Value).Data
                : null;

            var model = Mapper.Map<OrganizationMvcViewModel>(organization);
            if (image != null)
            {
                model.ImageData     = image.Content;
                model.ImageMimeType = image.MimeType;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index", "Home");

            var organization = _organizationService.Get(id).Data;
            var image = (organization != null && organization.ImageId.HasValue)
                ? _imageService.Get(organization.ImageId.Value).Data
                : null;

            var model = Mapper.Map<OrganizationMvcViewModel>(organization);
            if (image != null)
            {
                model.ImageData = image.Content;
                model.ImageMimeType = image.MimeType;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, OrganizationMvcViewModel model)
        {
            if (ModelState.IsValid == false)
                return View(model);

            if (string.IsNullOrEmpty(model.OwnerId))
            {
                throw new HttpException((int)HttpStatusCode.InternalServerError, 
                    "Не указан идентификатор пользователя");
            }

            OperationResult<ImageDto> imageResult = null;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var image = Mapper.Map<ImageDto>(file);
                imageResult = _imageService.SaveImage(image);
            }

            if (imageResult != null && !imageResult.Succeeded)
            {
                ModelState.AddModelError("", $"Ошибки при обновлении организации:</br>"
                                           + $"{string.Join("</br>", imageResult.Errors)}");

                return View(model);
            }

            var organization = Mapper.Map<OrganizationDto>(model);
            organization.ImageId = imageResult?.Data.Id;

            var organizationResult = _organizationService.Save(organization);

            if (!organizationResult.Succeeded)
            {
                ModelState.AddModelError("", $"Ошибки при обновлении организации:</br>"
                                            + $"{string.Join("</br>", organizationResult.Errors)}");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Неправильный логин или пароль");
                return View();
            }

            var result = _authorizationService.Login(model.Email, model.Password, "Organization");

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", string.Join("\n", result.Errors));
                return View();
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(OrganizationMvcViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                _authorizationService.Logout();

            if (ModelState.IsValid == false)
                return View(model);

            OperationResult<ImageDto> imageResult = null;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var image = Mapper.Map<ImageDto>(file);
                imageResult = _imageService.SaveImage(image);
            }

            if (imageResult != null && !imageResult.Succeeded)
            {
                ModelState.AddModelError("", $"Ошибки при регистрации:</br>"
                                            + $"{string.Join("</br>", imageResult.Errors)}");

                return View(model);
            }

            var organization = Mapper.Map<OrganizationDto>(model);
            organization.ImageId = imageResult?.Data.Id;

            var oranizationResult = _organizationService.Save(organization);
            if (!oranizationResult.Succeeded)
            {
                ModelState.AddModelError("", $"Ошибки при регистрации:</br>"
                                            + $"{string.Join("</br>", oranizationResult.Errors)}");
                return View(model);
            }

            _authorizationService.Login(model.Email, model.Password);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            _authorizationService.Logout();

            return RedirectToAction("Index","Home");
        }
    }
}
