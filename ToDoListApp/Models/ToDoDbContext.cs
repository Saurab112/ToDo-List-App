using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ToDoListApp.Models
{
	public class ToDoDbContext: DbContext
	{
		public ToDoDbContext(DbContextOptions optons): base(optons) { }
		public DbSet<ToDoItem> Items { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ToDoItem>().ToTable("Items");

			string itemsJson = System.IO.File.ReadAllText("ToDoItems.json");
			
			List<ToDoItem> items = System.Text.Json.JsonSerializer.Deserialize<List<ToDoItem>>(itemsJson);
			foreach(var item in items)
			{
				modelBuilder.Entity<ToDoItem>().HasData(item);
			}
		}
	}
}
