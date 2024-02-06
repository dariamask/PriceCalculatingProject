using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PriceCalculator.Data;

namespace PriceCalculatingProject.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Category>? Category { get; set; }
    public Review? Review { get; set; }
}

