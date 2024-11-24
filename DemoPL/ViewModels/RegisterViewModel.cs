using System.ComponentModel.DataAnnotations;

namespace DemoPL.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage ="First name is required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "First name is required")]
		public string LName { get; set; }

		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invaild Email")]
		public string Email { get; set; }

		[Required(ErrorMessage ="Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage ="Confirm password is required")]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage ="password doesn't match")]
		public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }
	}
}
