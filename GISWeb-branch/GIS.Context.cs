﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GISWeb
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GISEntities : DbContext
    {
        public GISEntities()
            : base("name=GISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<RepArea> RepAreas { get; set; }
        public virtual DbSet<SalesRep> SalesReps { get; set; }
        public virtual DbSet<Outcome> Outcomes { get; set; }
        public virtual DbSet<NotificationArea> NotificationAreas { get; set; }
        public virtual DbSet<FSRList> FSRLists { get; set; }
    
        public virtual ObjectResult<PremisesByPostCode_Result> PremisesByPostCode(string postCode)
        {
            var postCodeParameter = postCode != null ?
                new ObjectParameter("postCode", postCode) :
                new ObjectParameter("postCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PremisesByPostCode_Result>("PremisesByPostCode", postCodeParameter);
        }
    
        public virtual ObjectResult<PremisesBySalesRepIdMostRecentOutcome_Result> PremisesBySalesRepIdMostRecentOutcome(Nullable<int> salesRepId)
        {
            var salesRepIdParameter = salesRepId.HasValue ?
                new ObjectParameter("salesRepId", salesRepId) :
                new ObjectParameter("salesRepId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PremisesBySalesRepIdMostRecentOutcome_Result>("PremisesBySalesRepIdMostRecentOutcome", salesRepIdParameter);
        }
    
        public virtual ObjectResult<RepAreaPremisesAllocatedToSalesRepIdByDate_Result> RepAreaPremisesAllocatedToSalesRepIdByDate(Nullable<int> salesrepid, Nullable<System.DateTime> reportDate)
        {
            var salesrepidParameter = salesrepid.HasValue ?
                new ObjectParameter("salesrepid", salesrepid) :
                new ObjectParameter("salesrepid", typeof(int));
    
            var reportDateParameter = reportDate.HasValue ?
                new ObjectParameter("reportDate", reportDate) :
                new ObjectParameter("reportDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RepAreaPremisesAllocatedToSalesRepIdByDate_Result>("RepAreaPremisesAllocatedToSalesRepIdByDate", salesrepidParameter, reportDateParameter);
        }
    
        public virtual ObjectResult<OutcomeDNC_Result> OutcomeDNC()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<OutcomeDNC_Result>("OutcomeDNC");
        }
    
        public virtual ObjectResult<PremisesBySalesRepIdMostRecentOutcomeCurrentArea_Result> PremisesBySalesRepIdMostRecentOutcomeCurrentArea(Nullable<int> salesRepId)
        {
            var salesRepIdParameter = salesRepId.HasValue ?
                new ObjectParameter("salesRepId", salesRepId) :
                new ObjectParameter("salesRepId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PremisesBySalesRepIdMostRecentOutcomeCurrentArea_Result>("PremisesBySalesRepIdMostRecentOutcomeCurrentArea", salesRepIdParameter);
        }
    
        public virtual ObjectResult<NotificationArea> NotificationAreaByCompany(string companyName)
        {
            var companyNameParameter = companyName != null ?
                new ObjectParameter("companyName", companyName) :
                new ObjectParameter("companyName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NotificationArea>("NotificationAreaByCompany", companyNameParameter);
        }
    
        public virtual ObjectResult<NotificationArea> NotificationAreaByCompany(string companyName, MergeOption mergeOption)
        {
            var companyNameParameter = companyName != null ?
                new ObjectParameter("companyName", companyName) :
                new ObjectParameter("companyName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NotificationArea>("NotificationAreaByCompany", mergeOption, companyNameParameter);
        }
    }
}