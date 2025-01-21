using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Models
{
	public class ToDoItem
	{
		public int ID { get; set; }

		[Required(ErrorMessage ="Title is required")]
		[StringLength(20, ErrorMessage = "Title cannot exceed 20 characters")]
		public string? Title { get; set; }

		[StringLength(20, ErrorMessage = "Description cannot exceed 200 characters.")]
		public string? Description { get; set; }
		public bool IsCompleted { get; set; }
	}
}
