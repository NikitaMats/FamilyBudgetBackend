﻿using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class CategoryCreateDto
    {
        [Required] public string Name { get; set; }
        [Required] public int TransactionTypeId { get; set; }
    }
}
