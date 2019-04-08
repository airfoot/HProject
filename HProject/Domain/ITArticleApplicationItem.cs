using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Domain
{
    public class ITArticleApplicationItem
    {
        public Guid Id { get; set; }
        public int ITArticleNumber { get; set; }
        public virtual ITArticle ITArticleItem { get; set; }

        private ITArticleApplicationItem(ITArticle iTArticleItem,int iTArticleNumber)
        {
            this.Id = Guid.NewGuid();
            this.ITArticleItem = iTArticleItem;
            this.ITArticleNumber = iTArticleNumber;
        }

        public static ITArticleApplicationItem CreateITArticleApplicationItem(ITArticle iTArticle,int iTArticleNumbers)
        {
            ITArticleApplicationItem iTArticleApplicationItem = null;
            if (iTArticle.IsHavingStock(iTArticleNumbers))
             {
                iTArticle.ReduceStock(iTArticleNumbers);
                iTArticleApplicationItem = new ITArticleApplicationItem(iTArticle, iTArticleNumbers);
              }else
              {
                    throw(new ArgumentOutOfRangeException("Don't have enough stock!"));
              }
            return iTArticleApplicationItem;         
        }
        public void ChangeITArticleApplicationItem(int changeNumber)
        {
            
            
                int tempChange = 0;
                if(this.ITArticleNumber == changeNumber)
                {
                    return;
                }else if(this.ITArticleNumber > changeNumber)
                {
                    tempChange = this.ITArticleNumber - changeNumber;
                    this.ITArticleItem.IncreaseStock(tempChange);
                    this.ITArticleNumber = changeNumber;
                }else if(this.ITArticleNumber < changeNumber)
                {
                    tempChange = changeNumber - this.ITArticleNumber;
                    if(this.ITArticleItem.IsHavingStock(tempChange))
                    {
                        this.ITArticleItem.ReduceStock(tempChange);
                        this.ITArticleNumber = changeNumber;

                    }else
                    {
                        throw(new ArgumentOutOfRangeException("Don't have enough stock!"));
                    }
                }
            
        }


    }
}