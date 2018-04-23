using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiConsumer.Models;

namespace WebApiConsumer.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            var todos = GetTodosFromAPI();
            return View(todos);
        }

        private List<TodoItem> GetTodosFromAPI()
        {
            try
            {
                var resultList = new List<TodoItem>();
                var client = new HttpClient();
                var getDataTask = client.GetAsync("http://localhost:50219/api/todo")
                    .ContinueWith(response =>
                    {
                        var result = response.Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readResult = result.Content.ReadAsAsync<List<TodoItem>>();
                            readResult.Wait();
                            resultList = readResult.Result;
                        }
                    });
                getDataTask.Wait();
                return resultList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}