using PieShop.Models;
using PieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PieShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPieRepository _pieRepository;

		public HomeController(IPieRepository pieRepository)
		{
			_pieRepository = pieRepository;
		}

		public ViewResult Index()
		{
			var homeViewModel = new HomeViewModel
			{
				PiesOfTheWeek = _pieRepository.PiesOfTheWeek
			};

			return View(homeViewModel);
		}
	}
}
