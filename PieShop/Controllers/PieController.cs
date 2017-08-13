﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PieShop.Models;
using PieShop.ViewModels;

namespace PieShop.Controllers
{
    public class PieController : Controller
    {
		private readonly IPieRepository _pieRepository;
		private readonly ICategoryRepository _categoryRepository;

		public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
		{
			_pieRepository = pieRepository;
			_categoryRepository = categoryRepository;
		}

		public ViewResult List(string category)
		{
			IEnumerable<Pie> pies;
			string currentCategory = string.Empty;

			if (string.IsNullOrEmpty(category))
			{
				pies = _pieRepository.Pies.OrderBy(p => p.PieId);
				currentCategory = "All pies";
			}
			else
			{
				pies = _pieRepository.Pies.Where(p => p.Category.CategoryName == category)
				   .OrderBy(p => p.PieId);
				currentCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryName == category).CategoryName;
			}

			return View(new PiesListViewModel
			{
				Pies = pies,
				CurrentCategory = currentCategory
			});
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
