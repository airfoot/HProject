using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Domain
{
    public class ITArticle
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int Stock { get; set; }

        public ITArticle(string name,string description,string iconUrl,int stock)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.IconUrl = iconUrl;
            this.Stock = stock;
        }
        public void IncreaseStock(int increase)
        {
            try
            {
                this.Stock = this.Stock + increase;
            }catch(Exception e)
            {
                throw;
            }
            
        }
        public void ReduceStock(int reduce)
        {
            try
            {
                this.Stock = this.Stock - reduce;
            }catch(Exception e)
            {
                throw;
            }
           
        }
        public bool IsHavingStock(int inquiry)
        {
            if(this.Stock>=inquiry)
            {
                return true;
            }
            return false;
        }
    }
}