using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HProject.Infrastructure;

namespace HProject.Domain
{
    public class ITArticleApplication
    {
        public Guid Id { get; set; }
        public DateTime BeginDateTime { get; set; }
        public List<ITArticleApplicationItem> ITArticleItemList { get; set; }
        public User Applicant { get; set; }
        public StateEnum State { get; set; }
        public string Reason { get; set; }

        public ITArticleApplication(DateTime beginDateTime,List<ITArticleApplicationItem> iTArticleItemList, User applicant,StateEnum state,string reason)
        {
            this.Id = Guid.NewGuid();
            this.BeginDateTime = beginDateTime;
            this.ITArticleItemList = iTArticleItemList;
            this.Applicant = applicant;
            this.State = state;
            this.Reason = reason;
        }

        public void AllocateITArticleAppliction()
        {
            this.State = StateEnum.Allocated;
        }

        public void ChangeITArticleApplication(Dictionary<ITArticleApplicationItem,int> changeITArticleItemList)
        {
            foreach(KeyValuePair<ITArticleApplicationItem,int> kvp in changeITArticleItemList)
            {
                kvp.Key.ChangeITArticleApplicationItem(kvp.Value);
            }
        }

        public void ConfirmITArticleApplication()
        {
            this.State = StateEnum.End;
        }

        public void WidthdrawITArticleApplication()
        {
            this.State = StateEnum.Withdraw;
        }

        public void RefuseITArticleApplication()
        {
            this.State = StateEnum.Refused;
        }

    }
}