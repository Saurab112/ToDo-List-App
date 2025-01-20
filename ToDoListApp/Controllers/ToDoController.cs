using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
	public class ToDoController : Controller
	{
		// Make the list static so it persists across different requests
		public static List<ToDoItem> items_list { get; set; } = new List<ToDoItem>()
		{
			new ToDoItem { Title = "coding", Description = "bbb", ID = 1},
			new ToDoItem { Title = "abc", Description = "aaa", ID = 2}
		};

		public IActionResult Index()
		{
			return View(items_list);
		}

		public IActionResult Create()
		{
			return View();
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
		[HttpGet]
		public IActionResult Delete(int id)
		{
			ToDoItem? existingItemToDelete = items_list.FirstOrDefault(p => p.ID == id);
			if (existingItemToDelete == null)
			{
				return NotFound();
			}
			return View(existingItemToDelete);
		}

		[HttpPost]
		public IActionResult Delete(ToDoItem item)
		{
			ToDoItem? existingItemToDelete = items_list.FirstOrDefault(p => p.ID == item.ID);
			if (existingItemToDelete == null)
			{
				return NotFound();
			}
			items_list.Remove(existingItemToDelete);
			return RedirectToAction("Index");
		}
	}
}
