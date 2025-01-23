using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
	public class ToDoController : Controller
	{
		// Make the list static so it persists across different requests
		public static List<ToDoItem> items_list { get; set; } = new List<ToDoItem>()
		{
			new ToDoItem { Title = "herne katha", Description = "enjoy your free time with one episode of herne katha", ID = 1},
			new ToDoItem { Title = "watering", Description = "I have to water the plants today at evening.", ID = 2}
		};

public IActionResult Index(string? searchItem)
		{
			List<ToDoItem> filteredItem = items_list;
			if (!string.IsNullOrEmpty(searchItem))
			{
				filteredItem = items_list.Where(item => item.Title.Contains(searchItem, StringComparison.OrdinalIgnoreCase) || item.Description.Contains(searchItem, StringComparison.OrdinalIgnoreCase)).ToList();
			}
			ViewBag.SearchItem = searchItem;
			return View(filteredItem);

		}
		[HttpGet]
		public IActionResult Create()
		{
			ToDoItem item = new ToDoItem();
			return View(item);
		}
		[HttpPost]
		public IActionResult Create(ToDoItem item)
		{
			if (!ModelState.IsValid)
			{
				return View(item);
			}
			//generate one id for the new item to be added
			item.ID = items_list.Count + 1;
			items_list.Add(item);
			return RedirectToAction("Index");
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

		[HttpGet]
		public IActionResult Search(int id)
		{

			return View();
		}
	}
}
