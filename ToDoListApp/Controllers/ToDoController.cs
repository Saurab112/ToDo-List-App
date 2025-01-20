using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
	public class ToDoController : Controller
	{
		public List<ToDoItem> items_list { get; set; } = new List<ToDoItem>()
		{
			new ToDoItem { Title = "coding", Description = "work on linq", ID = 1},
			new ToDoItem { Title = "abc", Description = "aaa", ID = 2}
		};

		public IActionResult Index()
		{
			return View(items_list);
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			ToDoItem? item = items_list.FirstOrDefault(p=>p.ID == id);
			if (item == null)
			{
				return NotFound();
			}
			return View(item);
		}

		[HttpPost]
		public IActionResult Edit(ToDoItem item)
		{
			Console.WriteLine($"Received ID: {item.ID}, Title: {item.Title}, Description: {item.Description}, IsCompleted: {item.IsCompleted}");
			var existingItem = items_list.FirstOrDefault(temp=>temp.ID == item.ID);
			if (existingItem != null)
			{
				existingItem.Title = item.Title;
				existingItem.Description = item.Description;
				existingItem.IsCompleted = item.IsCompleted;
			}
			
			
			return RedirectToAction("Index");
		}
		public IActionResult Delete()
		{
			return View();
		}
	}
}
