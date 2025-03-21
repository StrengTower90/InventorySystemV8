﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models.ViewModels
{
    public class CompanyVM
    {
        public Company Company { get; set; }
        public IEnumerable<SelectListItem> StoreList { get; set; }
    }
}
