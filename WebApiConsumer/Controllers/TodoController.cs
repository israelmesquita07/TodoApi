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

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            DeleteTodoFromAPI(id);
            return RedirectToAction("Index");
        }

        private string DeleteTodoFromAPI(int id)
        {
            try
            {
                var resultList = "";
                var client = new HttpClient();
                var uri = String.Concat("http://localhost:50219/api/todo/", id);
                var getDataTask = client.DeleteAsync(uri)
                    .ContinueWith(response =>
                    {
                        var result = response.Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            resultList = "Deletado com Sucesso!";
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