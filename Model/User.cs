﻿namespace FamilyBudgetBackend.Model
{
    public class User
    {
        public int Id { get; set; }                  
        public string Name { get; set; }           
        public string Email { get; set; }            

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
