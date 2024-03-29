﻿using CMSWebsite.Areas.Admin.ViewModels;
using CMSWebsite.Models;
using CMSWebsite.ServiceInterfaces;
using CMSWebsite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Web.Razor.Generator;

namespace CMSWebsite.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IImageService _imageService;
        private readonly IEventRegistrationService _eventRegistrationService;
        private readonly IRegistrationService _registrationService;

        public EventController(IEventService eventService, IImageService imageService, IEventRegistrationService eventRegistration, IRegistrationService registrationService)
        {
            _eventService = eventService;
            _imageService = imageService;
            _eventRegistrationService = eventRegistration;
            _registrationService = registrationService;
        }


        public IActionResult Index()
        {
            List<Event> events = _eventService.GetAllEvents().ToList();            

            foreach (var model in events)
            {
                var registrationModel = _eventRegistrationService.GetAllEventRegistration().Where(e => e.EventId == model.EventId);

                if (model.EventRegistrations != null)
                {
                    foreach (var registration in model.EventRegistrations)
                    {
                        foreach (var item in registrationModel)
                        {
                            registration.EventId = item.EventId;
                            registration.Event = _eventService.GetEventById(item.EventId);
                            registration.RegistrationId = item.RegistrationId;
                            registration.Registration = _registrationService.GetRegistrationById(item.RegistrationId);
                        }
                    }
                }                
            }

            return View(events);
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            EventViewModel eventModel = new EventViewModel();

            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventAsync(EventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _imageService.AddImageAsync(eventVM.ImageUrl);

                if(result.Error == null)
                {
                    var eventModel = new Event()
                    {
                        EventName = eventVM.EventName,
                        ShortDescription = eventVM.ShortDescription,
                        LongDescription = eventVM.LongDescription,
                        StartDate = eventVM.StartDate,
                        EndDate = eventVM.EndDate,
                        StartTime = eventVM.StartTime.ToString(),
                        EndTime = eventVM.EndTime.ToString(),
                        Address = eventVM.Address,
                        City = eventVM.City,
                        State = eventVM.State,
                        ZipCode = eventVM.ZipCode,
                        Email = eventVM.Email,
                        PhoneNumber = eventVM.PhoneNumber,
                        PublicId = result.PublicId.ToString(),
                        ImageUrl = result.Url.ToString()
                    };

                    _eventService.AddEvent(eventModel);

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = result.Error.Message.ToString();
                    return View(eventVM);
                }
                
            }
            else
            {
                return View(eventVM);
            }
        }

        [HttpGet]
        public IActionResult EventDetails(int id)
        {
            var eventModel = _eventService.GetEventById(id);

            if (eventModel == null)
            {
                return RedirectToAction("PageNotFound", "Dashboard");
            }
            else
            {
                return View(eventModel);
            }
        }

        [HttpGet]
        public IActionResult EditEvent(int id)
        {
            var model = _eventService.GetEventById(id);

            if (model == null)
            {
                return RedirectToAction("PageNotFound", "Dashboard");
            }
            else
            {
                EventViewModel eventVM = new EventViewModel()
                {
                    EventId = model.EventId,
                    EventName = model.EventName,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PublicId = model.PublicId,                    
                };

                return View(eventVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEventAsync(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventModel = _eventService.GetEventById(model.EventId);

                if (model.ImageUrl != null)
                {
                    var result = await _imageService.AddImageAsync(model.ImageUrl);

                    if (result.Error == null)
                    {
                        await _imageService.DeleteImageAsync(eventModel.PublicId);

                        eventModel.EventName = model.EventName;
                        eventModel.ShortDescription = model.ShortDescription;
                        eventModel.LongDescription = model.LongDescription;
                        eventModel.StartDate = model.StartDate;
                        eventModel.EndDate = model.EndDate;
                        eventModel.StartTime = model.StartTime;
                        eventModel.EndTime = model.EndTime;
                        eventModel.Address = model.Address;
                        eventModel.City = model.City;
                        eventModel.State = model.State;
                        eventModel.ZipCode = model.ZipCode;
                        eventModel.Email = model.Email;
                        eventModel.PhoneNumber = model.PhoneNumber;
                        eventModel.PublicId = result.PublicId;
                        eventModel.ImageUrl = result.Url.ToString();

                        _eventService.EditEvent(eventModel);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = result.Error.Message.ToString();
                        return View(model);
                    }
                }
                else
                {
                    eventModel.EventName = model.EventName;
                    eventModel.ShortDescription = model.ShortDescription;
                    eventModel.LongDescription = model.LongDescription;
                    eventModel.StartDate = model.StartDate;
                    eventModel.EndDate = model.EndDate;
                    eventModel.StartTime = model.StartTime;
                    eventModel.EndTime = model.EndTime;
                    eventModel.Address = model.Address;
                    eventModel.City = model.City;
                    eventModel.State = model.State;
                    eventModel.ZipCode = model.ZipCode;
                    eventModel.Email = model.Email;
                    eventModel.PhoneNumber = model.PhoneNumber;

                    _eventService.EditEvent(eventModel);

                    return RedirectToAction("Index");

                }
                   
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult DeleteEvent(int id)
        {
            var eventModel = _eventService.GetEventById(id);

            if (eventModel == null)
            {
                return RedirectToAction("PageNotFound", "Dashboard");
            }
            else
            {
                return View(eventModel);
            }
        }

        [HttpPost]
        public IActionResult DeleteEvent(Event model)
        {

            var eventModel = _eventService.GetEventById(model.EventId);

            _imageService.DeleteImageAsync(eventModel.PublicId);
            _eventService.DeleteEvent(model.EventId);

            return RedirectToAction("Index");
        }
    }
}
