﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base
{

    public static class DataSourceLoadOptionsBuilder{
        public static DataSourceLoadOptions Build(
            string select, string sort, string filter, int skip, int take,
            string totalSummary, string group, string groupSummary, 
            bool requireTotalCount, bool requireGroupCount) {

            var loadOptions = new DataSourceLoadOptions() {
                Skip = skip,
                Take = take,
                RequireTotalCount = requireTotalCount,
                RequireGroupCount = requireGroupCount
            };

            try {
                loadOptions.Select = (select == null) ? null : JsonSerializer.Deserialize<string[]>(select);
            } catch {
                throw new ArgumentException($"Could not parse provided '{select}' argument into valid DevExtreme select expression");
            }

            try {
                loadOptions.Sort = (sort == null) ? null : JsonSerializer.Deserialize<SortingInfo[]>(sort);
            } catch {
                throw new ArgumentException($"Could not parse provided '{sort}' argument into valid DevExtreme SortingInfo[] expression");
            }

            try {
                loadOptions.Filter = (filter == null) ? null : JToken.Parse(filter).ToObject<List<dynamic>>();
            } catch {
                throw new ArgumentException($"Could not parse provided '{filter}' argument into valid DevExtreme Filter expression");
            }

            try {
                loadOptions.TotalSummary = (totalSummary == null) ? null : JsonSerializer.Deserialize<SummaryInfo[]>(totalSummary);
            } catch {
                throw new ArgumentException($"Could not parse provided '{totalSummary}' argument into valid DevExtreme SummaryInfo[] expression");
            }

            try {
                loadOptions.Group = (group == null) ? null : JsonSerializer.Deserialize<GroupingInfo[]>(group);
            } catch {
                throw new ArgumentException($"Could not parse provided '{group}' argument into valid DevExtreme GroupingInfo[] expression");
            }

            try {
                loadOptions.GroupSummary = (groupSummary == null) ? null : JsonSerializer.Deserialize<SummaryInfo[]>(groupSummary);
            } catch {
                throw new ArgumentException($"Could not parse provided '{groupSummary}' argument into valid DevExtreme SummaryInfo[] expression");
            }

            return loadOptions;
        }
    }


    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase { 
    
    }



    public class DataSourceLoadOptionsBinder : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext context) {
            var loadOptions = new DataSourceLoadOptions();

            DataSourceLoadOptionsParser.Parse(loadOptions, key =>
                context.ValueProvider.GetValue(key).FirstOrDefault());

            context.Result = ModelBindingResult.Success(loadOptions);

            return Task.CompletedTask;
        }

    }




}
