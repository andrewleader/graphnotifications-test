using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphNotificationsTest.Helpers;
using GraphNotificationsTest.Models.Notifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GraphNotificationsTest.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly IGraphSdkHelper _graphSdkHelper;

        public NotificationsController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IGraphSdkHelper graphSdkHelper)
        {
            _configuration = configuration;
            _env = hostingEnvironment;
            _graphSdkHelper = graphSdkHelper;
        }

        // GET: Notifications
        public ActionResult Index()
        {
            // API to view notifications doesn't seem to be on graph yet
            if (RedirectToSignIn(out ActionResult result))
            {
                return result;
            }

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
            if (RedirectToSignIn(out ActionResult result))
            {
                return result;
            }

            return View(new SendNotificationModel());
        }

        public bool RedirectToSignIn(out ActionResult result)
        {
            if (!User.Identity.IsAuthenticated)
            {
                result = RedirectToAction(actionName: nameof(AccountController.SignIn), controllerName: "Account");
                return true;
            }

            try
            {
                _graphSdkHelper.GetAuthenticatedClient(User);
            }
            catch
            {
                result = RedirectToAction(actionName: nameof(AccountController.SignOut), controllerName: "Account");
                return true;
            }

            result = null;
            return false;
        }

        // POST: Notifications/SendNotification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendNotification(SendNotificationModel model)
        {
            try
            {
                // Initialize the graph client
                var graphClient = _graphSdkHelper.GetAuthenticatedClient(User);

                await GraphService.PostNotificationAsync(graphClient, model.Notification);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                model.Error = ex.ToString();
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