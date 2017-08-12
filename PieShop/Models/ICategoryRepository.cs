﻿using System.Collections.Generic;

namespace PieShop.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}
