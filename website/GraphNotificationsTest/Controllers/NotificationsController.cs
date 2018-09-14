using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphNotificationsTest.Models.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphNotificationsTest.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        public ActionResult Index()
        {
            return View(new NotificationModel[0]);
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Notifications/SendNotification
        public ActionResult SendNotification()
        {
            return View(new SendNotificationModel());
        }

        // POST: Notifications/SendNotification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotification(SendNotificationModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notifications/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}