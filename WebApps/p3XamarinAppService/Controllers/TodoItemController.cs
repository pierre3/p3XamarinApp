using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using p3XamarinAppService.DataObjects;
using p3XamarinAppService.Models;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.Mobile.Server.Config;
using System.Collections.Generic;
using System.Security.Claims;

namespace p3XamarinAppService.Controllers
{
    [Authorize]
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            p3XamarinAppContext context = new p3XamarinAppContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
            
        }

        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            var claims = this.User as ClaimsPrincipal;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Query().Where(item=>item.UserId == userId);
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            var claims = this.User as ClaimsPrincipal;
            item.UserId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            TodoItem current = await InsertAsync(item);
            
            var config = this.Configuration;
            var settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();
            var hubName = settings.NotificationHubName;
            var connectionString = settings.Connections[MobileAppSettingsKeys.NotificationHubConnectionString].ConnectionString;
            var hubClient = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);
            var properties  = new Dictionary<string, string>();
            properties["messageParam"] = item.Text + " was added to the list.";
            try
            {
                var result = await hubClient.SendTemplateNotificationAsync(properties);
                config.Services.GetTraceWriter().Info(result.State.ToString());
            }
            catch(System.Exception ex)
            {
                config.Services.GetTraceWriter().Error(ex.Message, null, "Push.SendAsync error!");
            }
            
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}