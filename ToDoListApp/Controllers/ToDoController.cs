using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
	public class ToDoController : Controller
	{
		private readonly ToDoDbContext _db;
		
		public ToDoController(ToDoDbContext toDoDbContext)
		{
			_db = toDoDbContext;
		}
		// Make the list static so it persists across different requests
		//public static List<ToDoItem> _db { get; set; } = new List<ToDoItem>()
		//{
		//	new ToDoItem { Title = "herne katha", Description = "enjoy your free time with one episode of herne katha", ID = 1, CreatedDate = DateTime.Now.AddHours(4)},
		//	new ToDoItem { Title = "watering", Description = "I have to water the plants today at evening.", ID = 2, CreatedDate = DateTime.Now.AddDays(-1)},
		//	new ToDoItem { Title = "hello", Description = "11", IsCompleted = true, CreatedDate = DateTime.Now.AddDays(-2)}
		//};

		[HttpGet]
		public IActionResult Index(string? searchItem, string? statusFilter)
		{
			// Fetch all items from the database
			List<ToDoItem> filteredItem = _db.Items.ToList();

			// Apply search filter if any
			if (!string.IsNullOrEmpty(searchItem))
			{
				// First, get all items from the database
				var allItems = _db.Items.ToList();

				// Then, perform filtering on the client-side
				filteredItem = allItems
					.Where(item => item.Title.Contains(searchItem, StringComparison.OrdinalIgnoreCase)
								|| item.Description.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
					.ToList();
			}


			// Apply status filter if any
			if (!string.IsNullOrEmpty(statusFilter))
			{
				if (statusFilter == "Completed")
				{
					filteredItem = _db.Items.Where(item => item.IsCompleted).ToList();
				}
				else if (statusFilter == "NotCompleted")
				{
					filteredItem = _db.Items.Where(item => !item.IsCompleted).ToList();
				}
			}

			// Pass search term and status filter to the view
			ViewBag.SearchItem = searchItem;
			ViewBag.StatusFilter = statusFilter;
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
			item.CreatedDate = DateTime.Now;
			//generate one id for the new item to be added
			//item.ID = _db.Items.ToList().Count + 1; //Entity framework automatically creates id as its the primary key
			_db.Add(item);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			ToDoItem? item = _db.Items.FirstOrDefault(p => p.ID == id);
			if (item == null)
			{
				return NotFound();
			}
			return View(item);
		}

		[HttpPost]
		public IActionResult Edit(ToDoItem item)
		{
			var existingItem = _db.Items.FirstOrDefault(temp => temp.ID == item.ID);
			if (existingItem != null)
			{
				existingItem.Title = item.Title;
				existingItem.Description = item.Description;
				existingItem.IsCompleted = item.IsCompleted;
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult Delete(int id)
		{
			ToDoItem? existingItemToDelete = _db.Items.FirstOrDefault(p => p.ID == id);
			if (existingItemToDelete == null)
			{
				return NotFound();
			}
			return View(existingItemToDelete);
		}

		[HttpPost]
		public IActionResult Delete(ToDoItem item)
		{
			ToDoItem? existingItemToDelete = _db.Items.FirstOrDefault(p => p.ID == item.ID);
			if (existingItemToDelete == null)
			{
				return NotFound();
			}
			_db.Remove(existingItemToDelete);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}
